using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.FundManagement.State
{
    /// <summary>
    /// Represents a fund that is waiting for board members to be added
    /// Can transition to Active state when 2+ independent members are added
    /// </summary>
    public class WaitingForAddingMembersFund : IFundState
    {
        public FundStatusEnum Status => FundStatusEnum.WaitingForAddingMembers;

        public void Handle(Fund fund)
        {
            // This state waits for external trigger (adding board members)
            // Transition to Active happens when business rules are met
        }

        public bool CanTransitionTo(FundStatusEnum targetStatus)
        {
            return targetStatus == FundStatusEnum.Active ||
                   targetStatus == FundStatusEnum.Exited;
        }

        public IEnumerable<FundStatusEnum> GetAllowedTransitions()
        {
            return new[] { FundStatusEnum.Active, FundStatusEnum.Exited };
        }
    }
}
