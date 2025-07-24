using Domain.Entities.ResolutionManagement;

namespace Domain.Services.Audit
{
    /// <summary>
    /// Service interface for localizing audit history entries
    /// Follows the established notification pattern for multilingual support
    /// Provides localized descriptions based on stored localization keys
    /// </summary>
    public interface IAuditLocalizationService
    {
        /// <summary>
        /// Gets localized action description for a resolution status history entry
        /// Uses the localization key stored in the Notes field to retrieve translated text
        /// </summary>
        /// <param name="historyEntry">The resolution status history entry</param>
        /// <param name="culture">Optional culture code (e.g., "ar-EG", "en-US"). If null, uses current culture</param>
        /// <returns>Localized action description</returns>
        string GetLocalizedActionDescription(ResolutionStatusHistory historyEntry, string? culture = null);

        /// <summary>
        /// Gets localized action description for a resolution status history entry with user context
        /// Includes user-specific language preferences if available
        /// </summary>
        /// <param name="historyEntry">The resolution status history entry</param>
        /// <param name="userId">User ID to determine language preference</param>
        /// <returns>Localized action description</returns>
        Task<string> GetLocalizedActionDescriptionAsync(ResolutionStatusHistory historyEntry, int? userId = null);

        /// <summary>
        /// Gets localized comprehensive description including action details and status transitions
        /// Provides full context for audit trail display
        /// </summary>
        /// <param name="historyEntry">The resolution status history entry</param>
        /// <param name="culture">Optional culture code. If null, uses current culture</param>
        /// <returns>Comprehensive localized description</returns>
        string GetLocalizedComprehensiveDescription(ResolutionStatusHistory historyEntry, string? culture = null);

        /// <summary>
        /// Gets localized status transition description
        /// Formats status changes in user's preferred language
        /// </summary>
        /// <param name="previousStatus">Previous resolution status</param>
        /// <param name="newStatus">New resolution status</param>
        /// <param name="culture">Optional culture code. If null, uses current culture</param>
        /// <returns>Localized status transition description</returns>
        string GetLocalizedStatusTransition(ResolutionStatusEnum? previousStatus, ResolutionStatusEnum? newStatus, string? culture = null);



        /// <summary>
        /// Gets localized action   
        /// </summary>
        /// <returns>Localized action  description</returns>
        string GetLocalizedActionName(ResolutionActionEnum status, string? culture = null);
        string GetLocalizedRole(string role, string? culture = null);
        string GetLocalizedStatusName(ResolutionStatusEnum status, string? culture = null);


    }
}
