using Application.Features.Funds.Commands.Edit;
using Domain.Entities.FundManagement;

namespace Application.Mapping
{
    public partial class FundsProfile
    {
        public void EditFundMapping()
        {
            CreateMap<SaveFundCommand, Fund>()
              .ForMember(dest => dest.FundManagers, opt => opt.MapFrom(src => src.FundManagers))
              .ForMember(dest => dest.FundBoardSecretaries, opt => opt.MapFrom(src => src.FundBoardSecretaries));
            CreateMap<EditExitDateCommand, Fund>();


        }
    }
}
