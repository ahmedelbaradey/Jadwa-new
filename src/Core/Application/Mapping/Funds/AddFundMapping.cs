using Application.Features.Funds.Commands.Add;
using Domain.Entities.FundManagement;

namespace Application.Mapping
{
    public partial class FundsProfile
    {
        public void AddFundMapping()
        {
            CreateMap<AddFundCommand, Fund>()
                .ForMember(dest => dest.FundManagers, opt => opt.MapFrom(src => src.FundManagers))
                .ForMember(dest => dest.FundBoardSecretaries, opt => opt.MapFrom(src => src.FundBoardSecretaries));
        }
    }
}
