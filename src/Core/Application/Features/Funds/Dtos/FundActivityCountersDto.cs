using Abstraction.Base.Dto;

namespace Application.Features.Funds.Dtos
{
    /// <summary>
    /// Data Transfer Object for Fund Activity Counters
    /// Contains real-time counts for each activity type within a specific fund
    /// Based on JDWA-996 requirements for fund details dashboard with activity cards
    /// Follows CleanDTOsImplementation.md patterns for clean DTO structure
    /// </summary>
    public record FundActivityCountersDto : BaseDto
    {
        /// <summary>
        /// Fund identifier for which the activity counters are calculated
        /// Foreign key reference to Fund entity
        /// </summary>
        public int FundId { get; set; }

        /// <summary>
        /// Total count of resolutions for this fund
        /// Includes all resolution statuses visible to the current user based on RBAC
        /// </summary>
        public int ResolutionsCount { get; set; }

        /// <summary>
        /// Total count of active board members for this fund
        /// Includes only members with IsActive = true
        /// </summary>
        public int BoardMembersCount { get; set; }

        /// <summary>
        /// Total count of documents/attachments for this fund
        /// Includes resolution attachments and fund documents
        /// </summary>
        public int DocumentsCount { get; set; }

        /// <summary>
        /// Total count of meetings for this fund
        /// Placeholder for future meeting management functionality
        /// </summary>
        public int MeetingsCount { get; set; }

         /// <summary>
        /// Count of unread resolution-related notifications for the current fund
        /// Includes ResolutionCreated, ResolutionUpdated, ResolutionConfirmed, ResolutionRejected, etc.
        /// </summary>
        public int ResolutionNotificationsCount { get; set; }

        /// <summary>
        /// Count of unread board member-related notifications for the current fund
        /// Includes BoardMemberAdded, BoardMemberAddedToFund, etc.
        /// </summary>
        public int BoardMemberNotificationsCount { get; set; }

        /// <summary>
        /// Count of unread document-related notifications for the current fund
        /// Placeholder for future document management functionality
        /// </summary>
        public int DocumentNotificationsCount { get; set; }

        /// <summary>
        /// Count of unread meeting-related notifications for the current fund
        /// Placeholder for future meeting management functionality
        /// </summary>
        public int MeetingNotificationsCount { get; set; }

        /// <summary>
        /// Count of unread voting-related notifications for the current fund
        /// Includes ResolutionSentToVote, VotingReminder, etc.
        /// </summary>
        public int VotingNotificationsCount { get; set; }
    }
}
