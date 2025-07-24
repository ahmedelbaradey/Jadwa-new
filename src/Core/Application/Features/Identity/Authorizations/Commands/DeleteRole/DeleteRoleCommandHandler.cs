using Abstraction.Contracts.Logger;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contracts.Identity;

namespace Application.Features.Identity.Authorizations.Commands.DeleteRole
{
    public class RoleCommandHandler : BaseResponseHandler, ICommandHandler<DeleteRoleCommand, BaseResponse<string>>
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
        public async Task<BaseResponse<string>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _service.AuthorizationService.GetRoleByID(request.Id);
                if (role == null)
                    return NotFound<string>("role with this id not found!");

                var IsDeleted = await _service.AuthorizationService.DeleteRoleById(role);
                if (!IsDeleted)
                    return BadRequest<string>("Deleted Operation Failed.");

                return Success("Deleted Operation Successfully.");
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
