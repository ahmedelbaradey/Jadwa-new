using Abstraction.Contract.Service;
using Abstraction.Contracts.Repository.Resolution;
using Domain.Entities.ResolutionManagement;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Resolution
{
    /// <summary>
    /// Repository implementation for ResolutionItem entity operations
    /// Inherits from GenericRepository and implements IResolutionItemRepository
    /// Provides specific methods for resolution item business logic
    /// </summary>
    public class ResolutionItemRepository : GenericRepository, IResolutionItemRepository
    {
        #region Constructor
        
        public ResolutionItemRepository(AppDbContext repositoryContext, ICurrentUserService currentUserService)
            : base(repositoryContext, currentUserService)
        {
        }
        
        #endregion
        
        #region IResolutionItemRepository Implementation
        
        /// <summary>
        /// Gets all items for a specific resolution
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of resolution items ordered by display order</returns>
        public async Task<IEnumerable<ResolutionItem>> GetItemsByResolutionIdAsync(int resolutionId, bool trackChanges = false)
        {
            var query = GetByCondition<ResolutionItem>(
                ri => ri.ResolutionId == resolutionId, 
                trackChanges);
                
            return await query
                .Include(ri => ri.Resolution)
                .OrderBy(ri => ri.DisplayOrder)
                .ToListAsync();
        }

        /// <summary>
        /// Gets resolution items with conflict information
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of resolution items with conflict members</returns>
        public async Task<IEnumerable<ResolutionItem>> GetItemsWithConflictsAsync(int resolutionId, bool trackChanges = false)
        {
            var query = GetByCondition<ResolutionItem>(
                ri => ri.ResolutionId == resolutionId, 
                trackChanges);
                
            return await query
                .Include(ri => ri.Resolution)
                .Include(ri => ri.ConflictMembers)
                .ThenInclude(cm => cm.BoardMember)
                .ThenInclude(bm => bm.User)
                .OrderBy(ri => ri.DisplayOrder)
                .ToListAsync();
        }

        /// <summary>
        /// Gets the next display order for a new item in a resolution
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <returns>Next available display order</returns>
        public async Task<int> GetNextDisplayOrderAsync(int resolutionId)
        {
            var maxOrder = await GetByCondition<ResolutionItem>(
                ri => ri.ResolutionId == resolutionId,
                trackChanges: false)
                .MaxAsync(ri => (int?)ri.DisplayOrder);
                
            return (maxOrder ?? 0) + 1;
        }

        /// <summary>
        /// Reorders items after deletion to maintain sequential numbering
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="deletedItemOrder">Display order of the deleted item</param>
        /// <returns>Task representing the async operation</returns>
        public async Task ReorderItemsAfterDeletionAsync(int resolutionId, int deletedItemOrder)
        {
            // Get all items with display order greater than the deleted item
            var itemsToReorder = await GetByCondition<ResolutionItem>(
                ri => ri.ResolutionId == resolutionId && ri.DisplayOrder > deletedItemOrder,
                trackChanges: true)
                .ToListAsync();

            // Update display order and title for each item
            foreach (var item in itemsToReorder)
            {
                item.DisplayOrder--;
                item.Title = $"Item{item.DisplayOrder}";
            }

            // Save changes will be handled by the unit of work pattern
        }

        /// <summary>
        /// Gets items that have conflicts of interest
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of items with conflicts</returns>
        public async Task<IEnumerable<ResolutionItem>> GetItemsWithConflictAsync(int resolutionId, bool trackChanges = false)
        {
            var query = GetByCondition<ResolutionItem>(
                ri => ri.ResolutionId == resolutionId && ri.HasConflict, 
                trackChanges);
                
            return await query
                .Include(ri => ri.Resolution)
                .Include(ri => ri.ConflictMembers)
                .ThenInclude(cm => cm.BoardMember)
                .ThenInclude(bm => bm.User)
                .OrderBy(ri => ri.DisplayOrder)
                .ToListAsync();
        }
        
        #endregion
    }
}
