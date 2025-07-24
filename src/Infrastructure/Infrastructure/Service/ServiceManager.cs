using Abstraction.Contract.Service.Storage;
using Abstraction.Contracts.Repository;
using Abstraction.Contracts.Service;
using Abstraction.Contracts.Service.Catalog;
using Application.Common.Configurations;
using AutoMapper;
using Infrastructure.Service.Catalog;
using Infrastructure.Service.Storage;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using NLog;
using Resources;

namespace Infrastructure.Service
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<IStorageService> _storageService;
        private readonly IGenericRepository _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IMinioClient _minioClient;
        private readonly ILogger<StorageService> _logger;
        private readonly IOptions<MinIOConfiguration> _minioConfig;

        public ServiceManager(IMinioClient minioClient, IOptions<MinIOConfiguration> config, ILogger<StorageService> logger,IGenericRepository repository, IMapper mapper, IStringLocalizer<SharedResources> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _localizer = localizer;
            _minioClient = minioClient;
            _minioConfig = config;
            _logger = logger;
            _productService = new Lazy<IProductService>(() => new ProductService(_repository, _mapper, _localizer));
            _storageService = new Lazy<IStorageService>(() => new StorageService(_minioClient, _minioConfig, _logger));


        }
        public IProductService ProductService => _productService.Value;
        public IStorageService StorageService => _storageService.Value;

    }
}
