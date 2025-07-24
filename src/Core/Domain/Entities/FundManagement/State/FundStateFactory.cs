using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities.FundManagement.State
{
    /// <summary>
    /// Factory class for creating fund state instances
    /// Implements the Factory pattern for state management
    /// Based on Sprint.md requirements and Clean Architecture principles
    /// </summary>
    public static class FundStateFactory
    {
        /// <summary>
        /// Creates a fund state instance based on the status enum
        /// </summary>
        /// <param name="status">The fund status enum value</param>
        /// <returns>Appropriate IFundState implementation</returns>
        /// <exception cref="ArgumentException">Thrown when status is not supported</exception>
        public static IFundState CreateState(FundStatusEnum status)
        {
            return status switch
            {
                FundStatusEnum.UnderConstruction => new UnderConstructionFund(),
                FundStatusEnum.WaitingForAddingMembers => new WaitingForAddingMembersFund(),
                FundStatusEnum.Active => new ActiveFund(),
                FundStatusEnum.Exited => new ExitedFund(),
                _ => throw new ArgumentException($"Unsupported fund status: {status}", nameof(status))
            };
        }

        /// <summary>
        /// Creates a fund state instance based on StatusHistory ID
        /// Maps StatusHistory IDs to FundStatusEnum values
        /// </summary>
        /// <param name="statusHistoryId">The StatusHistory ID from database</param>
        /// <returns>Appropriate IFundState implementation</returns>
        /// <exception cref="ArgumentException">Thrown when statusHistoryId is not supported</exception>
        public static IFundState CreateStateFromStatusHistoryId(int statusHistoryId)
        {
            var status = (FundStatusEnum)statusHistoryId;
            return CreateState(status);
        }

        /// <summary>
        /// Gets the default initial state for a new fund
        /// </summary>
        /// <returns>Initial fund state (UnderConstruction)</returns>
        public static IFundState GetInitialState()
        {
            return new UnderConstructionFund();
        }

        /// <summary>
        /// Validates if a state transition is allowed
        /// </summary>
        /// <param name="currentStatus">Current fund status</param>
        /// <param name="targetStatus">Target fund status</param>
        /// <returns>True if transition is allowed, false otherwise</returns>
        public static bool IsTransitionAllowed(FundStatusEnum currentStatus, FundStatusEnum targetStatus)
        {
            var currentState = CreateState(currentStatus);
            return currentState.CanTransitionTo(targetStatus);
        }

        /// <summary>
        /// Gets all allowed transitions from a given status
        /// </summary>
        /// <param name="currentStatus">Current fund status</param>
        /// <returns>Collection of allowed target statuses</returns>
        public static IEnumerable<FundStatusEnum> GetAllowedTransitions(FundStatusEnum currentStatus)
        {
            var currentState = CreateState(currentStatus);
            return currentState.GetAllowedTransitions();
        }
    }
}
