using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ResolutionManagement.State
{
    /// <summary>
    /// Represents a rejected resolution
    /// Can be edited and sent back for confirmation
    /// Limited editing allowed by Legal Council/Board Secretary
    /// </summary>
    public class RejectedResolutionState : IResolutionState
    {
        public ResolutionStatusEnum Status => ResolutionStatusEnum.Rejected;

        public void Handle(Resolution resolution)
        {
            // Rejected state allows editing and resubmission
        }

        public bool CanTransitionTo(ResolutionStatusEnum targetStatus)
        {
            return targetStatus == ResolutionStatusEnum.WaitingForConfirmation; // Can be edited and resubmitted
        }

        public IEnumerable<ResolutionStatusEnum> GetAllowedTransitions()
        {
            return new[]
            {
                ResolutionStatusEnum.WaitingForConfirmation
            };
        }

        public bool CanEdit()
        {
            return true; // Rejected resolutions can be edited
        }

        public bool CanComplete()
        {
            return false; // Cannot complete rejected resolutions
        }

        public bool CanCancel()
        {
            return false; // Cannot cancel rejected resolutions
        }

        public IEnumerable<string> GetValidationMessages()
        {
            return new[]
            {
                "Resolution has been rejected and can be edited for resubmission"
            };
        }

        public string GetStateDescriptionKey()
        {
            return "ResolutionInRejectedState";
        }
    }
}
