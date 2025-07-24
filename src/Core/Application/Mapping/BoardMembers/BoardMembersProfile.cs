using AutoMapper;

namespace Application.Mapping
{
    /// <summary>
    /// AutoMapper profile for BoardMember entity mappings
    /// Contains all mapping configurations for BoardMember-related DTOs
    /// </summary>
    public partial class BoardMembersProfile : Profile
    {
        public BoardMembersProfile()
        {
            GetBoardMemberMapping();
            AddBoardMemberMapping();
            EditBoardMemberMapping();
        }
    }
}
