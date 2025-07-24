using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Service;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using AutoMapper;


namespace Application.Features.Catalog.Products.Commands.Add
{
    public class AddCategoryCommandHandler : BaseResponseHandler, ICommandHandler<AddProductCommand, BaseResponse<string>>
    {

        #region Fileds
        private readonly ILoggerManager _logger;
        private readonly IServiceManager _service;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        public AddCategoryCommandHandler(IServiceManager service, IMapper mapper, ILoggerManager logger)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }
        #endregion

        #region Handle Functions
        public async Task<BaseResponse<string>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null)
                    return BadRequest<string>("the request can't be blank");
              return await _service.ProductService.AddAsync<AddProductCommand>(request);      
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: in AddProductCommand");
                return ServerError<string>(ex.Message);
            }
        }

        #endregion


    }
}
