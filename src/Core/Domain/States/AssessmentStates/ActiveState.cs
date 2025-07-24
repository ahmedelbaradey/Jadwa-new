using Domain.Entities.AssessmentManagement;
using Microsoft.Extensions.Localization;
using Resources;

namespace Domain.States.AssessmentStates
{
    /// <summary>
    /// Represents the Active state of an assessment
    /// Assessment has been distributed and board members are responding
    /// Based on user story requirements from AssessmentStories.md
    /// </summary>
    public class ActiveState : AssessmentStateBase
    {
        /// <summary>
        /// Gets the status associated with this state
        /// </summary>
        public override AssessmentStatus Status => AssessmentStatus.Active;

        /// <summary>
        /// Gets the allowed transition statuses from Active state
        /// Can transition to Completed when all responses are collected or manually closed
        /// </summary>
        protected override List<AssessmentStatus> AllowedTransitions => new()
        {
            AssessmentStatus.Completed
        };

        /// <summary>
        /// Validates the Active state business rules
        /// </summary>
        /// <param name="assessment">The assessment to validate</param>
        /// <returns>Validation result with success status and messages</returns>
        public override (bool IsValid, List<string> ValidationMessages) ValidateState(Assessment assessment, IStringLocalizer<SharedResources> localizer)
        {
            var validationMessages = new List<string>();

            // Assessment should be complete and distributed
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

            // Check response status
            if (assessment.Responses != null && assessment.Responses.Any())
            {
                var totalResponses = assessment.Responses.Count;
                var completedResponses = assessment.Responses.Count(r => r.Status == ResponseStatus.Completed);
                var pendingResponses = totalResponses - completedResponses;

                if (pendingResponses > 0)
                {
                    validationMessages.Add(string.Format(localizer[SharedResourcesKey.AssessmentPendingResponsesInfo], pendingResponses, totalResponses));
                }

                // Check if all responses are completed
                if (completedResponses == totalResponses && totalResponses > 0)
                {
                    validationMessages.Add(localizer[SharedResourcesKey.AssessmentAllResponsesReceivedInfo]);
                }
            }
            else
            {
                validationMessages.Add(localizer[SharedResourcesKey.AssessmentNoResponsesWarning]);
            }

            return (validationMessages.Count == 0 || validationMessages.All(m => 
                m.StartsWith("Info") || m.StartsWith("Warning") || m.StartsWith("معلومات") || m.StartsWith("تحذير")), validationMessages);
        }

        /// <summary>
        /// Gets the available actions for Active state
        /// Returns AssessmentActionEnum values for consistency with Resolution pattern
        /// </summary>
        /// <returns>List of available action enums</returns>
        public override List<AssessmentActionEnum> GetAvailableActions()
        {
            return new List<AssessmentActionEnum>
            {
                AssessmentActionEnum.ViewDetails,
                AssessmentActionEnum.ViewResponses,
                AssessmentActionEnum.ViewResults,
                AssessmentActionEnum.CompleteAssessment,
                AssessmentActionEnum.ExportResults
            };
        }

        /// <summary>
        /// Handles Active state specific logic
        /// </summary>
        /// <param name="assessment">The assessment to handle</param>
        public override void Handle(Assessment assessment)
        {
            // Assessment is active and collecting responses
            // Board members can submit responses
            // Management can view progress and results
            base.Handle(assessment);
        }
    }
}
