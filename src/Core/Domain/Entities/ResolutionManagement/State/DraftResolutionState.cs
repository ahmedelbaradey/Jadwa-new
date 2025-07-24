using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ResolutionManagement.State
{
    /// <summary>
    /// Represents a resolution in draft status
    /// Can transition to Pending, CompletingData, or Cancelled states
    /// Allows editing and completion operations
    /// </summary>
    public class DraftResolutionState : IResolutionState
    {
        public ResolutionStatusEnum Status => ResolutionStatusEnum.Draft;

        public void Handle(Resolution resolution)
        {
            // Draft state allows editing and can transition to pending or completing data
        }

        public bool CanTransitionTo(ResolutionStatusEnum targetStatus)
        {
            return targetStatus == ResolutionStatusEnum.Pending ||
                   targetStatus == ResolutionStatusEnum.Draft; // Can save as draft again
        }

        public IEnumerable<ResolutionStatusEnum> GetAllowedTransitions()
        {
            return new[]
            {
                ResolutionStatusEnum.Pending,
                ResolutionStatusEnum.Draft
            };
        }

        public bool CanEdit()
        {
            return true; // Draft resolutions can be edited
        }

        public bool CanComplete()
        {
            return true; // Draft resolutions can be completed
        }

        public bool CanCancel()
        {
            return false; // Draft resolutions can be deleted, not cancelled
        }

        public IEnumerable<string> GetValidationMessages()
        {
            return new[]
            {
                "Resolution is in draft status and can be edited or sent for review"
            };
        }

        public string GetStateDescriptionKey()
        {
            return "ResolutionInDraftState";
        }
    }
}
