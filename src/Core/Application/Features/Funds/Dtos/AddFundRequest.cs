
namespace Application.Features.Funds.Dtos
{
    public record AddFundRequest : BaseFundDto
    {
        public string OldCode { get; set; } = string.Empty;
        public int PropertiesNumber { get; set; }
        public int AttachmentId { get; set; }
        public int VotingTypeId { get; set; }
        public int LegalCouncilId { get; set; }
        public DateTime? ExitDate { get; set; }
        public List<int> FundManagers { get; set; }
        public List<int>? FundBoardSecretaries { get; set; }
    }
}
