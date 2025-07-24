using Application.Features.Resolutions.Commands.Add;
using Domain.Entities.ResolutionManagement;

namespace Application.Mapping
{
    public partial class ResolutionsProfile
    {
        public void AddResolutionMapping()
        {
            CreateMap<AddResolutionCommand, Resolution>();
        }
    }
}
