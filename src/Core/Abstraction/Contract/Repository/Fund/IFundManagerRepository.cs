using Domain.Entities.FundManagement;

namespace Abstraction.Contracts.Repository.Fund
{
    /// <summary>
    /// Repository interface for FundManager entity operations
    /// Inherits from IGenericRepository to provide standard CRUD operations
    /// Includes specific methods for fund manager business logic and validation
    /// </summary>
    public interface IFundManagerRepository : IGenericRepository
    {
        /// <summary>
        /// Gets all fund managers for a specific fund
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of fund managers for the specified fund</returns>
        Task<IEnumerable<FundManager>> GetFundManagersByFundIdAsync(int fundId, bool trackChanges = false);

        /// <summary>
        /// Gets all funds where the user is a fund manager
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of funds where user is a fund manager</returns>
        Task<List<Domain.Entities.FundManagement.Fund>> GetFundsByUserIdAsync(int userId, bool trackChanges = false);

        /// <summary>
        /// Checks if a user is a fund manager for a specific fund
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="fundId">Fund identifier</param>
        /// <returns>True if user is fund manager for the specified fund</returns>
        Task<bool> IsUserFundManagerAsync(int userId, int fundId);

        /// <summary>
        /// Gets users who are sole managers for any fund
        /// Used for validation during role changes and deactivation
        /// </summary>
        /// <returns>Collection of users who are the only fund manager for at least one fund</returns>
        Task<IEnumerable<FundManager>> GetSoleFundManagersAsync();

        /// <summary>
        /// Gets the count of fund managers for a specific fund
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <returns>Number of fund managers assigned to the fund</returns>
        Task<int> GetFundManagerCountByFundIdAsync(int fundId);

        /// <summary>
        /// Removes a user as fund manager from a specific fund
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="fundId">Fund identifier</param>
        /// <returns>True if removal was successful</returns>
        Task<bool> RemoveFundManagerAsync(int userId, int fundId);

        /// <summary>
        /// Gets all fund IDs where the user is a fund manager
        /// Optimized method for validation scenarios
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>List of fund IDs where user is a fund manager</returns>
        Task<List<int>> GetFundIdsByUserIdAsync(int userId);

        /// <summary>
        /// Checks if a user is the sole fund manager for any assigned fund
        /// Used for role change validation
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>True if user is the only fund manager for any assigned fund</returns>
        Task<bool> IsUserSoleFundManagerForAnyFundAsync(int userId);

        /// <summary>
        /// Gets fund managers with user and fund details for a specific fund
        /// Includes navigation properties for comprehensive data retrieval
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of fund managers with related data</returns>
        Task<IEnumerable<FundManager>> GetFundManagersWithDetailsAsync(int fundId, bool trackChanges = false);

        /// <summary>
        /// Gets all active fund managers across all funds
        /// Used for reporting and administrative purposes
        /// </summary>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of all active fund managers</returns>
        Task<IEnumerable<FundManager>> GetAllActiveFundManagersAsync(bool trackChanges = false);

        /// <summary>
        /// Checks if removing board member role would violate business rules
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Tuple indicating if removal is allowed and error message if not</returns>
        Task<bool> CanRemoveFundManagerRoleAsync(int userId);
    }
}
