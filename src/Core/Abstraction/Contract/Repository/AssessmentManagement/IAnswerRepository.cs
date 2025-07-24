using Abstraction.Contracts.Repository;
using Domain.Entities.AssessmentManagement;

namespace Abstraction.Contract.Repository.AssessmentManagement
{
    /// <summary>
    /// Repository interface for Answer entity operations
    /// Inherits from IGenericRepository to provide standard CRUD operations
    /// Based on existing repository patterns in the codebase
    /// </summary>
    public interface IAnswerRepository : IGenericRepository
    {
        /// <summary>
        /// Gets answers by response ID
        /// </summary>
        /// <param name="responseId">Response ID to filter by</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Collection of answers</returns>
        Task<IEnumerable<Answer>> GetAnswersByResponseIdAsync(int responseId, bool trackChanges = false);

        /// <summary>
        /// Gets answers by question ID (for result aggregation)
        /// </summary>
        /// <param name="questionId">Question ID to filter by</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Collection of answers</returns>
        Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(int questionId, bool trackChanges = false);

        /// <summary>
        /// Gets answer by response and question (one answer per question per response)
        /// </summary>
        /// <param name="responseId">Response ID</param>
        /// <param name="questionId">Question ID</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Answer for the specific question in the response</returns>
        Task<Answer?> GetAnswerByResponseAndQuestionAsync(int responseId, int questionId, bool trackChanges = false);

        /// <summary>
        /// Deletes all answers for a response
        /// </summary>
        /// <param name="responseId">Response ID</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> DeleteAnswersByResponseIdAsync(int responseId);

        /// <summary>
        /// Gets aggregated answer statistics for a question
        /// </summary>
        /// <param name="questionId">Question ID</param>
        /// <returns>Answer statistics (for single choice questions)</returns>
        Task<Dictionary<string, int>> GetAnswerStatisticsAsync(int questionId);

        /// <summary>
        /// Bulk insert or update answers for a response
        /// </summary>
        /// <param name="responseId">Response ID</param>
        /// <param name="answers">Collection of answers to save</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> BulkSaveAnswersAsync(int responseId, IEnumerable<Answer> answers);
    }
}
