using Abstraction.Contract.Service;
using Abstraction.Contracts.Repository.Resolution;
using Application.Base.Abstracts;
using Domain.Entities.ResolutionManagement;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Resolution
{
    /// <summary>
    /// Repository implementation for Resolution entity operations
    /// Inherits from GenericRepository and implements IResolutionHistoryRepository
    /// Provides specific methods for resolution business logic
    /// </summary>
    public class ResolutionStatusHistoryRepository : GenericRepository, IResolutionStatusHistoryRepository
    {
        #region Constructor
        
        public ResolutionStatusHistoryRepository(AppDbContext repositoryContext, ICurrentUserService currentUserService)
            : base(repositoryContext, currentUserService)
        {
        }

        public Task AddHistoryEntryAsync(ResolutionStatusHistory historyEntry)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ResolutionStatusHistory>> GetHistoryByActionAsync(int resolutionId, string actionName, bool trackChanges = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ResolutionStatusHistory>> GetHistoryByDateRangeAsync(int resolutionId, DateTime fromDate, DateTime toDate, bool trackChanges = false)
        {
            throw new NotImplementedException();
        }

        public   IQueryable<ResolutionStatusHistory> GetHistoryByResolutionIdAsync(int resolutionId, bool trackChanges = false)
        {
            return    GetByCondition<ResolutionStatusHistory>(h => h.ResolutionId == resolutionId, trackChanges)
                .Include(rsh => rsh.CreatedByUser)
                .Include(rsh => rsh.ResolutionStatus).OrderByDescending(c=>c.CreatedAt);
        }

        public Task<IEnumerable<ResolutionStatusHistory>> GetHistoryByUserAsync(int userId, bool trackChanges = false)
        {
            throw new NotImplementedException();
        }

        public Task<ResolutionStatusHistory?> GetLatestHistoryEntryAsync(int resolutionId, bool trackChanges = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ResolutionStatusHistory>> GetStatusChangeHistoryAsync(int resolutionId, bool trackChanges = false)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
