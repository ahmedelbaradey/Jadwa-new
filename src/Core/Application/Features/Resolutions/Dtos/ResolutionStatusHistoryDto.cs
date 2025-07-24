using Abstraction.Base.Dto;
using Abstraction.Constants;
using Domain.Entities.ResolutionManagement;

namespace Application.Features.Resolutions.Dtos
{
    /// <summary>
    /// Data Transfer Object for ResolutionStatusHistory entity
    /// Contains status change history for resolutions
    /// Used for audit trail and status tracking
    /// </summary>
    public record ResolutionStatusHistoryDto : BaseDto
    {
        /// <summary>
        /// Resolution identifier
        /// </summary>
        public int ResolutionId { get; set; }

        /// <summary>
        /// Rejection reason when status is changed to "Rejected"
        /// Required when fund manager rejects a resolution
        /// </summary>
        public string? RejectionReason { get; set; } = string.Empty;

        /// <summary>
        /// User who made the status change
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// Date and time of the status change
        /// </summary>
        public DateTime CreatedAt { get; set; }


        /// <summary>
        /// New status of the resolution after this action
        /// Used to track status transitions
        /// </summary>
        public ResolutionStatusEnum Status { get; set; }
        /// <summary>
        /// Resolution status information with localization
        /// </summary>
        public string DisplayedStatus { get; set; }

        /// <summary>
        /// Resolution status information with localization
        /// </summary>
        public string DisplayedAction { get; set; }

        /// <summary>
        /// Resolution status information with localization
        /// </summary>
        public ResolutionActionEnum Action { get; set; }

        /// <summary>
        /// Resolution status information with localization
        /// </summary>
        public string UserRole { get; set; }
        /// <summary>
        /// Resolution status information with localization
        /// </summary>
        public string DisplayedUserRole { get; set; }
         

        /// <summary>
        /// User full name who made the change
        /// </summary>
        public string FullName { get; set; } = string.Empty;



      
    }


    /// <summary>
    /// Data Transfer Object for casting a vote
    /// Used in CastVote command
    /// </summary>
    public record CastVoteRequest
    {
        /// <summary>
        /// Resolution identifier
        /// </summary>
        public int ResolutionId { get; set; }

        /// <summary>
        /// Board member identifier who is casting the vote
        /// </summary>
        public int BoardMemberId { get; set; }

        /// <summary>
        /// Vote value (Approve, Reject, Abstain)
        /// </summary>
        public VoteValue VoteValue { get; set; }

        /// <summary>
        /// Optional comments about the vote
        /// </summary>
        public string? Comments { get; set; }
    }

    /// <summary>
    /// Response DTO for voting summary
    /// Contains aggregated voting information
    /// </summary>
    public record VotingSummaryDto
    {
        /// <summary>
        /// Resolution identifier
        /// </summary>
        public int ResolutionId { get; set; }

        /// <summary>
        /// Total number of eligible voters
        /// </summary>
        public int TotalEligibleVoters { get; set; }

        /// <summary>
        /// Number of votes cast
        /// </summary>
        public int VotesCast { get; set; }

        /// <summary>
        /// Number of approve votes
        /// </summary>
        public int ApproveVotes { get; set; }

        /// <summary>
        /// Number of reject votes
        /// </summary>
        public int RejectVotes { get; set; }

        /// <summary>
        /// Number of abstain votes
        /// </summary>
        public int AbstainVotes { get; set; }

        /// <summary>
        /// Voting result based on methodology
        /// </summary>
        public VotingResult Result { get; set; }

        /// <summary>
        /// Indicates if voting is complete
        /// </summary>
        public bool IsVotingComplete { get; set; }

        /// <summary>
        /// Collection of individual votes
        /// </summary>
        public IEnumerable<ResolutionVoteDto> Votes { get; set; } = new List<ResolutionVoteDto>();

        /// <summary>
        /// Collection of members who haven't voted yet
        /// </summary>
        public IEnumerable<string> PendingVoters { get; set; } = new List<string>();
    }

    /// <summary>
    /// Enum for voting results
    /// </summary>
    public enum VotingResult
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        Tied = 3
    }
}
