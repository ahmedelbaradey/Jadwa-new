using AutoMapper;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Common.Wappers;
using Application.Features.Catalog.Categories.Dtos;
using Domain.Entities.Products;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;

namespace Application.Features.Catalog.Categories.Queries.List
{
    public class ListQueryHandler : BaseResponseHandler, IQueryHandler<ListQuery, BaseResponse<PaginatedResult<SingleCategoryResponse>>>
    {
        #region Fileds
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor(s)
        public ListQueryHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager logger)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<PaginatedResult<SingleCategoryResponse>>> Handle(ListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = _repository.Categories.GetByCondition<Category>(c=> string.IsNullOrWhiteSpace(request.Search) ? true :  c.Name.Contains(request.Search), false);
                if (!result.Any())
                {
                    return EmptyCollection(PaginatedResult<SingleCategoryResponse>.Success(new List<SingleCategoryResponse>(), 0, 0, 0));
                }
                var categories = await _mapper.ProjectTo<SingleCategoryResponse>(result).ToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy);
                return Success(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: in ListQuery");
                return ServerError<PaginatedResult<SingleCategoryResponse>>(ex.Message);
            }
        }
        #endregion
    }
}
