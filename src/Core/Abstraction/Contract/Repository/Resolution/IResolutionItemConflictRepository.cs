using Domain.Entities.ResolutionManagement;

namespace Abstraction.Contracts.Repository.Resolution
{
    /// <summary>
    /// Repository interface for ResolutionItemConflict entity operations
    /// Inherits from IGenericRepository to provide standard CRUD operations
    /// Includes specific methods for conflict of interest management
    /// </summary>
    public interface IResolutionItemConflictRepository : IGenericRepository
    {
        /// <summary>
        /// Gets all conflicts for a specific resolution item
        /// </summary>
        /// <param name="resolutionItemId">Resolution item identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of conflicts for the item</returns>
        Task<IEnumerable<ResolutionItemConflict>> GetConflictsByItemIdAsync(int resolutionItemId, bool trackChanges = false);

        /// <summary>
        /// Gets all conflicts for a specific board member
        /// </summary>
        /// <param name="boardMemberId">Board member identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of conflicts for the board member</returns>
        Task<IEnumerable<ResolutionItemConflict>> GetConflictsByBoardMemberIdAsync(int boardMemberId, bool trackChanges = false);

        /// <summary>
        /// Checks if a conflict already exists between a resolution item and board member
        /// </summary>
        /// <param name="resolutionItemId">Resolution item identifier</param>
        /// <param name="boardMemberId">Board member identifier</param>
        /// <returns>True if conflict exists, false otherwise</returns>
        Task<bool> ConflictExistsAsync(int resolutionItemId, int boardMemberId);

        /// <summary>
        /// Gets conflicts for all items in a resolution
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of conflicts for all items in the resolution</returns>
        Task<IEnumerable<ResolutionItemConflict>> GetConflictsByResolutionIdAsync(int resolutionId, bool trackChanges = false);

        /// <summary>
        /// Removes all conflicts for a specific resolution item
        /// </summary>
        /// <param name="resolutionItemId">Resolution item identifier</param>
        /// <returns>Task representing the async operation</returns>
        Task RemoveConflictsByItemIdAsync(int resolutionItemId);

        /// <summary>
        /// Adds multiple conflicts for a resolution item
        /// </summary>
        /// <param name="resolutionItemId">Resolution item identifier</param>
        /// <param name="boardMemberIds">Collection of board member IDs with conflicts</param>
        /// <param name="conflictNotes">Optional notes about the conflicts</param>
        /// <returns>Task representing the async operation</returns>
        Task AddConflictsAsync(int resolutionItemId, IEnumerable<int> boardMemberIds, string? conflictNotes = null);

        Task RemoveConflictsByItemIdsAsync(List<int> conflictIds);
    }
}
