using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contracts.Repository;
using Application.Features.Resolutions.Dtos;
using AutoMapper;
using Domain.Entities.ResolutionManagement;
using Microsoft.Extensions.Logging;
using Abstraction.Contracts.Logger;
using Microsoft.Extensions.Localization;
using Resources;
using Application.Features.Funds.Dtos;
using System.Collections.Generic;

namespace Application.Features.Resolutions.Queries.GetResolutionTypes
{
    /// <summary>
    /// Handler for GetResolutionTypesQuery
    /// Retrieves all active resolution types from the database
    /// </summary>
    public class GetResolutionTypesQueryHandler : BaseResponseHandler, IQueryHandler<GetResolutionTypesQuery, BaseResponse<IEnumerable<ResolutionTypeDto>>>
    {
        private readonly ILoggerManager _logger;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public GetResolutionTypesQueryHandler(IRepositoryManager repositoryManager, IMapper mapper,ILoggerManager logger, IStringLocalizer<SharedResources> localizer)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _logger = logger;
            _localizer = localizer;
        }

        /// <summary>
        /// Handles the GetResolutionTypesQuery
        /// </summary>
        /// <param name="request">The query request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of resolution types</returns>
        public async Task<BaseResponse<IEnumerable<ResolutionTypeDto>>> Handle(GetResolutionTypesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Get active resolution types (we'll always use this method for consistency)
                var resolutionTypes = await _repositoryManager.ResolutionTypes.GetActiveResolutionTypesAsync(trackChanges: false);

                // Map to DTOs
                var resolutionTypeDtos = _mapper.Map<IEnumerable<ResolutionTypeDto>>(resolutionTypes);
                return Success(resolutionTypeDtos);
             
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: in GetResolutionTypesQueryHandler");
                return ServerError<IEnumerable<ResolutionTypeDto>> (ex.Message);
            }
        }
    }
}
