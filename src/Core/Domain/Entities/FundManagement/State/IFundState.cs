using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.FundManagement.State
{
    /// <summary>
    /// Interface for Fund State Pattern implementation
    /// Defines the contract for all fund states and their behaviors
    /// Based on Sprint.md requirements and Clean Architecture principles
    /// </summary>
    public interface IFundState
    {
        /// <summary>
        /// Gets the status enum value for this state
        /// Maps to FundStatusEnum values
        /// </summary>
        FundStatusEnum Status { get; }

        /// <summary>
        /// Handles the state-specific logic and transitions
        /// </summary>
        /// <param name="fund">The fund entity to operate on</param>
        void Handle(Fund fund);

        /// <summary>
        /// Determines if this state can transition to the target status
        /// </summary>
        /// <param name="targetStatus">Target status to transition to</param>
        /// <returns>True if transition is allowed, false otherwise</returns>
        bool CanTransitionTo(FundStatusEnum targetStatus);

        /// <summary>
        /// Gets the allowed transition statuses from this state
        /// </summary>
        /// <returns>Collection of allowed target statuses</returns>
        IEnumerable<FundStatusEnum> GetAllowedTransitions();
    }
}
