using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using AutoMapper;
using Domain.Entities.Products;


namespace Application.Features.Catalog.Categories.Commands.Add
{
    public class AddCategoryCommandHandler : BaseResponseHandler, ICommandHandler<AddCategoryCommand, BaseResponse<string>>
    {

        #region Fileds
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        public AddCategoryCommandHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager logger)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        #endregion

        #region Handle Functions
        public async Task<BaseResponse<string>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null)
                    return BadRequest<string>("the request can't be blank");
                var cateogryMapper = _mapper.Map<Category>(request);

                var result = await _repository.Categories.AddAsync(cateogryMapper);
                if (result is null)
                    return BadRequest<string>("Added Operation Failed.");
                return Success("Added Operation Successfully.");
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
