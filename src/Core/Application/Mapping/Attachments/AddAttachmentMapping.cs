using Application.Features.Catalog.Categories.Commands.Add;
using Application.Features.Shared.FileManagment.Commands.Add;
using AutoMapper;
using Domain.Entities.Shared;

namespace Application.Mapping
{
    public partial class AttachmentsProfile
    {
        public void AddAttachmentMapping()
        {
            CreateMap<AddAttachmentCommand, Attachment>();
        }
    }
}
