using Application.Features.Identity.Users.Queries.Responses;
using Application.Base;
using Application.Base.Abstracts;
using Abstraction.Common.Wappers;
using Abstraction.Base.Dto;

namespace Application.Features.Identity.Users.Queries.List
{
    /// <summary>
    /// Enhanced query for user listing with advanced filtering capabilities
    /// Sprint 3 enhancement with role, status, and registration filtering
    /// </summary>
    public record ListQuery : BaseListDto, IQuery<PaginatedResult<GetUserListResponse>>
    {
        /// <summary>
        /// Filter by user role (optional)
        /// </summary>
        public string? Role { get; set; }

        /// <summary>
        /// Filter by user status (optional)
        /// true = active, false = inactive, null = all
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Advanced search fields (optional)
        /// Searches in name, email, phone, and other text fields
        /// </summary>
        public string? Name { get; set; }

      

    }
}
