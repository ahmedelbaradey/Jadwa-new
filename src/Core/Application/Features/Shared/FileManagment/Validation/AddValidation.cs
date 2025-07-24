using Application.Common.Configurations;
using Application.Features.Shared.FileManagment.Commands.Add;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Resources;

namespace Application.Features.Shared.FileManagment.Validation
{
    public class AddValidation : AbstractValidator<AddAttachmentCommand>
    {
        public AddValidation(IStringLocalizer<SharedResources> localizer, IOptionsMonitor<AttachmentConfiguration> configuration)
        {
            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage(localizer[SharedResourcesKey.EmptyNameValidation])
                .NotNull().WithMessage(localizer[SharedResourcesKey.EmptyNameValidation]);

            RuleFor(x => x.File)
            .NotNull().WithMessage(localizer[SharedResourcesKey.Required])
            .Must(file => file.Length <= configuration.CurrentValue.MaxSize)
            .WithMessage(localizer[SharedResourcesKey.MaxFileSize, 10]);
        }
    }
}
