using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Base;


namespace Domain.Entities.Shared
{
    public class Attachment : FullAuditedEntity
    {
        public string FileName { get; set; }
        public string Path { get; set; }
        public int ModuleId { get; set; }
        public long FileSize { get; set; }
        public string ContentType { get; set; } = string.Empty;
    }
}
