using Abstraction.Base.Dto;
using Domain.Entities.ResolutionManagement;

namespace Application.Features.Resolutions.Dtos
{
    /// <summary>
    /// Single resolution response DTO following Clean DTOs template patterns
    /// Used for single entity API responses with display properties
    /// Based on requirements in Sprint.md for resolution display
    /// </summary>
    public record SingleResolutionResponse : BaseDto
    {
        /// <summary>
        /// Fund name for display
        /// </summary>
        public int CreatorID { get; set; }
        /// <summary>
        /// Fund name for display
        /// </summary>
        public string FundName { get; set; } = string.Empty;

        /// <summary>
        /// Auto-generated resolution code (fund code/resolution year/resolution no.)
        /// </summary>
        public string Code { get; set; } = string.Empty;
        /// <summary>
        /// Date of the resolution
        /// </summary>
        public DateTime ResolutionDate { get; set; }

        /// <summary>
        /// Description of the resolution (optional, max 500 characters)
        /// </summary>
        public string? Description { get; set; } = null;
        /// <summary>
        /// Current status of the resolution
        /// </summary>
        public ResolutionStatusEnum Status { get; set; }
        /// <summary>
        /// Resolution type information for display
        /// </summary>
        public ResolutionTypeDto? ResolutionType { get; set; }
        ///// <summary>
        ///// Resolution type identifier
        ///// </summary>
        //public int ResolutionTypeId { get; set; }

        /// <summary>
        /// Last update date for sorting and display
        /// </summary>
        public DateTime? LastUpdated { get; set; }

        /// <summary>
        /// Resolution status identifier
        /// Numeric value of the resolution status enum
        /// </summary>
        public int StatusId { get; set; }

        ///// <summary>
        ///// Localized display text for resolution status
        ///// </summary>
        //public string StatusDisplay { get; set; } = string.Empty;

        /// <summary>
        /// Resolution status information with localization
        /// </summary>
        public ResolutionStatusDto? ResolutionStatus { get; set; }

        // Role-based action availability
        /// <summary>
        /// Indicates if current user can confirm the resolution
        /// </summary>
        public bool CanConfirm { get; set; }

        /// <summary>
        /// Indicates if current user can reject the resolution
        /// </summary>
        public bool CanReject { get; set; }

        /// <summary>
        /// Indicates if current user can edit the resolution
        /// </summary>
        public bool CanEdit { get; set; }

        /// <summary>
        /// Indicates if current user can cancel the resolution
        /// </summary>
        public bool CanCancel { get; set; }

        /// <summary>
        /// Indicates if current user can delete the resolution
        /// </summary>
        public bool CanDelete { get; set; }
        public bool CanView { get; set; }







    }
}
