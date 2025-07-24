namespace Application.Features.Shared.FileManagment.Dtos
{
    public class DownloadAttachmentDTO
    {
        public int Id { get; internal set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public string? Path { get; set; }
        public string? Size { get; set; }
        public byte[]? FileBytes { get; set; }
    }
}
