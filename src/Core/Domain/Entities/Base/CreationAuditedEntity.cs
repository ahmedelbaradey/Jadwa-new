using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Users;
using Domain.Entities.Base;

namespace Domain.Entities.Base
{
    
    public class CreationAuditedEntity : BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public User CreatedByUser { get; set; }
    }

}
