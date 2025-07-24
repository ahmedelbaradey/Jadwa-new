using Domain.Entities.ResolutionManagement;

namespace Abstraction.Contracts.Repository.Resolution
{
    /// <summary>
    /// Repository interface for ResolutionItem entity operations
    /// Inherits from IGenericRepository to provide standard CRUD operations
    /// Includes specific methods for resolution item business logic
    /// </summary>
    public interface IResolutionItemRepository : IGenericRepository
    {
        /// <summary>
        /// Gets all items for a specific resolution
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of resolution items ordered by display order</returns>
        Task<IEnumerable<ResolutionItem>> GetItemsByResolutionIdAsync(int resolutionId, bool trackChanges = false);

        /// <summary>
        /// Gets resolution items with conflict information
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of resolution items with conflict members</returns>
        Task<IEnumerable<ResolutionItem>> GetItemsWithConflictsAsync(int resolutionId, bool trackChanges = false);

        /// <summary>
        /// Gets the next display order for a new item in a resolution
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <returns>Next available display order</returns>
        Task<int> GetNextDisplayOrderAsync(int resolutionId);

        /// <summary>
        /// Reorders items after deletion to maintain sequential numbering
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="deletedItemOrder">Display order of the deleted item</param>
        /// <returns>Task representing the async operation</returns>
        Task ReorderItemsAfterDeletionAsync(int resolutionId, int deletedItemOrder);

        /// <summary>
        /// Gets items that have conflicts of interest
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of items with conflicts</returns>
        Task<IEnumerable<ResolutionItem>> GetItemsWithConflictAsync(int resolutionId, bool trackChanges = false);
    }
}
