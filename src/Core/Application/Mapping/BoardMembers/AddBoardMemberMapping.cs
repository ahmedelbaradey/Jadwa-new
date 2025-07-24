using Application.Features.BoardMembers.Dtos;
using Domain.Entities.FundManagement;

namespace Application.Mapping
{
    /// <summary>
    /// Mapping configurations for adding BoardMember entities
    /// Maps from DTOs to domain entities for create operations
    /// </summary>
    public partial class BoardMembersProfile
    {
        public void AddBoardMemberMapping()
        {
            // AddBoardMemberDto to BoardMember entity
            CreateMap<AddBoardMemberDto, BoardMember>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Fund, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.ConflictItems, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore());

            // BoardMember entity to AddBoardMemberDto (for response after creation)
            CreateMap<BoardMember, AddBoardMemberDto>()
                .IncludeBase<BoardMember, BoardMemberDto>();
        }
    }
}
