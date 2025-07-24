using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ResolutionManagement.State
{
    /// <summary>
    /// Context class for managing resolution state transitions
    /// Implements the State Pattern context for resolution lifecycle management
    /// Based on Sprint.md requirements and Clean Architecture principles
    /// </summary>
    public class ResolutionStateContext
    {
        private IResolutionState _currentState;
        private readonly Resolution _resolution;

        /// <summary>
        /// Initializes a new instance of ResolutionStateContext
        /// </summary>
        /// <param name="resolution">The resolution entity to manage</param>
        public ResolutionStateContext(Resolution resolution)
        {
            _resolution = resolution ?? throw new ArgumentNullException(nameof(resolution));
            _currentState = ResolutionStateFactory.CreateState(resolution.Status);
        }

        /// <summary>
        /// Gets the current state
        /// </summary>
        public IResolutionState CurrentState => _currentState;

        /// <summary>
        /// Transitions to a new state with validation
        /// </summary>
        /// <param name="targetStatus">Target status to transition to</param>
        /// <param name="reason">Reason for the transition (for audit trail)</param>
        /// <returns>True if transition was successful, false if not allowed</returns>
        public bool TransitionTo(ResolutionStatusEnum targetStatus, ResolutionActionEnum action, string reason = "")
        {
            if (!_currentState.CanTransitionTo(targetStatus))
            {
                return false;
            }

            // Update the resolution status
            _resolution.Status = targetStatus;

            // Create new state instance
            _currentState = ResolutionStateFactory.CreateState(targetStatus);

            // Add status history entry if needed
            AddStatusHistoryEntry(targetStatus, action,reason);

            return true;
        }

        /// <summary>
        /// Validates if a transition is allowed
        /// </summary>
        /// <param name="targetStatus">Target status to validate</param>
        /// <returns>True if transition is allowed</returns>
        public bool CanTransitionTo(ResolutionStatusEnum targetStatus)
        {
            return _currentState.CanTransitionTo(targetStatus);
        }

        /// <summary>
        /// Gets all allowed transitions from current state
        /// </summary>
        /// <returns>Collection of allowed target statuses</returns>
        public IEnumerable<ResolutionStatusEnum> GetAllowedTransitions()
        {
            return _currentState.GetAllowedTransitions();
        }

        /// <summary>
        /// Validates if editing is allowed in current state
        /// </summary>
        /// <returns>True if editing is allowed</returns>
        public bool CanEdit()
        {
            return _currentState.CanEdit();
        }

        /// <summary>
        /// Validates if completion operations are allowed in current state
        /// </summary>
        /// <returns>True if completion is allowed</returns>
        public bool CanComplete()
        {
            return _currentState.CanComplete();
        }

        /// <summary>
        /// Validates if cancellation is allowed in current state
        /// </summary>
        /// <returns>True if cancellation is allowed</returns>
        public bool CanCancel()
        {
            return _currentState.CanCancel();
        }

        /// <summary>
        /// Gets validation messages for current state
        /// </summary>
        /// <returns>Collection of validation messages</returns>
        public IEnumerable<string> GetValidationMessages()
        {
            return _currentState.GetValidationMessages();
        }

        /// <summary>
        /// Handles state-specific logic
        /// </summary>
        public void Handle()
        {
            _currentState.Handle(_resolution);
        }

        /// <summary>
        /// Transitions to a new state with comprehensive audit logging
        /// </summary>
        /// <param name="targetStatus">Target status to transition to</param>
        /// <param name="action">Action being performed</param>
        /// <param name="reason">Reason for the transition</param>
        /// <param name="localizedActionName">Localized action name for audit trail</param>
        /// <param name="userId">User ID performing the action</param>
        /// <param name="userRole">User role performing the action</param>
        /// <param name="actionDetails">Additional action details</param>
        /// <returns>True if transition was successful, false if not allowed</returns>
        public bool TransitionToWithAudit(ResolutionStatusEnum targetStatus, ResolutionActionEnum action,
            string reason, string localizedActionName, int userId, string userRole, string actionDetails = "",string rejectionReason = "")
        {
            if (!_currentState.CanTransitionTo(targetStatus))
            {
                return false;
            }

            var previousStatus = _resolution.Status;

            // Update the resolution status
            _resolution.Status = targetStatus;

            // Create new state instance
            _currentState = ResolutionStateFactory.CreateState(targetStatus);

            // Add comprehensive audit entry
            AddStatusHistoryEntryWithAudit(targetStatus, action, reason, localizedActionName,
                userId, userRole, actionDetails, previousStatus, targetStatus, rejectionReason);

            return true;
        }

        /// <summary>
        /// Adds an audit entry for non-status-changing actions
        /// </summary>
        /// <param name="action">Action being performed</param>
        /// <param name="reason">Reason for the action</param>
        /// <param name="localizedActionName">Localized action name for audit trail</param>
        /// <param name="userId">User ID performing the action</param>
        /// <param name="userRole">User role performing the action</param>
        /// <param name="actionDetails">Additional action details</param>
        public void AddAuditEntry(ResolutionActionEnum action, string reason, string localizedActionName,
            int userId, string userRole, string actionDetails = "")
        {
            // For non-status-changing actions, set both previousStatus and newStatus to current status
            // This ensures that status information is always available in audit history
            var currentStatus = _resolution.Status;
            AddStatusHistoryEntryWithAudit(currentStatus, action, reason, localizedActionName,
                userId, userRole, actionDetails, currentStatus, currentStatus);
        }

        /// <summary>
        /// Adds a comprehensive status history entry for audit trail with all required fields
        /// Ensures complete audit logging following the notification pattern for localization
        /// </summary>
        /// <param name="status">New status</param>
        /// <param name="action">Action being performed</param>
        /// <param name="reason">Reason for the change</param>
        /// <param name="localizationKey">Localization key reference (NOT translated text) for retrieval localization</param>
        /// <param name="userId">User ID performing the action</param>
        /// <param name="userRole">User role performing the action</param>
        /// <param name="actionDetails">Comprehensive description of the operation performed</param>
        /// <param name="previousStatus">Previous status (for status changes, null for non-status actions)</param>
        /// <param name="newStatus">New status (for status changes, null for non-status actions)</param>
        private void AddStatusHistoryEntryWithAudit(ResolutionStatusEnum status, ResolutionActionEnum action,
            string reason, string localizationKey, int userId, string userRole, string actionDetails,
            ResolutionStatusEnum? previousStatus, ResolutionStatusEnum? newStatus, string rejectionReason = "")
        {
            // Initialize collection if null
            _resolution.ResolutionStatusHistories ??= new List<ResolutionStatusHistory>();

            // Create comprehensive status history entry with all required audit fields
            var statusHistory = new ResolutionStatusHistory
            {
                // Core resolution reference
                ResolutionId = _resolution.Id,
                ResolutionStatusId = (int)status,

                // Action information
                Action = action,
                Reason = reason,
                RejectionReason = rejectionReason,

                // Comprehensive action details - detailed description of the operation
                ActionDetails = actionDetails,

                // Localization key reference (NOT translated text) following notification pattern
                // This allows proper localization on retrieval for multilingual support
                Notes = localizationKey,

                // User context information
                UserRole = userRole,
                CreatedBy = userId,

                // Status transition information (for status changes)
                PreviousStatus = previousStatus,
                NewStatus = newStatus,

                // Timestamp (automatically set by FullAuditedEntity)
                CreatedAt = DateTime.Now
            };

            _resolution.ResolutionStatusHistories.Add(statusHistory);
        }

        /// <summary>
        /// Adds a status history entry for audit trail (legacy method for backward compatibility)
        /// </summary>
        /// <param name="status">New status</param>
        /// <param name="action">Action being performed</param>
        /// <param name="reason">Reason for the change</param>
        private void AddStatusHistoryEntry(ResolutionStatusEnum status, ResolutionActionEnum action, string reason)
        {
            // Initialize collection if null
            _resolution.ResolutionStatusHistories ??= new List<ResolutionStatusHistory>();

            // Add basic status history entry for backward compatibility
            var statusHistory = new ResolutionStatusHistory
            {
                ResolutionId = _resolution.Id,
                ResolutionStatusId = (int)status,
                Reason = reason,
                Action = action
            };

            _resolution.ResolutionStatusHistories.Add(statusHistory);
        }

        /// <summary>
        /// Initializes state from current resolution status
        /// Should be called after loading from database
        /// </summary>
        public void InitializeState()
        {
            _currentState = ResolutionStateFactory.CreateState(_resolution.Status);
        }
    }
}
