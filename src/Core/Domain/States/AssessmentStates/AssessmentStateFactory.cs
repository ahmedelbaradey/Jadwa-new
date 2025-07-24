using Domain.Entities.AssessmentManagement;
using Microsoft.Extensions.Localization;
using Resources;

namespace Domain.States.AssessmentStates
{
    /// <summary>
    /// Factory class for creating assessment state instances
    /// Implements the Factory pattern for state management
    /// Follows the exact structure and patterns of ResolutionStateFactory
    /// </summary>
    public static class AssessmentStateFactory
    {
        /// <summary>
        /// Creates an assessment state instance based on the status enum
        /// </summary>
        /// <param name="status">The assessment status enum value</param>
        /// <returns>Appropriate IAssessmentState implementation</returns>
        /// <exception cref="ArgumentException">Thrown when status is not supported</exception>
        public static IAssessmentState CreateState(AssessmentStatus status)
        {
            return status switch
            {
                AssessmentStatus.Draft => new DraftState(),
                AssessmentStatus.WaitingForApproval => new WaitingForApprovalState(),
                AssessmentStatus.Approved => new ApprovedState(),
                AssessmentStatus.Rejected => new RejectedState(),
                AssessmentStatus.Active => new ActiveState(),
                AssessmentStatus.Completed => new CompletedState(),
                _ => throw new ArgumentException($"Unsupported assessment status: {status}", nameof(status))
            };
        }

        /// <summary>
        /// Gets the default initial state for a new assessment
        /// </summary>
        /// <param name="saveAsDraft">Whether the assessment is being saved as draft</param>
        /// <returns>Initial assessment state (Draft)</returns>
        public static IAssessmentState GetInitialState(bool saveAsDraft = true)
        {
            return new DraftState();
        }

        /// <summary>
        /// Validates if a transition from current status to target status is allowed
        /// </summary>
        /// <param name="currentStatus">Current assessment status</param>
        /// <param name="targetStatus">Target assessment status</param>
        /// <returns>True if transition is allowed</returns>
        public static bool CanTransitionTo(AssessmentStatus currentStatus, AssessmentStatus targetStatus)
        {
            var currentState = CreateState(currentStatus);
            return currentState.CanTransitionTo(targetStatus);
        }

        /// <summary>
        /// Gets all allowed transitions from a given status
        /// </summary>
        /// <param name="currentStatus">Current assessment status</param>
        /// <returns>Collection of allowed target statuses</returns>
        public static IEnumerable<AssessmentStatus> GetAllowedTransitions(AssessmentStatus currentStatus)
        {
            var currentState = CreateState(currentStatus);
            return currentState.GetAllowedTransitions();
        }

        /// <summary>
        /// Gets available actions for a given status
        /// </summary>
        /// <param name="status">Current status</param>
        /// <returns>List of available action enums</returns>
        public static List<AssessmentActionEnum> GetAvailableActions(AssessmentStatus status)
        {
            var currentState = CreateState(status);
            return currentState.GetAvailableActions();
        }

        /// <summary>
        /// Validates an assessment against its current state business rules
        /// </summary>
        /// <param name="assessment">The assessment to validate</param>
        /// <param name="localizer">String localizer for localized messages</param>
        /// <returns>Validation result with success status and messages</returns>
        public static (bool IsValid, List<string> ValidationMessages) ValidateAssessment(Assessment assessment, IStringLocalizer<SharedResources> localizer)
        {
            var currentState = CreateState(assessment.Status);
            return currentState.ValidateState(assessment, localizer);
        }

        /// <summary>
        /// Checks if a status represents a terminal state (no further transitions)
        /// </summary>
        /// <param name="status">Status to check</param>
        /// <returns>True if terminal state, false otherwise</returns>
        public static bool IsTerminalState(AssessmentStatus status)
        {
            var allowedTransitions = GetAllowedTransitions(status);
            return !allowedTransitions.Any();
        }

        /// <summary>
        /// Validates if editing is allowed for the current status
        /// </summary>
        /// <param name="currentStatus">Current assessment status</param>
        /// <returns>True if editing is allowed</returns>
        public static bool CanEdit(AssessmentStatus currentStatus)
        {
            return currentStatus switch
            {
                AssessmentStatus.Draft => true,
                AssessmentStatus.Rejected => true,
                _ => false
            };
        }

        /// <summary>
        /// Validates if completion operations are allowed for the current status
        /// </summary>
        /// <param name="currentStatus">Current assessment status</param>
        /// <returns>True if completion is allowed</returns>
        public static bool CanComplete(AssessmentStatus currentStatus)
        {
            return currentStatus == AssessmentStatus.Active;
        }

        /// <summary>
        /// Validates if deletion is allowed for the current status
        /// </summary>
        /// <param name="currentStatus">Current assessment status</param>
        /// <returns>True if deletion is allowed</returns>
        public static bool CanDelete(AssessmentStatus currentStatus)
        {
            return currentStatus switch
            {
                AssessmentStatus.Draft => true,
                AssessmentStatus.Rejected => true,
                _ => false
            };
        }

        /// <summary>
        /// Validates if approval operations are allowed for the current status
        /// </summary>
        /// <param name="currentStatus">Current assessment status</param>
        /// <returns>True if approval is allowed</returns>
        public static bool CanApprove(AssessmentStatus currentStatus)
        {
            return currentStatus == AssessmentStatus.WaitingForApproval;
        }

        /// <summary>
        /// Validates if rejection operations are allowed for the current status
        /// </summary>
        /// <param name="currentStatus">Current assessment status</param>
        /// <returns>True if rejection is allowed</returns>
        public static bool CanReject(AssessmentStatus currentStatus)
        {
            return currentStatus == AssessmentStatus.WaitingForApproval;
        }

        /// <summary>
        /// Validates if distribution operations are allowed for the current status
        /// </summary>
        /// <param name="currentStatus">Current assessment status</param>
        /// <returns>True if distribution is allowed</returns>
        public static bool CanDistribute(AssessmentStatus currentStatus)
        {
            return currentStatus == AssessmentStatus.Approved;
        }
    }
}
