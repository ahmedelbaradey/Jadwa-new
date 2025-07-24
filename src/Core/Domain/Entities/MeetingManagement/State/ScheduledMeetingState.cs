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
    /// Represents a meeting in scheduled status
    /// Can transition to InProgress, Cancelled, or Postponed states
    /// Allows editing and cancellation operations
    /// Follows the same pattern as DraftResolutionState with full localization
    /// </summary>
    public class ScheduledMeetingState : IMeetingState
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public ScheduledMeetingState(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        public MeetingStatusEnum Status => MeetingStatusEnum.Scheduled;

        public void Handle(Meeting meeting)
        {
            // Scheduled state allows editing and can transition to in progress, cancelled, or postponed
        }

        public bool CanTransitionTo(MeetingStatusEnum targetStatus)
        {
            return targetStatus == MeetingStatusEnum.InProgress ||
                   targetStatus == MeetingStatusEnum.Cancelled ||
                   targetStatus == MeetingStatusEnum.Postponed ||
                   targetStatus == MeetingStatusEnum.Scheduled; // Can save changes
        }

        public IEnumerable<MeetingStatusEnum> GetAllowedTransitions()
        {
            return new[]
            {
                MeetingStatusEnum.InProgress,
                MeetingStatusEnum.Cancelled,
                MeetingStatusEnum.Postponed,
                MeetingStatusEnum.Scheduled
            };
        }

        public bool CanEdit()
        {
            return true; // Scheduled meetings can be edited
        }

        public bool CanComplete()
        {
            return false; // Cannot complete a scheduled meeting directly
        }

        public bool CanCancel()
        {
            return true; // Scheduled meetings can be cancelled
        }

        public IEnumerable<string> GetValidationMessages()
        {
            return new[]
            {
                _localizer[SharedResourcesKey.MeetingScheduledValidationMessage],
                _localizer[SharedResourcesKey.MeetingNotifyAttendeesMessage]
            };
        }

        public string GetStateDescriptionKey()
        {
            return SharedResourcesKey.MeetingStatusScheduled;
        }

        public IEnumerable<MeetingActionEnum> GetAvailableActions()
        {
            return new[]
            {
                MeetingActionEnum.MeetingStart,
                MeetingActionEnum.MeetingEdit,
                MeetingActionEnum.MeetingCancellation,
                MeetingActionEnum.MeetingPostponement
            };
        }
    }
}
