using Abstraction.Contracts.Logger;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using AutoMapper;
using Abstraction.Contracts.Service;


namespace Application.Features.Catalog.Products.Commands.Delete
{
    public class DeleteProductCommandHandler : BaseResponseHandler, ICommandHandler<DeleteProductCommand, BaseResponse<string>>
    {

        #region Fileds
        private readonly ILoggerManager _logger;
        private readonly IServiceManager _service;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        public DeleteProductCommandHandler(IServiceManager service, IMapper mapper, ILoggerManager logger)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }
        #endregion

        #region Handle Functions
        public async Task<BaseResponse<string>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null)
                    return BadRequest<string>("the request can't be blank");
                return await _service.ProductService.DeleteAsync(request.Id);
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: in EditProductCommand");
                return ServerError<string>(ex.Message);
            }
        }

        #endregion

    }
}
