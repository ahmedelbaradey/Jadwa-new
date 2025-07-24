using Domain.Entities.AssessmentManagement;
using Microsoft.Extensions.Localization;
using Resources;

namespace Domain.States.AssessmentStates
{
    /// <summary>
    /// Represents the Approved state of an assessment
    /// Assessment has been approved and is ready for distribution
    /// Based on user story requirements from AssessmentStories.md
    /// </summary>
    public class ApprovedState : AssessmentStateBase
    {
        /// <summary>
        /// Gets the status associated with this state
        /// </summary>
        public override AssessmentStatus Status => AssessmentStatus.Approved;

        /// <summary>
        /// Gets the allowed transition statuses from Approved state
        /// Can transition to Active (when distributed) or back to Rejected (if re-reviewed)
        /// </summary>
        protected override List<AssessmentStatus> AllowedTransitions => new()
        {
            AssessmentStatus.Active,
            AssessmentStatus.Rejected // In case of re-review
        };

        /// <summary>
        /// Validates the Approved state business rules
        /// </summary>
        /// <param name="assessment">The assessment to validate</param>
        /// <returns>Validation result with success status and messages</returns>
        public override (bool IsValid, List<string> ValidationMessages) ValidateState(Assessment assessment, IStringLocalizer<SharedResources> localizer)
        {
            var validationMessages = new List<string>();

            // Assessment should be complete and approved
            if (string.IsNullOrWhiteSpace(assessment.Title))
            {
                validationMessages.Add(localizer[SharedResourcesKey.AssessmentTitleRequired]);
            }

            // Validate reviewer information
            if (!assessment.ReviewedBy.HasValue)
            {
                validationMessages.Add(localizer[SharedResourcesKey.ReviewerInformationRequired]);
            }

            if (!assessment.ReviewedDate.HasValue)
            {
                validationMessages.Add(localizer[SharedResourcesKey.ReviewDateRequired]);
            }

            // Validate type-specific requirements
            if (assessment.Type == AssessmentType.Questionnaire)
            {
                if (assessment.Questions == null || !assessment.Questions.Any())
                {
                    validationMessages.Add(localizer[SharedResourcesKey.AtLeastOneQuestionRequired]);
                }
                else
                {
                    // Validate questions are properly configured
                    foreach (var question in assessment.Questions)
                    {
                        if (string.IsNullOrWhiteSpace(question.QuestionText))
                        {
                            validationMessages.Add(localizer[SharedResourcesKey.QuestionTextRequired]);
                        }

                        if (question.QuestionType == QuestionType.SingleChoice && string.IsNullOrWhiteSpace(question.Options))
                        {
                            validationMessages.Add(localizer[SharedResourcesKey.QuestionOptionsRequired]);
                        }
                    }
                }
            }
            else if (assessment.Type == AssessmentType.Attachment)
            {
                if (!assessment.AttachmentId.HasValue || assessment.AttachmentId.Value <= 0)
                {
                    validationMessages.Add(localizer[SharedResourcesKey.AttachmentRequired]);
                }
            }

            // Check if fund has board members for distribution
            if (assessment.Fund?.FundBoardMembers == null || !assessment.Fund.FundBoardMembers.Any())
            {
                validationMessages.Add(isArabic ? 
                    "تحذير: لا يوجد أعضاء مجلس إدارة في الصندوق لتوزيع التقييم" : 
                    "Warning: No board members found in the fund for assessment distribution");
            }

            return (validationMessages.Count == 0 || validationMessages.All(m => m.StartsWith("Warning") || m.StartsWith("تحذير")), validationMessages);
        }

        /// <summary>
        /// Gets the available actions for Approved state
        /// </summary>
        /// <returns>List of available actions</returns>
        public override List<string> GetAvailableActions()
        {
            var isArabic = CultureInfo.CurrentCulture.Name.StartsWith("ar");
            
            return new List<string>
            {
                isArabic ? "عرض التفاصيل" : "View Details",
                isArabic ? "توزيع" : "Distribute",
                isArabic ? "تصدير" : "Export"
            };
        }

        /// <summary>
        /// Handles Approved state specific logic
        /// </summary>
        /// <param name="assessment">The assessment to handle</param>
        public override void Handle(Assessment assessment)
        {
            // Assessment is ready for distribution
            // Fund Manager can distribute to board members
            base.Handle(assessment);
        }
    }
}
