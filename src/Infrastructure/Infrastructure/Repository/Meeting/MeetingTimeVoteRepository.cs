using Abstraction.Contract.Repository.Meeting;
using Domain.Entities.MeetingManagement;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Abstraction.Contract.Service;

namespace Infrastructure.Repository.Meeting
{
    /// <summary>
    /// Repository implementation for MeetingTimeVote entity operations
    /// Inherits from GenericRepository to provide standard CRUD operations
    /// Implements specific methods for meeting time vote business logic
    /// </summary>
    public class MeetingTimeVoteRepository : GenericRepository<MeetingTimeVote>, IMeetingTimeVoteRepository
    {
        private readonly AppDbContext _context;

        public MeetingTimeVoteRepository(AppDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all votes for a specific proposal
        /// </summary>
        public async Task<IEnumerable<MeetingTimeVote>> GetVotesByProposalIdAsync(int proposalId, bool trackChanges = false)
        {
            var query = _context.MeetingTimeVotes
                .Include(v => v.User)
                .Include(v => v.ProposedDate)
                .Where(v => v.ProposalId == proposalId);

            if (!trackChanges)
                query = query.AsNoTracking();

            return await query
                .OrderBy(v => v.VoteTimestamp)
                .ToListAsync();
        }

        /// <summary>
        /// Gets a specific vote by proposal and user
        /// </summary>
        public async Task<MeetingTimeVote?> GetVoteByProposalAndUserAsync(int proposalId, int userId, bool trackChanges = false)
        {
            var query = _context.MeetingTimeVotes
                .Include(v => v.ProposedDate)
                .Where(v => v.ProposalId == proposalId && v.UserId == userId);

            if (!trackChanges)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets vote count for each proposed date in a proposal
        /// </summary>
        public async Task<Dictionary<int, int>> GetVoteCountsByProposedDateAsync(int proposalId)
        {
            return await _context.MeetingTimeVotes
                .Where(v => v.ProposalId == proposalId)
                .GroupBy(v => v.ProposedDateId)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        /// <summary>
        /// Gets all votes cast by a specific user
        /// </summary>
        public async Task<IEnumerable<MeetingTimeVote>> GetVotesByUserIdAsync(int userId, bool trackChanges = false)
        {
            var query = _context.MeetingTimeVotes
                .Include(v => v.Proposal)
                .Include(v => v.ProposedDate)
                .Where(v => v.UserId == userId);

            if (!trackChanges)
                query = query.AsNoTracking();

            return await query
                .OrderByDescending(v => v.VoteTimestamp)
                .ToListAsync();
        }
    }
}
