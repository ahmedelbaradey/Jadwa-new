using Application.Features.Identity.Users.Commands.AddUser;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Localization;
using Moq;
using Resources;
using Xunit;
using Abstraction.Contracts.Identity;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Http;

namespace Application.UnitTests.Features.Identity.Users.Commands.AddUser
{
    /// <summary>
    /// Unit tests for AddUserCommandValidator
    /// Tests mobile number validation and unique role checking
    /// </summary>
    public class AddUserCommandValidatorTests
    {
        private readonly Mock<IStringLocalizer<SharedResources>> _mockLocalizer;
        private readonly Mock<IUserManagmentService> _mockUserService;
        private readonly AddUserCommandValidator _validator;

        public AddUserCommandValidatorTests()
        {
            _mockLocalizer = new Mock<IStringLocalizer<SharedResources>>();
            _mockUserService = new Mock<IUserManagmentService>();
            
            // Setup localizer to return the key as value for testing
            _mockLocalizer.Setup(x => x[It.IsAny<string>()])
                .Returns((string key) => new LocalizedString(key, key));
            _mockLocalizer.Setup(x => x[It.IsAny<string>(), It.IsAny<object[]>()])
                .Returns((string key, object[] args) => new LocalizedString(key, key));

            _validator = new AddUserCommandValidator(_mockLocalizer.Object, _mockUserService.Object);
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
            var command = CreateValidAddUserCommand();
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
            var command = CreateValidAddUserCommand();
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
            var command = CreateValidAddUserCommand();
            command.PhoneNumber = phoneNumber;

            // When
            var result = _validator.TestValidate(command);

            // Then
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                .WithErrorMessage(SharedResourcesKey.InvalidSaudiMobilePattern);
        }

        [Fact]
        public void UserName_WhenNotMatchingPhoneNumber_ShouldHaveValidationError()
        {
            // Given
            var command = CreateValidAddUserCommand();
            command.PhoneNumber = "05123456789";
            command.UserName = "differentusername";

            // When
            var result = _validator.TestValidate(command);

            // Then
            result.ShouldHaveValidationErrorFor(x => x.UserName)
                .WithErrorMessage("Username must match the mobile number");
        }

        [Fact]
        public void UserName_WhenMatchingPhoneNumber_ShouldNotHaveValidationError()
        {
            // Given
            var command = CreateValidAddUserCommand();
            command.PhoneNumber = "05123456789";
            command.UserName = "05123456789";

            // When
            var result = _validator.TestValidate(command);

            // Then
            result.ShouldNotHaveValidationErrorFor(x => x.UserName);
        }

        #endregion

        #region Unique Role Validation Tests

        [Fact]
        public async Task Roles_WithUniqueRoleAndNoConflict_ShouldNotHaveValidationError()
        {
            // Given
            var command = CreateValidAddUserCommand();
            command.Roles = new List<string> { "Legal Counsel" };

            _mockUserService.Setup(x => x.FindActiveUserWithOnlyRoleAsync("Legal Counsel", null))
                .ReturnsAsync((User)null); // No existing user with this role

            // When
            var result = await _validator.TestValidateAsync(command);

            // Then
            result.ShouldNotHaveValidationErrorFor(x => x);
        }

        [Fact]
        public async Task Roles_WithUniqueRoleAndConflict_ShouldHaveValidationError()
        {
            // Given
            var command = CreateValidAddUserCommand();
            command.Roles = new List<string> { "Finance Controller" };

            var existingUser = new User { Id = 999, FullName = "Existing User" };
            _mockUserService.Setup(x => x.FindActiveUserWithOnlyRoleAsync("Finance Controller", null))
                .ReturnsAsync(existingUser); // Existing user with this role

            // When
            var result = await _validator.TestValidateAsync(command);

            // Then
            result.ShouldHaveValidationErrorFor(x => x)
                .WithErrorMessage(SharedResourcesKey.UniqueRoleAlreadyAssigned);
        }

        [Fact]
        public async Task Roles_WithNonUniqueRole_ShouldNotHaveValidationError()
        {
            // Given
            var command = CreateValidAddUserCommand();
            command.Roles = new List<string> { "Board Member" }; // Non-unique role

            // When
            var result = await _validator.TestValidateAsync(command);

            // Then
            result.ShouldNotHaveValidationErrorFor(x => x);
        }

        [Fact]
        public async Task Roles_WithMultipleRolesIncludingUnique_ShouldValidateUniqueRole()
        {
            // Given
            var command = CreateValidAddUserCommand();
            command.Roles = new List<string> { "Board Member", "Head of Real Estate" };

            var existingUser = new User { Id = 999, FullName = "Existing User" };
            _mockUserService.Setup(x => x.FindActiveUserWithOnlyRoleAsync("Head of Real Estate", null))
                .ReturnsAsync(existingUser);

            // When
            var result = await _validator.TestValidateAsync(command);

            // Then
            result.ShouldHaveValidationErrorFor(x => x)
                .WithErrorMessage(SharedResourcesKey.UniqueRoleAlreadyAssigned);
        }

        [Fact]
        public void Roles_WithEmptyRolesList_ShouldHaveValidationError()
        {
            // Given
            var command = CreateValidAddUserCommand();
            command.Roles = new List<string>();

            // When
            var result = _validator.TestValidate(command);

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Roles)
                .WithErrorMessage(SharedResourcesKey.AtLeastOneRoleRequired);
        }

        #endregion

        #region Helper Methods

        private AddUserCommand CreateValidAddUserCommand()
        {
            return new AddUserCommand
            {
                FullName = "Test User",
                Email = "test@example.com",
                PhoneNumber = "05123456789",
                UserName = "05123456789",
                Password = "TestPassword123!",
                ConfirmPassword = "TestPassword123!",
                CountryCode = "+966",
                PreferredLanguage = "ar-EG",
                Roles = new List<string> { "Board Member" }
            };
        }

        #endregion
    }
}
