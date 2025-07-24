using System.Security.Cryptography;
using Abstraction.Contract.Repository.Fund;
using Abstraction.Contract.Service;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Resources;

namespace Infrastructure.Repository.Fund
{
    public class FundRepository : GenericRepository, IFundRepository
    {
        private readonly ICurrentUserService _currentUserService;

        public FundRepository(AppDbContext repositoryContext, ICurrentUserService currentUserService)
            : base(repositoryContext, currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task<Domain.Entities.FundManagement.Fund> EditFundById(int id, bool trackChanges)
        {
            var result = base.GetByCondition<Domain.Entities.FundManagement.Fund>(c => c.Id.Equals(id), trackChanges);
            return await result.Include(c => c.Attachment)
                .Include(c => c.Strategy)
                .Include(c => c.FundStatusHistories)
                .ThenInclude(c => c.StatusHistory)
                .Include(c => c.FundManagers)
                .Include(c => c.FundBoardSecretaries)
                .SingleOrDefaultAsync();
        }

        public IQueryable<Domain.Entities.FundManagement.Fund> GetAllAndInclude(bool trackChanges, int userId)
        {
            var result = base.GetAll<Domain.Entities.FundManagement.Fund>(trackChanges);
            return result.Where(c => c.FundBoardSecretaries.Select(c => c.UserId).Contains(userId) ||
            c.FundManagers.Select(c => c.UserId).Contains(userId) ||
            c.BoardMembers.Select(c => c.UserId).Contains(userId) ||
            c.LegalCouncilId == userId)
            .Include(c => c.Strategy).Include(c => c.FundStatusHistories).ThenInclude(c => c.StatusHistory);
        }

        public async Task<Domain.Entities.FundManagement.Fund> ViewFundDetails(int id, bool trackChanges)
        {
            var result = base.GetByCondition<Domain.Entities.FundManagement.Fund>(c => c.Id.Equals(id), trackChanges);
            return await result.Include(c => c.Strategy)
                .Include(c => c.Notifications.OrderByDescending(n => n.CreatedAt).Where(n=>n.UserId == _currentUserService.UserId).Take(10))
                .Include(c => c.FundStatusHistories.OrderByDescending(fs => fs.CreatedAt))
                .ThenInclude(c => c.StatusHistory)
                 .Include(c => c.FundStatusHistories.OrderByDescending(fs => fs.CreatedAt))
                .ThenInclude(c => c.CreatedByUser)
                .Include(c => c.FundBoardSecretaries)
                .Include(c => c.FundManagers)
                .Include(c => c.BoardMembers)
                 .Include(c=>c.Resolutions)
                .SingleOrDefaultAsync();
        }

        public async Task<Domain.Entities.FundManagement.Fund> ViewFundUsers(int id, bool trackChanges)
        {
            var result = base.GetByCondition<Domain.Entities.FundManagement.Fund>(c => c.Id.Equals(id), trackChanges);
            return await result
                .Include(c => c.FundBoardSecretaries)
                .Include(c => c.FundManagers)
                .Include(c => c.BoardMembers)
                .Include(c=>c.FundStatusHistories)
                .ThenInclude(c=>c.StatusHistory)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets all funds where the user is a fund manager
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of funds where user is a fund manager</returns>
        public async Task<List<Domain.Entities.FundManagement.Fund>> GetFundsByManagerIdAsync(int userId, bool trackChanges = false)
        {
            var query = base.GetByCondition<Domain.Entities.FundManagement.Fund>(
                f => f.FundManagers.Any(fm => fm.UserId == userId && (!fm.IsDeleted.HasValue || !fm.IsDeleted.Value)),
                trackChanges);

            return await query
                .Include(f => f.Strategy)
                .Include(f => f.FundManagers.Where(fm => fm.UserId == userId && (!fm.IsDeleted.HasValue || !fm.IsDeleted.Value)))
                .ThenInclude(fm => fm.User)
                .Include(f => f.FundStatusHistories.OrderByDescending(fsh => fsh.CreatedAt).Take(1))
                .ThenInclude(fsh => fsh.StatusHistory)
                .ToListAsync();
        }
    }
}
