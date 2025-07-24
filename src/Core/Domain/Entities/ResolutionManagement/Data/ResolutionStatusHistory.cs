using Domain.Entities.Base;
using Domain.Entities.FundManagement;
using Domain.Entities.Users;
using Domain.Services.Audit;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.ResolutionManagement
{
    /// <summary>
    /// Represents a resolution history entry for tracking detailed actions performed on resolutions
    /// Inherits from FullAuditedEntity to provide comprehensive audit trail functionality
    /// Based on requirements in Sprint.md for resolution history tracking (JDWA-570, JDWA-593, JDWA-589)
    /// </summary>
    public class ResolutionStatusHistory : FullAuditedEntity
    {
        /// <summary>
        /// Resolution identifier that this history entry belongs to
        /// Foreign key reference to Resolution entity
        /// </summary>
        public int ResolutionId { get; set; }
        public int ResolutionStatusId { get; set; }

        /// <summary>
        /// Rejection reason when resolution status is "Rejected"
        /// Required when fund manager rejects a resolution
        /// </summary>
        public string? RejectionReason { get; set; }

        /// <summary>
        ///  reason when resolution status changed
        /// Required when fund manager rejects a resolution
        /// </summary>
        public string? Reason { get; set; }

        /// <summary>
        /// Name of the action performed
        /// Examples: "resolution creation", "resolution edit", "resolution confirmed", 
        /// "resolution rejected", "resolution vote suspend", "resolution cancelation"
        /// </summary>
        public ResolutionActionEnum Action { get; set; }

        /// <summary>
        /// User identifier who performed the action
        /// Foreign key reference to User entity
        /// </summary>
        //public int UserId { get; set; }

        /// <summary>
        /// Role of the user when the action was performed
        /// Examples: "Fund Manager", "Legal Council", "Board Secretary", "Board Member"
        /// </summary>
        public string UserRole { get; set; } = string.Empty;

        /// <summary>
        /// Date and time when the action was performed
        /// Automatically set when history entry is created
        /// </summary>
        //public DateTime ActionDateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Previous status of the resolution before this action
        /// Used to track status transitions
        /// </summary>
        public ResolutionStatusEnum? PreviousStatus { get; set; }

        /// <summary>
        /// New status of the resolution after this action
        /// Used to track status transitions
        /// </summary>
        public ResolutionStatusEnum? NewStatus { get; set; }

        /// <summary>
        /// Localization key reference for the action performed
        /// Stores SharedResourcesKey constants (NOT translated text) following notification pattern
        /// Examples: "AuditActionResolutionCreation", "AuditActionResolutionConfirmation"
        /// Localization occurs on retrieval using IAuditLocalizationService
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Additional details about the action in JSON format
        /// Can store complex action-specific data
        /// </summary>
        public string? ActionDetails { get; set; }

        /// <summary>
        /// Navigation property to Resolution entity
        /// Provides access to the parent resolution
        /// </summary>
        [ForeignKey("ResolutionId")]
        public Resolution Resolution { get; set; } = null!;

        [ForeignKey("ResolutionStatusId")]
        public ResolutionStatus ResolutionStatus { get; set; }


        public string GetLocalizedStatus(IAuditLocalizationService localizationService)
        {
             return localizationService.GetLocalizedStatusName(NewStatus.Value, null);
        }

        public string GetLocalizedAction(IAuditLocalizationService localizationService)
        {
            return localizationService.GetLocalizedActionName(Action, null);
        }

        public string LocalizedRole(IAuditLocalizationService localizationService)
        {
            return localizationService.GetLocalizedRole(UserRole, null);
        }

         
        /// <summary>
        /// Navigation property to User entity
        /// Provides access to the user who performed the action
        /// </summary>
        //[ForeignKey("UserId")]
        //public  User User { get; set; } = null!;

        /// <summary>
        /// Gets a formatted description of the action (backward compatibility)
        /// For localized descriptions, use IAuditLocalizationService
        /// </summary>
        /// <returns>Human-readable action description</returns>
        //public string GetActionDescription()
        //{
        //    var description = Action.ToString();

        //    if (PreviousStatus.HasValue && NewStatus.HasValue)
        //    {
        //        description += $" (Status: {PreviousStatus} → {NewStatus})";
        //    }

        //    return description;
        //}

        ///// <summary>
        ///// Gets localized action description using the provided localization service
        ///// Uses the localization key stored in Notes field following the notification pattern
        ///// </summary>
        ///// <param name="localizationService">Audit localization service for retrieving translated text</param>
        ///// <param name="culture">Optional culture code (e.g., "ar-EG", "en-US")</param>
        ///// <returns>Localized action description</returns>
        //public string GetLocalizedActionDescription(IAuditLocalizationService localizationService, string? culture = null)
        //{
        //    return localizationService.GetLocalizedActionDescription(this, culture);
        //}

        ///// <summary>
        ///// Gets localized action description with user-specific language preferences
        ///// </summary>
        ///// <param name="localizationService">Audit localization service for retrieving translated text</param>
        ///// <param name="userId">User ID to determine language preference</param>
        ///// <returns>Localized action description</returns>
        //public async Task<string> GetLocalizedActionDescriptionAsync(IAuditLocalizationService localizationService, int? userId = null)
        //{
        //    return await localizationService.GetLocalizedActionDescriptionAsync(this, userId);
        //}

        ///// <summary>
        ///// Gets comprehensive localized description including action details and status transitions
        ///// Provides full context for audit trail display in user's preferred language
        ///// </summary>
        ///// <param name="localizationService">Audit localization service for retrieving translated text</param>
        ///// <param name="culture">Optional culture code (e.g., "ar-EG", "en-US")</param>
        ///// <returns>Comprehensive localized description</returns>
        //public string GetLocalizedComprehensiveDescription(IAuditLocalizationService localizationService, string? culture = null)
        //{
        //    return localizationService.GetLocalizedComprehensiveDescription(this, culture);
        //}

        ///// <summary>
        ///// Gets localized status transition description
        ///// Formats status changes in user's preferred language
        ///// </summary>
        ///// <param name="localizationService">Audit localization service for retrieving translated text</param>
        ///// <param name="culture">Optional culture code (e.g., "ar-EG", "en-US")</param>
        ///// <returns>Localized status transition description</returns>
        //public string GetLocalizedStatusTransition(IAuditLocalizationService localizationService, string? culture = null)
        //{
        //    return localizationService.GetLocalizedStatusTransition(PreviousStatus, NewStatus, culture);
        //}

        /// <summary>
        /// Checks if this action represents a status change
        /// </summary>
        /// <returns>True if status changed, false otherwise</returns>
        public bool IsStatusChange()
        {
            return PreviousStatus.HasValue && NewStatus.HasValue && PreviousStatus != NewStatus;
        }

 
    }
}
