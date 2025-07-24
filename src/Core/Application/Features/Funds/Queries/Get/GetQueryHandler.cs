using AutoMapper;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Funds.Dtos;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Service;
using Abstraction.Contracts.Repository;
using Domain.Entities.FundManagement;
using Domain.Entities.Shared;
using Abstraction.Contract.Service.Storage;


namespace Application.Features.Funds.Queries.Get
{
    public class GetQueryHandler : BaseResponseHandler, IQueryHandler<GetQuery, BaseResponse<GetFundResponse>>
    {
        #region Fileds
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IPreviewUrlHelper _previewUrlHelper;
        #endregion

        #region Constructor(s)
        public GetQueryHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager logger, IPreviewUrlHelper previewUrlHelper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _previewUrlHelper = previewUrlHelper;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<GetFundResponse>> Handle(GetQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.Funds.EditFundById(request.Id, false);
                if (result == null)
                    return NotFound<GetFundResponse>("Fund with this Id not found!");

                var resultMapper = _mapper.Map<GetFundResponse>(result);

                // Populate attachment preview URL
                if (result.Attachment != null)
                {
                    resultMapper.AttachmentPath = await _previewUrlHelper.GeneratePreviewUrlAsync(
                        result.Attachment.Path,
                        result.Attachment.ModuleId,
                        cancellationToken);
                }

                return Success(resultMapper);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: in GetResultByIdQuery");
                return ServerError<GetFundResponse>(ex.Message);
            }
        }

        #endregion
    }
}
