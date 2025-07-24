using Application.Features.Shared.FileManagment.Commands.MinIOUpload;
using Application.Features.Shared.FileManagment.Commands.MinIOUploadMultiple;
using AutoMapper;
using Domain.Entities.Shared;

namespace Application.Mapping
{
    /// <summary>
    /// AutoMapper profile for MinIO attachment operations
    /// </summary>
    public partial class AttachmentsProfile
    {
        /// <summary>
        /// Configures mapping for MinIO upload commands to Attachment entity
        /// </summary>
        public void MinIOAttachmentMapping()
        {
            CreateMap<MinIOUploadCommand, Attachment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Path, opt => opt.Ignore())
                .ForMember(dest => dest.FileSize, opt => opt.Ignore())
                .ForMember(dest => dest.ContentType, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore());

            CreateMap<MinIOUploadMultipleCommand, Attachment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FileName, opt => opt.Ignore()) // Set individually for each file
                .ForMember(dest => dest.Path, opt => opt.Ignore())
                .ForMember(dest => dest.FileSize, opt => opt.Ignore())
                .ForMember(dest => dest.ContentType, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore());
        }
    }
}
