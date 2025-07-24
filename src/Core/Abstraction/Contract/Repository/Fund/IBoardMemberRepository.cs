using Domain.Entities.FundManagement;

namespace Abstraction.Contracts.Repository.Fund
{
    /// <summary>
    /// Repository interface for BoardMember entity operations
    /// Inherits from IGenericRepository to provide standard CRUD operations
    /// Includes specific methods for board member business logic
    /// </summary>
    public interface IBoardMemberRepository : IGenericRepository
    {
        /// <summary>
        /// Gets all active board members for a specific fund
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of active board members</returns>
        Task<IEnumerable<BoardMember>> GetActiveBoardMembersByFundIdAsync(int fundId, bool trackChanges = false);

        /// <summary>
        /// Gets the count of active independent board members for a specific fund
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <returns>Count of active independent board members</returns>
        Task<int> GetActiveIndependentMemberCountAsync(int fundId);

        /// <summary>
        /// Gets the count of active independent board members for a specific fund
        /// Alternative method name for consistency with AddBoardMemberHandler usage
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <returns>Count of active independent board members</returns>
        Task<int> IndependentMembersCountAsync(int fundId);

        /// <summary>
        /// Gets the current chairman for a specific fund
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Chairman board member or null if none exists</returns>
        Task<BoardMember?> GetChairmanByFundIdAsync(int fundId, bool trackChanges = false);

        /// <summary>
        /// Checks if a user is already a board member of a specific fund
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns>True if user is already a board member, false otherwise</returns>
        Task<bool> IsUserBoardMemberAsync(int fundId, int userId);

        /// <summary>
        /// Gets board members by type for a specific fund
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="memberType">Type of board member</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of board members of specified type</returns>
        IQueryable<BoardMember> GetBoardMembersByTypeAsync(int fundId,  bool trackChanges = false);

        /// <summary>
        /// Checks if a user is a board member of any fund
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>True if user is a board member of any fund, false otherwise</returns>
        Task<bool> IsUserBoardMemberAsync(int userId);

        /// <summary>
        /// Gets all funds where the user is a board member
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of funds where user is a board member</returns>
        Task<List<Domain.Entities.FundManagement.Fund>> GetFundsByBoardMemberIdAsync(int userId, bool trackChanges = false);

        /// <summary>
        /// Checks if removing board member role would violate business rules
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Tuple indicating if removal is allowed and error message if not</returns>
        Task<bool> CanRemoveBoardMemberRoleAsync(int userId);

        /// <summary>
        /// Checks if deactivating a user would violate the minimum independent board member requirement
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>True if deactivation would violate minimum requirements</returns>
        Task<bool> WouldDeactivationViolateIndependentMemberMinimumAsync(int userId);

        /// <summary>
        /// Gets all fund IDs where the user is an independent board member
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>List of fund IDs where user is an independent board member</returns>
        Task<List<int>> GetIndependentBoardMemberFundIdsAsync(int userId);
    }
}
