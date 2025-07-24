using Abstraction.Base.Response;
using Abstraction.Common.Wappers;
using Abstraction.Contracts.Identity;
using Abstraction.Contracts.Logger;
using Application.Base.Abstracts;
using Application.Features.Catalog.Products.Dtos;
using Application.Features.Identity.Users.Queries.Responses;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Users.Queries.GetUsersByRole
{
    public class GetUsersByRoleQueryHandler : BaseResponseHandler, IQueryHandler<GetUsersByRoleQuery, PaginatedResult<GetUserListResponse>>
    {
        #region Fields
        private readonly IIdentityServiceManager _service;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        #endregion

        #region Constructors
        public GetUsersByRoleQueryHandler(IIdentityServiceManager service, IMapper mapper, ILoggerManager logger)
        {
            _mapper = mapper;
            _service = service;
            _logger = logger;
        }
        #endregion

        #region Functions
        public async Task<PaginatedResult<GetUserListResponse>> Handle(GetUsersByRoleQuery request, CancellationToken cancellationToken)
        {

            try
            {
                var users = await _service.UserManagmentService.GetUsersByRole(request.RoleName);
                if (!users.Any())
                {
                    return PaginatedResult<GetUserListResponse>.EmptyCollection();
                }
                var userList = _mapper.Map<List<GetUserListResponse>>(users);
                var paginatedResult = PaginatedResult<GetUserListResponse>.Success(
                    userList,
                    userList.Count, // Use actual total count if available
                    request.PageNumber,
                    request.PageSize
                );
                return paginatedResult;
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUserPaginatedListQuery");
                return PaginatedResult<GetUserListResponse>.ServerError("Error in GetUserPaginatedListQuery");
            }
        }
        #endregion
    }
}
