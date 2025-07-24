using Domain.Entities.Products;
using AutoMapper;
using Abstraction.Contracts.Service.Catalog;
using Abstraction.Contracts.Repository;
using Microsoft.Extensions.Localization;
using Resources;


namespace Infrastructure.Service.Catalog
{
    public class ProductService : BaseService<Product>, IProductService
    {
        public ProductService(IGenericRepository repository, IMapper mapper, IStringLocalizer<SharedResources> localizer) : base(repository, mapper, localizer)
        {
            _repository = repository;
        }
    }
}
