

using Abstraction.Contracts.Repository;

namespace Abstraction.Contract.Repository.Fund
{
    public interface IFundRepository :IGenericRepository
    {
        Task<Domain.Entities.FundManagement.Fund> ViewFundDetails(int id, bool trackChanges);
        Task<Domain.Entities.FundManagement.Fund> ViewFundUsers(int id, bool trackChanges);
        Task<Domain.Entities.FundManagement.Fund> EditFundById(int id, bool trackChanges);
        IQueryable<Domain.Entities.FundManagement.Fund> GetAllAndInclude(bool trackChanges, int userId);

        /// <summary>
        /// Gets all funds where the user is a fund manager
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of funds where user is a fund manager</returns>
        Task<List<Domain.Entities.FundManagement.Fund>> GetFundsByManagerIdAsync(int userId, bool trackChanges = false);
    }
}
