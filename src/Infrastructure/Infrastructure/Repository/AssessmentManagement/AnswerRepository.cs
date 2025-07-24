using Abstraction.Contract.Repository.AssessmentManagement;
using Abstraction.Contract.Service;
using Domain.Entities.AssessmentManagement;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.AssessmentManagement
{
    /// <summary>
    /// Repository implementation for Answer entity operations
    /// Provides data access functionality using Entity Framework Core
    /// Based on existing repository patterns in the codebase
    /// </summary>
    public class AnswerRepository : GenericRepository, IAnswerRepository
    {
        public AnswerRepository(AppDbContext repositoryContext, ICurrentUserService currentUserService)
            : base(repositoryContext, currentUserService)
        {
        }

        /// <summary>
        /// Gets answers by response ID
        /// </summary>
        /// <param name="responseId">Response ID to filter by</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Collection of answers</returns>
        public async Task<IEnumerable<Answer>> GetAnswersByResponseIdAsync(int responseId, bool trackChanges = false)
        {
            return await GetByCondition<Answer>(a => a.ResponseId == responseId, trackChanges)
                .Include(a => a.Question)
                .Include(a => a.Response)
                .OrderBy(a => a.Question.DisplayOrder)
                .ToListAsync();
        }

        /// <summary>
        /// Gets answers by question ID (for result aggregation)
        /// </summary>
        /// <param name="questionId">Question ID to filter by</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Collection of answers</returns>
        public async Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(int questionId, bool trackChanges = false)
        {
            return await GetByCondition<Answer>(a => a.QuestionId == questionId, trackChanges)
                .Include(a => a.Question)
                .Include(a => a.Response)
                    .ThenInclude(r => r.User)
                .Where(a => a.Response.Status == ResponseStatus.Completed)
                .OrderBy(a => a.Response.User.FullName)
                .ToListAsync();
        }

        /// <summary>
        /// Gets answer by response and question (one answer per question per response)
        /// </summary>
        /// <param name="responseId">Response ID</param>
        /// <param name="questionId">Question ID</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Answer for the specific question in the response</returns>
        public async Task<Answer?> GetAnswerByResponseAndQuestionAsync(int responseId, int questionId, bool trackChanges = false)
        {
            return await GetByCondition<Answer>(a => a.ResponseId == responseId && a.QuestionId == questionId, trackChanges)
                .Include(a => a.Question)
                .Include(a => a.Response)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Deletes all answers for a response
        /// </summary>
        /// <param name="responseId">Response ID</param>
        /// <returns>True if successful, false otherwise</returns>
        public async Task<bool> DeleteAnswersByResponseIdAsync(int responseId)
        {
            try
            {
                var answers = await GetByCondition<Answer>(a => a.ResponseId == responseId, true)
                    .ToListAsync();

                foreach (var answer in answers)
                {
                    await DeleteAsync(answer);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets aggregated answer statistics for a question
        /// </summary>
        /// <param name="questionId">Question ID</param>
        /// <returns>Answer statistics (for single choice questions)</returns>
        public async Task<Dictionary<string, int>> GetAnswerStatisticsAsync(int questionId)
        {
            var answers = await GetByCondition<Answer>(a => a.QuestionId == questionId, false)
                .Include(a => a.Response)
                .Where(a => a.Response.Status == ResponseStatus.Completed && !string.IsNullOrEmpty(a.AnswerValue))
                .Select(a => a.AnswerValue!)
                .ToListAsync();

            return answers
                .GroupBy(a => a)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        /// <summary>
        /// Bulk insert or update answers for a response
        /// </summary>
        /// <param name="responseId">Response ID</param>
        /// <param name="answers">Collection of answers to save</param>
        /// <returns>True if successful, false otherwise</returns>
        public async Task<bool> BulkSaveAnswersAsync(int responseId, IEnumerable<Answer> answers)
        {
            try
            {
                // Delete existing answers for this response
                await DeleteAnswersByResponseIdAsync(responseId);

                // Add new answers
                var answerList = answers.ToList();
                foreach (var answer in answerList)
                {
                    answer.ResponseId = responseId;
                    await AddAsync(answer);
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
