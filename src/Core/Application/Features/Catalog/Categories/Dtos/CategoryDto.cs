

using Abstraction.Base.Dto;

namespace Application.Features.Catalog.Categories.Dtos
{
    public record CategoryDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
