using Abstraction.Base.Dto;
using Abstraction.Common.Wappers;
using Application.Base.Abstracts;
using Application.Features.BoardMembers.Dtos;
using Application.Features.Identity.Users.Queries.Responses;
namespace Application.Features.Identity.Users.Queries.List
{
    public record ListForBoardMembersQuery : BaseListDto, IQuery<PaginatedResult<GetUserListResponse>>
    {
        public int FundId { get; set; }
    }

}
