using Domain.Entities.ResolutionManagement;

namespace Abstraction.Contracts.Repository.Resolution
{
    /// <summary>
    /// Repository interface for ResolutionHistory entity operations
    /// Inherits from IGenericRepository to provide standard CRUD operations
    /// Includes specific methods for resolution history tracking
    /// </summary>
    public interface IResolutionStatusHistoryRepository : IGenericRepository
    {
        /// <summary>
        /// Gets all history entries for a specific resolution
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of history entries ordered by action date time descending</returns>
        IQueryable<ResolutionStatusHistory> GetHistoryByResolutionIdAsync(int resolutionId, bool trackChanges = false);

        /// <summary>
        /// Gets history entries for a specific resolution and action type
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="actionName">Action name to filter by</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of history entries for the specified action</returns>
        Task<IEnumerable<ResolutionStatusHistory>> GetHistoryByActionAsync(int resolutionId, string actionName, bool trackChanges = false);

        /// <summary>
        /// Gets history entries for a specific user across all resolutions
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of history entries for the user</returns>
        Task<IEnumerable<ResolutionStatusHistory>> GetHistoryByUserAsync(int userId, bool trackChanges = false);

        /// <summary>
        /// Gets the latest history entry for a specific resolution
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Latest history entry or null</returns>
        Task<ResolutionStatusHistory?> GetLatestHistoryEntryAsync(int resolutionId, bool trackChanges = false);

        /// <summary>
        /// Gets history entries that represent status changes for a resolution
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of status change history entries</returns>
        Task<IEnumerable<ResolutionStatusHistory>> GetStatusChangeHistoryAsync(int resolutionId, bool trackChanges = false);

        /// <summary>
        /// Adds a new history entry for a resolution action
        /// </summary>
        /// <param name="historyEntry">History entry to add</param>
        /// <returns>Task representing the async operation</returns>
        Task AddHistoryEntryAsync(ResolutionStatusHistory historyEntry);

        /// <summary>
        /// Gets history entries for a date range
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="fromDate">Start date</param>
        /// <param name="toDate">End date</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of history entries within the date range</returns>
        Task<IEnumerable<ResolutionStatusHistory>> GetHistoryByDateRangeAsync(int resolutionId, DateTime fromDate, DateTime toDate, bool trackChanges = false);
    }
}
