using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Resolutions.Dtos;

namespace Application.Features.Resolutions.Queries.Get
{
    public record GetQuery : IQuery<BaseResponse<SingleResolutionResponseView>>
    {
        public int Id { get; set; }
    }
}
