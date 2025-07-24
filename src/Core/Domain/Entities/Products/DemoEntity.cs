using Domain.Entities.Base;

namespace Domain.Entities.Products
{
    public class DemoEntity : FullAuditedEntity
    {    
        public string Name { get; set; }
        public string Description { get; set; }        
    }    
}
