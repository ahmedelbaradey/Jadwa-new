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
    /// Represents a meeting in postponed status
    /// Can transition back to Scheduled or to Cancelled states
    /// Allows editing to reschedule the meeting
    /// Similar to a draft state but with postponement context and full localization
    /// </summary>
    public class PostponedMeetingState : IMeetingState
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public PostponedMeetingState(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        public MeetingStatusEnum Status => MeetingStatusEnum.Postponed;

        public void Handle(Meeting meeting)
        {
            // Postponed state allows rescheduling and can transition to scheduled or cancelled
        }

        public bool CanTransitionTo(MeetingStatusEnum targetStatus)
        {
            return targetStatus == MeetingStatusEnum.Scheduled ||
                   targetStatus == MeetingStatusEnum.Cancelled ||
                   targetStatus == MeetingStatusEnum.Postponed; // Can save changes
        }

        public IEnumerable<MeetingStatusEnum> GetAllowedTransitions()
        {
            return new[]
            {
                MeetingStatusEnum.Scheduled,
                MeetingStatusEnum.Cancelled,
                MeetingStatusEnum.Postponed
            };
        }

        public bool CanEdit()
        {
            return true; // Postponed meetings can be edited to reschedule
        }

        public bool CanComplete()
        {
            return false; // Cannot complete a postponed meeting directly
        }

        public bool CanCancel()
        {
            return true; // Postponed meetings can be cancelled
        }

        public IEnumerable<string> GetValidationMessages()
        {
            return new[]
            {
                _localizer[SharedResourcesKey.MeetingPostponedValidationMessage],
                _localizer[SharedResourcesKey.MeetingRescheduleMessage],
                _localizer[SharedResourcesKey.MeetingNotifyAttendeesMessage]
            };
        }

        public string GetStateDescriptionKey()
        {
            return SharedResourcesKey.MeetingStatusPostponed;
        }

        public IEnumerable<MeetingActionEnum> GetAvailableActions()
        {
            return new[]
            {
                MeetingActionEnum.MeetingEdit,
                MeetingActionEnum.MeetingCancellation
            };
        }
    }
}
