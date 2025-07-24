using Domain.Entities.Base;
using Domain.Entities.FundManagement;
using Domain.Entities.Users;
using Domain.Entities.MeetingManagement.State;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.MeetingManagement
{
    /// <summary>
    /// Represents a board meeting entity with state pattern implementation
    /// Supports the complete meeting lifecycle from scheduling to completion
    /// Based on User Stories 3, 4, and 5 requirements from Meetings.md
    /// Follows the same pattern as Resolution entity with state management
    /// </summary>
    public class Meeting : FullAuditedEntity
    {
        /// <summary>
        /// Fund identifier that this meeting belongs to
        /// Foreign key reference to Fund entity
        /// </summary>
        public int FundId { get; set; }

        /// <summary>
        /// Subject/title of the meeting
        /// Required field, maximum 255 characters
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Optional description of the meeting
        /// Maximum 2000 characters
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Meeting start date and time
        /// Must be in the future when scheduling
        /// </summary>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Meeting end date and time
        /// Must be after start date and time
        /// </summary>
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// Meeting location (physical address or "Online")
        /// Maximum 500 characters
        /// </summary>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Online meeting link (Zoom, Teams, etc.)
        /// Used when Location is "Online"
        /// Maximum 1000 characters
        /// </summary>
        public string? OnlineMeetingLink { get; set; }

        /// <summary>
        /// Current status of the meeting
        /// Maps to MeetingStatusEnum values
        /// Stored as integer in database
        /// </summary>
        public MeetingStatusEnum Status { get; set; } = MeetingStatusEnum.Scheduled;

        /// <summary>
        /// User ID of the person who created this meeting
        /// Must be Legal Counsel or Board Secretary
        /// </summary>
        public int CreatedByUserId { get; set; }

        /// <summary>
        /// Navigation property to Fund entity
        /// Provides access to the fund this meeting belongs to
        /// </summary>
        [ForeignKey("FundId")]
        public Fund Fund { get; set; } = null!;

        /// <summary>
        /// Navigation property to User entity (creator)
        /// Provides access to the user who created this meeting
        /// </summary>
        [ForeignKey("CreatedByUserId")]
        public User CreatedByUser { get; set; } = null!;

        /// <summary>
        /// Collection navigation property to MeetingAgendaItem entities
        /// Represents all agenda items for this meeting
        /// </summary>
        public virtual ICollection<MeetingAgendaItem> AgendaItems { get; set; } = new List<MeetingAgendaItem>();

        /// <summary>
        /// Collection navigation property to MeetingAttendee entities
        /// Represents all attendees for this meeting
        /// </summary>
        public virtual ICollection<MeetingAttendee> Attendees { get; set; } = new List<MeetingAttendee>();

        /// <summary>
        /// Collection navigation property to MeetingNote entities
        /// Represents all notes taken during the meeting
        /// </summary>
        public virtual ICollection<MeetingNote> Notes { get; set; } = new List<MeetingNote>();

        /// <summary>
        /// Collection navigation property to MeetingMinutes entities
        /// Represents meeting minutes created after the meeting
        /// </summary>
        public virtual ICollection<MeetingMinutes> Minutes { get; set; } = new List<MeetingMinutes>();

        /// <summary>
        /// Collection navigation property to MeetingStatusHistory entities
        /// Represents all status change history for this meeting
        /// Used for audit trail and status tracking
        /// </summary>
        public virtual ICollection<MeetingStatusHistory> StatusHistories { get; set; } = new List<MeetingStatusHistory>();

        /// <summary>
        /// State context for managing meeting state transitions
        /// Not mapped to database - used for business logic
        /// </summary>
        [NotMapped]
        public MeetingStateContext? StateContext { get; set; }

        /// <summary>
        /// Initializes the state context for this meeting
        /// Should be called after loading from database
        /// </summary>
        /// <param name="stateFactory">Factory for creating state instances</param>
        public void InitializeStateContext(MeetingStateFactory stateFactory)
        {
            StateContext = stateFactory.CreateContext(Status);
        }

        /// <summary>
        /// Transitions the meeting to a new status using the state pattern
        /// </summary>
        /// <param name="newStatus">Target status to transition to</param>
        /// <returns>True if transition was successful, false otherwise</returns>
        public bool TransitionToStatus(MeetingStatusEnum newStatus)
        {
            if (StateContext == null)
                throw new InvalidOperationException("State context must be initialized before state transitions");

            var newState = StateContext.CurrentState switch
            {
                ScheduledMeetingState => CreateStateForStatus(newStatus),
                InProgressMeetingState => CreateStateForStatus(newStatus),
                FinishedMeetingState => CreateStateForStatus(newStatus),
                CancelledMeetingState => CreateStateForStatus(newStatus),
                PostponedMeetingState => CreateStateForStatus(newStatus),
                _ => throw new InvalidOperationException($"Unknown state type: {StateContext.CurrentState.GetType()}")
            };

            if (StateContext.TransitionTo(newState, this))
            {
                Status = newStatus;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Helper method to create state instances for status transitions
        /// Uses the state factory if available, otherwise creates basic instances
        /// </summary>
        /// <param name="status">Status to create state for</param>
        /// <returns>State instance for the specified status</returns>
        private IMeetingState CreateStateForStatus(MeetingStatusEnum status)
        {
            // If StateContext is available, use its factory
            if (StateContext != null)
            {
                var factory = new MeetingStateFactory(null!); // This will be injected properly in real usage
                return factory.CreateState(status);
            }

            // Fallback to basic state creation (should not be used in production)
            throw new InvalidOperationException("State context must be initialized before creating states");
        }

        /// <summary>
        /// Checks if the meeting can be edited in its current state
        /// </summary>
        /// <returns>True if editing is allowed, false otherwise</returns>
        public bool CanEdit()
        {
            return StateContext?.CanEdit() ?? false;
        }

        /// <summary>
        /// Checks if the meeting can be completed in its current state
        /// </summary>
        /// <returns>True if completion is allowed, false otherwise</returns>
        public bool CanComplete()
        {
            return StateContext?.CanComplete() ?? false;
        }

        /// <summary>
        /// Checks if the meeting can be cancelled in its current state
        /// </summary>
        /// <returns>True if cancellation is allowed, false otherwise</returns>
        public bool CanCancel()
        {
            return StateContext?.CanCancel() ?? false;
        }

        /// <summary>
        /// Gets the available actions for the current meeting state
        /// </summary>
        /// <returns>Collection of available actions</returns>
        public IEnumerable<MeetingActionEnum> GetAvailableActions()
        {
            return StateContext?.GetAvailableActions() ?? new List<MeetingActionEnum>();
        }
    }
}
