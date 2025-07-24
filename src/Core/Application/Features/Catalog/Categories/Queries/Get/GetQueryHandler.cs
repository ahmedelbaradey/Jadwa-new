using AutoMapper;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Catalog.Categories.Dtos;
using Domain.Entities.Products;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;

namespace Application.Features.Catalog.Categories.Queries.Get
{
    public class GetQueryHandler : BaseResponseHandler, IQueryHandler<GetQuery, BaseResponse<SingleCategoryResponse>>
    {
        #region Fileds
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor(s)
        public GetQueryHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager logger)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<SingleCategoryResponse>> Handle(GetQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var categorty = await _repository.Categories.GetByIdAsync<Category>(request.Id,false);
                if (categorty is null)
                    return NotFound<SingleCategoryResponse>("Category with this Id not found!");

                var categortyMapper = _mapper.Map<SingleCategoryResponse>(categorty);
                return Success(categortyMapper);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: in GetCategoryByIdQuery");
                return ServerError<SingleCategoryResponse>(ex.Message);
            }
        }

        #endregion
    }

}
