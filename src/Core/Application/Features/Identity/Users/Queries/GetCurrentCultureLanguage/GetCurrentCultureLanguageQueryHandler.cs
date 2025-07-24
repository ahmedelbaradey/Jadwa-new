using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contract.Service;
using Abstraction.Contracts.Identity;
using Abstraction.Contracts.Logger;
using Microsoft.Extensions.Localization;
using Resources;
using System.Globalization;
using Application.Features.Identity.Users.Queries.Responses;

namespace Application.Features.Identity.Users.Queries.GetCurrentCultureLanguage
{
    /// <summary>
    /// Handler for retrieving current culture language information
    /// Gets both the current request culture and user's preferred language from database
    /// </summary>
    public class GetCurrentCultureLanguageQueryHandler : BaseResponseHandler, IQueryHandler<GetCurrentCultureLanguageQuery, BaseResponse<GetCurrentCultureLanguageResponse>>
    {
        #region Fields
        private readonly IIdentityServiceManager _identityService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILoggerManager _logger;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public GetCurrentCultureLanguageQueryHandler(
            IIdentityServiceManager identityService,
            ICurrentUserService currentUserService,
            ILoggerManager logger,
            IStringLocalizer<SharedResources> localizer)
        {
            _identityService = identityService;
            _currentUserService = currentUserService;
            _logger = logger;
            _localizer = localizer;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<GetCurrentCultureLanguageResponse>> Handle(GetCurrentCultureLanguageQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo("Getting current culture language information");

                // Get current user ID
                var currentUserId = _currentUserService.UserId;
                if (!currentUserId.HasValue)
                {
                    _logger.LogWarn("No authenticated user found");
                    return Unauthorized<GetCurrentCultureLanguageResponse>(_localizer[SharedResourcesKey.UnauthorizedAccess]);
                }

                // Get current culture from thread
                var currentCulture = CultureInfo.CurrentCulture.Name;
                var currentCultureDisplayName = CultureInfo.CurrentCulture.DisplayName;

                // Get user's preferred language from database
                var user = await _identityService.UserManagmentService.FindByIdAsync(currentUserId.Value.ToString());
                if (user == null)
                {
                    _logger.LogWarn($"User with ID {currentUserId.Value} not found");
                    return NotFound<GetCurrentCultureLanguageResponse>(_localizer[SharedResourcesKey.UserNotFound]);
                }

                // Get preferred language display name
                var preferredLanguageDisplayName = string.Empty;
                try
                {
                    var preferredCulture = new CultureInfo(user.PreferredLanguage);
                    preferredLanguageDisplayName = preferredCulture.DisplayName;
                }
                catch (CultureNotFoundException)
                {
                    _logger.LogWarn($"Invalid preferred language culture: {user.PreferredLanguage}");
                    preferredLanguageDisplayName = user.PreferredLanguage;
                }

                var response = new GetCurrentCultureLanguageResponse
                {
                    CurrentCulture = currentCulture,
                    PreferredLanguage = user.PreferredLanguage,      
                };

                _logger.LogInfo($"Successfully retrieved culture language information for user {currentUserId.Value}");
                return Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving current culture language information");
                return ServerError<GetCurrentCultureLanguageResponse>(_localizer[SharedResourcesKey.SystemErrorRetrievingData]);
            }
        }
        #endregion
    }
}
