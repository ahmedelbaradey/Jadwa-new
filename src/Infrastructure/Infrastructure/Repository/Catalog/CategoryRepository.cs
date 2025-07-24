using Abstraction.Contract.Service;
using Abstraction.Contracts.Repository.Products;
using Infrastructure.Data;
using Infrastructure.Repository;



namespace Repository.Catalog
{
    public class CategoryRepository :  GenericRepository, ICategoryRepository
    {
        public CategoryRepository(AppDbContext repositoryContext, ICurrentUserService currentUserService)
            : base(repositoryContext, currentUserService)
        {

        }
       
    }
}
