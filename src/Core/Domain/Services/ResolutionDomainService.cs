using Domain.Entities.ResolutionManagement;

namespace Domain.Services
{
    /// <summary>
    /// Domain service for resolution business rules and operations
    /// Encapsulates complex business logic related to resolution management
    /// Based on requirements in Sprint.md for resolution business rules
    /// </summary>
    public class ResolutionDomainService
    {
        /// <summary>
        /// Maximum number of attachments allowed per resolution
        /// Business rule from Sprint.md (JDWA-505, JDWA-568)
        /// </summary>
        public const int MaxAttachmentsPerResolution = 10;

        /// <summary>
        /// Maximum file size allowed per attachment in bytes (10MB)
        /// Business rule from Sprint.md
        /// </summary>
        public const long MaxAttachmentSizeBytes = 10 * 1024 * 1024;
        /// <summary>
        /// Generates a new resolution code based on fund code and existing resolutions
        /// Format: fund code/resolution year/resolution no.
        /// </summary>
        /// <param name="fundCode">The fund code</param>
        /// <param name="existingResolutions">Existing resolutions for the fund in the current year</param>
        /// <param name="year">The year for the resolution (optional, defaults to current year)</param>
        /// <returns>A new ResolutionCode</returns>
        public static ResolutionCode GenerateResolutionCode(string fundCode, IEnumerable<Resolution> existingResolutions, int? year = null)
        {
            var resolutionYear = year ?? DateTime.Now.Year;
            
            // Get the highest sequential number for this fund and year
            var maxSequentialNumber = existingResolutions
                .Where(r => r.Code.Contains($"/{resolutionYear}/"))
                .Select(r => ExtractSequentialNumber(r.Code))
                .DefaultIfEmpty(0)
                .Max();

            var nextSequentialNumber = maxSequentialNumber + 1;

            return ResolutionCode.Create(fundCode, resolutionYear, nextSequentialNumber);
        }

        /// <summary>
        /// Extracts the sequential number from a resolution code
        /// </summary>
        /// <param name="resolutionCode">The resolution code to parse</param>
        /// <returns>The sequential number</returns>
        private static int ExtractSequentialNumber(string resolutionCode)
        {
            try
            {
                var parts = resolutionCode.Split('/');
                if (parts.Length == 3 && int.TryParse(parts[2], out int number))
                {
                    return number;
                }
            }
            catch
            {
                // If parsing fails, return 0
            }
            return 0;
        }

        /// <summary>
        /// Validates if a resolution can transition to a new status
        /// </summary>
        /// <param name="currentStatus">Current resolution status</param>
        /// <param name="newStatus">Desired new status</param>
        /// <returns>True if transition is allowed, false otherwise</returns>
        public static bool CanTransitionToStatus(ResolutionStatusEnum currentStatus, ResolutionStatusEnum newStatus)
        {
            return currentStatus switch
            {
                ResolutionStatusEnum.Draft => newStatus is ResolutionStatusEnum.Pending or ResolutionStatusEnum.Draft,
                ResolutionStatusEnum.Pending => newStatus is ResolutionStatusEnum.CompletingData or ResolutionStatusEnum.Cancelled or ResolutionStatusEnum.Draft,
                ResolutionStatusEnum.CompletingData => newStatus is ResolutionStatusEnum.WaitingForConfirmation or ResolutionStatusEnum.CompletingData,
                ResolutionStatusEnum.WaitingForConfirmation => newStatus is ResolutionStatusEnum.Confirmed or ResolutionStatusEnum.Rejected or ResolutionStatusEnum.WaitingForConfirmation,
                ResolutionStatusEnum.Confirmed => newStatus is ResolutionStatusEnum.VotingInProgress or ResolutionStatusEnum.WaitingForConfirmation,
                ResolutionStatusEnum.Rejected => newStatus is ResolutionStatusEnum.WaitingForConfirmation,
                ResolutionStatusEnum.VotingInProgress => newStatus is ResolutionStatusEnum.Approved or ResolutionStatusEnum.NotApproved or ResolutionStatusEnum.WaitingForConfirmation,
                ResolutionStatusEnum.Approved => false, // Final status - no transitions allowed
                ResolutionStatusEnum.NotApproved => false, // Final status - no transitions allowed
                ResolutionStatusEnum.Cancelled => false, // Final status - no transitions allowed
                _ => false
            };
        }

        /// <summary>
        /// Checks if a resolution can be edited based on its current status
        /// </summary>
        /// <param name="status">Current resolution status</param>
        /// <returns>True if resolution can be edited, false otherwise</returns>
        public static bool CanEditResolution(ResolutionStatusEnum status)
        {
            return status is ResolutionStatusEnum.Draft or 
                           ResolutionStatusEnum.Pending or 
                           ResolutionStatusEnum.CompletingData or 
                           ResolutionStatusEnum.WaitingForConfirmation or 
                           ResolutionStatusEnum.Confirmed or 
                           ResolutionStatusEnum.Rejected or 
                           ResolutionStatusEnum.VotingInProgress;
        }

        /// <summary>
        /// Checks if a resolution can be deleted based on its current status
        /// </summary>
        /// <param name="status">Current resolution status</param>
        /// <returns>True if resolution can be deleted, false otherwise</returns>
        public static bool CanDeleteResolution(ResolutionStatusEnum status)
        {
            return status == ResolutionStatusEnum.Draft;
        }

