using Abstraction.Contract.Service.Storage;
using Abstraction.Contracts.Service.Catalog;

namespace Abstraction.Contracts.Service
{
    public interface IServiceManager
    {
        IProductService ProductService { get; }
        IStorageService StorageService { get; }
    }
}
