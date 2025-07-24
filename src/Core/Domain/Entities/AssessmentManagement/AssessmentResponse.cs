using Domain.Entities.Base;
using Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.AssessmentManagement
{
    /// <summary>
    /// Represents a response submitted by a board member for an assessment
    /// Inherits from FullAuditedEntity to provide comprehensive audit trail functionality
    /// Properties are defined based on requirements in AssessmentStories.md
    /// </summary>
    public class AssessmentResponse : FullAuditedEntity
    {
        /// <summary>
        /// Foreign key reference to the Assessment this response is for
        /// Required field as specified in user stories
        /// </summary>
        public int AssessmentId { get; set; }

        /// <summary>
        /// Foreign key reference to the User (Board Member) who submitted this response
        /// Required field as specified in user stories
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Status of the response (Pending or Completed)
        /// Required field for tracking response state
        /// </summary>
        public ResponseStatus Status { get; set; } = ResponseStatus.Pending;

        /// <summary>
        /// Date when the response was submitted
        /// Optional field, set when response is completed
        /// </summary>
        public DateTime? SubmissionDate { get; set; }

        /// <summary>
        /// General comments provided by the board member
        /// Optional field for additional feedback
        /// Maximum 4000 characters as specified in user stories
        /// </summary>
        public string? GeneralComments { get; set; }

        /// <summary>
        /// Navigation property to the Assessment this response is for
        /// Provides access to assessment information
        /// </summary>
        [ForeignKey("AssessmentId")]
        public virtual Assessment Assessment { get; set; } = null!;

        /// <summary>
        /// Navigation property to the User who submitted this response
        /// Provides access to board member information
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        /// <summary>
        /// Collection navigation property to Answer entities
        /// Represents all individual answers within this response
        /// </summary>
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }

    /// <summary>
    /// Enumeration representing the status of an assessment response
    /// Based on user story requirements
    /// </summary>
    public enum ResponseStatus
    {
        /// <summary>
        /// Response is pending - board member hasn't submitted yet
        /// Arabic: معلق
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Response is completed - board member has submitted
        /// Arabic: مكتمل
        /// </summary>
        Completed = 2
    }
}
