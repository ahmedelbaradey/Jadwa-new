using AutoMapper;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Catalog.Products.Dtos;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Service;

namespace Application.Features.Catalog.Products.Queries.Get
{
    public class GetQueryHandler : BaseResponseHandler, IQueryHandler<GetQuery, BaseResponse<SingleProductResponse>>
    {
        #region Fileds
        private readonly ILoggerManager _logger;
        private readonly IServiceManager _service;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor(s)
        public GetQueryHandler(IServiceManager service, IMapper mapper, ILoggerManager logger)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<SingleProductResponse>> Handle(GetQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var doctor = await _service.ProductService.GetByIdAsync<SingleProductResponse>(request.Id, false);
                if (doctor == null)
                    return NotFound<SingleProductResponse>("Product with this Id not found!");
                var doctorMapper = _mapper.Map<SingleProductResponse>(doctor);
                return Success(doctorMapper);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: in GetDoctorByIdQuery");
                return ServerError<SingleProductResponse>(ex.Message);
            }
        }

        #endregion
    }
}
