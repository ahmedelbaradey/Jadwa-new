using Abstraction.Contract.Repository.AssessmentManagement;
using Abstraction.Contract.Service;
using Domain.Entities.AssessmentManagement;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.AssessmentManagement
{
    /// <summary>
    /// Repository implementation for Assessment entity operations
    /// Provides data access functionality using Entity Framework Core
    /// Based on existing repository patterns in the codebase
    /// </summary>
    public class AssessmentRepository : GenericRepository, IAssessmentRepository
    {
        public AssessmentRepository(AppDbContext repositoryContext, ICurrentUserService currentUserService)
            : base(repositoryContext, currentUserService)
        {
        }

        /// <summary>
        /// Gets assessments by fund ID with optional filtering
        /// </summary>
        /// <param name="fundId">Fund ID to filter by</param>
        /// <param name="status">Optional status filter</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Collection of assessments</returns>
        public async Task<IEnumerable<Assessment>> GetAssessmentsByFundIdAsync(int fundId, AssessmentStatus? status = null, bool trackChanges = false)
        {
            var query = GetByCondition<Assessment>(a => a.FundId == fundId, trackChanges)
                .Include(a => a.Fund)
                .Include(a => a.Reviewer)
                .Include(a => a.Questions)
                .Include(a => a.Responses);

            if (status.HasValue)
            {
                query = query.Where(a => a.Status == status.Value);
            }

            return await query.OrderByDescending(a => a.CreatedAt).ToListAsync();
        }

        /// <summary>
        /// Gets assessments waiting for approval by specific reviewer roles
        /// </summary>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Collection of assessments waiting for approval</returns>
        public async Task<IEnumerable<Assessment>> GetAssessmentsWaitingForApprovalAsync(bool trackChanges = false)
        {
            return await GetByCondition<Assessment>(a => a.Status == AssessmentStatus.WaitingForApproval, trackChanges)
                .Include(a => a.Fund)
                .Include(a => a.Questions)
                .OrderBy(a => a.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Gets active assessments that board members can respond to
        /// </summary>
        /// <param name="userId">Board member user ID</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Collection of active assessments for the user</returns>
        public async Task<IEnumerable<Assessment>> GetActiveAssessmentsForUserAsync(int userId, bool trackChanges = false)
        {
            return await GetByCondition<Assessment>(a => a.Status == AssessmentStatus.Active, trackChanges)
                .Include(a => a.Fund)
                .Include(a => a.Questions)
                .Include(a => a.Responses.Where(r => r.UserId == userId))
                .Where(a => a.Fund.FundBoardMembers.Any(fbm => fbm.UserId == userId))
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Gets assessment with all related data (questions, responses, etc.)
        /// </summary>
        /// <param name="assessmentId">Assessment ID</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Assessment with related data</returns>
        public async Task<Assessment?> GetAssessmentWithDetailsAsync(int assessmentId, bool trackChanges = false)
        {
            return await GetByCondition<Assessment>(a => a.Id == assessmentId, trackChanges)
                .Include(a => a.Fund)
                .Include(a => a.Reviewer)
                .Include(a => a.Questions.OrderBy(q => q.DisplayOrder))
                .Include(a => a.Responses)
                    .ThenInclude(r => r.User)
                .Include(a => a.Responses)
                    .ThenInclude(r => r.Answers)
                        .ThenInclude(ans => ans.Question)
                .Include(a => a.Attachment)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets assessment with questions only
        /// </summary>
        /// <param name="assessmentId">Assessment ID</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Assessment with questions</returns>
        public async Task<Assessment?> GetAssessmentWithQuestionsAsync(int assessmentId, bool trackChanges = false)
        {
            return await GetByCondition<Assessment>(a => a.Id == assessmentId, trackChanges)
                .Include(a => a.Questions.OrderBy(q => q.DisplayOrder))
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets assessments by creator user ID
        /// </summary>
        /// <param name="createdBy">Creator user ID</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Collection of assessments created by the user</returns>
        public async Task<IEnumerable<Assessment>> GetAssessmentsByCreatorAsync(int createdBy, bool trackChanges = false)
        {
            return await GetByCondition<Assessment>(a => a.CreatedBy == createdBy, trackChanges)
                .Include(a => a.Fund)
                .Include(a => a.Questions)
                .Include(a => a.Responses)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Checks if an assessment title already exists for a fund
        /// </summary>
        /// <param name="fundId">Fund ID</param>
        /// <param name="title">Assessment title</param>
        /// <param name="excludeAssessmentId">Assessment ID to exclude from check (for updates)</param>
        /// <returns>True if title exists, false otherwise</returns>
        public async Task<bool> IsTitleExistsAsync(int fundId, string title, int? excludeAssessmentId = null)
        {
            var query = GetByCondition<Assessment>(a => a.FundId == fundId && a.Title.ToLower() == title.ToLower(), false);
            
            if (excludeAssessmentId.HasValue)
            {
                query = query.Where(a => a.Id != excludeAssessmentId.Value);
            }

            return await query.AnyAsync();
        }
    }
}
