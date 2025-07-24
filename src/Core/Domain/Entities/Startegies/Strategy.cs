using Domain.Entities.Base;


namespace Domain.Entities.Startegies;

public class Strategy : FullAuditedEntity
{
    public string NameAr { get; set; }
    public string NameEn { get; set; }
}
