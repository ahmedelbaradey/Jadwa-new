using Application.Features.Resolutions.Commands.Edit;
using Application.Features.Resolutions.Dtos;
using Domain.Entities;
using Domain.Entities.ResolutionManagement;

namespace Application.Mapping
{
    /// <summary>
    /// Mapping configurations for editing Resolution entities
    /// Maps from consolidated EditResolutionCommand to domain entities
    /// Handles basic resolution properties, items, and attachments
    /// </summary>
    public partial class ResolutionsProfile
    {
        public void EditResolutionMapping()
        {
            // EditResolutionCommand to Resolution entity (basic properties only)
            // Items and attachments are handled separately in the command handler
            CreateMap<EditResolutionCommand, Resolution>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Code, opt => opt.Ignore()) // Code should not be changed
                .ForMember(dest => dest.ResolutionDate, opt => opt.MapFrom(src => src.ResolutionDate))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ResolutionTypeId, opt => opt.MapFrom(src => src.ResolutionTypeId))
                .ForMember(dest => dest.NewType, opt => opt.MapFrom(src => src.NewType))
                .ForMember(dest => dest.AttachmentId, opt => opt.MapFrom(src => src.AttachmentId))
                .ForMember(dest => dest.VotingType, opt => opt.MapFrom(src => src.VotingType))
                .ForMember(dest => dest.MemberVotingResult, opt => opt.MapFrom(src => src.MemberVotingResult))
                .ForMember(dest => dest.FundId, opt => opt.MapFrom(src => src.FundId))
                .ForMember(dest => dest.Status, opt => opt.Ignore()) // Status is handled by state pattern
                .ForMember(dest => dest.ResolutionItems, opt => opt.Ignore()) // Handled separately
                .ForMember(dest => dest.OtherAttachments, opt => opt.Ignore()) // Handled separately
                .ForMember(dest => dest.Fund, opt => opt.Ignore())
                .ForMember(dest => dest.ResolutionType, opt => opt.Ignore())
                .ForMember(dest => dest.Attachment, opt => opt.Ignore());

            // EditResolutionDto to Resolution entity (for direct DTO mapping if needed)
            CreateMap<EditResolutionDto, Resolution>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // ID should be set separately for edit
                .ForMember(dest => dest.Code, opt => opt.Ignore()) // Code should not be changed
                .ForMember(dest => dest.ResolutionDate, opt => opt.MapFrom(src => src.ResolutionDate))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ResolutionTypeId, opt => opt.MapFrom(src => src.ResolutionTypeId))
                .ForMember(dest => dest.NewType, opt => opt.MapFrom(src => src.NewType))
                .ForMember(dest => dest.AttachmentId, opt => opt.MapFrom(src => src.AttachmentId))
                .ForMember(dest => dest.VotingType, opt => opt.MapFrom(src => src.VotingType))
                .ForMember(dest => dest.MemberVotingResult, opt => opt.MapFrom(src => src.MemberVotingResult))
                .ForMember(dest => dest.FundId, opt => opt.MapFrom(src => src.FundId))
                .ForMember(dest => dest.Status, opt => opt.Ignore()) // Status is handled by state pattern
                .ForMember(dest => dest.ResolutionItems, opt => opt.Ignore()) // Handled separately
                .ForMember(dest => dest.OtherAttachments, opt => opt.Ignore()) // Handled separately
                .ForMember(dest => dest.Fund, opt => opt.Ignore())
                .ForMember(dest => dest.ResolutionType, opt => opt.Ignore())
                .ForMember(dest => dest.Attachment, opt => opt.Ignore());
        }
    }
}
