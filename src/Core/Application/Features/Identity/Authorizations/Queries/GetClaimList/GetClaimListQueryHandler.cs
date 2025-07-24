using Abstraction.Contracts.Logger;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Domain.Helpers;
using Abstraction.Constants;



namespace Application.Features.Identity.Authorizations.Queries.GetClaimList
{
    public class GetClaimListQueryHandler : BaseResponseHandler, IQueryHandler<GetClaimListQuery, BaseResponse<List<RoleClaims>>>
    {
        #region Fileds
        private readonly ILoggerManager _logger;
        #endregion

        #region Constructors
        public GetClaimListQueryHandler(ILoggerManager logger)
        {
            _logger = logger;
        }
        #endregion

        #region Functions

        public async Task<BaseResponse<List<RoleClaims>>> Handle(GetClaimListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var claims = new List<RoleClaims>();
                foreach (var module in Claims.GenerateModules())
                {
                    foreach (var permission in Claims.GeneratePermissions(module))
                    {
                        claims.Add(new RoleClaims
                        {
                            Value = $"{module}.{permission}",
                            Type = "Permission",
                            HasClaim = false,
                            
                        });
                    }
                }
                if (claims == null)
                {
                    return NotFound<List<RoleClaims>>("Not Found claims");
                }

               
                return Success(claims);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return ServerError<List<RoleClaims>>(ex.Message);
            }
        }

        #endregion
    }
}
