using Abstraction.Contracts.Logger;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contracts.Identity;


namespace Application.Features.Identity.Authorizations.Commands.EditRole
{
    public class RoleCommandHandler : BaseResponseHandler, ICommandHandler<EditRoleCommand, BaseResponse<string>>
    {
        #region Fileds
        private readonly IIdentityServiceManager _service;
        private readonly ILoggerManager _logger;
        #endregion

        #region Constructors
        public RoleCommandHandler(IIdentityServiceManager service, ILoggerManager logger)
        {
            _service = service;
            _logger = logger;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<string>> Handle(EditRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var isExist = await _service.AuthorizationService.IsRoleNameExist(request.roleName);
                if (!isExist)
                    return BadRequest<string>("this role name is not Exist.");

                var IsEdited = await _service.AuthorizationService.EditRoleById(request.Id, request.roleName, request.RoleClaims);

                if (!IsEdited) return BadRequest<string>("Edited operation failed.");

                return Success("Edited Operation Successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return ServerError<string>(ex.Message);
            }
        }
        #endregion
    }
}
