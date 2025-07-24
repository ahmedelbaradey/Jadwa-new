using Microsoft.Extensions.Localization;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MeetingManagement.State
{
    /// <summary>
    /// Represents a meeting in progress status
    /// Can transition to Finished or Cancelled states
    /// Limited editing allowed during live meeting
    /// Follows the same pattern as VotingInProgressResolutionState with full localization
    /// </summary>
    public class InProgressMeetingState : IMeetingState
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public InProgressMeetingState(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        public MeetingStatusEnum Status => MeetingStatusEnum.InProgress;

        public void Handle(Meeting meeting)
        {
            // InProgress state allows live meeting management and can transition to finished or cancelled
        }

        public bool CanTransitionTo(MeetingStatusEnum targetStatus)
        {
            return targetStatus == MeetingStatusEnum.Finished ||
                   targetStatus == MeetingStatusEnum.Cancelled;
        }

        public IEnumerable<MeetingStatusEnum> GetAllowedTransitions()
        {
            return new[]
            {
                MeetingStatusEnum.Finished,
                MeetingStatusEnum.Cancelled
            };
        }

        public bool CanEdit()
        {
            return false; // Cannot edit basic meeting details while in progress
        }

        public bool CanComplete()
        {
            return true; // Can complete (finish) the meeting
        }

        public bool CanCancel()
        {
            return true; // Can cancel the meeting even if in progress
        }

        public IEnumerable<string> GetValidationMessages()
        {
            return new[]
            {
                _localizer[SharedResourcesKey.MeetingInProgressValidationMessage],
                _localizer[SharedResourcesKey.MeetingLiveFeaturesMessage]
            };
        }

        public string GetStateDescriptionKey()
        {
            return SharedResourcesKey.MeetingStatusInProgress;
        }

        public IEnumerable<MeetingActionEnum> GetAvailableActions()
        {
            return new[]
            {
                MeetingActionEnum.MeetingEnd,
                MeetingActionEnum.MeetingCancellation
            };
        }
    }
}
