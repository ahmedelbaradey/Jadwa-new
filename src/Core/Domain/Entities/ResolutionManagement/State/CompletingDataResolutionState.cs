using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ResolutionManagement.State
{
    /// <summary>
    /// Represents a resolution in completing data status
    /// Legal Council/Board Secretary is completing resolution data
    /// Can transition to WaitingForConfirmation or remain in CompletingData
    /// Allows editing and completion operations
    /// </summary>
    public class CompletingDataResolutionState : IResolutionState
    {
        public ResolutionStatusEnum Status => ResolutionStatusEnum.CompletingData;

        public void Handle(Resolution resolution)
        {
            // CompletingData state allows legal council/board secretary to complete data
        }

        public bool CanTransitionTo(ResolutionStatusEnum targetStatus)
        {
            return targetStatus == ResolutionStatusEnum.WaitingForConfirmation ||
                   targetStatus == ResolutionStatusEnum.CompletingData; // Can save as draft
        }

        public IEnumerable<ResolutionStatusEnum> GetAllowedTransitions()
        {
            return new[]
            {
                ResolutionStatusEnum.WaitingForConfirmation,
                ResolutionStatusEnum.CompletingData
            };
        }

        public bool CanEdit()
        {
            return true; // CompletingData resolutions can be edited
        }

        public bool CanComplete()
        {
            return true; // CompletingData resolutions can be completed
        }

        public bool CanCancel()
        {
            return false; // Cannot cancel while completing data
        }

        public IEnumerable<string> GetValidationMessages()
        {
            return new[]
            {
                "Resolution data is being completed by Legal Council or Board Secretary"
            };
        }

        public string GetStateDescriptionKey()
        {
            return "ResolutionInCompletingDataState";
        }
    }
}
