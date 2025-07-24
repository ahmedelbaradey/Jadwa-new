using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MeetingManagement.State
{
    /// <summary>
    /// Interface for Meeting State Pattern implementation
    /// Defines the contract for all meeting states and their behaviors
    /// Based on Meetings.md requirements and Clean Architecture principles
    /// Follows the exact same pattern as IResolutionState
    /// </summary>
    public interface IMeetingState
    {
        /// <summary>
        /// Gets the status enum value for this state
        /// Maps to MeetingStatusEnum values
        /// </summary>
        MeetingStatusEnum Status { get; }

        /// <summary>
        /// Handles the state-specific logic and transitions
        /// </summary>
        /// <param name="meeting">The meeting entity to operate on</param>
        void Handle(Meeting meeting);

        /// <summary>
        /// Determines if this state can transition to the target status
        /// </summary>
        /// <param name="targetStatus">Target status to transition to</param>
        /// <returns>True if transition is allowed, false otherwise</returns>
        bool CanTransitionTo(MeetingStatusEnum targetStatus);

        /// <summary>
        /// Gets the allowed transition statuses from this state
        /// </summary>
        /// <returns>Collection of allowed target statuses</returns>
        IEnumerable<MeetingStatusEnum> GetAllowedTransitions();

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
        /// Validates if the current state allows cancellation
        /// </summary>
        /// <returns>True if cancellation is allowed in this state</returns>
        bool CanCancel();

        /// <summary>
        /// Gets the state-specific business rules and validation messages
        /// </summary>
        /// <returns>Collection of validation messages for this state</returns>
        IEnumerable<string> GetValidationMessages();

        /// <summary>
        /// Gets the localized resource key for the current state description
        /// </summary>
        /// <returns>Resource key for state description</returns>
        string GetStateDescriptionKey();

        /// <summary>
        /// Gets the available actions for the current state
        /// Returns MeetingActionEnum values that correspond to localized action keys
        /// </summary>
        /// <returns>Collection of available actions for this state</returns>
        IEnumerable<MeetingActionEnum> GetAvailableActions();
    }
}
