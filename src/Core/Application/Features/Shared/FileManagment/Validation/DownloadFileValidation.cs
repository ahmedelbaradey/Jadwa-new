using Application.Features.Shared.FileManagment.Commands.NewFolder;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.Shared.FileManagment.Validation
{
    public class DownloadFileValidation : AbstractValidator<DownloadAttachmentCommand>
    {
        public DownloadFileValidation(IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(localizer[SharedResourcesKey.RequiredField])
                .NotNull().WithMessage(localizer[SharedResourcesKey.RequiredField]);
        }
    }
}
