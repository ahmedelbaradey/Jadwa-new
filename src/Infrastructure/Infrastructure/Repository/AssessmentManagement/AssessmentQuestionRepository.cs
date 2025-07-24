using Abstraction.Contract.Repository.AssessmentManagement;
using Abstraction.Contract.Service;
using Domain.Entities.AssessmentManagement;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.AssessmentManagement
{
    /// <summary>
    /// Repository implementation for AssessmentQuestion entity operations
    /// Provides data access functionality using Entity Framework Core
    /// Based on existing repository patterns in the codebase
    /// </summary>
    public class AssessmentQuestionRepository : GenericRepository, IAssessmentQuestionRepository
    {
        public AssessmentQuestionRepository(AppDbContext repositoryContext, ICurrentUserService currentUserService)
            : base(repositoryContext, currentUserService)
        {
        }

        /// <summary>
        /// Gets questions by assessment ID ordered by display order
        /// </summary>
        /// <param name="assessmentId">Assessment ID to filter by</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Ordered collection of questions</returns>
        public async Task<IEnumerable<AssessmentQuestion>> GetQuestionsByAssessmentIdAsync(int assessmentId, bool trackChanges = false)
        {
            return await GetByCondition<AssessmentQuestion>(q => q.AssessmentId == assessmentId, trackChanges)
                .Include(q => q.Assessment)
                .OrderBy(q => q.DisplayOrder)
                .ThenBy(q => q.Id)
                .ToListAsync();
        }

        /// <summary>
        /// Gets a specific question with its answers
        /// </summary>
        /// <param name="questionId">Question ID</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Question with answers</returns>
        public async Task<AssessmentQuestion?> GetQuestionWithAnswersAsync(int questionId, bool trackChanges = false)
        {
            return await GetByCondition<AssessmentQuestion>(q => q.Id == questionId, trackChanges)
                .Include(q => q.Assessment)
                .Include(q => q.Answers)
                    .ThenInclude(a => a.Response)
                        .ThenInclude(r => r.User)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Updates the display order of questions for an assessment
        /// </summary>
        /// <param name="assessmentId">Assessment ID</param>
        /// <param name="questionOrders">Dictionary of question ID to display order</param>
        /// <returns>True if successful, false otherwise</returns>
        public async Task<bool> UpdateQuestionOrdersAsync(int assessmentId, Dictionary<int, int> questionOrders)
        {
            try
            {
                var questions = await GetByCondition<AssessmentQuestion>(q => q.AssessmentId == assessmentId, true)
                    .ToListAsync();

                foreach (var question in questions)
                {
                    if (questionOrders.TryGetValue(question.Id, out int newOrder))
                    {
                        question.DisplayOrder = newOrder;
                    }
                }

                await _repositoryContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes all questions for an assessment
        /// </summary>
        /// <param name="assessmentId">Assessment ID</param>
        /// <returns>True if successful, false otherwise</returns>
        public async Task<bool> DeleteQuestionsByAssessmentIdAsync(int assessmentId)
        {
            try
            {
                var questions = await GetByCondition<AssessmentQuestion>(q => q.AssessmentId == assessmentId, true)
                    .ToListAsync();

                foreach (var question in questions)
                {
                    await DeleteAsync(question);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
