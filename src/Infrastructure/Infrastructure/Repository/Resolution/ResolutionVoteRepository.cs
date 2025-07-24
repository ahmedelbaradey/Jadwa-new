using Abstraction.Contract.Service;
using Abstraction.Contracts.Repository.Resolution;
using Domain.Entities.ResolutionManagement;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Resolution
{
    /// <summary>
    /// Repository implementation for ResolutionVote entity operations
    /// Inherits from GenericRepository and implements IResolutionVoteRepository
    /// Provides specific methods for voting business logic
    /// </summary>
    public class ResolutionVoteRepository : GenericRepository, IResolutionVoteRepository
    {
        #region Constructor
        
        public ResolutionVoteRepository(AppDbContext repositoryContext, ICurrentUserService currentUserService)
            : base(repositoryContext, currentUserService)
        {
        }
        
        #endregion
        
        #region IResolutionVoteRepository Implementation
        
        /// <summary>
        /// Gets all active votes for a specific resolution
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of active votes for the resolution</returns>
        public async Task<IEnumerable<ResolutionVote>> GetVotesByResolutionIdAsync(int resolutionId, bool trackChanges = false)
        {
            var query = GetByCondition<ResolutionVote>(
                rv => rv.ResolutionId == resolutionId && rv.IsActive, 
                trackChanges);
                
            return await query
                .Include(rv => rv.BoardMember)
                .Include(rv => rv.ResolutionItem)
                .OrderBy(rv => rv.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all active votes for a specific resolution item
        /// </summary>
        /// <param name="resolutionItemId">Resolution item identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of active votes for the resolution item</returns>
        public async Task<IEnumerable<ResolutionVote>> GetVotesByResolutionItemIdAsync(int resolutionItemId, bool trackChanges = false)
        {
            var query = GetByCondition<ResolutionVote>(
                rv => rv.ResolutionItemId == resolutionItemId && rv.IsActive, 
                trackChanges);
                
            return await query
                .Include(rv => rv.BoardMember)
                .OrderBy(rv => rv.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all votes cast by a specific board member
        /// </summary>
        /// <param name="boardMemberId">Board member identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of votes cast by the board member</returns>
        public async Task<IEnumerable<ResolutionVote>> GetVotesByBoardMemberIdAsync(int boardMemberId, bool trackChanges = false)
        {
            var query = GetByCondition<ResolutionVote>(
                rv => rv.BoardMemberId == boardMemberId, 
                trackChanges);
                
            return await query
                .Include(rv => rv.Resolution)
                .Include(rv => rv.ResolutionItem)
                .OrderByDescending(rv => rv.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Gets a specific vote by board member and resolution
        /// </summary>
        /// <param name="boardMemberId">Board member identifier</param>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="resolutionItemId">Resolution item identifier (optional)</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Vote cast by the member on the resolution/item or null</returns>
        public async Task<ResolutionVote?> GetVoteByMemberAndResolutionAsync(
            int boardMemberId, 
            int resolutionId, 
            int? resolutionItemId = null, 
            bool trackChanges = false)
        {
            var query = GetByCondition<ResolutionVote>(
                rv => rv.BoardMemberId == boardMemberId && 
                      rv.ResolutionId == resolutionId && 
                      rv.ResolutionItemId == resolutionItemId && 
                      rv.IsActive, 
                trackChanges);
                
            return await query
                .Include(rv => rv.BoardMember)
                .Include(rv => rv.ResolutionItem)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets votes for a specific voting session
        /// </summary>
        /// <param name="votingSessionId">Voting session identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of votes for the voting session</returns>
        //public async Task<IEnumerable<ResolutionVote>> GetVotesBySessionIdAsync(Guid votingSessionId, bool trackChanges = false)
        //{
        //    var query = GetByCondition<ResolutionVote>(
        //        rv => rv.VotingSessionId == votingSessionId, 
        //        trackChanges);
                
        //    return await query
        //        .Include(rv => rv.BoardMember)
        //        .Include(rv => rv.ResolutionItem)
        //        .OrderBy(rv => rv.CreatedAt)
        //        .ToListAsync();
        //}

        /// <summary>
        /// Checks if a board member has already voted on a resolution
        /// </summary>
        /// <param name="boardMemberId">Board member identifier</param>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="resolutionItemId">Resolution item identifier (optional)</param>
        /// <returns>True if member has voted, false otherwise</returns>
        public async Task<bool> HasMemberVotedAsync(int boardMemberId, int resolutionId, int? resolutionItemId = null)
        {
            var query = GetByCondition<ResolutionVote>(
                rv => rv.BoardMemberId == boardMemberId && 
                      rv.ResolutionId == resolutionId && 
                      rv.ResolutionItemId == resolutionItemId && 
                      rv.IsActive, 
                false);
                
            return await query.AnyAsync();
        }

        /// <summary>
        /// Gets voting statistics for a resolution
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <returns>Voting statistics including counts by vote value</returns>
        public async Task<VotingStatistics> GetVotingStatisticsAsync(int resolutionId)
        {
            var votes = await GetByCondition<ResolutionVote>(
                rv => rv.ResolutionId == resolutionId && rv.IsActive && rv.ResolutionItemId == null, 
                false).ToListAsync();

            return new VotingStatistics
            {
                TotalVotes = votes.Count,
                ApproveVotes = votes.Count(v => v.VoteValue == VoteValue.Approve),
                RejectVotes = votes.Count(v => v.VoteValue == VoteValue.Reject),
                AbstainVotes = votes.Count(v => v.VoteValue == VoteValue.Abstain)
            };
        }

        /// <summary>
        /// Gets voting statistics for a resolution item
        /// </summary>
        /// <param name="resolutionItemId">Resolution item identifier</param>
        /// <returns>Voting statistics for the item</returns>
        public async Task<VotingStatistics> GetItemVotingStatisticsAsync(int resolutionItemId)
        {
            var votes = await GetByCondition<ResolutionVote>(
                rv => rv.ResolutionItemId == resolutionItemId && rv.IsActive, 
                false).ToListAsync();

            return new VotingStatistics
            {
                TotalVotes = votes.Count,
                ApproveVotes = votes.Count(v => v.VoteValue == VoteValue.Approve),
                RejectVotes = votes.Count(v => v.VoteValue == VoteValue.Reject),
                AbstainVotes = votes.Count(v => v.VoteValue == VoteValue.Abstain)
            };
        }

        /// <summary>
        /// Suspends all active votes for a resolution (marks as inactive)
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <returns>Number of votes suspended</returns>
        public async Task<int> SuspendActiveVotesAsync(int resolutionId)
        {
            var activeVotes = await GetByCondition<ResolutionVote>(
                rv => rv.ResolutionId == resolutionId && rv.IsActive, 
                true).ToListAsync();

            foreach (var vote in activeVotes)
            {
                vote.IsActive = false;
            }

            await RepositoryContext.SaveChangesAsync(_currentUserService.UserId.GetValueOrDefault());
            return activeVotes.Count;
        }

        /// <summary>
        /// Gets the current voting session ID for a resolution
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <returns>Current voting session ID or null if no active voting</returns>
        //public async Task<Guid?> GetCurrentVotingSessionAsync(int resolutionId)
        //{
        //    var latestVote = await GetByCondition<ResolutionVote>(
        //        rv => rv.ResolutionId == resolutionId && rv.IsActive, 
        //        false)
        //        .OrderByDescending(rv => rv.CreatedAt)
        //        .FirstOrDefaultAsync();

        //    return latestVote?.VotingSessionId;
        //}

        /// <summary>
        /// Gets all votes for a resolution with member and item details
        /// </summary>
        /// <param name="resolutionId">Resolution identifier</param>
        /// <param name="trackChanges">Whether to track changes for updates</param>
        /// <returns>Collection of votes with related data</returns>
        public async Task<IEnumerable<ResolutionVote>> GetVotesWithDetailsAsync(int resolutionId, bool trackChanges = false)
        {
            var query = GetByCondition<ResolutionVote>(
                rv => rv.ResolutionId == resolutionId && rv.IsActive, 
                trackChanges);
                
            return await query
                .Include(rv => rv.BoardMember)
                    .ThenInclude(bm => bm.User)
                .Include(rv => rv.ResolutionItem)
                .Include(rv => rv.Resolution)
                .OrderBy(rv => rv.ResolutionItemId)
                .ThenBy(rv => rv.CreatedAt)
                .ToListAsync();
        }
        
        #endregion
    }
}
