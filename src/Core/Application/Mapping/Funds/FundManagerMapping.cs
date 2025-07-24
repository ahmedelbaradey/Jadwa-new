using Application.Features.Funds.Commands.Add;
using Application.Features.Funds.Dtos;
using Domain.Entities.FundManagement;

namespace Application.Mapping
{
    public partial class FundsProfile
    {
        public void FundManagerMapping()
        {
            CreateMap<int, FundManager>()
              .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src));
        }
    }
}
