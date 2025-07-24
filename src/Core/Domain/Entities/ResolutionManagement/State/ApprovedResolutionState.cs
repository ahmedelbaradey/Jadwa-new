using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ResolutionManagement.State
{
    /// <summary>
    /// Represents an approved resolution (final state after successful voting)
    /// Can create referral resolutions but cannot be directly modified
    /// Terminal state with limited actions
    /// </summary>
    public class ApprovedResolutionState : IResolutionState
    {
        public ResolutionStatusEnum Status => ResolutionStatusEnum.Approved;

        public void Handle(Resolution resolution)
        {
            // Approved state is terminal - no automatic transitions
        }

        public bool CanTransitionTo(ResolutionStatusEnum targetStatus)
        {
            return false; // Terminal state - no direct transitions allowed
        }

        public IEnumerable<ResolutionStatusEnum> GetAllowedTransitions()
        {
            return Enumerable.Empty<ResolutionStatusEnum>();
        }

        public bool CanEdit()
        {
            return false; // Approved resolutions cannot be edited directly
        }

        public bool CanComplete()
        {
            return false; // Cannot complete approved resolutions
        }

        public bool CanCancel()
        {
            return false; // Cannot cancel approved resolutions
        }

        public IEnumerable<string> GetValidationMessages()
        {
            return new[]
            {
                "Resolution has been approved. Create a referral resolution for modifications."
            };
        }

        public string GetStateDescriptionKey()
        {
            return "ResolutionInApprovedState";
        }
    }
}
