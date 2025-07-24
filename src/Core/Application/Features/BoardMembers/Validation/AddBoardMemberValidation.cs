using Abstraction.Contracts.Repository;
using Application.Features.BoardMembers.Commands.Add;
using Domain.Entities.FundManagement;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.BoardMembers.Commands.AddBoardMember
{
    /// <summary>
    /// Validation rules for AddBoardMemberCommand
    /// Implements FluentValidation for comprehensive input validation
    /// Supports localization for Arabic and English error messages
    /// </summary>
    public class AddBoardMemberValidation : AbstractValidator<AddBoardMemberCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IRepositoryManager _repository;

        public AddBoardMemberValidation(IStringLocalizer<SharedResources> localizer, IRepositoryManager repository)
        {
            _localizer = localizer;
            _repository = repository;
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            // Fund ID validation
            RuleFor(x => x.FundId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKey.InvalidIdValidation]);

            // User ID validation
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKey.InvalidIdValidation]);

            // Member type validation
            RuleFor(x => x.MemberType)
                .IsInEnum()
                .WithMessage(_localizer[SharedResourcesKey.InvalidBoardMemberType]);

            // Business rule: Independent member type validation
            RuleFor(x => x.MemberType)
                .Must(BeValidMemberType)
                .WithMessage(_localizer[SharedResourcesKey.InvalidBoardMemberType]);

            // Chairman validation
            RuleFor(x => x.IsChairman)
                .NotNull()
                .WithMessage(_localizer[SharedResourcesKey.RequiredField]);

            // Business rule: User must not already be a board member of this fund
            RuleFor(x => x)
                .MustAsync(async (command, cancellation) =>
                    !await _repository.BoardMembers.IsUserBoardMemberAsync(command.FundId, command.UserId))
                .WithMessage(_localizer[SharedResourcesKey.UserAlreadyBoardMember]);

            // Business rule: Maximum 14 independent board members per fund
            RuleFor(x => x)
                .MustAsync(async (command, cancellation) =>
                {
                    if (command.MemberType != BoardMemberType.Independent)
                        return true; // Rule only applies to independent members

                    var independentCount = await _repository.BoardMembers.GetActiveIndependentMemberCountAsync(command.FundId);
                    return independentCount < 14;
                })
                .WithMessage(_localizer[SharedResourcesKey.MaxIndependentMembersReached]);

            // Business rule: Only one chairman per fund
            RuleFor(x => x)
                .MustAsync(async (command, cancellation) =>
                {
                    if (!command.IsChairman)
                        return true; // Rule only applies when trying to set as chairman

                    var existingChairman = await _repository.BoardMembers.GetChairmanByFundIdAsync(command.FundId);
                    return existingChairman == null;
                })
                .WithMessage(_localizer[SharedResourcesKey.FundAlreadyHasChairman]);
        }

        /// <summary>
        /// Validates that the member type is a valid enum value
        /// </summary>
        /// <param name="memberType">The member type to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        private static bool BeValidMemberType(BoardMemberType memberType)
        {
            return memberType == BoardMemberType.Independent || 
                   memberType == BoardMemberType.NotIndependent;
        }
    }
}
