using Abstraction.Base.Response;
using Application.Base.Abstracts;
using Application.Features.Shared.FileManagment.Dtos;

namespace Application.Features.Shared.FileManagment.Commands.MinIODownload
{
    /// <summary>
    /// Command for downloading files from MinIO storage
    /// Separate from DownloadAttachmentCommand to maintain existing functionality
    /// </summary>
    public record MinIODownloadCommand : ICommand<BaseResponse<DownloadAttachmentDTO>>
    {
        /// <summary>
        /// Attachment ID to download
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Optional bucket name override (if not provided, will determine from attachment module)
        /// </summary>
        public string? BucketName { get; set; }
    }
}
