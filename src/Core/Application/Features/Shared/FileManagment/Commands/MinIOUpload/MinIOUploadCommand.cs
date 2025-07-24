using Abstraction.Base.Response;
using Abstraction.Enums;
using Application.Base.Abstracts;
using Application.Features.Shared.FileManagment.Dtos;

namespace Application.Features.Shared.FileManagment.Commands.MinIOUpload
{
    /// <summary>
    /// Command for uploading files to MinIO storage
    /// Separate from AddAttachmentCommand to maintain existing functionality
    /// </summary>
    public record MinIOUploadCommand : AddAttachment, ICommand<BaseResponse<AttachmentDTO>>
    {
        /// <summary>
        /// Optional bucket name override (if not provided, will use module-based bucket)
        /// </summary>
        public ModuleEnum? BucketName { get; set; }
    }
}
