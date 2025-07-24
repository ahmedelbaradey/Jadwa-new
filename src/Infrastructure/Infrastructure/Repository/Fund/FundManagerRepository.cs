using Abstraction.Contract.Service;
using Abstraction.Contracts.Repository;
using Abstraction.Contracts.Repository.Fund;
using Domain.Entities.FundManagement;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Resources;

namespace Infrastructure.Repository.Fund
{
    /// <summary>
    /// Repository implementation for FundManager entity operations
    /// Inherits from GenericRepository and implements IFundManagerRepository
    /// Provides specific methods for fund manager business logic and validation
    /// </summary>
    public class FundManagerRepository : GenericRepository, IFundManagerRepository
    {
        #region Constructor
        
        public FundManagerRepository(AppDbContext repositoryContext, ICurrentUserService currentUserService)
            : base(repositoryContext, currentUserService)
        {
        }
        
        #endregion
        
        #region IFundManagerRepository Implementation
        
        /// <summary>
        /// Gets all fund managers for a specific fund
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of fund managers for the specified fund</returns>
        public async Task<IEnumerable<FundManager>> GetFundManagersByFundIdAsync(int fundId, bool trackChanges = false)
        {
            try
            {
                var query = GetByCondition<FundManager>(
                    fm => fm.FundId == fundId, 
                    trackChanges);
                    
                return await query
                    .Include(fm => fm.User)
                    .Include(fm => fm.Fund)
                    .OrderByDescending(fm => fm.UpdatedAt ?? fm.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving fund managers for fund {fundId}", ex);
            }
        }

        /// <summary>
        /// Gets all funds where the user is a fund manager
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of funds where user is a fund manager</returns>
        public async Task<List<Domain.Entities.FundManagement.Fund>> GetFundsByUserIdAsync(int userId, bool trackChanges = false)
        {
            try
            {
                var query = GetByCondition<FundManager>(
                    fm => fm.UserId == userId,
                    trackChanges);

                var fundManagers = await query
                    .Include(fm => fm.Fund)
                    .ThenInclude(f => f.Strategy)
                    .ToListAsync();

                return fundManagers.Select(fm => fm.Fund).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving funds for user {userId}", ex);
            }
        }

        /// <summary>
        /// Checks if a user is a fund manager for a specific fund
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="fundId">Fund identifier</param>
        /// <returns>True if user is fund manager for the specified fund</returns>
        public async Task<bool> IsUserFundManagerAsync(int userId, int fundId)
        {
            try
            {
                return await GetByCondition<FundManager>(
                    fm => fm.UserId == userId && fm.FundId == fundId,
                    false)
                    .AnyAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking if user {userId} is fund manager for fund {fundId}", ex);
            }
        }

        /// <summary>
        /// Gets users who are sole managers for any fund
        /// Used for validation during role changes and deactivation
        /// </summary>
        /// <returns>Collection of users who are the only fund manager for at least one fund</returns>
        public async Task<IEnumerable<FundManager>> GetSoleFundManagersAsync()
        {
            try
            {
                var fundManagerCounts = await GetByCondition<FundManager>(fm => true, false)
                    .GroupBy(fm => fm.FundId)
                    .Select(g => new { FundId = g.Key, Count = g.Count() })
                    .Where(x => x.Count == 1)
                    .ToListAsync();

                var soleFundIds = fundManagerCounts.Select(x => x.FundId).ToList();

                return await GetByCondition<FundManager>(
                    fm => soleFundIds.Contains(fm.FundId),
                    false)
                    .Include(fm => fm.User)
                    .Include(fm => fm.Fund)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving sole fund managers", ex);
            }
        }

        /// <summary>
        /// Gets the count of fund managers for a specific fund
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <returns>Number of fund managers assigned to the fund</returns>
        public async Task<int> GetFundManagerCountByFundIdAsync(int fundId)
        {
            try
            {
                return await GetByCondition<FundManager>(
                    fm => fm.FundId == fundId,
                    false)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error counting fund managers for fund {fundId}", ex);
            }
        }

        /// <summary>
        /// Removes a user as fund manager from a specific fund
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="fundId">Fund identifier</param>
        /// <returns>True if removal was successful</returns>
        public async Task<bool> RemoveFundManagerAsync(int userId, int fundId)
        {
            try
            {
                var fundManager = await GetByCondition<FundManager>(
                    fm => fm.UserId == userId && fm.FundId == fundId,
                    true)
                    .FirstOrDefaultAsync();

                if (fundManager == null)
                    return false;

                return await DeleteAsync(fundManager);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error removing user {userId} as fund manager from fund {fundId}", ex);
            }
        }

        /// <summary>
        /// Gets all fund IDs where the user is a fund manager
        /// Optimized method for validation scenarios
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>List of fund IDs where user is a fund manager</returns>
        public async Task<List<int>> GetFundIdsByUserIdAsync(int userId)
        {
            try
            {
                return await GetByCondition<FundManager>(
                    fm => fm.UserId == userId,
                    false)
                    .Select(fm => fm.FundId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving fund IDs for user {userId}", ex);
            }
        }

        /// <summary>
        /// Checks if a user is the sole fund manager for any assigned fund
        /// Used for role change validation
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>True if user is the only fund manager for any assigned fund</returns>
        public async Task<bool> IsUserSoleFundManagerForAnyFundAsync(int userId)
        {
            try
            {
                // Get all funds where user is a fund manager
                var userFunds = await GetFundIdsByUserIdAsync(userId);

                // Check if user is the only manager for any of these funds
                foreach (var fundId in userFunds)
                {
                    var managerCount = await GetFundManagerCountByFundIdAsync(fundId);
                    if (managerCount == 1)
                    {
                        return true; // User is the sole manager for this fund
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking if user {userId} is sole fund manager for any fund", ex);
            }
        }

        /// <summary>
        /// Gets fund managers with user and fund details for a specific fund
        /// Includes navigation properties for comprehensive data retrieval
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of fund managers with related data</returns>
        public async Task<IEnumerable<FundManager>> GetFundManagersWithDetailsAsync(int fundId, bool trackChanges = false)
        {
            try
            {
                var query = GetByCondition<FundManager>(
                    fm => fm.FundId == fundId,
                    trackChanges);

                return await query
                    .Include(fm => fm.User)
                    .Include(fm => fm.Fund)
                    .ThenInclude(f => f.Strategy)
                    .OrderByDescending(fm => fm.UpdatedAt ?? fm.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving fund managers with details for fund {fundId}", ex);
            }
        }

        /// <summary>
        /// Gets all active fund managers across all funds
        /// Used for reporting and administrative purposes
        /// </summary>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of all active fund managers</returns>
        public async Task<IEnumerable<FundManager>> GetAllActiveFundManagersAsync(bool trackChanges = false)
        {
            try
            {
                var query = GetAll<FundManager>(trackChanges);

                return await query
                    .Include(fm => fm.User)
                    .Include(fm => fm.Fund)
                    .Where(fm => fm.User.IsActive && !fm.User.IsDeleted.HasValue || !fm.User.IsDeleted.Value)
                    .OrderByDescending(fm => fm.UpdatedAt ?? fm.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving all active fund managers", ex);
            }
        }

        public async Task<bool> IsUserAssignedAsFundManagerAsync(int userId)
        {
            var fundIds = await  GetFundIdsByUserIdAsync(userId);
            return fundIds.Any();
        }

       
        public async Task<List<Domain.Entities.FundManagement.Fund>> GetFundsWhereUserIsFundManagerAsync(int userId)
        {
            return await  GetFundsByUserIdAsync(userId);
        }

        public async Task<bool> CanRemoveFundManagerRoleAsync(int userId)
        {
            var isSoleManager = await  IsUserSoleFundManagerForAnyFundAsync(userId);
            if (isSoleManager)
            {
                return false;
            }
            return true;
        }


        #endregion
    }
}
