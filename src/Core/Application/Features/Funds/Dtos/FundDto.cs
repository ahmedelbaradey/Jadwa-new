

using Abstraction.Base.Dto;

namespace Application.Features.Funds.Dtos
{
    public record FundDto : BaseFundDto
    {
        public DateTime? ExitDate { get; set; } = null;
    }
    public record BaseFundDto : BaseDto
    {
        public string Name { get; set; }
        public int StrategyId { get; set; }
        public string StrategyName { get; set; }
        public string Status { get; set; }
        public int? StatusId { get; set; }
        public DateTime InitiationDate { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
