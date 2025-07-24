using Abstraction.Contracts.Logger;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contracts.Identity;


namespace Application.Features.Identity.Authorizations.Commands.AddRole
{
    public class AddRoleCommandHandler : BaseResponseHandler, ICommandHandler<AddRoleCommand, BaseResponse<string>>
    {
        #region Fileds
        private readonly IIdentityServiceManager _service;
        private readonly ILoggerManager _logger;
        #endregion

        #region Constructors
        public AddRoleCommandHandler(IIdentityServiceManager service, ILoggerManager logger)
        {
            _service = service;
            _logger = logger;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<string>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var isExist = await _service.AuthorizationService.IsRoleNameExist(request.roleName);
                if (isExist)
                    return BadRequest<string>("this role name is already exist.");

                var IsAdded = await _service.AuthorizationService.AddRoleAsync(request.roleName,request.RoleClaims);

                if (!IsAdded) return BadRequest<string>("Added operation failed.");

                return Success("Added Operation Successfully.");
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
