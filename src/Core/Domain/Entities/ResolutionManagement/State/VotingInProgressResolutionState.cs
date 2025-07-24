using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ResolutionManagement.State
{
    /// <summary>
    /// Represents a resolution with voting in progress
    /// Can transition to Approved, NotApproved, or back to WaitingForConfirmation (if voting suspended)
    /// Limited editing allowed with voting suspension
    /// </summary>
    public class VotingInProgressResolutionState : IResolutionState
    {
        public ResolutionStatusEnum Status => ResolutionStatusEnum.VotingInProgress;

        public void Handle(Resolution resolution)
        {
            // VotingInProgress state manages active voting process
        }

        public bool CanTransitionTo(ResolutionStatusEnum targetStatus)
        {
            return targetStatus == ResolutionStatusEnum.Approved ||
                   targetStatus == ResolutionStatusEnum.NotApproved ||
                   targetStatus == ResolutionStatusEnum.WaitingForConfirmation; // If voting suspended
        }

        public IEnumerable<ResolutionStatusEnum> GetAllowedTransitions()
        {
            return new[]
            {
                ResolutionStatusEnum.Approved,
                ResolutionStatusEnum.NotApproved,
                ResolutionStatusEnum.WaitingForConfirmation
            };
        }

        public bool CanEdit()
        {
            return true; // Limited editing with voting suspension
        }

        public bool CanComplete()
        {
            return false; // Cannot complete while voting is in progress
        }

        public bool CanCancel()
        {
            return false; // Cannot cancel while voting is in progress
        }

        public IEnumerable<string> GetValidationMessages()
        {
            return new[]
            {
                "Resolution voting is in progress. Editing requires voting suspension."
            };
        }

        public string GetStateDescriptionKey()
        {
            return "ResolutionInVotingInProgressState";
        }
    }
}
