using Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ResolutionManagement
{
    public class ResolutionStatus : BaseEntity
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
