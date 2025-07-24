using Domain.Entities.AssessmentManagement;
using Microsoft.Extensions.Localization;
using Resources;

namespace Domain.States.AssessmentStates
{
    /// <summary>
    /// Represents the Rejected state of an assessment
    /// Assessment has been rejected and needs revision
    /// Based on user story requirements from AssessmentStories.md
    /// </summary>
    public class RejectedState : AssessmentStateBase
    {
        /// <summary>
        /// Gets the status associated with this state
        /// </summary>
        public override AssessmentStatus Status => AssessmentStatus.Rejected;

        /// <summary>
        /// Gets the allowed transition statuses from Rejected state
        /// Can transition back to Draft (for editing) or WaitingForApproval (resubmit)
        /// </summary>
        protected override List<AssessmentStatus> AllowedTransitions => new()
        {
            AssessmentStatus.Draft,
            AssessmentStatus.WaitingForApproval
        };

        /// <summary>
        /// Validates the Rejected state business rules with localized messages
        /// </summary>
        /// <param name="assessment">The assessment to validate</param>
        /// <param name="localizer">String localizer for localized messages</param>
        /// <returns>Validation result with success status and localized messages</returns>
        public override (bool IsValid, List<string> ValidationMessages) ValidateState(Assessment assessment, IStringLocalizer<SharedResources> localizer)
        {
            var validationMessages = new List<string>();

            // Validate rejection information
            if (!assessment.ReviewedBy.HasValue)
            {
                validationMessages.Add(localizer[SharedResourcesKey.ReviewerInformationRequired]);
            }

            if (!assessment.ReviewedDate.HasValue)
            {
                validationMessages.Add(localizer[SharedResourcesKey.ReviewDateRequired]);
            }

            if (string.IsNullOrWhiteSpace(assessment.ReviewerComments))
            {
                validationMessages.Add(localizer[SharedResourcesKey.RejectionReasonRequired]);
            }

            // Basic assessment validation
            if (string.IsNullOrWhiteSpace(assessment.Title))
            {
                validationMessages.Add(localizer[SharedResourcesKey.AssessmentTitleRequired]);
            }

            return (validationMessages.Count == 0, validationMessages);
        }

        /// <summary>
        /// Gets the available actions for Rejected state
        /// Returns AssessmentActionEnum values for consistency with Resolution pattern
        /// </summary>
        /// <returns>List of available action enums</returns>
        public override List<AssessmentActionEnum> GetAvailableActions()
        {
            return new List<AssessmentActionEnum>
            {
                AssessmentActionEnum.ViewDetails,
                AssessmentActionEnum.ViewRejectionReason,
                AssessmentActionEnum.Edit,
                AssessmentActionEnum.Resubmit,
                AssessmentActionEnum.Delete
            };
        }

        /// <summary>
        /// Handles Rejected state specific logic
        /// </summary>
        /// <param name="assessment">The assessment to handle</param>
        public override void Handle(Assessment assessment)
        {
            // Assessment can be edited and resubmitted
            // Creator should address the rejection comments
            base.Handle(assessment);
        }
    }
}
