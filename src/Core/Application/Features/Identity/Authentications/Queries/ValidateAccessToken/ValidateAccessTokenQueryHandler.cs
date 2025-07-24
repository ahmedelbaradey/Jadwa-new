using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Identity;

namespace Application.Features.Identity.Authentications.Queries.ValidateAccessToken
{
    public class ValidateAccessTokenQueryHandler : BaseResponseHandler, IQueryHandler<AccessTokenQuery, BaseResponse<string>>
    {
        #region Fields
        private readonly IIdentityServiceManager _service;
        private readonly ILoggerManager _logger;
        #endregion

        #region Constructors
        public ValidateAccessTokenQueryHandler(IIdentityServiceManager service, ILoggerManager logger)
        {
            _service = service;
            _logger = logger;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<string>> Handle(AccessTokenQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _service.AuthenticationService.ValidateJwtToken(request.Accesstoken);
                if (result == "NotExpired")
                {
                    return Success("this token is not expired.");
                }

                return Unauthorized<string>("this token is expired.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: in AddAppointmentCommand");
                return ServerError<string>(ex.Message);
            }
        }
        #endregion
    }
}
