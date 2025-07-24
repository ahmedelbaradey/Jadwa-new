using Domain.Entities.ResolutionManagement;

namespace Abstraction.Contracts.Repository.Resolution
{
    /// <summary>
    /// Repository interface for ResolutionType entity operations
    /// Inherits from IGenericRepository to provide standard CRUD operations
    /// Includes specific methods for resolution type business logic
    /// </summary>
    public interface IResolutionTypeRepository : IGenericRepository
    {
        /// <summary>
        /// Gets all active resolution types ordered by display order
        /// </summary>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of active resolution types</returns>
        Task<IEnumerable<ResolutionType>> GetActiveResolutionTypesAsync(bool trackChanges = false);

        /// <summary>
        /// Gets a resolution type by its Arabic name
        /// </summary>
        /// <param name="nameAr">Arabic name</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Resolution type or null</returns>
        Task<ResolutionType?> GetByNameArAsync(string nameAr, bool trackChanges = false);

        /// <summary>
        /// Gets a resolution type by its English name
        /// </summary>
        /// <param name="nameEn">English name</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Resolution type or null</returns>
        Task<ResolutionType?> GetByNameEnAsync(string nameEn, bool trackChanges = false);

        /// <summary>
        /// Checks if a resolution type name already exists (Arabic or English)
        /// </summary>
        /// <param name="nameAr">Arabic name</param>
        /// <param name="nameEn">English name</param>
        /// <param name="excludeId">ID to exclude from check (for updates)</param>
        /// <returns>True if name exists, false otherwise</returns>
        Task<bool> ResolutionTypeNameExistsAsync(string nameAr, string nameEn, int? excludeId = null);
    }
}
