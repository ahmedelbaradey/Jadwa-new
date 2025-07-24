using Application.Features.BoardMembers.Dtos;
using Domain.Entities.FundManagement;

namespace Application.Mapping
{
    /// <summary>
    /// Mapping configurations for editing BoardMember entities
    /// Maps from DTOs to domain entities for update operations
    /// </summary>
    public partial class BoardMembersProfile
    {
        public void EditBoardMemberMapping()
        {
            // EditBoardMemberDto to BoardMember entity
            CreateMap<EditBoardMemberDto, BoardMember>()
                .ForMember(dest => dest.Fund, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.ConflictItems, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore());

            // BoardMember entity to EditBoardMemberDto (for response after update)
            CreateMap<BoardMember, EditBoardMemberDto>()
                .IncludeBase<BoardMember, BoardMemberDto>();
        }
    }
}
