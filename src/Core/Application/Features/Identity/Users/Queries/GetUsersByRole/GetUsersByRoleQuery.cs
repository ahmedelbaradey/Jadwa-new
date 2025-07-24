using Abstraction.Base.Dto;
using Abstraction.Base.Response;
using Abstraction.Common.Wappers;
using Application.Base.Abstracts;
using Application.Features.Identity.Users.Queries.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Users.Queries.GetUsersByRole
{
    public record GetUsersByRoleQuery: BaseListDto, IQuery<PaginatedResult<GetUserListResponse>>
    {
        public string RoleName { get; set; }
    }
}
