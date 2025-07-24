using AutoMapper;
using Domain.Helpers;
using Application.Base.Abstracts;
using Microsoft.AspNetCore.Identity;
using Domain.Entities.Users;
using Abstraction.Base.Response;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Identity;

namespace Application.Features.Identity.Users.Queries.GetUserRoles
{
    public class GetUserRolesQueryHandler : BaseResponseHandler, IQueryHandler<GetUserRolesQuery, BaseResponse<ManageUserRolesResponse>>
    {
        #region Fileds
        private readonly IIdentityServiceManager _service;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ILoggerManager _logger;
        #endregion

        #region Constructors
        public GetUserRolesQueryHandler(IIdentityServiceManager service, IMapper mapper, UserManager<User> userManager, ILoggerManager logger)
        {
            _service = service;
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<ManageUserRolesResponse>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.Id.ToString());
                if (user == null)
                    return NotFound<ManageUserRolesResponse>("user with this Id not Found!");

                var result = await _service.AuthorizationService.GetUsersRoles(user);
                return Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return ServerError<ManageUserRolesResponse>(ex.Message);
            }
        }
        #endregion
    }
}
