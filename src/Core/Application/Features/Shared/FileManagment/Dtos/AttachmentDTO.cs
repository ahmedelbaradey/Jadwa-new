using Abstraction.Base.Dto;

namespace Application.Features.Shared.FileManagment.Dtos
{
    public record AttachmentDTO :BaseDto
    {
        public string? FileName { get; set; }
        public string? Folder { get; set; }
        public string? ServerFileName { get; set; }
        public string? URL { get; set; }
        public string? PreviewUrl { get; set; }
        public string? Extension { get; set; }
    }
}
