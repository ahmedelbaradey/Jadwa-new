using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ResolutionManagement.State
{
    /// <summary>
    /// Represents a confirmed resolution
    /// Can transition to VotingInProgress or be edited (with restrictions)
    /// Limited editing allowed by Legal Council/Board Secretary
    /// </summary>
    public class ConfirmedResolutionState : IResolutionState
    {
        public ResolutionStatusEnum Status => ResolutionStatusEnum.Confirmed;

        public void Handle(Resolution resolution)
        {
            // Confirmed state can be sent to voting or edited with restrictions
        }

        public bool CanTransitionTo(ResolutionStatusEnum targetStatus)
        {
            return targetStatus == ResolutionStatusEnum.VotingInProgress ||
                   targetStatus == ResolutionStatusEnum.WaitingForConfirmation; // Can be edited and sent back
        }

        public IEnumerable<ResolutionStatusEnum> GetAllowedTransitions()
        {
            return new[]
            {
                ResolutionStatusEnum.VotingInProgress,
                ResolutionStatusEnum.WaitingForConfirmation
            };
        }

        public bool CanEdit()
        {
            return true; // Limited editing by Legal Council/Board Secretary
        }

        public bool CanComplete()
        {
            return false; // Cannot complete confirmed resolutions
        }

        public bool CanCancel()
        {
            return false; // Cannot cancel confirmed resolutions
        }

        public IEnumerable<string> GetValidationMessages()
        {
            return new[]
            {
                "Resolution has been confirmed and can be sent to voting"
            };
        }

        public string GetStateDescriptionKey()
        {
            return "ResolutionInConfirmedState";
        }
    }
}
