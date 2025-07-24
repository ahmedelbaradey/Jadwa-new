using Abstraction.Contract.Service;
using Abstraction.Contracts.Repository.Resolution;
using Domain.Entities.ResolutionManagement;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Resolution
{
    /// <summary>
    /// Repository implementation for ResolutionType entity operations
    /// Inherits from GenericRepository and implements IResolutionTypeRepository
    /// Provides specific methods for resolution type business logic
    /// </summary>
    public class ResolutionTypeRepository : GenericRepository, IResolutionTypeRepository
    {
        #region Constructor
        
        public ResolutionTypeRepository(AppDbContext repositoryContext, ICurrentUserService currentUserService)
            : base(repositoryContext, currentUserService)
        {
        }
        
        #endregion
        
        #region IResolutionTypeRepository Implementation
        
        /// <summary>
        /// Gets all active resolution types ordered by display order
        /// </summary>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of active resolution types</returns>
        public async Task<IEnumerable<ResolutionType>> GetActiveResolutionTypesAsync(bool trackChanges = false)
        {
            var query = GetByCondition<ResolutionType>(
                rt => rt.IsActive, 
                trackChanges);
                
            return await query
                .OrderBy(rt => rt.DisplayOrder)
                .ToListAsync();
        }

        /// <summary>
        /// Gets a resolution type by its Arabic name
        /// </summary>
        /// <param name="nameAr">Arabic name</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Resolution type or null</returns>
        public async Task<ResolutionType?> GetByNameArAsync(string nameAr, bool trackChanges = false)
        {
            var query = GetByCondition<ResolutionType>(
                rt => rt.NameAr == nameAr, 
                trackChanges);
                
            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets a resolution type by its English name
        /// </summary>
        /// <param name="nameEn">English name</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Resolution type or null</returns>
        public async Task<ResolutionType?> GetByNameEnAsync(string nameEn, bool trackChanges = false)
        {
            var query = GetByCondition<ResolutionType>(
                rt => rt.NameEn == nameEn, 
                trackChanges);
                
            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Checks if a resolution type name already exists (Arabic or English)
        /// </summary>
        /// <param name="nameAr">Arabic name</param>
        /// <param name="nameEn">English name</param>
        /// <param name="excludeId">ID to exclude from check (for updates)</param>
        /// <returns>True if name exists, false otherwise</returns>
        public async Task<bool> ResolutionTypeNameExistsAsync(string nameAr, string nameEn, int? excludeId = null)
        {
            var query = GetByCondition<ResolutionType>(
                rt => (rt.NameAr == nameAr || rt.NameEn == nameEn) && 
                      (!excludeId.HasValue || rt.Id != excludeId.Value), 
                false);
                
            return await query.AnyAsync();
        }
        
        #endregion
    }
}
