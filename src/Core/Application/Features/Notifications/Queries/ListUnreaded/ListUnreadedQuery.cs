using Abstraction.Base.Dto;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Common.Wappers;
using Application.Features.Funds.Dtos;
using Application.Features.Notifications.Dtos;

namespace Application.Features.Notifications.Queries.List
{
    public record ListUnreadedQuery : BaseListDto, IQuery<PaginatedResult<NotificationDto>>
    {
    }
}
