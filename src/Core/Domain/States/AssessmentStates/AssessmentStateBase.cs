using Domain.Entities.AssessmentManagement;
using Microsoft.Extensions.Localization;
using Resources;

namespace Domain.States.AssessmentStates
{
    /// <summary>
    /// Abstract base class for all assessment states
    /// Implements common functionality for the State Design Pattern
    /// Based on Fund state pattern implementation
    /// </summary>
    public abstract class AssessmentStateBase : IAssessmentState
    {
        /// <summary>
        /// Gets the status associated with this state
        /// </summary>
        public abstract AssessmentStatus Status { get; }

        /// <summary>
        /// Gets the allowed transition statuses from this state
        /// Must be implemented by concrete state classes
        /// </summary>
        protected abstract List<AssessmentStatus> AllowedTransitions { get; }

        /// <summary>
        /// Validates if the assessment can transition to the target status
        /// </summary>
        /// <param name="targetStatus">The target status to transition to</param>
        /// <returns>True if transition is allowed, false otherwise</returns>
        public virtual bool CanTransitionTo(AssessmentStatus targetStatus)
        {
            return AllowedTransitions.Contains(targetStatus);
        }

        /// <summary>
        /// Performs the transition to the target status
        /// </summary>
        /// <param name="targetStatus">The target status to transition to</param>
        /// <param name="reason">Reason for the transition</param>
        /// <returns>True if transition was successful, false otherwise</returns>
        public virtual bool TransitionTo(AssessmentStatus targetStatus, string reason)
        {
            if (!CanTransitionTo(targetStatus))
            {
                return false;
            }

            // Log the transition (in a real implementation, this would use a proper logger)
            var transitionMessage = GetLocalizedTransitionMessage(targetStatus, reason);
            
            return true;
        }

        /// <summary>
        /// Gets all allowed transition statuses from the current state
        /// </summary>
        /// <returns>List of allowed target statuses</returns>
        public virtual List<AssessmentStatus> GetAllowedTransitions()
        {
            return new List<AssessmentStatus>(AllowedTransitions);
        }

        /// <summary>
        /// Validates the current state business rules with localized messages
        /// Must be implemented by concrete state classes
        /// </summary>
        /// <param name="assessment">The assessment to validate</param>
        /// <param name="localizer">String localizer for localized messages</param>
        /// <returns>Validation result with success status and localized messages</returns>
        public abstract (bool IsValid, List<string> ValidationMessages) ValidateState(Assessment assessment, IStringLocalizer<SharedResources> localizer);

        /// <summary>
        /// Gets the available actions for the current state
        /// Must be implemented by concrete state classes
        /// </summary>
        /// <returns>List of available action enums</returns>
        public abstract List<AssessmentActionEnum> GetAvailableActions();

        /// <summary>
        /// Validates if the current state allows editing
        /// Default implementation - can be overridden by concrete states
        /// </summary>
        /// <returns>True if editing is allowed in this state</returns>
        public virtual bool CanEdit()
        {
            return Status == AssessmentStatus.Draft || Status == AssessmentStatus.Rejected;
        }

        /// <summary>
        /// Validates if the current state allows completion operations
        /// Default implementation - can be overridden by concrete states
        /// </summary>
        /// <returns>True if completion is allowed in this state</returns>
        public virtual bool CanComplete()
        {
            return Status == AssessmentStatus.Active;
        }

        /// <summary>
        /// Validates if the current state allows deletion
        /// Default implementation - can be overridden by concrete states
        /// </summary>
        /// <returns>True if deletion is allowed in this state</returns>
        public virtual bool CanDelete()
        {
            return Status == AssessmentStatus.Draft || Status == AssessmentStatus.Rejected;
        }

        /// <summary>
        /// Gets the state-specific business rules and validation messages
        /// Default implementation - can be overridden by concrete states
        /// </summary>
        /// <returns>Collection of validation messages for this state</returns>
        public virtual IEnumerable<string> GetValidationMessages()
        {
            return new List<string>();
        }

        /// <summary>
        /// Handles the state-specific logic and transitions
        /// Default implementation - can be overridden by concrete states
        /// </summary>
        /// <param name="assessment">The assessment entity to operate on</param>
        public virtual void Handle(Assessment assessment)
        {
            // Default implementation - no specific handling required
        }

        /// <summary>
        /// Gets the localized resource key for the current state description
        /// Default implementation - can be overridden by concrete states
        /// </summary>
        /// <returns>Resource key for state description</returns>
        public virtual string GetStateDescriptionKey()
        {
            return Status switch
            {
                AssessmentStatus.Draft => SharedResourcesKey.AssessmentStatusDraft,
                AssessmentStatus.WaitingForApproval => SharedResourcesKey.AssessmentStatusWaitingForApproval,
                AssessmentStatus.Approved => SharedResourcesKey.AssessmentStatusApproved,
                AssessmentStatus.Rejected => SharedResourcesKey.AssessmentStatusRejected,
                AssessmentStatus.Active => SharedResourcesKey.AssessmentStatusActive,
                AssessmentStatus.Completed => SharedResourcesKey.AssessmentStatusCompleted,
                _ => Status.ToString()
            };
        }

        /// <summary>
        /// Handles state-specific logic
        /// Default implementation does nothing, can be overridden by concrete states
        /// </summary>
        /// <param name="assessment">The assessment to handle</param>
        public virtual void Handle(Assessment assessment)
        {
            // Default implementation - can be overridden by concrete states
        }

        /// <summary>
        /// Gets localized transition message using proper localization
        /// </summary>
        /// <param name="targetStatus">Target status</param>
        /// <param name="reason">Transition reason</param>
        /// <param name="localizer">String localizer for localized messages</param>
        /// <returns>Localized transition message</returns>
        protected virtual string GetLocalizedTransitionMessage(AssessmentStatus targetStatus, string reason, IStringLocalizer<SharedResources> localizer)
        {
            var fromStatus = GetLocalizedStatusName(Status, localizer);
            var toStatus = GetLocalizedStatusName(targetStatus, localizer);

            return string.Format(localizer[SharedResourcesKey.AssessmentStatusTransitionMessage], fromStatus, toStatus, reason);
        }

        /// <summary>
        /// Gets localized status name using proper localization
        /// </summary>
        /// <param name="status">Assessment status</param>
        /// <param name="localizer">String localizer for localized messages</param>
        /// <returns>Localized status name</returns>
        protected virtual string GetLocalizedStatusName(AssessmentStatus status, IStringLocalizer<SharedResources> localizer)
        {
            return status switch
            {
                AssessmentStatus.Draft => localizer[SharedResourcesKey.AssessmentStatusDraft],
                AssessmentStatus.WaitingForApproval => localizer[SharedResourcesKey.AssessmentStatusWaitingForApproval],
                AssessmentStatus.Approved => localizer[SharedResourcesKey.AssessmentStatusApproved],
                AssessmentStatus.Rejected => localizer[SharedResourcesKey.AssessmentStatusRejected],
                AssessmentStatus.Active => localizer[SharedResourcesKey.AssessmentStatusActive],
                AssessmentStatus.Completed => localizer[SharedResourcesKey.AssessmentStatusCompleted],
                _ => status.ToString()
            };
        }
    }
}
