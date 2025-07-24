using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ResolutionManagement.State
{
    /// <summary>
    /// Factory class for creating resolution state instances
    /// Implements the Factory pattern for state management
    /// Based on Sprint.md requirements and Clean Architecture principles
    /// </summary>
    public static class ResolutionStateFactory
    {
        /// <summary>
        /// Creates a resolution state instance based on the status enum
        /// </summary>
        /// <param name="status">The resolution status enum value</param>
        /// <returns>Appropriate IResolutionState implementation</returns>
        /// <exception cref="ArgumentException">Thrown when status is not supported</exception>
        public static IResolutionState CreateState(ResolutionStatusEnum status)
        {
            return status switch
            {
                ResolutionStatusEnum.Draft => new DraftResolutionState(),
                ResolutionStatusEnum.Pending => new PendingResolutionState(),
                ResolutionStatusEnum.CompletingData => new CompletingDataResolutionState(),
                ResolutionStatusEnum.WaitingForConfirmation => new WaitingForConfirmationResolutionState(),
                ResolutionStatusEnum.Confirmed => new ConfirmedResolutionState(),
                ResolutionStatusEnum.Rejected => new RejectedResolutionState(),
                ResolutionStatusEnum.VotingInProgress => new VotingInProgressResolutionState(),
                ResolutionStatusEnum.Approved => new ApprovedResolutionState(),
                ResolutionStatusEnum.NotApproved => new NotApprovedResolutionState(),
                ResolutionStatusEnum.Cancelled => new CancelledResolutionState(),
                _ => throw new ArgumentException($"Unsupported resolution status: {status}", nameof(status))
            };
        }

        /// <summary>
        /// Gets the default initial state for a new resolution
        /// </summary>
        /// <param name="saveAsDraft">Whether the resolution is being saved as draft</param>
        /// <returns>Initial resolution state (Draft or Pending)</returns>
        public static IResolutionState GetInitialState(bool saveAsDraft = true)
        {
            return saveAsDraft ? new DraftResolutionState() : new PendingResolutionState();
        }

        /// <summary>
        /// Validates if a state transition is allowed
        /// </summary>
        /// <param name="currentStatus">Current resolution status</param>
        /// <param name="targetStatus">Target resolution status</param>
        /// <returns>True if transition is allowed, false otherwise</returns>
        public static bool IsTransitionAllowed(ResolutionStatusEnum currentStatus, ResolutionStatusEnum targetStatus)
        {
            var currentState = CreateState(currentStatus);
            return currentState.CanTransitionTo(targetStatus);
        }

        /// <summary>
        /// Gets all allowed transitions from a given status
        /// </summary>
        /// <param name="currentStatus">Current resolution status</param>
        /// <returns>Collection of allowed target statuses</returns>
        public static IEnumerable<ResolutionStatusEnum> GetAllowedTransitions(ResolutionStatusEnum currentStatus)
        {
            var currentState = CreateState(currentStatus);
            return currentState.GetAllowedTransitions();
        }

        /// <summary>
        /// Validates if editing is allowed for the current status
        /// </summary>
        /// <param name="currentStatus">Current resolution status</param>
        /// <returns>True if editing is allowed</returns>
        public static bool CanEdit(ResolutionStatusEnum currentStatus)
        {
            var currentState = CreateState(currentStatus);
            return currentState.CanEdit();
        }

        /// <summary>
        /// Validates if completion operations are allowed for the current status
        /// </summary>
        /// <param name="currentStatus">Current resolution status</param>
        /// <returns>True if completion is allowed</returns>
        public static bool CanComplete(ResolutionStatusEnum currentStatus)
        {
            var currentState = CreateState(currentStatus);
            return currentState.CanComplete();
        }

        /// <summary>
        /// Validates if cancellation is allowed for the current status
        /// </summary>
        /// <param name="currentStatus">Current resolution status</param>
        /// <returns>True if cancellation is allowed</returns>
        public static bool CanCancel(ResolutionStatusEnum currentStatus)
        {
            var currentState = CreateState(currentStatus);
            return currentState.CanCancel();
        }
    }
}
