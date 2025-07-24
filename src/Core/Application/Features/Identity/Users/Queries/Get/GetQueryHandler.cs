using AutoMapper;
using Application.Features.Identity.Users.Queries.Responses;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contracts.Identity;


namespace Application.Features.Identity.Users.Queries.Get
{
    public class GetQueryHandler : BaseResponseHandler, IQueryHandler<GetUserQuery, BaseResponse<GetUserResponse>>
    {
        #region Fields
        private readonly IIdentityServiceManager _service;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        public GetQueryHandler(IIdentityServiceManager service, IMapper mapper)
        {
            _mapper = mapper;
            _service = service;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<GetUserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _service.UserManagmentService.FindByIdAsync(request.Id.ToString());
                
                if (user == null)
                    return NotFound<GetUserResponse>($"User with Id: {request.Id} not found!");

                var usermapper = _mapper.Map<GetUserResponse>(user);
                var roles = await _service.AuthorizationService.GetUsersRoles(user);
                if(roles.UserRoles.Any())
                    usermapper.UserRoles =  roles.UserRoles;
                 
                return Success(usermapper);
            }
            catch (Exception ex)
            {

                return ServerError<GetUserResponse>(ex.Message);
            }
        }



        #endregion

    }
}
