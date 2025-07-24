using Abstraction.Contract.Service;
using Abstraction.Contracts.Repository.Fund;
using Domain.Entities.FundManagement;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Resources;

namespace Infrastructure.Repository.Fund
{
    /// <summary>
    /// Repository implementation for BoardMember entity operations
    /// Inherits from GenericRepository and implements IBoardMemberRepository
    /// Provides specific methods for board member business logic
    /// </summary>
    public class BoardMemberRepository : GenericRepository, IBoardMemberRepository
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructor

        public BoardMemberRepository(
            AppDbContext repositoryContext,
            ICurrentUserService currentUserService
           )
            : base(repositoryContext, currentUserService)
        {

        }

        #endregion
        
        #region IBoardMemberRepository Implementation
        
        /// <summary>
        /// Gets all active board members for a specific fund
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of active board members</returns>
        public async Task<IEnumerable<BoardMember>> GetActiveBoardMembersByFundIdAsync(int fundId, bool trackChanges = false)
        {
            var query = GetByCondition<BoardMember>(
                bm => bm.FundId == fundId && bm.IsActive, 
                trackChanges);
                
            return await query
                .Include(bm => bm.User)
                .Include(bm => bm.Fund)
                .OrderByDescending(bm => bm.UpdatedAt ?? bm.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Gets the count of active independent board members for a specific fund
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <returns>Count of active independent board members</returns>
        public async Task<int> GetActiveIndependentMemberCountAsync(int fundId)
        {
            return await GetByCondition<BoardMember>(
                bm => bm.FundId == fundId &&
                      bm.IsActive &&
                      bm.MemberType == BoardMemberType.Independent,
                trackChanges: false)
                .CountAsync();
        }

        /// <summary>
        /// Gets the count of active independent board members for a specific fund
        /// Alternative method name for consistency with AddBoardMemberHandler usage
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <returns>Count of active independent board members</returns>
        public async Task<int> IndependentMembersCountAsync(int fundId)
        {
            return await GetActiveIndependentMemberCountAsync(fundId);
        }

        /// <summary>
        /// Gets the current chairman for a specific fund
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Chairman board member or null if none exists</returns>
        public async Task<BoardMember?> GetChairmanByFundIdAsync(int fundId, bool trackChanges = false)
        {
            var query = GetByCondition<BoardMember>(
                bm => bm.FundId == fundId && 
                      bm.IsActive && 
                      bm.IsChairman, 
                trackChanges);
                
            return await query
                .Include(bm => bm.User)
                .Include(bm => bm.Fund)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Checks if a user is already a board member of a specific fund
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns>True if user is already a board member, false otherwise</returns>
        public async Task<bool> IsUserBoardMemberAsync(int fundId, int userId)
        {
            return await GetByCondition<BoardMember>(
                bm => bm.FundId == fundId && 
                      bm.UserId == userId && 
                      bm.IsActive,
                trackChanges: false)
                .AnyAsync();
        }

        /// <summary>
        /// Gets board members by type for a specific fund
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="memberType">Type of board member</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of board members of specified type</returns>
        public   IQueryable<BoardMember> GetBoardMembersByTypeAsync(int fundId, bool trackChanges = false)
        {
            var query = GetByCondition<BoardMember>(
                bm => bm.FundId == fundId ,
                trackChanges);

            return query
                .Include(bm => bm.User)
                .Include(bm => bm.Fund)
                .OrderByDescending(bm => bm.UpdatedAt ?? bm.CreatedAt);

        }

        /// <summary>
        /// Checks if a user is a board member of any fund
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>True if user is a board member of any fund, false otherwise</returns>
        public async Task<bool> IsUserBoardMemberAsync(int userId)
        {
            var query = GetByCondition<BoardMember>(
                bm => bm.UserId == userId && bm.IsActive,
                trackChanges: false);

            return await query.AnyAsync();
        }

        /// <summary>
        /// Gets all funds where the user is a board member
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of funds where user is a board member</returns>
        public async Task<List<Domain.Entities.FundManagement.Fund>> GetFundsByBoardMemberIdAsync(int userId, bool trackChanges = false)
        {
            var query = GetByCondition<BoardMember>(
                bm => bm.UserId == userId && bm.IsActive,
                trackChanges);

            var boardMembers = await query
                .Include(bm => bm.Fund)
                .ThenInclude(f => f.Strategy)
                .ToListAsync();

            return boardMembers.Select(bm => bm.Fund).ToList();
        }

        /// <summary>
        /// Checks if removing board member role would violate business rules
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Tuple indicating if removal is allowed and error message if not</returns>
        public async Task<bool>  CanRemoveBoardMemberRoleAsync(int userId)
        {
            try
            {
                var isAssigned = await IsUserBoardMemberAsync(userId);
                if (isAssigned)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking if board member role can be removed for user {userId}", ex);
            }
        }

        /// <summary>
        /// Checks if deactivating a user would violate the minimum independent board member requirement
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>True if deactivation would violate minimum requirements</returns>
        public async Task<bool> WouldDeactivationViolateIndependentMemberMinimumAsync(int userId)
        {
            try
            {
                // Get all funds where user is an independent board member
                var userIndependentFundIds = await GetIndependentBoardMemberFundIdsAsync(userId);

                // Check if deactivating this user would drop any fund below 2 independent members
                foreach (var fundId in userIndependentFundIds)
                {
                    var currentCount = await GetActiveIndependentMemberCountAsync(fundId);
                    if (currentCount <= 2) // Would drop below minimum of 2
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking independent member minimum violation for user {userId}", ex);
            }
        }

        /// <summary>
        /// Gets all fund IDs where the user is an independent board member
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>List of fund IDs where user is an independent board member</returns>
        public async Task<List<int>> GetIndependentBoardMemberFundIdsAsync(int userId)
        {
            try
            {
                return await GetByCondition<BoardMember>(
                    bm => bm.UserId == userId &&
                          bm.IsActive &&
                          bm.MemberType == BoardMemberType.Independent,
                    false)
                    .Select(bm => bm.FundId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving independent board member fund IDs for user {userId}", ex);
            }
        }

        #endregion
    }
}
