using Domain.Entities.Products;
using AutoMapper;
using Abstraction.Contracts.Service.Catalog;
using Abstraction.Contracts.Repository;
using Infrastructure.Service;
using Domain.Entities.Startegies;
using Abstraction.Contract.Service;
using Microsoft.Extensions.Localization;
using Resources;

namespace Infrastructure.Onion.Service
{
    public class StrategyService : BaseService<Strategy>, IStrategyService
    {
        public StrategyService(IGenericRepository repository, IMapper mapper, IStringLocalizer<SharedResources> localizer) : base(repository, mapper, localizer)
        {
            _repository = repository;
        }
    }
}
