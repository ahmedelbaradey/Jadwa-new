using Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Base;

namespace Domain.Entities.Base
{ 

    public class FullAuditedEntity : AduitedEntity
    {
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
        public int? DeletedBy { get; set; }
        [ForeignKey("DeletedBy")]
        public User? DeletedByUser { get; set; }

    }

}