        /// <summary>
        /// Checks if a resolution can be cancelled based on its current status
        /// </summary>
        /// <param name="status">Current resolution status</param>
        /// <returns>True if resolution can be cancelled, false otherwise</returns>
        public static bool CanCancelResolution(ResolutionStatusEnum status)
        {
            return status == ResolutionStatusEnum.Pending;
        }

        /// <summary>
        /// Validates if resolution date is within allowed range
        /// </summary>
        /// <param name="resolutionDate">The proposed resolution date</param>
        /// <param name="fundInitiationDate">Fund initiation date</param>
        /// <returns>True if date is valid, false otherwise</returns>
        public static bool IsValidResolutionDate(DateTime resolutionDate, DateTime fundInitiationDate)
        {
            return resolutionDate >= fundInitiationDate.Date && resolutionDate <= DateTime.Today;
        }

        /// <summary>
        /// Checks if editing a resolution in voting status will require vote suspension
        /// </summary>
        /// <param name="status">Current resolution status</param>
        /// <returns>True if vote suspension is required, false otherwise</returns>
        public static bool RequiresVoteSuspension(ResolutionStatusEnum status)
        {
            return status == ResolutionStatusEnum.VotingInProgress;
        }

        /// <summary>
        /// Checks if a resolution can start voting
        /// </summary>
        /// <param name="status">Current resolution status</param>
        /// <returns>True if voting can start, false otherwise</returns>
        public static bool CanStartVoting(ResolutionStatusEnum status)
        {
            return status == ResolutionStatusEnum.Confirmed;
        }

        /// <summary>
        /// Determines the final status based on voting results
        /// </summary>
        /// <param name="isApproved">Whether the resolution was approved by voting</param>
        /// <returns>Final resolution status</returns>
        public static ResolutionStatusEnum GetFinalStatusFromVoting(bool isApproved)
        {
            return isApproved ? ResolutionStatusEnum.Approved : ResolutionStatusEnum.NotApproved;
        }

        /// <summary>
        /// Checks if editing an approved/not approved resolution will create a new resolution
        /// </summary>
        /// <param name="status">Current resolution status</param>
        /// <returns>True if a new resolution will be created, false otherwise</returns>
        public static bool WillCreateNewResolution(ResolutionStatusEnum status)
        {
            return status is ResolutionStatusEnum.Approved or ResolutionStatusEnum.NotApproved;
        }

        /// <summary>
        /// Validates if a new attachment can be added to a resolution
        /// </summary>
        /// <param name="currentAttachmentCount">Current number of attachments</param>
        /// <param name="fileSizeBytes">Size of the new file in bytes</param>
        /// <returns>Validation result with success status and error message</returns>
        public static (bool IsValid, string ErrorMessage) ValidateNewAttachment(int currentAttachmentCount, long fileSizeBytes)
        {
            if (currentAttachmentCount >= MaxAttachmentsPerResolution)
            {
                return (false, $"Maximum number of attachments ({MaxAttachmentsPerResolution}) has been reached for this resolution.");
            }

            if (fileSizeBytes > MaxAttachmentSizeBytes)
            {
                return (false, $"File size exceeds the maximum allowed size of {MaxAttachmentSizeBytes / (1024 * 1024)}MB.");
            }

            return (true, string.Empty);
        }

        /// <summary>
        /// Validates if a custom type name is required for a resolution
        /// </summary>
        /// <param name="resolutionTypeId">Resolution type ID</param>
        /// <param name="customTypeName">Custom type name provided</param>
        /// <returns>True if validation passes, false otherwise</returns>
        public static bool ValidateCustomTypeName(int resolutionTypeId, string? customTypeName)
        {
            if (ResolutionTypeSeedData.IsOtherType(resolutionTypeId))
            {
                return !string.IsNullOrWhiteSpace(customTypeName);
            }
            return true; // Custom type name not required for predefined types
        }

        /// <summary>
        /// Validates if a rejection reason is provided when rejecting a resolution
        /// </summary>
        /// <param name="rejectionReason">Rejection reason provided</param>
        /// <returns>True if rejection reason is valid, false otherwise</returns>
        public static bool ValidateRejectionReason(string? rejectionReason)
        {
            return !string.IsNullOrWhiteSpace(rejectionReason);
        }

        /// <summary>
        /// Creates a resolution history entry for an action
        /// </summary>
        /// <param name="resolutionId">Resolution ID</param>
        /// <param name="actionName">Name of the action</param>
        /// <param name="userId">User who performed the action</param>
        /// <param name="userRole">Role of the user</param>
        /// <param name="previousStatus">Previous resolution status</param>
        /// <param name="newStatus">New resolution status</param>
        /// <param name="notes">Optional notes about the action</param>
        /// <returns>New ResolutionHistory entity</returns>
        public static ResolutionStatusHistory CreateHistoryEntry(
            int resolutionId,
            string actionName,
            int userId,
            string userRole,
            ResolutionStatusEnum? previousStatus = null,
            ResolutionStatusEnum? newStatus = null,
            ResolutionActionEnum? action = null,
            string? notes = null)
        {
            return new ResolutionStatusHistory
            {
                ResolutionId = resolutionId,
                Action = action.Value,
                CreatedBy = userId,
                //UserRole = userRole,
                PreviousStatus = previousStatus,
                NewStatus = newStatus,
                Notes = notes,
                CreatedAt = DateTime.Now
            };
        }
    }
}
