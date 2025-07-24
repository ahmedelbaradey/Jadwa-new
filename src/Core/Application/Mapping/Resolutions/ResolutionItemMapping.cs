using Application.Features.Resolutions.Dtos;
using Application.Mapping.Resolutions;
using Domain.Entities.FundManagement;
using Domain.Entities.ResolutionManagement;

namespace Application.Mapping
{
    /// <summary>
    /// Mapping configurations for ResolutionItem entities
    /// Maps between ResolutionItem entities and DTOs
    /// </summary>
    public partial class ResolutionsProfile
    {
        public void ResolutionItemMapping()
        {
            // ResolutionItem entity to ResolutionItemDto
            CreateMap<ResolutionItem, ResolutionItemDto>()
                .ForMember(dest => dest.ConflictMembers, opt => opt.MapFrom(src => src.ConflictMembers))
                .ForMember(dest => dest.ConflictMembersCount, opt => opt.MapFrom(src => src.ConflictMembers.Count));

            // ResolutionItemDto to ResolutionItem entity (for EditResolution operations)
            CreateMap<ResolutionItemDto, ResolutionItem>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title)) // ✅ Map Title field
                .ForMember(dest => dest.ResolutionId, opt => opt.MapFrom(src => src.ResolutionId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.HasConflict, opt => opt.MapFrom(src => src.HasConflict))
                .ForMember(dest => dest.DisplayOrder, opt => opt.MapFrom(src => src.DisplayOrder))
                .ForMember(dest => dest.Resolution, opt => opt.Ignore())
                .ForMember(dest => dest.ConflictMembers, opt => opt.MapFrom(src => src.ConflictMembers)) // ✅ Map ConflictMembers
                .ForMember(dest => dest.Votes, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore());

            // ResolutionItemConflict entity to ResolutionItemConflictDto
            CreateMap<ResolutionItemConflict, ResolutionItemConflictDto>()
                .ForMember(dest => dest.BoardMemberName, opt => opt.MapFrom(src => src.BoardMember.User.FullName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.BoardMember.User.UserName))
                .ForMember(dest => dest.MemberType, opt => opt.MapFrom(src =>
                    src.BoardMember.MemberType == BoardMemberType.Independent ? "Independent" : "Not Independent"));

            // ResolutionItemConflictDto to ResolutionItemConflict entity (for EditResolution operations)
            CreateMap<ResolutionItemConflictDto, ResolutionItemConflict>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ResolutionItemId, opt => opt.Ignore()) // Will be set by parent
                .ForMember(dest => dest.ResolutionItem, opt => opt.Ignore())
                .ForMember(dest => dest.BoardMember, opt => opt.Ignore())
                .ForMember(dest => dest.BoardMemberId, opt => opt.MapFrom(src => src.BoardMemberId))
                .ForMember(dest => dest.ConflictNotes, opt => opt.MapFrom(src => src.ConflictNotes))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore());

            // CreateResolutionItemRequest to ResolutionItem entity
            CreateMap<CreateResolutionItemRequest, ResolutionItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.Ignore()) // Will be generated
                .ForMember(dest => dest.DisplayOrder, opt => opt.Ignore()) // Will be calculated
                .ForMember(dest => dest.Resolution, opt => opt.Ignore())
                .ForMember(dest => dest.ConflictMembers, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore());

            // EditResolutionItemRequest to ResolutionItem entity
            CreateMap<EditResolutionItemRequest, ResolutionItem>()
                .IncludeBase<CreateResolutionItemRequest, ResolutionItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            // ResolutionItem entity to ResolutionItemResponse
            CreateMap<ResolutionItem, ResolutionItemResponse>()
                .IncludeBase<ResolutionItem, ResolutionItemDto>()
                .ForMember(dest => dest.Message, opt => opt.Ignore());

          

            // ResolutionVote entity to ResolutionVoteDto
            CreateMap<ResolutionVote, ResolutionVoteDto>()
                .ForMember(dest => dest.BoardMemberName, opt => opt.MapFrom(src => src.BoardMember.User.FullName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.BoardMember.User.UserName))
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.BoardMember.User.FullName))
                .ForMember(dest => dest.MemberType, opt => opt.MapFrom(src =>
                    src.BoardMember.MemberType == BoardMemberType.Independent ? "Independent" : "Not Independent"))
                .ForMember(dest => dest.VoteValueDisplay, opt => opt.Ignore()) // Will be set by localization service
                .ForMember(dest => dest.VotedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.IsChairman, opt => opt.MapFrom(src => src.BoardMember.IsChairman));

            // CastVoteRequest to ResolutionVote entity
            CreateMap<CastVoteRequest, ResolutionVote>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                // .ForMember(dest => dest.VotedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Resolution, opt => opt.Ignore())
                .ForMember(dest => dest.BoardMember, opt => opt.Ignore())
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