using Domain.Entities.ResolutionManagement;

namespace Abstraction.Contracts.Repository.Resolution
{
    /// <summary>
    /// Repository interface for Resolution entity operations
    /// Inherits from IGenericRepository to provide standard CRUD operations
    /// Includes specific methods for resolution business logic
    /// </summary>
    public interface IResolutionRepository : IGenericRepository
    {
        /// <summary>
        /// Gets all resolutions for a specific fund
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of resolutions for the fund</returns>
        IQueryable<Domain.Entities.ResolutionManagement.Resolution> GetResolutionsByFundIdAsync(int fundId, bool trackChanges = false);

        /// <summary>
        /// Gets resolutions by status for a specific fund
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="status">Resolution status</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of resolutions with specified status</returns>
        Task<IEnumerable<Domain.Entities.ResolutionManagement.Resolution>> GetResolutionsByStatusAsync(int fundId, ResolutionStatusEnum status, bool trackChanges = false);

        /// <summary>
        /// Gets a resolution by its code
        /// </summary>
        /// <param name="code">Resolution code</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Resolution with the specified code or null</returns>
        Task<Domain.Entities.ResolutionManagement.Resolution?> GetResolutionByCodeAsync(string code, bool trackChanges = false);

        /// <summary>
        /// Gets resolutions for a fund in a specific year
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="year">Year to filter by</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of resolutions for the specified year</returns>
        Task<IEnumerable<Domain.Entities.ResolutionManagement.Resolution>> GetResolutionsByYearAsync(int fundId, int year, bool trackChanges = false);

        /// <summary>
        /// Gets the highest sequential number for resolutions in a specific fund and year
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="year">Year to check</param>
        /// <returns>Highest sequential number used</returns>
        Task<int> GetMaxSequentialNumberAsync(int fundId, int year);

        /// <summary>
        /// Gets resolution with its items included
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Resolution with items or null</returns>
        Task<Domain.Entities.ResolutionManagement.Resolution?> GetResolutionWithItemsAsync(int resolutionId, bool trackChanges = false);

        /// <summary>
        /// Checks if a resolution code already exists
        /// </summary>
        /// <param name="code">Resolution code to check</param>
        /// <returns>True if code exists, false otherwise</returns>
        Task<bool> ResolutionCodeExistsAsync(string code);

        /// <summary>
        /// Gets resolution with all related data (items, attachments, history)
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Resolution with all related data or null</returns>
        Task<Domain.Entities.ResolutionManagement.Resolution?> GetResolutionWithAllDataAsync(int resolutionId, bool trackChanges = false);

        /// <summary>
        /// Gets resolution with minimal data needed for edit screen
        /// Includes resolution items and attachment details but excludes heavy display-only data
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Resolution with edit-specific data or null</returns>
        Task<Domain.Entities.ResolutionManagement.Resolution?> GetResolutionForEditAsync(int resolutionId, bool trackChanges = false);

        /// <summary>
        /// Gets child resolutions for a parent resolution
        /// </summary>
        /// <param name="parentResolutionId">Parent resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of child resolutions</returns>
        Task<IEnumerable<Domain.Entities.ResolutionManagement.Resolution>> GetChildResolutionsAsync(int parentResolutionId, bool trackChanges = false);

        /// <summary>
        /// Gets resolutions by role-based filtering
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="userRole">User role for filtering</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of resolutions filtered by role</returns>
        IQueryable<Domain.Entities.ResolutionManagement.Resolution> GetResolutionsByRoleAsync(int fundId, string? searchTerm, int? resolutionTypeId, ResolutionStatusEnum? status, DateTime? fromDate, DateTime? toDate, string userRole, bool trackChanges = false);

       // IQueryable<Domain.Entities.ResolutionManagement.Resolution> GetResolutionsByRoleAsync(int fundId, string userRole, bool trackChanges = false);

    }
}
