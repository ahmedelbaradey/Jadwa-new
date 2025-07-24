using Abstraction.Contracts.Repository;
using Domain.Entities.AssessmentManagement;

namespace Abstraction.Contract.Repository.AssessmentManagement
{
    /// <summary>
    /// Repository interface for AssessmentQuestion entity operations
    /// Inherits from IGenericRepository to provide standard CRUD operations
    /// Based on existing repository patterns in the codebase
    /// </summary>
    public interface IAssessmentQuestionRepository : IGenericRepository
    {
        /// <summary>
        /// Gets questions by assessment ID ordered by display order
        /// </summary>
        /// <param name="assessmentId">Assessment ID to filter by</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Ordered collection of questions</returns>
        Task<IEnumerable<AssessmentQuestion>> GetQuestionsByAssessmentIdAsync(int assessmentId, bool trackChanges = false);

        /// <summary>
        /// Gets a specific question with its answers
        /// </summary>
        /// <param name="questionId">Question ID</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Question with answers</returns>
        Task<AssessmentQuestion?> GetQuestionWithAnswersAsync(int questionId, bool trackChanges = false);

        /// <summary>
        /// Updates the display order of questions for an assessment
        /// </summary>
        /// <param name="assessmentId">Assessment ID</param>
        /// <param name="questionOrders">Dictionary of question ID to display order</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> UpdateQuestionOrdersAsync(int assessmentId, Dictionary<int, int> questionOrders);

        /// <summary>
        /// Deletes all questions for an assessment
        /// </summary>
        /// <param name="assessmentId">Assessment ID</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> DeleteQuestionsByAssessmentIdAsync(int assessmentId);
    }
}
