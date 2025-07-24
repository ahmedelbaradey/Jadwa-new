using Domain.Entities.AssessmentManagement;
using Microsoft.Extensions.Localization;
using Resources;

namespace Domain.States.AssessmentStates
{
    /// <summary>
    /// Represents the Draft state of an assessment
    /// Assessment is being created or edited by Fund Manager
    /// Based on user story requirements from AssessmentStories.md
    /// </summary>
    public class DraftState : AssessmentStateBase
    {
        /// <summary>
        /// Gets the status associated with this state
        /// </summary>
        public override AssessmentStatus Status => AssessmentStatus.Draft;

        /// <summary>
        /// Gets the allowed transition statuses from Draft state
        /// Can transition to WaitingForApproval (submit) or remain Draft (save)
        /// </summary>
        protected override List<AssessmentStatus> AllowedTransitions => new()
        {
            AssessmentStatus.WaitingForApproval
        };

        /// <summary>
        /// Validates the Draft state business rules
        /// </summary>
        /// <param name="assessment">The assessment to validate</param>
        /// <returns>Validation result with success status and messages</returns>
        public override (bool IsValid, List<string> ValidationMessages) ValidateState(Assessment assessment, IStringLocalizer<SharedResources> localizer)
        {
            var validationMessages = new List<string>();

            // Validate title is provided
            if (string.IsNullOrWhiteSpace(assessment.Title))
            {
                validationMessages.Add(localizer[SharedResourcesKey.AssessmentTitleRequired]);
            }

            // Validate title length
            if (!string.IsNullOrWhiteSpace(assessment.Title) && assessment.Title.Length > 255)
            {
                validationMessages.Add(localizer[SharedResourcesKey.AssessmentTitleMaxLength]);
            }

            // Validate assessment type specific requirements
            if (assessment.Type == AssessmentType.Questionnaire)
            {
                // For questionnaire type, must have at least one question
                if (assessment.Questions == null || !assessment.Questions.Any())
                {
                    validationMessages.Add(localizer[SharedResourcesKey.AtLeastOneQuestionRequired]);
                }
                else
                {
                    // Validate each question
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
                // For attachment type, must have attachment ID
                if (!assessment.AttachmentId.HasValue || assessment.AttachmentId.Value <= 0)
                {
                    validationMessages.Add(localizer[SharedResourcesKey.AttachmentRequired]);
                }
            }

            return (validationMessages.Count == 0, validationMessages);
        }

        /// <summary>
        /// Gets the available actions for Draft state
        /// Actions will be localized by the AssessmentStateContext
        /// </summary>
        /// <returns>List of available action keys</returns>
        public override List<string> GetAvailableActions()
        {
            return new List<string>
            {
                "Edit",
                "Save",
                "Submit for Approval",
                "Delete"
            };
        }

        /// <summary>
        /// Handles Draft state specific logic
        /// </summary>
        /// <param name="assessment">The assessment to handle</param>
        public override void Handle(Assessment assessment)
        {
            // Draft state allows editing
            // No specific handling required for draft state
            base.Handle(assessment);
        }
    }
}
