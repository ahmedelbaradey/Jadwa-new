using Domain.Entities.AssessmentManagement;
using Microsoft.Extensions.Localization;
using Resources;

namespace Domain.States.AssessmentStates
{
    /// <summary>
    /// Represents the Waiting for Approval state of an assessment
    /// Assessment has been submitted and is awaiting review by Legal Council or Board Secretary
    /// Based on user story requirements from AssessmentStories.md
    /// </summary>
    public class WaitingForApprovalState : AssessmentStateBase
    {
        /// <summary>
        /// Gets the status associated with this state
        /// </summary>
        public override AssessmentStatus Status => AssessmentStatus.WaitingForApproval;

        /// <summary>
        /// Gets the allowed transition statuses from WaitingForApproval state
        /// Can transition to Approved or Rejected by authorized reviewers
        /// </summary>
        protected override List<AssessmentStatus> AllowedTransitions => new()
        {
            AssessmentStatus.Approved,
            AssessmentStatus.Rejected
        };

        /// <summary>
        /// Validates the WaitingForApproval state business rules
        /// </summary>
        /// <param name="assessment">The assessment to validate</param>
        /// <returns>Validation result with success status and messages</returns>
        public override (bool IsValid, List<string> ValidationMessages) ValidateState(Assessment assessment, IStringLocalizer<SharedResources> localizer)
        {
            var validationMessages = new List<string>();

            // Assessment should be complete and ready for review
            if (string.IsNullOrWhiteSpace(assessment.Title))
            {
                validationMessages.Add(localizer[SharedResourcesKey.AssessmentTitleRequired]);
            }

            // Validate type-specific requirements
            if (assessment.Type == AssessmentType.Questionnaire)
            {
                if (assessment.Questions == null || !assessment.Questions.Any())
                {
                    validationMessages.Add(localizer[SharedResourcesKey.AtLeastOneQuestionRequired]);
                }
            }
            else if (assessment.Type == AssessmentType.Attachment)
            {
                if (!assessment.AttachmentId.HasValue || assessment.AttachmentId.Value <= 0)
                {
                    validationMessages.Add(localizer[SharedResourcesKey.AttachmentRequired]);
                }
            }

            // Check if assessment has been waiting too long (warning, not error)
            if (assessment.CreatedAt.HasValue)
            {
                var waitingDays = (DateTime.Now - assessment.CreatedAt.Value).Days;
                if (waitingDays > 7) // More than 7 days waiting
                {
                    validationMessages.Add(string.Format(localizer[SharedResourcesKey.AssessmentWaitingTooLong], waitingDays));
                }
            }

            return (validationMessages.Count == 0 || validationMessages.All(m => m.StartsWith("Warning") || m.StartsWith("تحذير")), validationMessages);
        }

        /// <summary>
        /// Gets the available actions for WaitingForApproval state
        /// Actions will be localized by the AssessmentStateContext
        /// </summary>
        /// <returns>List of available action keys</returns>
        public override List<string> GetAvailableActions()
        {
            return new List<string>
            {
                "View Details",
                "Approve",
                "Reject"
            };
        }

        /// <summary>
        /// Handles WaitingForApproval state specific logic
        /// </summary>
        /// <param name="assessment">The assessment to handle</param>
        public override void Handle(Assessment assessment)
        {
            // Assessment is read-only for creator in this state
            // Only authorized reviewers can approve or reject
            base.Handle(assessment);
        }
    }
}
