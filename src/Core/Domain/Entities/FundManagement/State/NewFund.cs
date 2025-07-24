using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.FundManagement.State
{
    /// <summary>
    /// Represents the initial state of a fund when it's first created
    /// Transitions to UnderConstruction state automatically
    /// </summary>
    public class NewFund : IFundState
    {
        public FundStatusEnum Status => FundStatusEnum.UnderConstruction;

        public void Handle(Fund fund)
        {
            fund.ChangeState(new UnderConstructionFund());
        }

        public bool CanTransitionTo(FundStatusEnum targetStatus)
        {
            return targetStatus == FundStatusEnum.UnderConstruction;
        }

        public IEnumerable<FundStatusEnum> GetAllowedTransitions()
        {
            return new[] { FundStatusEnum.UnderConstruction };
        }
    }
}
