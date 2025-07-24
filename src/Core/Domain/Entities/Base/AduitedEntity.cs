using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Users;
using Domain.Entities.Base;

namespace Domain.Entities.Base
{
    public class AduitedEntity : CreationAuditedEntity
    {  
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        [ForeignKey("UpdatedBy")]
        public User? UpdatedByUser { get; set; }
    }
}
