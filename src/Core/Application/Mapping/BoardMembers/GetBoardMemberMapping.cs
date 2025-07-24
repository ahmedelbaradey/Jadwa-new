using Application.Features.BoardMembers.Dtos;
using Domain.Entities.FundManagement;

namespace Application.Mapping
{
    /// <summary>
    /// Mapping configurations for retrieving BoardMember entities
    /// Maps from domain entities to DTOs for read operations
    /// </summary>
    public partial class BoardMembersProfile
    {
        public void GetBoardMemberMapping()
        {
            // BoardMember entity to BoardMemberDto
            CreateMap<BoardMember, BoardMemberDto>()
                .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.LastUpdateDate, opt => opt.MapFrom(src => src.UpdatedAt ?? src.CreatedAt));

            // BoardMember entity to BoardMemberDetailsDto
            CreateMap<BoardMember, BoardMemberDetailsDto>()
                .IncludeBase<BoardMember, BoardMemberDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.FundName, opt => opt.MapFrom(src => src.Fund.Name))
                .ForMember(dest => dest.JoinedDate, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ConflictCount, opt => opt.MapFrom(src => src.ConflictItems.Count))
                .ForMember(dest => dest.VoteCount, opt => opt.MapFrom(src => 0)); // Will be calculated separately

            // User entity to BoardMemberUserDto
            CreateMap<Domain.Entities.Users.User, BoardMemberUserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                //.ForMember(dest => dest.JoinedDate, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.DeactivatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeactivationReason, opt => opt.Ignore());

            // BoardMember entity to BoardMemberResponse
            CreateMap<BoardMember, BoardMemberResponse>()
                .IncludeBase<BoardMember, BoardMemberDto>()
                .ForMember(dest => dest.Message, opt => opt.Ignore());

            // BoardMember entity to SingleBoardMemberResponse
            CreateMap<BoardMember, SingleBoardMemberResponse>()
                .IncludeBase<BoardMember, BoardMemberDto>()
                .ForMember(dest => dest.FundName, opt => opt.MapFrom(src => src.Fund.Name))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => src.UpdatedAt ?? src.CreatedAt));




        }
    }
}
