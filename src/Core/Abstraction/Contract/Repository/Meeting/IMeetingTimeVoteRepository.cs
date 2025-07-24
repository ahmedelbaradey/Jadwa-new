using Domain.Entities.MeetingManagement;

namespace Abstraction.Contract.Repository.Meeting
{
    /// <summary>
    /// Repository interface for MeetingTimeVote entity operations
    /// Inherits from IGenericRepository to provide standard CRUD operations
    /// Includes specific methods for meeting time vote business logic
    /// </summary>
    public interface IMeetingTimeVoteRepository : IGenericRepository
    {
        /// <summary>
        /// Gets all votes for a specific proposal
        /// </summary>
        /// <param name="proposalId">Proposal identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of votes for the proposal</returns>
        Task<IEnumerable<MeetingTimeVote>> GetVotesByProposalIdAsync(int proposalId, bool trackChanges = false);

        /// <summary>
        /// Gets a specific vote by proposal and user
        /// </summary>
        /// <param name="proposalId">Proposal identifier</param>
        /// <param name="userId">User identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Vote if exists, null otherwise</returns>
        Task<MeetingTimeVote?> GetVoteByProposalAndUserAsync(int proposalId, int userId, bool trackChanges = false);

        /// <summary>
        /// Gets vote count for each proposed date in a proposal
        /// </summary>
        /// <param name="proposalId">Proposal identifier</param>
        /// <returns>Dictionary with proposed date ID as key and vote count as value</returns>
        Task<Dictionary<int, int>> GetVoteCountsByProposedDateAsync(int proposalId);

        /// <summary>
        /// Gets all votes cast by a specific user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of votes cast by the user</returns>
        Task<IEnumerable<MeetingTimeVote>> GetVotesByUserIdAsync(int userId, bool trackChanges = false);
    }
}
