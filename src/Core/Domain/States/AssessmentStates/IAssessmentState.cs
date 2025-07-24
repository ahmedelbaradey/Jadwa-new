using Domain.Entities.AssessmentManagement;
using Microsoft.Extensions.Localization;
using Resources;

namespace Domain.States.AssessmentStates
{
    /// <summary>
    /// Interface defining the contract for all assessment states
    /// Implements State Design Pattern for assessment lifecycle management
    /// Based on Fund state pattern implementation
    /// </summary>
    public interface IAssessmentState
    {
        /// <summary>
        /// Gets the status associated with this state
        /// </summary>
        AssessmentStatus Status { get; }

        /// <summary>
        /// Validates if the assessment can transition to the target status
        /// </summary>
        /// <param name="targetStatus">The target status to transition to</param>
        /// <returns>True if transition is allowed, false otherwise</returns>
        bool CanTransitionTo(AssessmentStatus targetStatus);

        /// <summary>
        /// Performs the transition to the target status
        /// </summary>
        /// <param name="targetStatus">The target status to transition to</param>
        /// <param name="reason">Reason for the transition</param>
        /// <returns>True if transition was successful, false otherwise</returns>
        bool TransitionTo(AssessmentStatus targetStatus, string reason);

        /// <summary>
        /// Gets all allowed transition statuses from the current state
        /// </summary>
        /// <returns>List of allowed target statuses</returns>
        List<AssessmentStatus> GetAllowedTransitions();

        /// <summary>
        /// Validates the current state business rules with localized messages
        /// </summary>
        /// <param name="assessment">The assessment to validate</param>
        /// <param name="localizer">String localizer for localized messages</param>
        /// <returns>Validation result with success status and localized messages</returns>
        (bool IsValid, List<string> ValidationMessages) ValidateState(Assessment assessment, IStringLocalizer<SharedResources> localizer);

        /// <summary>
        /// Gets the available actions for the current state
        /// </summary>
        /// <returns>List of available actions</returns>
        List<string> GetAvailableActions();

        /// <summary>
        /// Handles state-specific logic
        /// </summary>
        /// <param name="assessment">The assessment to handle</param>
        void Handle(Assessment assessment);
    }
}
