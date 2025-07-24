namespace Application.Features.Identity.Users.Queries.Responses
{
    /// <summary>
    /// Response DTO for single-holder role availability status
    /// Provides boolean flags indicating whether each single-holder role has an active user assigned
    /// Used by frontend for role assignment workflows and UI decision-making
    /// </summary>
    public class SingleHolderRoleAvailabilityResponse
    {
        /// <summary>
        /// True if there is an active user assigned to the Legal Counsel role
        /// </summary>
        public bool LegalCouncilHasActiveUser { get; set; }

        /// <summary>
        /// True if there is an active user assigned to the Finance Controller role
        /// </summary>
        public bool FinanceControllerHasActiveUser { get; set; }

        /// <summary>
        /// True if there is an active user assigned to the Compliance Legal Managing Director role
        /// </summary>
        public bool ComplianceLegalManagingDirectorHasActiveUser { get; set; }

        /// <summary>
        /// True if there is an active user assigned to the Head of Real Estate role
        /// </summary>
        public bool HeadOfRealEstateHasActiveUser { get; set; }


    }
}
