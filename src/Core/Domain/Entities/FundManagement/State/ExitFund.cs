using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.FundManagement.State
{
    /// <summary>
    /// Represents an exited fund - final state
    /// No transitions allowed from this state
    /// </summary>
    public class ExitedFund : IFundState
    {
        public FundStatusEnum Status => FundStatusEnum.Exited;

        public void Handle(Fund fund)
        {
            // Final state - no transitions allowed
        }

        public bool CanTransitionTo(FundStatusEnum targetStatus)
        {
            return false; // No transitions from exited state
        }

        public IEnumerable<FundStatusEnum> GetAllowedTransitions()
        {
            return Enumerable.Empty<FundStatusEnum>();
        }
    }
}
