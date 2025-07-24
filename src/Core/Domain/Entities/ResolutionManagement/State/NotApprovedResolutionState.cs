using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ResolutionManagement.State
{
    /// <summary>
    /// Represents a not approved resolution (final state after unsuccessful voting)
    /// Can create referral resolutions but cannot be directly modified
    /// Terminal state with limited actions
    /// </summary>
    public class NotApprovedResolutionState : IResolutionState
    {
        public ResolutionStatusEnum Status => ResolutionStatusEnum.NotApproved;

        public void Handle(Resolution resolution)
        {
            // NotApproved state is terminal - no automatic transitions
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
            return false; // NotApproved resolutions cannot be edited directly
        }

        public bool CanComplete()
        {
            return false; // Cannot complete not approved resolutions
        }

        public bool CanCancel()
        {
            return false; // Cannot cancel not approved resolutions
        }

        public IEnumerable<string> GetValidationMessages()
        {
            return new[]
            {
                "Resolution was not approved. Create a referral resolution for modifications."
            };
        }

        public string GetStateDescriptionKey()
        {
            return "ResolutionInNotApprovedState";
        }
    }
}
