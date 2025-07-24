using Microsoft.AspNetCore.Http;

namespace Application.Features.Shared.FileManagment.Dtos
{
    public record AddAttachment
    {
        public IFormFile File { get; set; }
        public string FileName { get; set; }
        public int ModuleId { get; set; }
    }
}
