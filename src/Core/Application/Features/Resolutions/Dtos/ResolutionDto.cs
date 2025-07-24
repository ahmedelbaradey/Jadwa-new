using Abstraction.Base.Dto;
using Domain.Entities.FundManagement;
using Domain.Entities.ResolutionManagement;

namespace Application.Features.Resolutions.Dtos
{
    /// <summary>
    /// Base Data Transfer Object for Resolution entity
    /// Contains core resolution properties following Clean Architecture principles
    /// Based on requirements in Sprint.md for resolution management (JDWA-511, JDWA-588, etc.)
    /// Follows Clean DTOs Implementation Reference Template patterns
    /// </summary>
    /// 
    public record ResolutionDto : BaseDto
    {
        /// <summary>
        /// Auto-generated resolution code (fund code/resolution year/resolution no.)
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Date of the resolution
        /// </summary>
        public DateTime ResolutionDate { get; set; }

        /// <summary>
        /// Description of the resolution (optional, max 500 characters)
        /// </summary>
        public string? Description { get; set; } = null;

        /// <summary>
        /// Resolution type identifier
        /// </summary>
        public int ResolutionTypeId { get; set; }

        /// <summary>
        /// Custom resolution type name when 'Other' is selected
        /// </summary>
        public string? NewType { get; set; } = null;

        /// <summary>
        /// Main attachment identifier
        /// </summary>
        public int AttachmentId { get; set; }

        /// <summary>
        /// Voting methodology (AllMembers=1, Majority=2)
        /// </summary>
        public VotingType VotingType { get; set; }

        /// <summary>
        /// How member voting results are calculated
        /// </summary>
        public MemberVotingResult MemberVotingResult { get; set; }

        /// <summary>
        /// Current status of the resolution
        /// </summary>
        public ResolutionStatusEnum Status { get; set; }

        /// <summary>
        /// Fund identifier that this resolution belongs to
        /// </summary>
        public int FundId { get; set; }

        /// <summary>
        /// Parent resolution identifier for tracking relationships
        /// </summary>
        public int? ParentResolutionId { get; set; } = null;

        /// <summary>
        /// Old resolution code when this is an update to existing resolution
        /// </summary>
        public string? OldResolutionCode { get; set; } = null;

    }
}
