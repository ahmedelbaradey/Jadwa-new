using AutoMapper;

namespace Application.Mapping
{
    public partial class FundsProfile : Profile
    {
        public FundsProfile()
        {
            AddFundMapping();
            EditFundMapping();
            GetFundMapping();
            FundManagerMapping();
            FundBoardSecretaryMapping();
            GetFundDetailsMapping();
        }
    }
}
