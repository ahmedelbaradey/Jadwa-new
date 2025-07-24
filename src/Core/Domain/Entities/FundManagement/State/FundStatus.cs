using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.FundManagement.State
{
    public enum FundStatus
    {
        New = 0,
        UnderConstruction = 1,
        WaitingForAddingMembers = 2,
        Active = 3,
        Exited = 4,
    }
}
