using Abstraction.Base.Response;
using Abstraction.Common.Wappers;
using Abstraction.Contract.Service;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Application.Base.Abstracts;
using Application.Features.Resolutions.Dtos;
using AutoMapper;
using Domain.Entities.ResolutionManagement;
using Domain.Services.Audit;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.Resolutions.Queries.GetResolutionAuditHistory
{
    /// <summary>
    /// Query handler for retrieving localized resolution audit history
    /// Demonstrates usage of IAuditLocalizationService for multilingual audit trail display
    /// Follows Clean Architecture and CQRS patterns with proper localization support
    /// </summary>
    public class GetResolutionAuditHistoryQueryHandler : BaseResponseHandler, IQueryHandler<GetResolutionAuditHistoryQuery, BaseResponse<List<ResolutionStatusHistoryDto>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IAuditLocalizationService _auditLocalizationService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILoggerManager _logger;
        private readonly IAuditLocalizationService _localizationService;

        public GetResolutionAuditHistoryQueryHandler(
            IRepositoryManager repository,
            IMapper mapper,
            IStringLocalizer<SharedResources> localizer,
            IAuditLocalizationService auditLocalizationService,
            ICurrentUserService currentUserService,
           IAuditLocalizationService localizationService,
            ILoggerManager logger)
        {
            _repository = repository;
            _mapper = mapper;
            _localizer = localizer;
            _auditLocalizationService = auditLocalizationService;
            _currentUserService = currentUserService;
            _logger = logger;
            _localizationService = localizationService;
        }

        public async Task<BaseResponse<List<ResolutionStatusHistoryDto>>> Handle(GetResolutionAuditHistoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo($"Getting localized audit history for resolution ID: {request.ResolutionId}");

                // 1. Validate request
                if (request == null)
                {
                    return PaginatedResult<ResolutionStatusHistoryDto>.ServerError(_localizer[SharedResourcesKey.SystemErrorRetrievingData]);
                }

                // 2. Get resolution to verify it exists
                var resolution = await _repository.Resolutions.GetByIdAsync<Resolution>(request.ResolutionId, trackChanges: false);
                if (resolution == null)
                {
                    return PaginatedResult<ResolutionStatusHistoryDto>.ServerError(_localizer[SharedResourcesKey.ResolutionNotFound]);
                }

                // 3. Get audit history entries for the resolution with necessary includes
                var auditHistoryEntries = _repository.ResolutionStatusHistory.GetHistoryByResolutionIdAsync(request.ResolutionId, trackChanges: false);

                if (!auditHistoryEntries.Any())
                {
                    return new PaginatedResult<ResolutionStatusHistoryDto>();
                }

                //4. Map to DTOs
                var historyData =   auditHistoryEntries.ToList();
                var statusHistoryDtos = _mapper.Map<List<ResolutionStatusHistoryDto>>(historyData);
                // 5. Apply localization to the DTOs
                foreach (var item in statusHistoryDtos)
                {
                    item.DisplayedAction = _localizationService.GetLocalizedActionName(item.Action, null);
                    item.DisplayedStatus = _localizationService.GetLocalizedStatusName(item.Status, null);
                    item.DisplayedUserRole = _localizationService.GetLocalizedRole(item.UserRole, null);
                }

                _logger.LogInfo($"Retrieved {statusHistoryDtos.Count} localized audit history entries for resolution ID: {request.ResolutionId}");

                return Success(statusHistoryDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving localized audit history for resolution ID: {request.ResolutionId}");
                return PaginatedResult<ResolutionStatusHistoryDto>.ServerError(_localizer[SharedResourcesKey.SystemErrorRetrievingData]);
            }
        }
    }
}