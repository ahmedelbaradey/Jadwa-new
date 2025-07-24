using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.FundManagement.State
{
    /// <summary>
    /// Represents a fund that is under construction
    /// Can transition to WaitingForAddingMembers or Active state
    /// </summary>
    public class UnderConstructionFund : IFundState
    {
        public FundStatusEnum Status => FundStatusEnum.UnderConstruction;

        public void Handle(Fund fund)
        {
            fund.ChangeState(new WaitingForAddingMembersFund());
        }

        public bool CanTransitionTo(FundStatusEnum targetStatus)
        {
            return targetStatus == FundStatusEnum.WaitingForAddingMembers ||
                   targetStatus == FundStatusEnum.Active;
        }

        public IEnumerable<FundStatusEnum> GetAllowedTransitions()
        {
            return new[] { FundStatusEnum.WaitingForAddingMembers, FundStatusEnum.Active };
        }
    }
}
