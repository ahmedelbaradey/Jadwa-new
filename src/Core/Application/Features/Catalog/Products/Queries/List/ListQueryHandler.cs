using AutoMapper;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Common.Wappers;
using Application.Features.Catalog.Products.Dtos;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Service;


namespace Application.Features.Catalog.Products.Queries.List
{
    public class ListQueryHandler : BaseResponseHandler, IQueryHandler<ListQuery, BaseResponse<PaginatedResult<SingleProductResponse>>>
    {
        #region Fileds
        private readonly ILoggerManager _logger;
        private readonly IServiceManager _service;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor(s)
        public ListQueryHandler(IServiceManager service, IMapper mapper, ILoggerManager logger)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<PaginatedResult<SingleProductResponse>>> Handle(ListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = _service.ProductService.GetAllAsync(false);
                if (!result.Any())
                {
                    return EmptyCollection(PaginatedResult<SingleProductResponse>.Success(new List<SingleProductResponse>(), 0, 0, 0));
                }
                var productList = await _mapper.ProjectTo<SingleProductResponse>(result).ToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy);
                return Success(productList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: in ListQuery");
                return ServerError<PaginatedResult<SingleProductResponse>>(ex.Message);
            }
        }
        #endregion
    }
}
