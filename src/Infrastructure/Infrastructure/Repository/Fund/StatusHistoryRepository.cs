using Abstraction.Contract.Repository.Fund;
using Abstraction.Contract.Service;
using Infrastructure.Data;

namespace Infrastructure.Repository.Fund
{
    public class StatusHistoryRepository : GenericRepository, IStatusHistoryRepository
    {
        public StatusHistoryRepository(AppDbContext dbContext, ICurrentUserService currentUserService) : base(dbContext, currentUserService)
        {
        }
    }
}
