using Domain.Entities.FundManagement;
using Microsoft.Extensions.Localization;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MeetingManagement.State
{
    /// <summary>
    /// Context class for Meeting State Pattern implementation
    /// Manages state transitions and business logic for meetings
    /// Based on ResolutionStateContext pattern and Clean Architecture principles
    /// Follows the exact same structure as ResolutionStateContext
    /// </summary>
    public class MeetingStateContext
    {
        private IMeetingState _currentState;
        private readonly IStringLocalizer<SharedResources> _localizer;

        /// <summary>
        /// Constructor that initializes the context with a specific state
        /// </summary>
        /// <param name="initialState">The initial state for the meeting</param>
        /// <param name="localizer">String localizer for localized messages</param>
        public MeetingStateContext(IMeetingState initialState, IStringLocalizer<SharedResources> localizer)
        {
            _currentState = initialState ?? throw new ArgumentNullException(nameof(initialState));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        /// <summary>
        /// Gets the current state of the meeting
        /// </summary>
        public IMeetingState CurrentState => _currentState;

        /// <summary>
        /// Gets the current status enum value
        /// </summary>
        public MeetingStatusEnum CurrentStatus => _currentState.Status;

        /// <summary>
        /// Transitions the meeting to a new state
        /// </summary>
        /// <param name="newState">The new state to transition to</param>
        /// <param name="meeting">The meeting entity to operate on</param>
        /// <returns>True if transition was successful, false otherwise</returns>
        public bool TransitionTo(IMeetingState newState, Meeting meeting)
        {
            if (newState == null)
                throw new ArgumentNullException(nameof(newState));

            if (meeting == null)
                throw new ArgumentNullException(nameof(meeting));

            // Check if the transition is allowed
            if (!_currentState.CanTransitionTo(newState.Status))
            {
                return false;
            }

            // Perform the transition
            _currentState = newState;
            _currentState.Handle(meeting);

            return true;
        }

        /// <summary>
        /// Handles the current state logic
        /// </summary>
        /// <param name="meeting">The meeting entity to operate on</param>
        public void Handle(Meeting meeting)
        {
            _currentState.Handle(meeting);
        }

        /// <summary>
        /// Checks if the current state can transition to the target status
        /// </summary>
        /// <param name="targetStatus">Target status to check</param>
        /// <returns>True if transition is allowed, false otherwise</returns>
        public bool CanTransitionTo(MeetingStatusEnum targetStatus)
        {
            return _currentState.CanTransitionTo(targetStatus);
        }

        /// <summary>
        /// Gets the allowed transition statuses from the current state
        /// </summary>
        /// <returns>Collection of allowed target statuses</returns>
        public IEnumerable<MeetingStatusEnum> GetAllowedTransitions()
        {
            return _currentState.GetAllowedTransitions();
        }

        /// <summary>
        /// Validates if the current state allows editing
        /// </summary>
        /// <returns>True if editing is allowed in the current state</returns>
        public bool CanEdit()
        {
            return _currentState.CanEdit();
        }

        /// <summary>
        /// Validates if the current state allows completion operations
        /// </summary>
        /// <returns>True if completion is allowed in the current state</returns>
        public bool CanComplete()
        {
            return _currentState.CanComplete();
        }

        /// <summary>
        /// Validates if the current state allows cancellation
        /// </summary>
        /// <returns>True if cancellation is allowed in the current state</returns>
        public bool CanCancel()
        {
            return _currentState.CanCancel();
        }

        /// <summary>
        /// Gets the state-specific business rules and validation messages
        /// </summary>
        /// <returns>Collection of localized validation messages for the current state</returns>
        public IEnumerable<string> GetValidationMessages()
        {
            return _currentState.GetValidationMessages();
        }

        /// <summary>
        /// Gets the localized description for the current state
        /// </summary>
        /// <returns>Localized state description</returns>
        public string GetStateDescription()
        {
            var key = _currentState.GetStateDescriptionKey();
            return _localizer[key];
        }

        /// <summary>
        /// Gets the available actions for the current state
        /// </summary>
        /// <returns>Collection of available actions for the current state</returns>
        public IEnumerable<MeetingActionEnum> GetAvailableActions()
        {
            return _currentState.GetAvailableActions();
        }

        /// <summary>
        /// Gets the current user's role in the fund context
        /// Replicates the logic from AddResolutionCommandHandler.GetUserFundRole() method
        /// </summary>
        /// <param name="userId">Current user ID</param>
        /// <param name="fundId">Fund ID</param>
        /// <param name="boardMembers">Collection of board members</param>
        /// <param name="fundManagers">Collection of fund managers</param>
        /// <param name="legalCounsels">Collection of legal counsels</param>
        /// <param name="boardSecretaries">Collection of board secretaries</param>
        /// <returns>User's role in the fund context</returns>
        public Roles GetCurrentUserRole(int userId, int fundId,
            IEnumerable<BoardMember> boardMembers,
            IEnumerable<FundManager> fundManagers,
            IEnumerable<LegalCounsel> legalCounsels,
            IEnumerable<BoardSecretary> boardSecretaries)
        {
            // Check if user is a Board Member
            if (boardMembers.Any(bm => bm.UserId == userId && bm.FundId == fundId && !bm.IsDeleted))
            {
                return Roles.BoardMember;
            }

            // Check if user is a Fund Manager
            if (fundManagers.Any(fm => fm.UserId == userId && fm.FundId == fundId && !fm.IsDeleted))
            {
                return Roles.FundManager;
            }

            // Check if user is a Legal Counsel
            if (legalCounsels.Any(lc => lc.UserId == userId && lc.FundId == fundId && !lc.IsDeleted))
            {
                return Roles.LegalCounsel;
            }

            // Check if user is a Board Secretary
            if (boardSecretaries.Any(bs => bs.UserId == userId && bs.FundId == fundId && !bs.IsDeleted))
            {
                return Roles.BoardSecretary;
            }

            // Default role if no specific role found
            return Roles.User;
        }

        /// <summary>
        /// Adds a comprehensive audit entry for meeting status changes
        /// Follows the exact same pattern as ResolutionStateContext.AddAuditEntry
        /// </summary>
        /// <param name="meeting">Meeting entity</param>
        /// <param name="status">New meeting status</param>
        /// <param name="action">Action being performed</param>
        /// <param name="reason">Reason for the change</param>
        /// <param name="rejectionReason">Rejection reason if applicable</param>
        /// <param name="actionDetails">Detailed description of the operation</param>
        /// <param name="localizationKey">Localization key for the action</param>
        /// <param name="userRole">User role performing the action</param>
        /// <param name="userId">User ID performing the action</param>
        public void AddAuditEntry(Meeting meeting, MeetingStatusEnum status, MeetingActionEnum action,
            string reason, string? rejectionReason = null, string? actionDetails = null,
            string? localizationKey = null, string? userRole = null, int? userId = null)
        {
            // Initialize collection if null
            meeting.StatusHistories ??= new List<MeetingStatusHistory>();

            // Create comprehensive status history entry with all required audit fields
            var statusHistory = new MeetingStatusHistory
            {
                // Core meeting reference
                MeetingId = meeting.Id,
                MeetingStatusId = (int)status,
                PreviousStatus = meeting.Status,
                NewStatus = status,

                // Action information
                Action = action,
                Reason = reason,
                RejectionReason = rejectionReason,

                // Comprehensive action details - detailed description of the operation
                ActionDetails = actionDetails,

                // Localization key reference (NOT translated text) following notification pattern
                // This allows proper localization on retrieval for multilingual support
                Notes = localizationKey,

                // User context information
                UserRole = userRole,
                ChangedBy = userId ?? 0,
                ChangedAt = DateTime.UtcNow
            };

            meeting.StatusHistories.Add(statusHistory);
        }
    }
}
