using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ResolutionManagement.State
{
    /// <summary>
    /// Represents a cancelled resolution
    /// Terminal state with no transitions allowed
    /// Read-only state for historical purposes
    /// </summary>
    public class CancelledResolutionState : IResolutionState
    {
        public ResolutionStatusEnum Status => ResolutionStatusEnum.Cancelled;

        public void Handle(Resolution resolution)
        {
            // Cancelled state is terminal - no transitions allowed
        }

        public bool CanTransitionTo(ResolutionStatusEnum targetStatus)
        {
            return false; // Terminal state - no transitions allowed
        }

        public IEnumerable<ResolutionStatusEnum> GetAllowedTransitions()
        {
            return Enumerable.Empty<ResolutionStatusEnum>();
        }

        public bool CanEdit()
        {
            return false; // Cancelled resolutions cannot be edited
        }

        public bool CanComplete()
        {
            return false; // Cannot complete cancelled resolutions
        }

        public bool CanCancel()
        {
            return false; // Already cancelled
        }

        public IEnumerable<string> GetValidationMessages()
        {
            return new[]
            {
                "Resolution has been cancelled and cannot be modified"
            };
        }

        public string GetStateDescriptionKey()
        {
            return "ResolutionInCancelledState";
        }
    }
}
