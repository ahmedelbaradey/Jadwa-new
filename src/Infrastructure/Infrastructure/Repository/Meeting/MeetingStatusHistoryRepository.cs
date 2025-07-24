using Abstraction.Contract.Repository.Meeting;
using Domain.Entities.MeetingManagement;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Abstraction.Contract.Service;

namespace Infrastructure.Repository.Meeting
{
    /// <summary>
    /// Repository implementation for MeetingStatusHistory entity operations
    /// Inherits from GenericRepository to provide standard CRUD operations
    /// Implements specific methods for meeting status history business logic
    /// Follows the exact same pattern as ResolutionStatusHistoryRepository
    /// </summary>
    public class MeetingStatusHistoryRepository : GenericRepository<MeetingStatusHistory>, IMeetingStatusHistoryRepository
    {
        private readonly AppDbContext _context;

        public MeetingStatusHistoryRepository(AppDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all status history entries for a specific meeting
        /// </summary>
        public async Task<IEnumerable<MeetingStatusHistory>> GetHistoryByMeetingIdAsync(int meetingId, bool trackChanges = false)
        {
            var query = _context.MeetingStatusHistories
                .Include(h => h.ChangedByUser)
                .Include(h => h.Meeting)
                .Where(h => h.MeetingId == meetingId);

            if (!trackChanges)
                query = query.AsNoTracking();

            return await query
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Gets the latest status history entry for a specific meeting
        /// </summary>
        public async Task<MeetingStatusHistory?> GetLatestHistoryByMeetingIdAsync(int meetingId, bool trackChanges = false)
        {
            var query = _context.MeetingStatusHistories
                .Include(h => h.ChangedByUser)
                .Where(h => h.MeetingId == meetingId);

            if (!trackChanges)
                query = query.AsNoTracking();

            return await query
                .OrderByDescending(h => h.ChangedAt)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets status history entries for a specific meeting and status
        /// </summary>
        public async Task<IEnumerable<MeetingStatusHistory>> GetHistoryByMeetingAndStatusAsync(int meetingId, MeetingStatusEnum status, bool trackChanges = false)
        {
            var query = _context.MeetingStatusHistories
                .Include(h => h.ChangedByUser)
                .Where(h => h.MeetingId == meetingId && h.NewStatus == status);

            if (!trackChanges)
                query = query.AsNoTracking();

            return await query
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Gets status history entries performed by a specific user
        /// </summary>
        public async Task<IEnumerable<MeetingStatusHistory>> GetHistoryByUserIdAsync(int userId, bool trackChanges = false)
        {
            var query = _context.MeetingStatusHistories
                .Include(h => h.Meeting)
                .Include(h => h.ChangedByUser)
                .Where(h => h.ChangedBy == userId);

            if (!trackChanges)
                query = query.AsNoTracking();

            return await query
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Gets status history entries for a specific fund
        /// </summary>
        public async Task<IEnumerable<MeetingStatusHistory>> GetHistoryByFundIdAsync(int fundId, bool trackChanges = false)
        {
            var query = _context.MeetingStatusHistories
                .Include(h => h.Meeting)
                .Include(h => h.ChangedByUser)
                .Where(h => h.Meeting.FundId == fundId);

            if (!trackChanges)
                query = query.AsNoTracking();

            return await query
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Gets status history entries within a date range
        /// </summary>
        public async Task<IEnumerable<MeetingStatusHistory>> GetHistoryByDateRangeAsync(DateTime startDate, DateTime endDate, bool trackChanges = false)
        {
            var query = _context.MeetingStatusHistories
                .Include(h => h.Meeting)
                .Include(h => h.ChangedByUser)
                .Where(h => h.ChangedAt >= startDate && h.ChangedAt <= endDate);

            if (!trackChanges)
                query = query.AsNoTracking();

            return await query
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Checks if a meeting has any status history entries
        /// </summary>
        public async Task<bool> HasHistoryAsync(int meetingId)
        {
            return await _context.MeetingStatusHistories
                .AnyAsync(h => h.MeetingId == meetingId);
        }

        /// <summary>
        /// Gets the count of status changes for a specific meeting
        /// </summary>
        public async Task<int> GetStatusChangeCountAsync(int meetingId)
        {
            return await _context.MeetingStatusHistories
                .Where(h => h.MeetingId == meetingId)
                .CountAsync();
        }
    }
}
