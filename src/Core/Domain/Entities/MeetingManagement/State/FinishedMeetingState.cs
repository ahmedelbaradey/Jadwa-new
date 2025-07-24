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
    /// Represents a meeting in finished status
    /// Final state - no transitions allowed
    /// No editing allowed, only viewing and minutes creation
    /// Follows the same pattern as ApprovedResolutionState with full localization
    /// </summary>
    public class FinishedMeetingState : IMeetingState
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public FinishedMeetingState(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        public MeetingStatusEnum Status => MeetingStatusEnum.Finished;

        public void Handle(Meeting meeting)
        {
            // Finished state is final - only allows viewing and minutes creation
        }

        public bool CanTransitionTo(MeetingStatusEnum targetStatus)
        {
            return false; // Finished meetings cannot transition to other states
        }

        public IEnumerable<MeetingStatusEnum> GetAllowedTransitions()
        {
            return new MeetingStatusEnum[0]; // No transitions allowed
        }

        public bool CanEdit()
        {
            return false; // Finished meetings cannot be edited
        }

        public bool CanComplete()
        {
            return false; // Already completed
        }

        public bool CanCancel()
        {
            return false; // Cannot cancel a finished meeting
        }

        public IEnumerable<string> GetValidationMessages()
        {
            return new[]
            {
                _localizer[SharedResourcesKey.MeetingFinishedValidationMessage],
                _localizer[SharedResourcesKey.MeetingMinutesCanBeCreatedMessage]
            };
        }

        public string GetStateDescriptionKey()
        {
            return SharedResourcesKey.MeetingStatusFinished;
        }

        public IEnumerable<MeetingActionEnum> GetAvailableActions()
        {
            return new[]
            {
                MeetingActionEnum.MeetingMinutesCreation
            };
        }
    }
}
