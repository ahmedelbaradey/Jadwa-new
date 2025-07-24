using Abstraction.Base.Dto;


namespace Infrastructure.Dto.DemoEntity
{
    public record DemoEntityDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
