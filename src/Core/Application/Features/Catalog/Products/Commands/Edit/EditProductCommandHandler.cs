using Abstraction.Contracts.Logger;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using AutoMapper;
using Abstraction.Contracts.Service;

namespace Application.Features.Catalog.Products.Commands.Edit
{
    public class EditProductCommandHandler : BaseResponseHandler, ICommandHandler<EditProductCommand, BaseResponse<string>>
    {

        #region Fileds
        private readonly ILoggerManager _logger;
        private readonly IServiceManager _service;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        public EditProductCommandHandler(IServiceManager service, IMapper mapper, ILoggerManager logger)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }
        #endregion

        #region Handle Functions
        public async Task<BaseResponse<string>> Handle(EditProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null)
                    return BadRequest<string>("the request can't be blank");
                return await _service.ProductService.UpdateAsync(request);
           
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
