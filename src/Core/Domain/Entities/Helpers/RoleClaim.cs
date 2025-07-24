using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helpers
{
    public class RoleClaim
    {
        public string Type { get; set; }
        public string Value { get; set; }
       
    }
    public class RoleClaims : RoleClaim
    {
        public bool HasClaim { get; set; }
    }
}
