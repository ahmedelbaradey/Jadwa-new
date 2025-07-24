using Abstraction.Contract.Repository.AssessmentManagement;
using Abstraction.Contract.Service;
using Domain.Entities.AssessmentManagement;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.AssessmentManagement
{
    /// <summary>
    /// Repository implementation for AssessmentResponse entity operations
    /// Provides data access functionality using Entity Framework Core
    /// Based on existing repository patterns in the codebase
    /// </summary>
    public class AssessmentResponseRepository : GenericRepository, IAssessmentResponseRepository
    {
        public AssessmentResponseRepository(AppDbContext repositoryContext, ICurrentUserService currentUserService)
            : base(repositoryContext, currentUserService)
        {
        }

        /// <summary>
        /// Gets responses by assessment ID
        /// </summary>
        /// <param name="assessmentId">Assessment ID to filter by</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Collection of responses</returns>
        public async Task<IEnumerable<AssessmentResponse>> GetResponsesByAssessmentIdAsync(int assessmentId, bool trackChanges = false)
        {
            return await GetByCondition<AssessmentResponse>(r => r.AssessmentId == assessmentId, trackChanges)
                .Include(r => r.User)
                .Include(r => r.Assessment)
                .Include(r => r.Answers)
                    .ThenInclude(a => a.Question)
                .OrderBy(r => r.User.FullName)
                .ToListAsync();
        }

        /// <summary>
        /// Gets responses by user ID (for personal assessment view)
        /// </summary>
        /// <param name="userId">User ID to filter by</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Collection of responses</returns>
        public async Task<IEnumerable<AssessmentResponse>> GetResponsesByUserIdAsync(int userId, bool trackChanges = false)
        {
            return await GetByCondition<AssessmentResponse>(r => r.UserId == userId, trackChanges)
                .Include(r => r.Assessment)
                    .ThenInclude(a => a.Fund)
                .Include(r => r.Answers)
                    .ThenInclude(a => a.Question)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Gets a specific response with all answers
        /// </summary>
        /// <param name="responseId">Response ID</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Response with answers</returns>
        public async Task<AssessmentResponse?> GetResponseWithAnswersAsync(int responseId, bool trackChanges = false)
        {
            return await GetByCondition<AssessmentResponse>(r => r.Id == responseId, trackChanges)
                .Include(r => r.User)
                .Include(r => r.Assessment)
                    .ThenInclude(a => a.Questions.OrderBy(q => q.DisplayOrder))
                .Include(r => r.Answers)
                    .ThenInclude(a => a.Question)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets response by assessment and user (one response per user per assessment)
        /// </summary>
        /// <param name="assessmentId">Assessment ID</param>
        /// <param name="userId">User ID</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>User's response for the assessment</returns>
        public async Task<AssessmentResponse?> GetResponseByAssessmentAndUserAsync(int assessmentId, int userId, bool trackChanges = false)
        {
            return await GetByCondition<AssessmentResponse>(r => r.AssessmentId == assessmentId && r.UserId == userId, trackChanges)
                .Include(r => r.User)
                .Include(r => r.Assessment)
                    .ThenInclude(a => a.Questions.OrderBy(q => q.DisplayOrder))
                .Include(r => r.Answers)
                    .ThenInclude(a => a.Question)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Creates response records for all board members of a fund when assessment is distributed
        /// </summary>
        /// <param name="assessmentId">Assessment ID</param>
        /// <param name="fundId">Fund ID to get board members from</param>
        /// <returns>Number of response records created</returns>
        public async Task<int> CreateResponsesForBoardMembersAsync(int assessmentId, int fundId)
        {
            try
            {
                // Get all board members for the fund
                var boardMembers = await _repositoryContext.FundBoardMembers
                    .Where(fbm => fbm.FundId == fundId)
                    .Select(fbm => fbm.UserId)
                    .Distinct()
                    .ToListAsync();

                // Check which board members don't already have responses
                var existingResponses = await GetByCondition<AssessmentResponse>(
                    r => r.AssessmentId == assessmentId, false)
                    .Select(r => r.UserId)
                    .ToListAsync();

                var newBoardMembers = boardMembers.Except(existingResponses).ToList();

                // Create response records for new board members
                var responses = new List<AssessmentResponse>();
                foreach (var userId in newBoardMembers)
                {
                    responses.Add(new AssessmentResponse
                    {
                        AssessmentId = assessmentId,
                        UserId = userId,
                        Status = ResponseStatus.Pending
                    });
                }

                if (responses.Any())
                {
                    await _repositoryContext.AssessmentResponses.AddRangeAsync(responses);
                    await _repositoryContext.SaveChangesAsync();
                }

                return responses.Count;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets completion statistics for an assessment
        /// </summary>
        /// <param name="assessmentId">Assessment ID</param>
        /// <returns>Completion statistics (total, completed, pending)</returns>
        public async Task<(int Total, int Completed, int Pending)> GetCompletionStatisticsAsync(int assessmentId)
        {
            var responses = await GetByCondition<AssessmentResponse>(r => r.AssessmentId == assessmentId, false)
                .ToListAsync();

            var total = responses.Count;
            var completed = responses.Count(r => r.Status == ResponseStatus.Completed);
            var pending = total - completed;

            return (total, completed, pending);
        }

        /// <summary>
        /// Checks if a user has already responded to an assessment
        /// </summary>
        /// <param name="assessmentId">Assessment ID</param>
        /// <param name="userId">User ID</param>
        /// <returns>True if user has responded, false otherwise</returns>
        public async Task<bool> HasUserRespondedAsync(int assessmentId, int userId)
        {
            return await GetByCondition<AssessmentResponse>(
                r => r.AssessmentId == assessmentId && r.UserId == userId && r.Status == ResponseStatus.Completed, false)
                .AnyAsync();
        }
    }
}
