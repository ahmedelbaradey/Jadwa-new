using Application.Features.Identity.Users.Commands.EditUser;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Localization;
using Moq;
using Resources;
using Xunit;
using Abstraction.Contracts.Identity;
using Domain.Entities.Users;

namespace Application.UnitTests.Features.Identity.Users.Commands.EditUser
{
    /// <summary>
    /// Unit tests for EditUserCommandValidator
    /// Tests mobile number validation and unique role checking with current user exclusion
    /// </summary>
    public class EditUserCommandValidatorTests
    {
        private readonly Mock<IStringLocalizer<SharedResources>> _mockLocalizer;
        private readonly Mock<IUserManagmentService> _mockUserService;
        private readonly EditUserCommandValidator _validator;

        public EditUserCommandValidatorTests()
        {
            _mockLocalizer = new Mock<IStringLocalizer<SharedResources>>();
            _mockUserService = new Mock<IUserManagmentService>();
            
            // Setup localizer to return the key as value for testing
            _mockLocalizer.Setup(x => x[It.IsAny<string>()])
                .Returns((string key) => new LocalizedString(key, key));
            _mockLocalizer.Setup(x => x[It.IsAny<string>(), It.IsAny<object[]>()])
                .Returns((string key, object[] args) => new LocalizedString(key, key));

            _validator = new EditUserCommandValidator(_mockLocalizer.Object, _mockUserService.Object);
        }

        #region Mobile Number Validation Tests

        [Theory]
        [InlineData("0551234567")] // Valid Saudi mobile (05 + STC prefix 5 + 7 digits)
        [InlineData("0501234567")] // Valid Saudi mobile (05 + STC prefix 0 + 7 digits)
        [InlineData("0531234567")] // Valid Saudi mobile (05 + STC prefix 3 + 7 digits)
        [InlineData("0561234567")] // Valid Saudi mobile (05 + Mobily prefix 6 + 7 digits)
        [InlineData("0541234567")] // Valid Saudi mobile (05 + Mobily prefix 4 + 7 digits)
        [InlineData("0591234567")] // Valid Saudi mobile (05 + Zain prefix 9 + 7 digits)
        [InlineData("0581234567")] // Valid Saudi mobile (05 + Zain prefix 8 + 7 digits)
        [InlineData("0571234567")] // Valid Saudi mobile (05 + MVNO prefix 7 + 7 digits)
        [InlineData("0511234567")] // Valid Saudi mobile (05 + Bravo prefix 1 + 7 digits)
        [InlineData("551234567")] // Valid short format (5 + telecom prefix + 7 digits)
        public void PhoneNumber_WithValidSaudiFormat_ShouldNotHaveValidationError(string phoneNumber)
        {
            // Given
            var command = CreateValidEditUserCommand();
            command.PhoneNumber = phoneNumber;
            command.UserName = phoneNumber; // Username should match phone number

            // When
            var result = _validator.TestValidate(command);

            // Then
            result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
        }

        [Theory]
        [InlineData("")] // Empty
        [InlineData(null)] // Null
        [InlineData("   ")] // Whitespace
        public void PhoneNumber_WithEmptyValue_ShouldHaveValidationError(string phoneNumber)
        {
            // Given
            var command = CreateValidEditUserCommand();
            command.PhoneNumber = phoneNumber;

            // When
            var result = _validator.TestValidate(command);

            // Then
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                .WithErrorMessage(SharedResourcesKey.MobileNumberRequired);
        }

        [Theory]
        [InlineData("0412345678")] // Wrong prefix (04 instead of 05)
        [InlineData("0612345678")] // Wrong prefix (06 instead of 05)
        [InlineData("0522345678")] // Invalid telecom prefix (2 is not valid)
        [InlineData("05123456789")] // Too long (extra digit)
        [InlineData("051234567")] // Too short (missing digit)
        [InlineData("0512345abc")] // Contains letters
        [InlineData("05-123-4567")] // Contains dashes
        [InlineData("05 123 4567")] // Contains spaces
        [InlineData("+96622345678")] // Invalid telecom prefix in international format
        [InlineData("96622345678")] // Invalid telecom prefix in country code format
        [InlineData("123456789")] // Random number
        [InlineData("0123456789")] // Wrong country/area code
        public void PhoneNumber_WithInvalidSaudiFormat_ShouldHaveValidationError(string phoneNumber)
        {
            // Given
            var command = CreateValidEditUserCommand();
            command.PhoneNumber = phoneNumber;

            // When
            var result = _validator.TestValidate(command);

            // Then
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                .WithErrorMessage(SharedResourcesKey.InvalidSaudiMobilePattern);
        }

