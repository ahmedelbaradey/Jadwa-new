using Abstraction.Base.Dto;

namespace Infrastructure.Dto.Strategies
{
    public record StrategyEditDto : BaseDto
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
