using Domain.Entities.AssessmentManagement;

namespace Domain.States.AssessmentStates
{
    /// <summary>
    /// Factory class for creating assessment state instances
    /// Implements Factory Design Pattern for state creation
    /// Based on Fund state factory implementation
    /// </summary>
    public static class AssessmentStateFactory
    {
        /// <summary>
        /// Creates an assessment state instance based on the provided status
        /// </summary>
        /// <param name="status">The assessment status</param>
        /// <returns>The corresponding state instance</returns>
        /// <exception cref="ArgumentException">Thrown when an invalid status is provided</exception>
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
                _ => throw new ArgumentException($"Invalid assessment status: {status}", nameof(status))
            };
        }

        /// <summary>
        /// Gets the initial state for a new assessment based on the action
        /// </summary>
        /// <param name="isDraft">True if saving as draft, false if submitting for approval</param>
        /// <returns>The initial state instance</returns>
        public static IAssessmentState GetInitialState(bool isDraft = true)
        {
            return isDraft ? new DraftState() : new WaitingForApprovalState();
        }

        /// <summary>
        /// Validates if a transition from one status to another is allowed
        /// </summary>
        /// <param name="fromStatus">Current status</param>
        /// <param name="toStatus">Target status</param>
        /// <returns>True if transition is allowed, false otherwise</returns>
        public static bool IsTransitionAllowed(AssessmentStatus fromStatus, AssessmentStatus toStatus)
        {
            var currentState = CreateState(fromStatus);
            return currentState.CanTransitionTo(toStatus);
        }

        /// <summary>
        /// Gets all allowed transitions from a given status
        /// </summary>
        /// <param name="status">Current status</param>
        /// <returns>List of allowed target statuses</returns>
        public static List<AssessmentStatus> GetAllowedTransitions(AssessmentStatus status)
        {
            var currentState = CreateState(status);
            return currentState.GetAllowedTransitions();
        }

        /// <summary>
        /// Gets available actions for a given status
        /// </summary>
        /// <param name="status">Current status</param>
        /// <returns>List of available actions</returns>
        public static List<string> GetAvailableActions(AssessmentStatus status)
        {
            var currentState = CreateState(status);
            return currentState.GetAvailableActions();
        }

        /// <summary>
        /// Validates an assessment against its current state business rules
        /// </summary>
        /// <param name="assessment">The assessment to validate</param>
        /// <returns>Validation result with success status and messages</returns>
        public static (bool IsValid, List<string> ValidationMessages) ValidateAssessment(Assessment assessment)
        {
            var currentState = CreateState(assessment.Status);
            return currentState.ValidateState(assessment);
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
        /// Checks if a status allows editing
        /// </summary>
        /// <param name="status">Status to check</param>
        /// <returns>True if editing is allowed, false otherwise</returns>
        public static bool AllowsEditing(AssessmentStatus status)
        {
            return status == AssessmentStatus.Draft || status == AssessmentStatus.Rejected;
        }

        /// <summary>
        /// Checks if a status allows distribution
        /// </summary>
        /// <param name="status">Status to check</param>
        /// <returns>True if distribution is allowed, false otherwise</returns>
        public static bool AllowsDistribution(AssessmentStatus status)
        {
            return status == AssessmentStatus.Approved;
        }

        /// <summary>
        /// Checks if a status allows responses
        /// </summary>
        /// <param name="status">Status to check</param>
        /// <returns>True if responses are allowed, false otherwise</returns>
        public static bool AllowsResponses(AssessmentStatus status)
        {
            return status == AssessmentStatus.Active;
        }
    }
}
