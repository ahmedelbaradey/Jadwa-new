using Abstraction.Contracts.Repository;
using Domain.Entities.AssessmentManagement;

namespace Abstraction.Contract.Repository.AssessmentManagement
{
    /// <summary>
    /// Repository interface for AssessmentResponse entity operations
    /// Inherits from IGenericRepository to provide standard CRUD operations
    /// Based on existing repository patterns in the codebase
    /// </summary>
    public interface IAssessmentResponseRepository : IGenericRepository
    {
        /// <summary>
        /// Gets responses by assessment ID
        /// </summary>
        /// <param name="assessmentId">Assessment ID to filter by</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Collection of responses</returns>
        Task<IEnumerable<AssessmentResponse>> GetResponsesByAssessmentIdAsync(int assessmentId, bool trackChanges = false);

        /// <summary>
        /// Gets responses by user ID (for personal assessment view)
        /// </summary>
        /// <param name="userId">User ID to filter by</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Collection of responses</returns>
        Task<IEnumerable<AssessmentResponse>> GetResponsesByUserIdAsync(int userId, bool trackChanges = false);

        /// <summary>
        /// Gets a specific response with all answers
        /// </summary>
        /// <param name="responseId">Response ID</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Response with answers</returns>
        Task<AssessmentResponse?> GetResponseWithAnswersAsync(int responseId, bool trackChanges = false);

        /// <summary>
        /// Gets response by assessment and user (one response per user per assessment)
        /// </summary>
        /// <param name="assessmentId">Assessment ID</param>
        /// <param name="userId">User ID</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>User's response for the assessment</returns>
        Task<AssessmentResponse?> GetResponseByAssessmentAndUserAsync(int assessmentId, int userId, bool trackChanges = false);

        /// <summary>
        /// Creates response records for all board members of a fund when assessment is distributed
        /// </summary>
        /// <param name="assessmentId">Assessment ID</param>
        /// <param name="fundId">Fund ID to get board members from</param>
        /// <returns>Number of response records created</returns>
        Task<int> CreateResponsesForBoardMembersAsync(int assessmentId, int fundId);

        /// <summary>
        /// Gets completion statistics for an assessment
        /// </summary>
        /// <param name="assessmentId">Assessment ID</param>
        /// <returns>Completion statistics (total, completed, pending)</returns>
        Task<(int Total, int Completed, int Pending)> GetCompletionStatisticsAsync(int assessmentId);

        /// <summary>
        /// Checks if a user has already responded to an assessment
        /// </summary>
        /// <param name="assessmentId">Assessment ID</param>
        /// <param name="userId">User ID</param>
        /// <returns>True if user has responded, false otherwise</returns>
        Task<bool> HasUserRespondedAsync(int assessmentId, int userId);
    }
}
