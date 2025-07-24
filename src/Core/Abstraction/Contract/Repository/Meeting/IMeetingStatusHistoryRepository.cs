using Domain.Entities.MeetingManagement;

namespace Abstraction.Contract.Repository.Meeting
{
    /// <summary>
    /// Repository interface for MeetingStatusHistory entity operations
    /// Inherits from IGenericRepository to provide standard CRUD operations
    /// Includes specific methods for meeting status history business logic
    /// Follows the exact same pattern as IResolutionStatusHistoryRepository
    /// </summary>
    public interface IMeetingStatusHistoryRepository : IGenericRepository
    {
        /// <summary>
        /// Gets all status history entries for a specific meeting
        /// </summary>
        /// <param name="meetingId">Meeting identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of status history entries ordered by creation date</returns>
        Task<IEnumerable<MeetingStatusHistory>> GetHistoryByMeetingIdAsync(int meetingId, bool trackChanges = false);

        /// <summary>
        /// Gets the latest status history entry for a specific meeting
        /// </summary>
        /// <param name="meetingId">Meeting identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Latest status history entry or null if none exists</returns>
        Task<MeetingStatusHistory?> GetLatestHistoryByMeetingIdAsync(int meetingId, bool trackChanges = false);

        /// <summary>
        /// Gets status history entries for a specific meeting and status
        /// </summary>
        /// <param name="meetingId">Meeting identifier</param>
        /// <param name="status">Meeting status to filter by</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of status history entries for the specified status</returns>
        Task<IEnumerable<MeetingStatusHistory>> GetHistoryByMeetingAndStatusAsync(int meetingId, MeetingStatusEnum status, bool trackChanges = false);

        /// <summary>
        /// Gets status history entries performed by a specific user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of status history entries performed by the user</returns>
        Task<IEnumerable<MeetingStatusHistory>> GetHistoryByUserIdAsync(int userId, bool trackChanges = false);

        /// <summary>
        /// Gets status history entries for a specific fund
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of status history entries for meetings in the fund</returns>
        Task<IEnumerable<MeetingStatusHistory>> GetHistoryByFundIdAsync(int fundId, bool trackChanges = false);

        /// <summary>
        /// Gets status history entries within a date range
        /// </summary>
        /// <param name="startDate">Start date for the range</param>
        /// <param name="endDate">End date for the range</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of status history entries within the date range</returns>
        Task<IEnumerable<MeetingStatusHistory>> GetHistoryByDateRangeAsync(DateTime startDate, DateTime endDate, bool trackChanges = false);

        /// <summary>
        /// Checks if a meeting has any status history entries
        /// </summary>
        /// <param name="meetingId">Meeting identifier</param>
        /// <returns>True if meeting has status history, false otherwise</returns>
        Task<bool> HasHistoryAsync(int meetingId);

        /// <summary>
        /// Gets the count of status changes for a specific meeting
        /// </summary>
        /// <param name="meetingId">Meeting identifier</param>
        /// <returns>Number of status changes for the meeting</returns>
        Task<int> GetStatusChangeCountAsync(int meetingId);
    }
}