        #endregion

        #region Unique Role Validation Tests (with Current User Exclusion)

        [Fact]
        public async Task Roles_WithUniqueRoleAndNoConflict_ShouldNotHaveValidationError()
        {
            // Given
            var command = CreateValidEditUserCommand();
            command.Id = 123;
            command.Roles = new List<string> { "Legal Counsel" };

            _mockUserService.Setup(x => x.FindActiveUserWithOnlyRoleAsync("Legal Counsel", 123))
                .ReturnsAsync((User)null); // No existing user with this role (excluding current user)

            // When
            var result = await _validator.TestValidateAsync(command);

            // Then
            result.ShouldNotHaveValidationErrorFor(x => x);
        }

        [Fact]
        public async Task Roles_WithUniqueRoleAndConflictExcludingCurrentUser_ShouldHaveValidationError()
        {
            // Given
            var command = CreateValidEditUserCommand();
            command.Id = 123;
            command.Roles = new List<string> { "Finance Controller" };

            var existingUser = new User { Id = 999, FullName = "Another User" };
            _mockUserService.Setup(x => x.FindActiveUserWithOnlyRoleAsync("Finance Controller", 123))
                .ReturnsAsync(existingUser); // Another user (not current user) has this role

            // When
            var result = await _validator.TestValidateAsync(command);

            // Then
            result.ShouldHaveValidationErrorFor(x => x)
                .WithErrorMessage(SharedResourcesKey.UniqueRoleAlreadyAssigned);
        }

        [Fact]
        public async Task Roles_WithCurrentUserHavingSameUniqueRole_ShouldNotHaveValidationError()
        {
            // Given
            var command = CreateValidEditUserCommand();
            command.Id = 123;
            command.Roles = new List<string> { "Compliance and Legal Managing Director" };

            // Current user already has this role, so no conflict when excluding current user
            _mockUserService.Setup(x => x.FindActiveUserWithOnlyRoleAsync("Compliance and Legal Managing Director", 123))
                .ReturnsAsync((User)null);

            // When
            var result = await _validator.TestValidateAsync(command);

            // Then
            result.ShouldNotHaveValidationErrorFor(x => x);
        }

        [Fact]
        public async Task Roles_WithMultipleUniqueRoles_ShouldValidateEachUniqueRole()
        {
            // Given
            var command = CreateValidEditUserCommand();
            command.Id = 123;
            command.Roles = new List<string> { "Legal Counsel", "Finance Controller" };

            // Legal Counsel is available
            _mockUserService.Setup(x => x.FindActiveUserWithOnlyRoleAsync("Legal Counsel", 123))
                .ReturnsAsync((User)null);

            // Finance Controller is taken by another user
            var existingUser = new User { Id = 999, FullName = "Another User" };
            _mockUserService.Setup(x => x.FindActiveUserWithOnlyRoleAsync("Finance Controller", 123))
                .ReturnsAsync(existingUser);

            // When
            var result = await _validator.TestValidateAsync(command);

            // Then
            result.ShouldHaveValidationErrorFor(x => x)
                .WithErrorMessage(SharedResourcesKey.UniqueRoleAlreadyAssigned);
        }

        [Fact]
        public void UserId_WithZeroOrNegativeValue_ShouldHaveValidationError()
        {
            // Given
            var command = CreateValidEditUserCommand();
            command.Id = 0;

            // When
            var result = _validator.TestValidate(command);

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage(SharedResourcesKey.ProfileRequiredField);
        }

        [Fact]
        public void Roles_WithEmptyRolesList_ShouldHaveValidationError()
        {
            // Given
            var command = CreateValidEditUserCommand();
            command.Roles = new List<string>();

            // When
            var result = _validator.TestValidate(command);

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Roles)
                .WithErrorMessage(SharedResourcesKey.AtLeastOneRoleRequired);
        }

        #endregion

        #region Helper Methods

        private EditUserCommand CreateValidEditUserCommand()
        {
            return new EditUserCommand
            {
                Id = 123,
                FullName = "Test User",
                Email = "test@example.com",
                PhoneNumber = "05123456789",
                UserName = "05123456789",
                CountryCode = "+966",
                PreferredLanguage = "ar-EG",
                Roles = new List<string> { "Board Member" }
            };
        }

        #endregion
    }
}
