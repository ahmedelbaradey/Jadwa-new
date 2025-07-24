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
    /// Represents a meeting in cancelled status
    /// Final state - no transitions allowed
    /// No editing or operations allowed
    /// Follows the same pattern as RejectedResolutionState with full localization
    /// </summary>
    public class CancelledMeetingState : IMeetingState
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public CancelledMeetingState(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        public MeetingStatusEnum Status => MeetingStatusEnum.Cancelled;

        public void Handle(Meeting meeting)
        {
            // Cancelled state is final - no operations allowed
        }

        public bool CanTransitionTo(MeetingStatusEnum targetStatus)
        {
            return false; // Cancelled meetings cannot transition to other states
        }

        public IEnumerable<MeetingStatusEnum> GetAllowedTransitions()
        {
            return new MeetingStatusEnum[0]; // No transitions allowed
        }

        public bool CanEdit()
        {
            return false; // Cancelled meetings cannot be edited
        }

        public bool CanComplete()
        {
            return false; // Cannot complete a cancelled meeting
        }

        public bool CanCancel()
        {
            return false; // Already cancelled
        }

        public IEnumerable<string> GetValidationMessages()
        {
            return new[]
            {
                _localizer[SharedResourcesKey.MeetingCancelledValidationMessage]
            };
        }

        public string GetStateDescriptionKey()
        {
            return SharedResourcesKey.MeetingStatusCancelled;
        }

        public IEnumerable<MeetingActionEnum> GetAvailableActions()
        {
            return new MeetingActionEnum[0]; // No actions available for cancelled meetings
        }
    }
}
