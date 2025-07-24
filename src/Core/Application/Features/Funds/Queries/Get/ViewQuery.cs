using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Funds.Dtos;

namespace Application.Features.Funds.Queries.Get
{
    public record ViewQuery : IQuery<BaseResponse<FundDetailsResponse>>
    {
        public int Id { get; set; }
    }
}
