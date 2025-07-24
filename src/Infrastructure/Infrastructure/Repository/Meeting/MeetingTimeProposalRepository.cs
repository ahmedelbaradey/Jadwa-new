using Abstraction.Contract.Repository.Meeting;
using Domain.Entities.MeetingManagement;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Abstraction.Contract.Service;

namespace Infrastructure.Repository.Meeting
{
    /// <summary>
    /// Repository implementation for MeetingTimeProposal entity operations
    /// Inherits from GenericRepository to provide standard CRUD operations
    /// Implements specific methods for meeting time proposal business logic
    /// </summary>
    public class MeetingTimeProposalRepository : GenericRepository<MeetingTimeProposal>, IMeetingTimeProposalRepository
    {
        private readonly AppDbContext _context;

        public MeetingTimeProposalRepository(AppDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all meeting time proposals for a specific fund
        /// </summary>
        public async Task<IEnumerable<MeetingTimeProposal>> GetProposalsByFundIdAsync(int fundId, bool trackChanges = false)
        {
            var query = _context.MeetingTimeProposals
                .Include(p => p.ProposedDates)
                .Include(p => p.CreatedByUser)
                .Include(p => p.Attachment)
                .Where(p => p.FundId == fundId);

            if (!trackChanges)
                query = query.AsNoTracking();

            return await query
                .OrderByDescending(p => p.CreationTime)
                .ToListAsync();
        }

        /// <summary>
        /// Gets a meeting time proposal by ID with all related data
        /// </summary>
        public async Task<MeetingTimeProposal?> GetProposalWithDetailsAsync(int proposalId, bool trackChanges = false)
        {
            var query = _context.MeetingTimeProposals
                .Include(p => p.ProposedDates)
                .Include(p => p.CreatedByUser)
                .Include(p => p.Fund)
                .Include(p => p.Attachment)
                .Include(p => p.Votes)
                    .ThenInclude(v => v.User)
                .Where(p => p.Id == proposalId);

            if (!trackChanges)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets all active (Under Voting) proposals for a specific fund
        /// </summary>
        public async Task<IEnumerable<MeetingTimeProposal>> GetActiveProposalsByFundIdAsync(int fundId, bool trackChanges = false)
        {
            var query = _context.MeetingTimeProposals
                .Include(p => p.ProposedDates)
                .Include(p => p.CreatedByUser)
                .Where(p => p.FundId == fundId && p.Status == "Under Voting");

            if (!trackChanges)
                query = query.AsNoTracking();

            return await query
                .OrderByDescending(p => p.CreationTime)
                .ToListAsync();
        }

        /// <summary>
        /// Checks if a user has already voted on a specific proposal
        /// </summary>
        public async Task<bool> HasUserVotedAsync(int proposalId, int userId)
        {
            return await _context.MeetingTimeVotes
                .AnyAsync(v => v.ProposalId == proposalId && v.UserId == userId);
        }

        /// <summary>
        /// Gets the total number of board members for a fund (for voting completion check)
        /// </summary>
        public async Task<int> GetBoardMemberCountAsync(int fundId)
        {
            return await _context.BoardMembers
                .Where(bm => bm.FundId == fundId && !bm.IsDeleted)
                .CountAsync();
        }

        /// <summary>
        /// Gets the total number of votes cast for a specific proposal
        /// </summary>
        public async Task<int> GetVoteCountAsync(int proposalId)
        {
            return await _context.MeetingTimeVotes
                .Where(v => v.ProposalId == proposalId)
                .Select(v => v.UserId)
                .Distinct()
                .CountAsync();
        }

        /// <summary>
        /// Gets all board members for a specific fund (for notification purposes)
        /// </summary>
        public async Task<IEnumerable<int>> GetBoardMemberUserIdsAsync(int fundId)
        {
            return await _context.BoardMembers
                .Where(bm => bm.FundId == fundId && !bm.IsDeleted)
                .Select(bm => bm.UserId)
                .ToListAsync();
        }
    }
}
