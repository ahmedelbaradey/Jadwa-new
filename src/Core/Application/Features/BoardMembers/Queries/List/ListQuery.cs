using Abstraction.Base.Dto;
using Abstraction.Base.Response;
using Abstraction.Common.Wappers;
using Application.Base.Abstracts;
using Application.Features.BoardMembers.Dtos;

namespace Application.Features.BoardMembers.Queries.List
{
    /// <summary>
    /// Query for retrieving board members for a specific fund
    /// Implements CQRS pattern using MediatR
    /// Based on requirements in Sprint.md (JDWA-595)
    /// </summary>
    public record ListQuery : BaseListDto, IQuery<PaginatedResult<BoardMemberDto>>
    {

         /// <summary>
        /// Type of board member (Independent or Not Independent)
        /// </summary>
    //    public BoardMemberType MemberType { get; set; }
        /// <summary>
        /// Fund identifier that this board member belongs to
        /// </summary>
        public int FundId { get; set; }
        /// <summary>
        /// Total count of board members
        /// </summary>
    //    public int TotalCount { get; set; }

        /// <summary>
        /// Count of independent members
        /// </summary>
   //     public int IndependentCount { get; set; }

        /// <summary>
        /// Count of non-independent members
        /// </summary>
   //     public int NonIndependentCount { get; set; }

        /// <summary>
        /// Indicates if fund has a chairman
        /// </summary>
   //     public bool HasChairman { get; set; }

        /// <summary>
        /// Fund name for context
        /// </summary>
     //   public string FundName { get; set; } = string.Empty;
    }
     
}
