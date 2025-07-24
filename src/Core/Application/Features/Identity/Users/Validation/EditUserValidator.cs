using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contracts.Identity;
using Abstraction.Constants;
using Application.Features.Identity.Users.Commands.EditUser;
using Abstraction.Contracts.Repository;

namespace Application.Features.Identity.Users.Validation
{
    /// <summary>
    /// Validator for EditUserCommand with Sprint 3 enhancements
    /// Implements mobile number validation and unique role checking with current user exclusion
    /// </summary>
    public class EditUserValidator : AbstractValidator<EditUserCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IRepositoryManager _iRepositoryManager;
        private readonly IIdentityServiceManager _identityServiceManager;

        public EditUserValidator(
            IStringLocalizer<SharedResources> localizer,
            IRepositoryManager iRepositoryManager,
            IIdentityServiceManager identityServiceManager)
        {
            _localizer = localizer;
            _iRepositoryManager = iRepositoryManager;
            _identityServiceManager = identityServiceManager;
            Include(new BaseUserValidator(_localizer));
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            // User ID Validation
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKey.ProfileRequiredField]);

            // Role Validation - Basic requirements
            RuleFor(x => x.Roles)
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKey.AtLeastOneRoleRequired])
                .Must(roles => roles.Count > 0)
                .WithMessage(_localizer[SharedResourcesKey.AtLeastOneRoleRequired]);

            // Role Selection Logic Validation
            RuleFor(x => x.Roles)
                .Must(BeValidRoleSelection)
                .WithMessage(_localizer[SharedResourcesKey.EditUserInvalidRoleSelection]);

            // Board Member Role Change Restriction
            RuleFor(x => x)
                .MustAsync(async (command, cancellation) =>
                {
                    var user = await _identityServiceManager.UserManagmentService.FindByIdWithRolesAsync(command.Id.ToString());

                    var hasBoardMemberRole = user.Roles.Select(c => c.Name).Contains(RoleHelper.BoardMember);
                    var willRemoveBoardMemberRole = hasBoardMemberRole && !command.Roles.Contains(RoleHelper.BoardMember);

                    if (willRemoveBoardMemberRole)
                    {
                        var canRemove = await _iRepositoryManager.BoardMembers.CanRemoveBoardMemberRoleAsync(command.Id);
                        return canRemove;
                    }

                    return true;
                })
                .WithMessage(_localizer[SharedResourcesKey.EditUserCannotChangeBoardMemberRole]);

            // Fund Manager Role Change Restriction
            RuleFor(x => x)
                .MustAsync(async (command, cancellation) =>
                {
                    var user = await _identityServiceManager.UserManagmentService.FindByIdWithRolesAsync(command.Id.ToString());
                    var hasFundManagerRole = user.Roles.Select(c => c.Name).Contains(RoleHelper.FundManager);
                    var willRemoveFundManagerRole = hasFundManagerRole && !command.Roles.Contains(RoleHelper.FundManager);

                    if (willRemoveFundManagerRole)
                    {
                        var canRemove = await _iRepositoryManager.FundManagers.CanRemoveFundManagerRoleAsync(command.Id);
                        return canRemove;
                    }

                    return true;
                })
                .WithMessage(string.Format(_localizer[SharedResourcesKey.EditUserCannotChangeFundManagerRole], "Fund Manager"));

            // Associate Fund Manager Role Change Restriction
            //RuleFor(x => x)
            //    .MustAsync(async (command, cancellation) =>
            //    {                  
            //        var user = await _identityServiceManager.UserManagmentService.FindByIdWithRolesAsync(command.Id.ToString());
            //        var hasAssociateFundManagerRole = user.Roles.Select(c=>c.Name).Contains(RoleHelper.AssociateFundManager);
            //        var willRemoveAssociateFundManagerRole = hasAssociateFundManagerRole && !command.Roles.Contains(RoleHelper.AssociateFundManager);

            //        if (willRemoveAssociateFundManagerRole)
            //        {
            //            var (canRemove, _) = await _fundAssignmentService.CanRemoveAssociateFundManagerRoleAsync(command.Id);
            //            return canRemove;
            //        }

            //        return true;
            //    })
            //    .WithMessage(string.Format(_localizer[SharedResourcesKey.EditUserCannotChangeFundManagerRole], "Associate Fund Manager"));

            // File Validation
            RuleFor(x => x.CVFile)
                .Must(BeValidCVFile)
                .WithMessage(_localizer[SharedResourcesKey.EditUserInvalidCVFile])
                .When(x => !string.IsNullOrWhiteSpace(x.CVFile));

            RuleFor(x => x.PersonalPhoto)
                .Must(BeValidPhotoFile)
                .WithMessage(_localizer[SharedResourcesKey.ProfileInvalidPhotoFile])
                .When(x => !string.IsNullOrWhiteSpace(x.PersonalPhoto));
        }

        /// <summary>
        /// Validates role selection logic according to JDWA-1251 requirements
        /// Multi-select enabled ONLY IF roles are ('Fund Manager' AND 'Board Member') OR ('Associate Fund Manager' AND 'Board Member')
        /// Otherwise, single role selection only
        /// </summary>
        private bool BeValidRoleSelection(List<string> roles)
        {
            if (roles == null || roles.Count == 0)
                return false;

            // Single role is always valid
            if (roles.Count == 1)
                return true;

            // Multi-select is only allowed for specific combinations
            if (roles.Count == 2)
            {
                var hasValidCombination =
                    (roles.Contains(RoleHelper.FundManager) && roles.Contains(RoleHelper.BoardMember)) ||
                    (roles.Contains(RoleHelper.AssociateFundManager) && roles.Contains(RoleHelper.BoardMember));

                return hasValidCombination;
            }

            // More than 2 roles is not allowed
            return false;
        }

        /// <summary>
        /// Validates CV file format and size (string path validation)
        /// </summary>
        private bool BeValidCVFile(string? filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return true; // Optional field

            // Basic validation - in real implementation, this would validate file path/URL
            // For now, just check if it's a reasonable string
            return filePath.Length <= 500; // Reasonable path length
        }

        /// <summary>
        /// Validates personal photo file format and size (string path validation)
        /// </summary>
        private bool BeValidPhotoFile(string? filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return true; // Optional field

            // Basic validation - in real implementation, this would validate file path/URL
            // For now, just check if it's a reasonable string
            return filePath.Length <= 500; // Reasonable path length
        }
    }
}
