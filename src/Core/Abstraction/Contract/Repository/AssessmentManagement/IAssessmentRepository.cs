using Abstraction.Contracts.Repository;
using Domain.Entities.AssessmentManagement;

namespace Abstraction.Contract.Repository.AssessmentManagement
{
    /// <summary>
    /// Repository interface for Assessment entity operations
    /// Inherits from IGenericRepository to provide standard CRUD operations
    /// Based on existing repository patterns in the codebase
    /// </summary>
    public interface IAssessmentRepository : IGenericRepository
    {
        /// <summary>
        /// Gets assessments by fund ID with optional filtering
        /// </summary>
        /// <param name="fundId">Fund ID to filter by</param>
        /// <param name="status">Optional status filter</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Queryable collection of assessments</returns>
        Task<IEnumerable<Assessment>> GetAssessmentsByFundIdAsync(int fundId, AssessmentStatus? status = null, bool trackChanges = false);

        /// <summary>
        /// Gets assessments waiting for approval by specific reviewer roles
        /// </summary>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Queryable collection of assessments waiting for approval</returns>
        Task<IEnumerable<Assessment>> GetAssessmentsWaitingForApprovalAsync(bool trackChanges = false);

        /// <summary>
        /// Gets active assessments that board members can respond to
        /// </summary>
        /// <param name="userId">Board member user ID</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Queryable collection of active assessments for the user</returns>
        Task<IEnumerable<Assessment>> GetActiveAssessmentsForUserAsync(int userId, bool trackChanges = false);

        /// <summary>
        /// Gets assessment with all related data (questions, responses, etc.)
        /// </summary>
        /// <param name="assessmentId">Assessment ID</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Assessment with related data</returns>
        Task<Assessment?> GetAssessmentWithDetailsAsync(int assessmentId, bool trackChanges = false);

        /// <summary>
        /// Gets assessment with questions only
        /// </summary>
        /// <param name="assessmentId">Assessment ID</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Assessment with questions</returns>
        Task<Assessment?> GetAssessmentWithQuestionsAsync(int assessmentId, bool trackChanges = false);

        /// <summary>
        /// Gets assessments by creator user ID
        /// </summary>
        /// <param name="createdBy">Creator user ID</param>
        /// <param name="trackChanges">Whether to track changes</param>
        /// <returns>Queryable collection of assessments created by the user</returns>
        Task<IEnumerable<Assessment>> GetAssessmentsByCreatorAsync(int createdBy, bool trackChanges = false);

        /// <summary>
        /// Checks if an assessment title already exists for a fund
        /// </summary>
        /// <param name="fundId">Fund ID</param>
        /// <param name="title">Assessment title</param>
        /// <param name="excludeAssessmentId">Assessment ID to exclude from check (for updates)</param>
        /// <returns>True if title exists, false otherwise</returns>
        Task<bool> IsTitleExistsAsync(int fundId, string title, int? excludeAssessmentId = null);
    }
}
