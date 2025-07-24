using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Application.Mapping
{
 
    public partial class AttachmentsProfile : Profile
    {
        public AttachmentsProfile()
        {
            AddAttachmentMapping();
            MinIOAttachmentMapping();
        }
    }
}
