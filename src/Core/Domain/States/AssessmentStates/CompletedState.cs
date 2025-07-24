using Domain.Entities.AssessmentManagement;
using Microsoft.Extensions.Localization;
using Resources;

namespace Domain.States.AssessmentStates
{
    /// <summary>
    /// Represents the Completed state of an assessment
    /// Assessment has been completed and no more responses are accepted
    /// Based on user story requirements from AssessmentStories.md
    /// </summary>
    public class CompletedState : AssessmentStateBase
    {
        /// <summary>
        /// Gets the status associated with this state
        /// </summary>
        public override AssessmentStatus Status => AssessmentStatus.Completed;

        /// <summary>
        /// Gets the allowed transition statuses from Completed state
        /// Terminal state - no transitions allowed
        /// </summary>
        protected override List<AssessmentStatus> AllowedTransitions => new();

        /// <summary>
        /// Validates the Completed state business rules
        /// </summary>
        /// <param name="assessment">The assessment to validate</param>
        /// <returns>Validation result with success status and messages</returns>
        public override (bool IsValid, List<string> ValidationMessages) ValidateState(Assessment assessment, IStringLocalizer<SharedResources> localizer)
        {
            var validationMessages = new List<string>();

            // Assessment should be complete with all required information
            if (string.IsNullOrWhiteSpace(assessment.Title))
            {
                validationMessages.Add(localizer[SharedResourcesKey.AssessmentTitleRequired]);
            }

            // Validate approval information
            if (!assessment.ReviewedBy.HasValue)
            {
                validationMessages.Add(localizer[SharedResourcesKey.ReviewerInformationRequired]);
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

            // Provide completion statistics
            if (assessment.Responses != null && assessment.Responses.Any())
            {
                var totalResponses = assessment.Responses.Count;
                var completedResponses = assessment.Responses.Count(r => r.Status == ResponseStatus.Completed);
                var completionRate = totalResponses > 0 ? (completedResponses * 100.0 / totalResponses) : 0;

                validationMessages.Add(string.Format(localizer[SharedResourcesKey.AssessmentCompletionStatistics], completedResponses, totalResponses, completionRate));

                if (completedResponses == 0)
                {
                    validationMessages.Add(localizer[SharedResourcesKey.AssessmentNoResponsesReceived]);
                }
            }
            else
            {
                validationMessages.Add(localizer[SharedResourcesKey.AssessmentNoResponsesExist]);
            }

            return (validationMessages.Count == 0 || validationMessages.All(m => 
                m.StartsWith("Info") || m.StartsWith("Warning") || m.StartsWith("معلومات") || m.StartsWith("تحذير")), validationMessages);
        }

        /// <summary>
        /// Gets the available actions for Completed state
        /// Returns AssessmentActionEnum values for consistency with Resolution pattern
        /// </summary>
        /// <returns>List of available action enums</returns>
        public override List<AssessmentActionEnum> GetAvailableActions()
        {
            return new List<AssessmentActionEnum>
            {
                AssessmentActionEnum.ViewDetails,
                AssessmentActionEnum.ViewResults,
                AssessmentActionEnum.ViewResponses,
                AssessmentActionEnum.ExportResults,
                AssessmentActionEnum.ExportData,
                AssessmentActionEnum.Archive
            };
        }

        /// <summary>
        /// Handles Completed state specific logic
        /// </summary>
        /// <param name="assessment">The assessment to handle</param>
        public override void Handle(Assessment assessment)
        {
            // Assessment is completed and read-only
            // No more responses can be submitted
            // Results are final and can be viewed/exported
            base.Handle(assessment);
        }
    }
}
