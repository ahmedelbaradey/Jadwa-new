using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.FundManagement.State
{
    /// <summary>
    /// Represents an active fund with 2+ independent board members
    /// Can transition to Exited state
    /// </summary>
    public class ActiveFund : IFundState
    {
        public FundStatusEnum Status => FundStatusEnum.Active;

        public void Handle(Fund fund)
        {
            // Active fund can transition to exited when business rules are met
        }

        public bool CanTransitionTo(FundStatusEnum targetStatus)
        {
            return targetStatus == FundStatusEnum.Exited;
        }

        public IEnumerable<FundStatusEnum> GetAllowedTransitions()
        {
            return new[] { FundStatusEnum.Exited };
        }
    }
}
