using Abstraction.Constants;
using Abstraction.Contract.Service;
using Abstraction.Contracts.Repository.Resolution;
using Azure.Core;
using Domain.Entities.ResolutionManagement;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Resolution
{
    /// <summary>
    /// Repository implementation for Resolution entity operations
    /// Inherits from GenericRepository and implements IResolutionRepository
    /// Provides specific methods for resolution business logic
    /// </summary>
    public class ResolutionRepository : GenericRepository, IResolutionRepository
    {
        #region Constructor

        public ResolutionRepository(AppDbContext repositoryContext, ICurrentUserService currentUserService)
            : base(repositoryContext, currentUserService)
        {
        }

        #endregion

        #region IResolutionRepository Implementation

        /// <summary>
        /// Gets all resolutions for a specific fund
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of resolutions for the fund</returns>
        public IQueryable<Domain.Entities.ResolutionManagement.Resolution> GetResolutionsByFundIdAsync(int fundId, bool trackChanges = false)
        {
            var query = GetByCondition<Domain.Entities.ResolutionManagement.Resolution>(
                r => r.FundId == fundId,
                trackChanges);

            return query
                .Include(r => r.Fund)
                .Include(r => r.ResolutionType)
                .Include(r => r.Attachment)
                .OrderByDescending(r => r.UpdatedAt ?? r.CreatedAt);

        }

        /// <summary>
        /// Gets resolutions by status for a specific fund
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="status">Resolution status</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of resolutions with specified status</returns>
        public async Task<IEnumerable<Domain.Entities.ResolutionManagement.Resolution>> GetResolutionsByStatusAsync(int fundId, ResolutionStatusEnum status, bool trackChanges = false)
        {
            var query = GetByCondition<Domain.Entities.ResolutionManagement.Resolution>(
                r => r.FundId == fundId && r.Status == status,
                trackChanges);

            return await query
                .Include(r => r.Fund)
                .Include(r => r.ResolutionType)
                .Include(r => r.Attachment)
                .OrderByDescending(r => r.UpdatedAt ?? r.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Gets a resolution by its code
        /// </summary>
        /// <param name="code">Resolution code</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Resolution with the specified code or null</returns>
        public async Task<Domain.Entities.ResolutionManagement.Resolution?> GetResolutionByCodeAsync(string code, bool trackChanges = false)
        {
            var query = GetByCondition<Domain.Entities.ResolutionManagement.Resolution>(
                r => r.Code == code,
                trackChanges);

            return await query
                .Include(r => r.Fund)
                .Include(r => r.ResolutionType)
                .Include(r => r.Attachment)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets resolutions for a fund in a specific year
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="year">Year to filter by</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of resolutions for the specified year</returns>
        public async Task<IEnumerable<Domain.Entities.ResolutionManagement.Resolution>> GetResolutionsByYearAsync(int fundId, int year, bool trackChanges = false)
        {
            var query = GetByCondition<Domain.Entities.ResolutionManagement.Resolution>(
                r => r.FundId == fundId && r.ResolutionDate.Year == year,
                trackChanges);

            return await query
                .Include(r => r.Fund)
                .Include(r => r.ResolutionType)
                .Include(r => r.Attachment)
                .OrderByDescending(r => r.ResolutionDate)
                .ToListAsync();
        }

        /// <summary>
        /// Gets the highest sequential number for resolutions in a specific fund and year
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="year">Year to check</param>
        /// <returns>Highest sequential number used</returns>
        public async Task<int> GetMaxSequentialNumberAsync(int fundId, int year)
        {
            var resolutions = await GetByCondition<Domain.Entities.ResolutionManagement.Resolution>(
                r => r.FundId == fundId && r.ResolutionDate.Year == year,
                trackChanges: false)
                .Select(r => r.Code)
                .ToListAsync();

            if (!resolutions.Any())
                return 0;

            // Extract sequential numbers from codes (format: fundcode/year/number)
            var maxNumber = 0;
            foreach (var code in resolutions)
            {
                var parts = code.Split('/');
                if (parts.Length == 3 && int.TryParse(parts[2], out var number))
                {
                    maxNumber = Math.Max(maxNumber, number);
                }
            }

            return maxNumber;
        }

        /// <summary>
        /// Gets resolution with its items included
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Resolution with items or null</returns>
        public async Task<Domain.Entities.ResolutionManagement.Resolution?> GetResolutionWithItemsAsync(int resolutionId, bool trackChanges = false)
        {
            var query = GetByCondition<Domain.Entities.ResolutionManagement.Resolution>(
                r => r.Id == resolutionId,
                trackChanges);

            return await query
                .Include(r => r.Fund)
                .Include(r => r.ResolutionType)
                .Include(r => r.Attachment)
                .Include(r => r.ResolutionItems.OrderBy(ri => ri.DisplayOrder))
                .ThenInclude(ri => ri.ConflictMembers)
                .ThenInclude(cm => cm.BoardMember)
                .ThenInclude(bm => bm.User)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Checks if a resolution code already exists
        /// </summary>
        /// <param name="code">Resolution code to check</param>
        /// <returns>True if code exists, false otherwise</returns>
        public async Task<bool> ResolutionCodeExistsAsync(string code)
        {
            return await GetByCondition<Domain.Entities.ResolutionManagement.Resolution>(
                r => r.Code == code,
                trackChanges: false)
                .AnyAsync();
        }

        /// <summary>
        /// Gets resolution with all related data (items, attachments, history)
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Resolution with all related data or null</returns>
        public async Task<Domain.Entities.ResolutionManagement.Resolution?> GetResolutionWithAllDataAsync(int resolutionId, bool trackChanges = false)
        {
            var query = GetByCondition<Domain.Entities.ResolutionManagement.Resolution>(
                r => r.Id == resolutionId,
                trackChanges);

            return await query
                .Include(r => r.Fund)
                .Include(r => r.ResolutionType)
                .Include(r => r.Attachment)
                .Include(r => r.OtherAttachments)
                .ThenInclude(oa => oa.Attachment)
                .Include(r => r.ResolutionItems.OrderBy(ri => ri.DisplayOrder))
                .ThenInclude(ri => ri.ConflictMembers)
                .ThenInclude(cm => cm.BoardMember)
                .ThenInclude(bm => bm.User)
                .Include(r => r.Votes)
                .ThenInclude(v => v.BoardMember)
                .ThenInclude(bm => bm.User)
                .Include(r => r.ResolutionStatusHistories)
                .ThenInclude(rsh => rsh.ResolutionStatus)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets resolution with minimal data needed for edit screen
        /// Includes resolution items and attachment details but excludes heavy display-only data
        /// Optimized for edit screen performance by loading only essential data
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Resolution with edit-specific data or null</returns>
        public async Task<Domain.Entities.ResolutionManagement.Resolution?> GetResolutionForEditAsync(int resolutionId, bool trackChanges = false)
        {
            var query = GetByCondition<Domain.Entities.ResolutionManagement.Resolution>(
                r => r.Id == resolutionId,
                trackChanges);

            return await query
                .Include(r=>r.Fund)
                .Include(r=>r.ResolutionStatusHistories)
                .ThenInclude(r=>r.ResolutionStatus)
                .Include(r => r.Attachment) // Include main attachment details
                .Include(r => r.ResolutionItems.OrderBy(ri => ri.DisplayOrder))
                .ThenInclude(ri => ri.ConflictMembers)
                .ThenInclude(cm => cm.BoardMember)
                .ThenInclude(bm => bm.User)
                .Include(r => r.OtherAttachments)
                .ThenInclude(oa => oa.Attachment) // Include other attachment details
                .Include(r=>r.ParentResolution)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets child resolutions for a parent resolution
        /// </summary>
        /// <param name="parentResolutionId">Parent resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of child resolutions</returns>
        public async Task<IEnumerable<Domain.Entities.ResolutionManagement.Resolution>> GetChildResolutionsAsync(int parentResolutionId, bool trackChanges = false)
        {
            var query = GetByCondition<Domain.Entities.ResolutionManagement.Resolution>(
                r => r.ParentResolutionId == parentResolutionId,
                trackChanges);

            return await query
                .Include(r => r.Fund)
                .Include(r => r.ResolutionType)
                .Include(r => r.Attachment)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Gets resolutions by role-based filtering
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="userRole">User role for filtering</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of resolutions filtered by role</returns>
        public IQueryable<Domain.Entities.ResolutionManagement.Resolution> GetResolutionsByRoleAsync //(int fundId, string userRole, bool trackChanges = false) //
                                                                                                     (int fundId, string? searchTerm, int? resolutionTypeId, ResolutionStatusEnum? status, DateTime? fromDate, DateTime? toDate, string userRole, bool trackChanges = false)
        {
            var query = GetByCondition<Domain.Entities.ResolutionManagement.Resolution>(
                r => r.FundId == fundId && !r.IsDeleted.Value
                && (string.IsNullOrWhiteSpace(searchTerm) || r.Code.Contains(searchTerm))
                && (!resolutionTypeId.HasValue || r.ResolutionTypeId == resolutionTypeId)
                && (status == null || r.Status == status)
                && (fromDate == null || r.ResolutionDate.Date >= fromDate.Value.Date)
                && (toDate == null || r.ResolutionDate.Date <= toDate.Value.Date)
                ,
                trackChanges);
            //var query = GetByCondition<Domain.Entities.ResolutionManagement.Resolution>(
            //    r => r.FundId == fundId && !r.IsDeleted.Value,
            //    trackChanges);
            // Apply role-based filtering
            switch (userRole.ToLower())
            {
                case RoleHelper.FundManager:
                    // Fund managers can see all resolutions
                    break;
                case RoleHelper.LegalCouncil:
                case RoleHelper.BoardSecretary:
                    // Legal council and board secretary can see pending, confirmed, rejected, and voting resolutions
                    query = query.Where(r => r.Status != ResolutionStatusEnum.Draft);
                    break;
                case RoleHelper.BoardMember:
                    // Board members can only see resolutions in voting or completed status
                    query = query.Where(r => r.Status == ResolutionStatusEnum.VotingInProgress ||
                                           r.Status == ResolutionStatusEnum.Approved ||
                                           r.Status == ResolutionStatusEnum.NotApproved);
                    break;
                default:
                    // Unknown role, return empty collection
                    return Enumerable.Empty<Domain.Entities.ResolutionManagement.Resolution>().AsQueryable();
            }

            return query
               // .Include(r => r.Fund)
               // .Include(r => r.ResolutionType)
               //.Include(r => r.Attachment)
               .Include(r => r.ResolutionStatusHistories.OrderByDescending(x => x.CreatedAt).Take(1))
                .ThenInclude(rsh => rsh.ResolutionStatus);

        }

        #endregion
    }
}
