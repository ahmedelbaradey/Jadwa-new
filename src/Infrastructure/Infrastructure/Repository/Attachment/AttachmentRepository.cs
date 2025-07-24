using Abstraction.Contract.Service;
using Abstraction.Contracts.Repository.Products;
using Infrastructure.Data;
using Infrastructure.Repository;

namespace Repository.Catalog
{
    public class AttachmentRepository :  GenericRepository, IAttachmentRepository
    {
        public AttachmentRepository(AppDbContext repositoryContext, ICurrentUserService currentUserService)
            : base(repositoryContext,currentUserService)
        {

        }
    }
}
