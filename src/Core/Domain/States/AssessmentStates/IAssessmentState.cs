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
        /// <returns>List of available action enums</returns>
        List<AssessmentActionEnum> GetAvailableActions();

        /// <summary>
        /// Validates if the current state allows editing
        /// </summary>
        /// <returns>True if editing is allowed in this state</returns>
        bool CanEdit();

        /// <summary>
        /// Validates if the current state allows completion operations
        /// </summary>
        /// <returns>True if completion is allowed in this state</returns>
        bool CanComplete();

        /// <summary>
        /// Validates if the current state allows deletion
        /// </summary>
        /// <returns>True if deletion is allowed in this state</returns>
        bool CanDelete();

        /// <summary>
        /// Gets the state-specific business rules and validation messages
        /// </summary>
        /// <returns>Collection of validation messages for this state</returns>
        IEnumerable<string> GetValidationMessages();

        /// <summary>
        /// Handles the state-specific logic and transitions
        /// </summary>
        /// <param name="assessment">The assessment entity to operate on</param>
        void Handle(Assessment assessment);

        /// <summary>
        /// Gets the localized resource key for the current state description
        /// </summary>
        /// <returns>Resource key for state description</returns>
        string GetStateDescriptionKey();

        /// <summary>
        /// Handles state-specific logic
        /// </summary>
        /// <param name="assessment">The assessment to handle</param>
        void Handle(Assessment assessment);
    }
}
