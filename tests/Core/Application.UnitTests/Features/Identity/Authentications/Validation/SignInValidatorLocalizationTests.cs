using Application.Features.Identity.Authentications.Commands.SignIn;
using Application.Features.Identity.Authentications.Validation;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Localization;
using Moq;
using Resources;
using Xunit;

namespace Application.UnitTests.Features.Identity.Authentications.Validation
{
    /// <summary>
    /// Unit tests for SignInValidatior localization
    /// Verifies that validation messages are properly localized
    /// </summary>
    public class SignInValidatorLocalizationTests
    {
        private readonly Mock<IStringLocalizer<SharedResources>> _mockLocalizer;
        private readonly SignInValidatior _validator;

        public SignInValidatorLocalizationTests()
        {
            _mockLocalizer = new Mock<IStringLocalizer<SharedResources>>();
            
            // Setup localized strings
            SetupLocalizedStrings();
            
            _validator = new SignInValidatior(_mockLocalizer.Object);
        }

        #region Username Validation Tests

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void UserName_WithEmptyValue_ShouldReturnLocalizedMessage(string userName)
        {
            // Given
            var command = new SignInCommand { UserName = userName, Password = "ValidPassword123!" };

            // When
            var result = _validator.TestValidate(command);

            // Then
            result.ShouldHaveValidationErrorFor(x => x.UserName)
                .WithErrorMessage("Username is required.");
        }

        [Fact]
        public void UserName_WithValidValue_ShouldNotHaveValidationError()
        {
            // Given
            var command = new SignInCommand { UserName = "05123456789", Password = "ValidPassword123!" };

            // When
            var result = _validator.TestValidate(command);

            // Then
            result.ShouldNotHaveValidationErrorFor(x => x.UserName);
        }

        #endregion

        #region Password Validation Tests

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Password_WithEmptyValue_ShouldReturnLocalizedMessage(string password)
        {
            // Given
            var command = new SignInCommand { UserName = "05123456789", Password = password };

            // When
            var result = _validator.TestValidate(command);

            // Then
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password is required.");
        }

        [Fact]
        public void Password_WithValidValue_ShouldNotHaveValidationError()
        {
            // Given
            var command = new SignInCommand { UserName = "05123456789", Password = "ValidPassword123!" };

            // When
            var result = _validator.TestValidate(command);

            // Then
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        #endregion

        #region Localization Tests

        [Fact]
        public void Validator_ShouldUseCorrectResourceKeys()
        {
            // Given
            var command = new SignInCommand { UserName = "", Password = "" };

            // When
            var result = _validator.TestValidate(command);

            // Then
            _mockLocalizer.Verify(x => x[SharedResourcesKey.LoginUsernameRequired], Times.AtLeastOnce);
            _mockLocalizer.Verify(x => x[SharedResourcesKey.LoginPasswordRequired], Times.AtLeastOnce);
        }

        [Fact]
        public void Validator_WithArabicCulture_ShouldReturnArabicMessages()
        {
            // Given
            SetupArabicLocalizedStrings();
            var arabicValidator = new SignInValidatior(_mockLocalizer.Object);
            var command = new SignInCommand { UserName = "", Password = "" };

            // When
            var result = arabicValidator.TestValidate(command);

            // Then
            result.ShouldHaveValidationErrorFor(x => x.UserName)
                .WithErrorMessage("اسم المستخدم مطلوب.");
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("كلمة المرور مطلوبة.");
        }

        #endregion

        #region Helper Methods

        private void SetupLocalizedStrings()
        {
            // Setup English localized strings
            _mockLocalizer.Setup(x => x[SharedResourcesKey.LoginUsernameRequired])
                .Returns(new LocalizedString(SharedResourcesKey.LoginUsernameRequired, "Username is required."));
            
            _mockLocalizer.Setup(x => x[SharedResourcesKey.LoginPasswordRequired])
                .Returns(new LocalizedString(SharedResourcesKey.LoginPasswordRequired, "Password is required."));
        }

        private void SetupArabicLocalizedStrings()
        {
            // Setup Arabic localized strings
            _mockLocalizer.Setup(x => x[SharedResourcesKey.LoginUsernameRequired])
                .Returns(new LocalizedString(SharedResourcesKey.LoginUsernameRequired, "اسم المستخدم مطلوب."));
            
            _mockLocalizer.Setup(x => x[SharedResourcesKey.LoginPasswordRequired])
                .Returns(new LocalizedString(SharedResourcesKey.LoginPasswordRequired, "كلمة المرور مطلوبة."));
        }

        #endregion
    }
}
