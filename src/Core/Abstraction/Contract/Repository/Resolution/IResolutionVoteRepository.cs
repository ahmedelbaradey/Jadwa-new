using Domain.Entities.ResolutionManagement;

namespace Abstraction.Contracts.Repository.Resolution
{
    /// <summary>
    /// Repository interface for ResolutionVote entity operations
    /// Inherits from IGenericRepository to provide standard CRUD operations
    /// Includes specific methods for voting business logic
    /// </summary>
    public interface IResolutionVoteRepository : IGenericRepository
    {
        /// <summary>
        /// Gets all active votes for a specific resolution
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of active votes for the resolution</returns>
        Task<IEnumerable<ResolutionVote>> GetVotesByResolutionIdAsync(int resolutionId, bool trackChanges = false);

        /// <summary>
        /// Gets all active votes for a specific resolution item
        /// </summary>
        /// <param name="resolutionItemId">Resolution item identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of active votes for the resolution item</returns>
        Task<IEnumerable<ResolutionVote>> GetVotesByResolutionItemIdAsync(int resolutionItemId, bool trackChanges = false);

        /// <summary>
        /// Gets all votes cast by a specific board member
        /// </summary>
        /// <param name="boardMemberId">Board member identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of votes cast by the board member</returns>
        Task<IEnumerable<ResolutionVote>> GetVotesByBoardMemberIdAsync(int boardMemberId, bool trackChanges = false);

        /// <summary>
        /// Gets a specific vote by board member and resolution
        /// </summary>
        /// <param name="boardMemberId">Board member identifier</param>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="resolutionItemId">Resolution item identifier (optional)</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Vote cast by the member on the resolution/item or null</returns>
        Task<ResolutionVote?> GetVoteByMemberAndResolutionAsync(
            int boardMemberId, 
            int resolutionId, 
            int? resolutionItemId = null, 
            bool trackChanges = false);

        /// <summary>
        /// Gets votes for a specific voting session
        /// </summary>
        /// <param name="votingSessionId">Voting session identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of votes for the voting session</returns>
        //Task<IEnumerable<ResolutionVote>> GetVotesBySessionIdAsync(Guid votingSessionId, bool trackChanges = false);

        /// <summary>
        /// Checks if a board member has already voted on a resolution
        /// </summary>
        /// <param name="boardMemberId">Board member identifier</param>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="resolutionItemId">Resolution item identifier (optional)</param>
        /// <returns>True if member has voted, false otherwise</returns>
        Task<bool> HasMemberVotedAsync(int boardMemberId, int resolutionId, int? resolutionItemId = null);

        /// <summary>
        /// Gets voting statistics for a resolution
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <returns>Voting statistics including counts by vote value</returns>
        Task<VotingStatistics> GetVotingStatisticsAsync(int resolutionId);

        /// <summary>
        /// Gets voting statistics for a resolution item
        /// </summary>
        /// <param name="resolutionItemId">Resolution item identifier</param>
        /// <returns>Voting statistics for the item</returns>
        Task<VotingStatistics> GetItemVotingStatisticsAsync(int resolutionItemId);

        /// <summary>
        /// Suspends all active votes for a resolution (marks as inactive)
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <returns>Number of votes suspended</returns>
        Task<int> SuspendActiveVotesAsync(int resolutionId);

        /// <summary>
        /// Gets the current voting session ID for a resolution
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <returns>Current voting session ID or null if no active voting</returns>
        //Task<Guid?> GetCurrentVotingSessionAsync(int resolutionId);

        /// <summary>
        /// Gets all votes for a resolution with member and item details
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of votes with related data</returns>
        Task<IEnumerable<ResolutionVote>> GetVotesWithDetailsAsync(int resolutionId, bool trackChanges = false);
    }

    /// <summary>
    /// Represents voting statistics for a resolution or resolution item
    /// </summary>
    public class VotingStatistics
    {
        public int TotalVotes { get; set; }
        public int ApproveVotes { get; set; }
        public int RejectVotes { get; set; }
        public int AbstainVotes { get; set; }
        public int EligibleVoters { get; set; }
        public double ParticipationRate => EligibleVoters > 0 ? (double)TotalVotes / EligibleVoters * 100 : 0;
        public double ApprovalRate => TotalVotes > 0 ? (double)ApproveVotes / TotalVotes * 100 : 0;
    }
}
