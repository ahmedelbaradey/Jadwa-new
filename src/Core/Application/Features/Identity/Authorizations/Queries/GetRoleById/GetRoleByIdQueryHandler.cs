using Abstraction.Contracts.Logger;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Identity.Authorizations.Queries.Responses;
using AutoMapper;
using Abstraction.Contracts.Identity;


namespace Application.Features.Identity.Authorizations.Queries.GetRoleById
{
    public class GetRoleByIdQueryHandler : BaseResponseHandler, IQueryHandler<GetRoleByIdQuery, BaseResponse<GetRoleByIdResponse>>

    {
        #region Fileds
        private readonly IIdentityServiceManager _service;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        #endregion

        #region Constructors
        public GetRoleByIdQueryHandler(IIdentityServiceManager service, IMapper mapper, ILoggerManager logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<GetRoleByIdResponse>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _service.AuthorizationService.GetRoleByID(request.Id);
                if (role == null)
                    return NotFound<GetRoleByIdResponse>("role with this Id not Found!");

                var roleMapper = _mapper.Map<GetRoleByIdResponse>(role);
                var rolePermissions = await _service.AuthorizationService.GetRoleClaims(request.Id.ToString());
                if (rolePermissions == null)
                    return NotFound<GetRoleByIdResponse>("role claims with this Id not Found!");
                roleMapper.RoleClaims = rolePermissions;

                return Success(roleMapper);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return ServerError<GetRoleByIdResponse>(ex.Message);
            }
        }
        #endregion
    }
}
