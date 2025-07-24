namespace Application.Features.Identity.Authorizations.Queries.Responses
{
    public record GetRoleListResponse
    {
        public int Id { get; set; }

        /// <summary>
        /// Original role name from database (for internal use)
        /// </summary>
        public string roleName { get; set; } = null!;

        /// <summary>
        /// Localized display name for the role (for UI display)
        /// </summary>
        public string DisplayName { get; set; } = null!;
    }
}
