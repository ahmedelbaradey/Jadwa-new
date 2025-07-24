using Abstraction.Contract.Service;
using Abstraction.Contracts.Repository.Resolution;
using Domain.Entities.ResolutionManagement;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Resolution
{
    /// <summary>
    /// Repository implementation for ResolutionItemConflict entity operations
    /// Inherits from GenericRepository and implements IResolutionItemConflictRepository
    /// Provides specific methods for resolution item conflict business logic
    /// </summary>
    public class ResolutionItemConflictRepository : GenericRepository, IResolutionItemConflictRepository
    {
        #region Constructor
        
        public ResolutionItemConflictRepository(AppDbContext repositoryContext, ICurrentUserService currentUserService)
            : base(repositoryContext, currentUserService)
        {
        }
        
        #endregion
        
        #region IResolutionItemConflictRepository Implementation
        
        /// <summary>
        /// Gets all conflicts for a specific resolution item
        /// </summary>
        /// <param name="resolutionItemId">Resolution item identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of conflicts for the resolution item</returns>
        public async Task<IEnumerable<ResolutionItemConflict>> GetConflictsByItemIdAsync(int resolutionItemId, bool trackChanges = false)
        {
            var query = GetByCondition<ResolutionItemConflict>(
                ric => ric.ResolutionItemId == resolutionItemId, 
                trackChanges);
                
            return await query
                .Include(ric => ric.ResolutionItem)
                .Include(ric => ric.BoardMember)
                .ThenInclude(bm => bm.User)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all conflicts for a specific board member
        /// </summary>
        /// <param name="boardMemberId">Board member identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of conflicts for the board member</returns>
        public async Task<IEnumerable<ResolutionItemConflict>> GetConflictsByBoardMemberIdAsync(int boardMemberId, bool trackChanges = false)
        {
            var query = GetByCondition<ResolutionItemConflict>(
                ric => ric.BoardMemberId == boardMemberId, 
                trackChanges);
                
            return await query
                .Include(ric => ric.ResolutionItem)
                .ThenInclude(ri => ri.Resolution)
                .Include(ric => ric.BoardMember)
                .ThenInclude(bm => bm.User)
                .ToListAsync();
        }

        /// <summary>
        /// Checks if a specific board member has a conflict with a resolution item
        /// </summary>
        /// <param name="resolutionItemId">Resolution item identifier</param>
        /// <param name="boardMemberId">Board member identifier</param>
        /// <returns>True if conflict exists, false otherwise</returns>
        public async Task<bool> ConflictExistsAsync(int resolutionItemId, int boardMemberId)
        {
            return await GetByCondition<ResolutionItemConflict>(
                ric => ric.ResolutionItemId == resolutionItemId && ric.BoardMemberId == boardMemberId && (!ric.IsDeleted.HasValue || !ric.IsDeleted.Value),
                trackChanges: false)
                .AnyAsync();
        }

        /// <summary>
        /// Gets conflicts for all items in a resolution
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of conflicts for all resolution items</returns>
        public async Task<IEnumerable<ResolutionItemConflict>> GetConflictsByResolutionIdAsync(int resolutionId, bool trackChanges = false)
        {
            var query = GetByCondition<ResolutionItemConflict>(
                ric => ric.ResolutionItem.ResolutionId == resolutionId, 
                trackChanges);
                
            return await query
                .Include(ric => ric.ResolutionItem)
                .Include(ric => ric.BoardMember)
                .ThenInclude(bm => bm.User)
                .ToListAsync();
        }

        /// <summary>
        /// Removes all conflicts for a specific resolution item
        /// </summary>
        /// <param name="resolutionItemId">Resolution item identifier</param>
        /// <returns>Task representing the async operation</returns>
        public async Task RemoveConflictsByItemIdAsync(int resolutionItemId)
        {
            var conflicts = await GetByCondition<ResolutionItemConflict>(
                ric => ric.ResolutionItemId == resolutionItemId,
                trackChanges: true)
                .ToListAsync();

            foreach (var conflict in conflicts)
            {
                // Soft delete by setting IsDeleted = true
                conflict.IsDeleted = true;
                conflict.DeletedAt = DateTime.Now;
                // DeletedBy will be set by the audit system
            }

            // Save changes will be handled by the unit of work pattern
        }

        /// <summary>
        /// Gets board members who have conflicts with any item in a resolution
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of board members with conflicts</returns>
        public async Task<IEnumerable<int>> GetConflictedBoardMemberIdsByResolutionIdAsync(int resolutionId, bool trackChanges = false)
        {
            return await GetByCondition<ResolutionItemConflict>(
                ric => ric.ResolutionItem.ResolutionId == resolutionId,
                trackChanges)
                .Select(ric => ric.BoardMemberId)
                .Distinct()
                .ToListAsync();
        }

        /// <summary>
        /// Adds multiple conflicts for a resolution item
        /// </summary>
        /// <param name="resolutionItemId">Resolution item identifier</param>
        /// <param name="boardMemberIds">Collection of board member IDs with conflicts</param>
        /// <param name="conflictNotes">Optional notes about the conflicts</param>
        /// <returns>Task representing the async operation</returns>
        public async Task AddConflictsAsync(int resolutionItemId, IEnumerable<int> boardMemberIds, string? conflictNotes = null)
        {
            foreach (var boardMemberId in boardMemberIds)
            {
                // Check if conflict already exists
                var existingConflict = await ConflictExistsAsync(resolutionItemId, boardMemberId);
                if (!existingConflict)
                {
                    var conflict = new ResolutionItemConflict
                    {
                        ResolutionItemId = resolutionItemId,
                        BoardMemberId = boardMemberId,
                        ConflictNotes = conflictNotes
                    };
                    await AddAsync(conflict);
                }
            }
        }

        public Task RemoveConflictsByItemIdsAsync(List<int> conflictIds)
        {
            var conflicts = GetByCondition<ResolutionItemConflict>(
            ric => conflictIds.Contains(ric.ResolutionItemId),
                trackChanges: true)
                .ToList();
            foreach (var conflict in conflicts)
            {
                // Soft delete by setting IsDeleted = true
                conflict.IsDeleted = true;
            }
            return UpdateRangeAsync(conflicts);
        }



        #endregion
    }
}
