using Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.AssessmentManagement
{
    /// <summary>
    /// Represents an individual answer to a specific question within an assessment response
    /// Inherits from FullAuditedEntity to provide comprehensive audit trail functionality
    /// Properties are defined based on requirements in AssessmentStories.md
    /// </summary>
    public class Answer : FullAuditedEntity
    {
        /// <summary>
        /// Foreign key reference to the AssessmentResponse this answer belongs to
        /// Required field as specified in user stories
        /// </summary>
        public int ResponseId { get; set; }

        /// <summary>
        /// Foreign key reference to the AssessmentQuestion this answer is for
        /// Required field as specified in user stories
        /// </summary>
        public int QuestionId { get; set; }

        /// <summary>
        /// The actual answer value provided by the board member
        /// Can be text input or selected option value
        /// Maximum 4000 characters as specified in user stories
        /// </summary>
        public string? AnswerValue { get; set; }

        /// <summary>
        /// Navigation property to the AssessmentResponse this answer belongs to
        /// Provides access to response information
        /// </summary>
        [ForeignKey("ResponseId")]
        public virtual AssessmentResponse Response { get; set; } = null!;

        /// <summary>
        /// Navigation property to the AssessmentQuestion this answer is for
        /// Provides access to question information
        /// </summary>
        [ForeignKey("QuestionId")]
        public virtual AssessmentQuestion Question { get; set; } = null!;
    }
}
