using Abstraction.Base.Dto;
using Application.Common.Dtos;
using Domain.Entities.Base;


namespace Infrastructure.Dto.Strategies
{
    public record BaseStrategyDto : LocalizedDto
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
