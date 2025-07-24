using AutoMapper;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contracts.Identity;
using Domain.Entities.Users;
using DocumentFormat.OpenXml.Spreadsheet;
using Resources;
using Microsoft.Extensions.Localization;


namespace Application.Features.Identity.Users.Commands.EditUserPreferredLanguage
{
    public class EditUserPreferredLanguageCommandHandler : BaseResponseHandler, ICommandHandler<EditUserPreferredLanguageCommand, BaseResponse<string>>
    {
        #region Fields
        private readonly IIdentityServiceManager _service;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;

        #endregion

        #region Constructors
        public EditUserPreferredLanguageCommandHandler(IIdentityServiceManager service, IMapper mapper, IStringLocalizer<SharedResources> localizer)
        {
            _mapper = mapper;
            _service = service;
            _localizer = localizer;
        }
        #endregion

            #region Functions
        public async Task<BaseResponse<string>> Handle(EditUserPreferredLanguageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var oldUser = await _service.UserManagmentService.FindByIdAsync(request.Id.ToString());
                if (oldUser == null)
                    return NotFound<string>(_localizer[SharedResourcesKey.UserNotFound]);

                oldUser.PreferredLanguage = request.UserPreferredLanguage;
                var result = await _service.UserManagmentService.UpdateAsync(oldUser);
                if (!result.Succeeded)
                    return BadRequest<string>(result.Errors.FirstOrDefault()?.Description);

                return Success<string>(_localizer[SharedResourcesKey.PreferredLanguageUpdatedSuccessfully]);
            }
            catch (Exception ex)
            {
                return ServerError<string>(ex.Message);
            }
        }
        #endregion
    }
}
