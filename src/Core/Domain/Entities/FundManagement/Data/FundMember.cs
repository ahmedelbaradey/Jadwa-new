using Domain.Entities.Base;
using Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities.FundManagement;

public class FundMember : FullAuditedEntity
{
    public int FundId { get; set; }
    public int UserId { get; set; }

    [ForeignKey("FundId")]
    public Fund Fund { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
}
