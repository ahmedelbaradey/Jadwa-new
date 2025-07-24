using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ResolutionManagement.State
{
    /// <summary>
    /// Interface for Resolution State Pattern implementation
    /// Defines the contract for all resolution states and their behaviors
    /// Based on Sprint.md requirements and Clean Architecture principles
    /// </summary>
    public interface IResolutionState
    {
        /// <summary>
        /// Gets the status enum value for this state
        /// Maps to ResolutionStatusEnum values
        /// </summary>
        ResolutionStatusEnum Status { get; }

        /// <summary>
        /// Handles the state-specific logic and transitions
        /// </summary>
        /// <param name="resolution">The resolution entity to operate on</param>
        void Handle(Resolution resolution);

        /// <summary>
        /// Determines if this state can transition to the target status
        /// </summary>
        /// <param name="targetStatus">Target status to transition to</param>
        /// <returns>True if transition is allowed, false otherwise</returns>
        bool CanTransitionTo(ResolutionStatusEnum targetStatus);

        /// <summary>
        /// Gets the allowed transition statuses from this state
        /// </summary>
        /// <returns>Collection of allowed target statuses</returns>
        IEnumerable<ResolutionStatusEnum> GetAllowedTransitions();

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
    }
}
