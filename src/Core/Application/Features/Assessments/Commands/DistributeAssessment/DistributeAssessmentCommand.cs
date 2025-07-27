using Abstraction.Base.Response;
using Application.Base.Abstracts;

namespace Application.Features.Assessments.Commands.DistributeAssessment
{
    /// <summary>
    /// Command for distributing an approved assessment to board members
    /// Implements User Story 3: Distribute Assessment from AssessmentStories.md
    /// Follows CQRS pattern with ICommand interface from Abstract project
    /// </summary>
    public class DistributeAssessmentCommand : ICommand<BaseResponse<DistributeAssessmentResponse>>
    {
        /// <summary>
        /// Assessment ID to distribute
        /// </summary>
        public int AssessmentId { get; set; }
    }

    /// <summary>
    /// Response DTO for assessment distribution operations
    /// Contains the distribution result information
    /// </summary>
    public class DistributeAssessmentResponse
    {
        /// <summary>
        /// Assessment ID that was distributed
        /// </summary>
        public int AssessmentId { get; set; }

        /// <summary>
        /// Assessment title
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// New status after distribution (Active)
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Localized status name for display
        /// </summary>
        public string StatusDisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Number of board members the assessment was sent to
        /// </summary>
        public int BoardMemberCount { get; set; }

        /// <summary>
        /// Success message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Distribution date
        /// </summary>
        public DateTime DistributionDate { get; set; }

        /// <summary>
        /// Available actions after distribution
        /// </summary>
        public List<string> AvailableActions { get; set; } = new List<string>();
    }
}
