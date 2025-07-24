using Domain.Entities.Products;
using AutoMapper;
using Abstraction.Contracts.Service.Catalog;
using Abstraction.Contracts.Repository;
using Infrastructure.Service;
using Microsoft.Extensions.Localization;
using Resources;

namespace Infrastructure.Onion.Service.Catalog
{
    public class DemoEntityService : BaseService<DemoEntity>, IDemoEntityService
    {
        public DemoEntityService(IGenericRepository repository, IMapper mapper, IStringLocalizer<SharedResources> localizer) : base(repository, mapper, localizer)
        {
            _repository = repository;
        }
      
    }
}
