using Domain.Entities.MeetingManagement;

namespace Abstraction.Contract.Repository.Meeting
{
    /// <summary>
    /// Repository interface for MeetingTimeProposal entity operations
    /// Inherits from IGenericRepository to provide standard CRUD operations
    /// Includes specific methods for meeting time proposal business logic
    /// </summary>
    public interface IMeetingTimeProposalRepository : IGenericRepository
    {
        /// <summary>
        /// Gets all meeting time proposals for a specific fund
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of meeting time proposals</returns>
        Task<IEnumerable<MeetingTimeProposal>> GetProposalsByFundIdAsync(int fundId, bool trackChanges = false);

        /// <summary>
        /// Gets a meeting time proposal by ID with all related data
        /// </summary>
        /// <param name="proposalId">Proposal identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Meeting time proposal with related data</returns>
        Task<MeetingTimeProposal?> GetProposalWithDetailsAsync(int proposalId, bool trackChanges = false);

        /// <summary>
        /// Gets all active (Under Voting) proposals for a specific fund
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of active proposals</returns>
        Task<IEnumerable<MeetingTimeProposal>> GetActiveProposalsByFundIdAsync(int fundId, bool trackChanges = false);

        /// <summary>
        /// Checks if a user has already voted on a specific proposal
        /// </summary>
        /// <param name="proposalId">Proposal identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns>True if user has voted, false otherwise</returns>
        Task<bool> HasUserVotedAsync(int proposalId, int userId);

        /// <summary>
        /// Gets the total number of board members for a fund (for voting completion check)
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <returns>Number of active board members</returns>
        Task<int> GetBoardMemberCountAsync(int fundId);

        /// <summary>
        /// Gets the total number of votes cast for a specific proposal
        /// </summary>
        /// <param name="proposalId">Proposal identifier</param>
        /// <returns>Number of votes cast</returns>
        Task<int> GetVoteCountAsync(int proposalId);

        /// <summary>
        /// Gets all board members for a specific fund (for notification purposes)
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <returns>Collection of board member user IDs</returns>
        Task<IEnumerable<int>> GetBoardMemberUserIdsAsync(int fundId);
    }
}
