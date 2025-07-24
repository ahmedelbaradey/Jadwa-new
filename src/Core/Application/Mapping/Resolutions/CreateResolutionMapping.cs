using Application.Features.Resolutions.Dtos;
using Domain.Entities.ResolutionManagement;

namespace Application.Mapping
{
    /// <summary>
    /// Mapping configurations for creating Resolution entities
    /// Maps from DTOs to domain entities for create and update operations
    /// </summary>
    public partial class ResolutionsProfile
    {
        public void CreateResolutionMapping()
        {
            // AddResolutionDto to Resolution entity
            CreateMap<AddResolutionDto, Resolution>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Code, opt => opt.Ignore()) // Will be generated
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ResolutionStatusEnum.Draft))
                .ForMember(dest => dest.Fund, opt => opt.Ignore())
                .ForMember(dest => dest.ResolutionType, opt => opt.Ignore())
                .ForMember(dest => dest.Attachment, opt => opt.Ignore())
                .ForMember(dest => dest.OldResolutionCode, opt => opt.Ignore()) // Will be set by handler for Alternative 2
                .ForMember(dest => dest.ParentResolutionId, opt => opt.Ignore()) // DTO property, not mapped to entity
                .ForMember(dest => dest.ResolutionItems, opt => opt.Ignore()) // Will be set by handler for Alternative 2
                .ForMember(dest => dest.ChildResolutions, opt => opt.Ignore()) // Navigation property
                .ForMember(dest => dest.ParentResolution, opt => opt.Ignore()) // Navigation property
                .ForMember(dest => dest.OtherAttachments, opt => opt.Ignore()) // Will be handled separately
                .ForMember(dest => dest.ResolutionStatusHistories, opt => opt.Ignore()) // Will be handled by state pattern
                .ForMember(dest => dest.Votes, opt => opt.Ignore()); // Will be handled separately

            // CompleteResolutionDataDto to Resolution entity
            CreateMap<CompleteResolutionDataDto, Resolution>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Code, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore()) // Will be set by handler
                .ForMember(dest => dest.Fund, opt => opt.Ignore())
                .ForMember(dest => dest.ResolutionType, opt => opt.Ignore())
                .ForMember(dest => dest.Attachment, opt => opt.Ignore());

            // ResolutionItemDto to ResolutionItem entity (for CreateResolution operations)
            CreateMap<ResolutionItemDto, ResolutionItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ResolutionId, opt => opt.Ignore()) // Will be set by handler
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.Title) ? string.Empty : src.Title)) // ✅ Map Title field, use empty if null
                .ForMember(dest => dest.DisplayOrder, opt => opt.Ignore()) // Will be set by handler
                .ForMember(dest => dest.Resolution, opt => opt.Ignore())
                .ForMember(dest => dest.ConflictMembers, opt => opt.MapFrom(src => src.ConflictMembers)); // Map nested conflicts

            // ResolutionItemConflictDto to ResolutionItemConflict entity
            CreateMap<ResolutionItemConflictDto, ResolutionItemConflict>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ResolutionItemId, opt => opt.Ignore()) // Will be set by parent
                .ForMember(dest => dest.ResolutionItem, opt => opt.Ignore())
                .ForMember(dest => dest.BoardMember, opt => opt.Ignore())
                .ForMember(dest => dest.BoardMemberId, opt => opt.MapFrom(src => src.BoardMemberId))
                .ForMember(dest => dest.ConflictNotes, opt => opt.MapFrom(src => src.ConflictNotes));

            // EditResolutionDto to Resolution entity
            CreateMap<EditResolutionDto, Resolution>()
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
                .ForMember(dest => dest.Fund, opt => opt.Ignore())
                .ForMember(dest => dest.ResolutionType, opt => opt.Ignore())
                .ForMember(dest => dest.Attachment, opt => opt.Ignore())
                .ForMember(dest => dest.OldResolutionCode, opt => opt.Ignore())
                .ForMember(dest => dest.ParentResolutionId, opt => opt.Ignore())
                .ForMember(dest => dest.ResolutionItems, opt => opt.Ignore())
                .ForMember(dest => dest.ChildResolutions, opt => opt.Ignore())
                .ForMember(dest => dest.ParentResolution, opt => opt.Ignore())
                .ForMember(dest => dest.OtherAttachments, opt => opt.Ignore())
                .ForMember(dest => dest.ResolutionStatusHistories, opt => opt.Ignore())
                .ForMember(dest => dest.Votes, opt => opt.Ignore());

            // Alternative 2: ResolutionItem to ResolutionItem (for copying from original resolution)
            CreateMap<ResolutionItem, ResolutionItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Reset ID for new entity
                .ForMember(dest => dest.ResolutionId, opt => opt.Ignore()) // Will be set by handler
                .ForMember(dest => dest.Resolution, opt => opt.Ignore()) // Navigation property
                .ForMember(dest => dest.ConflictMembers, opt => opt.MapFrom(src => src.ConflictMembers)) // Copy conflicts
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Reset audit fields
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore());

            // Alternative 2: ResolutionItemConflict to ResolutionItemConflict (for copying conflicts)
            CreateMap<ResolutionItemConflict, ResolutionItemConflict>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Reset ID for new entity
                .ForMember(dest => dest.ResolutionItemId, opt => opt.Ignore()) // Will be set by parent item
                .ForMember(dest => dest.ResolutionItem, opt => opt.Ignore()) // Navigation property
                .ForMember(dest => dest.BoardMember, opt => opt.Ignore()) // Navigation property
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Reset audit fields
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore());
        }
    }
}
