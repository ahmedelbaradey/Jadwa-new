using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ResolutionManagement.State
{
    /// <summary>
    /// Represents a resolution in pending status (sent for legal review)
    /// Can transition to CompletingData, WaitingForConfirmation, or Cancelled states
    /// Allows editing and completion operations by Legal Council/Board Secretary
    /// </summary>
    public class PendingResolutionState : IResolutionState
    {
        public ResolutionStatusEnum Status => ResolutionStatusEnum.Pending;

        public void Handle(Resolution resolution)
        {
            // Pending state waits for legal council/board secretary to complete data
        }

        public bool CanTransitionTo(ResolutionStatusEnum targetStatus)
        {
            return targetStatus == ResolutionStatusEnum.CompletingData ||
                   targetStatus == ResolutionStatusEnum.WaitingForConfirmation ||
                   targetStatus == ResolutionStatusEnum.Pending || // Can save as pending again
                   targetStatus == ResolutionStatusEnum.Cancelled;
        }

        public IEnumerable<ResolutionStatusEnum> GetAllowedTransitions()
        {
            return new[]
            {
                ResolutionStatusEnum.CompletingData,
                ResolutionStatusEnum.WaitingForConfirmation,
                ResolutionStatusEnum.Pending,
                ResolutionStatusEnum.Cancelled
            };
        }

        public bool CanEdit()
        {
            return true; // Pending resolutions can be edited by Fund Manager
        }

        public bool CanComplete()
        {
            return true; // Pending resolutions can be completed by Legal Council/Board Secretary
        }

        public bool CanCancel()
        {
            return true; // Pending resolutions can be cancelled by Fund Manager
        }

        public IEnumerable<string> GetValidationMessages()
        {
            return new[]
            {
                "Resolution is pending legal review and can be completed by Legal Council or Board Secretary"
            };
        }

        public string GetStateDescriptionKey()
        {
            return "ResolutionInPendingState";
        }
    }
}
