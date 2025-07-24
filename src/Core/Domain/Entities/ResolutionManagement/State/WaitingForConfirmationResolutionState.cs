using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ResolutionManagement.State
{
    /// <summary>
    /// Represents a resolution waiting for fund manager confirmation
    /// Can transition to Confirmed or Rejected states
    /// Limited editing allowed (only by Legal Council/Board Secretary)
    /// </summary>
    public class WaitingForConfirmationResolutionState : IResolutionState
    {
        public ResolutionStatusEnum Status => ResolutionStatusEnum.WaitingForConfirmation;

        public void Handle(Resolution resolution)
        {
            // WaitingForConfirmation state waits for fund manager to confirm or reject
        }

        public bool CanTransitionTo(ResolutionStatusEnum targetStatus)
        {
            return targetStatus == ResolutionStatusEnum.Confirmed ||
                   targetStatus == ResolutionStatusEnum.Rejected ||
                   targetStatus == ResolutionStatusEnum.WaitingForConfirmation; // Can be updated by Legal Council
        }

        public IEnumerable<ResolutionStatusEnum> GetAllowedTransitions()
        {
            return new[]
            {
                ResolutionStatusEnum.Confirmed,
                ResolutionStatusEnum.Rejected,
                ResolutionStatusEnum.WaitingForConfirmation
            };
        }

        public bool CanEdit()
        {
            return true; // Limited editing by Legal Council/Board Secretary
        }

        public bool CanComplete()
        {
            return false; // Cannot complete while waiting for confirmation
        }

        public bool CanCancel()
        {
            return false; // Cannot cancel while waiting for confirmation
        }

        public IEnumerable<string> GetValidationMessages()
        {
            return new[]
            {
                "Resolution is waiting for Fund Manager confirmation"
            };
        }

        public string GetStateDescriptionKey()
        {
            return "ResolutionInWaitingForConfirmationState";
        }
    }
}
