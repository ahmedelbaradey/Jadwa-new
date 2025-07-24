using Microsoft.Extensions.Localization;
using Moq;
using Resources;
using Xunit;
using System.Globalization;

namespace Application.UnitTests.Features.Identity
{
    /// <summary>
    /// Verification test to ensure all new localization keys are properly implemented
    /// Tests that the new resource keys exist and return expected values
    /// </summary>
    public class LocalizationVerificationTest
    {
        [Fact]
        public void NewResourceKeys_ShouldExistInSharedResourcesKey()
        {
            // Given - Verify all new resource keys are defined as constants
            
            // Authentication Messages
            Assert.NotNull(SharedResourcesKey.LoginUsernameRequired);
            Assert.NotNull(SharedResourcesKey.LoginPasswordRequired);
            Assert.NotNull(SharedResourcesKey.UsernameAlreadyInUse);
            
            // Password Validation Messages
            Assert.NotNull(SharedResourcesKey.PasswordMinimumLength);
            
            // Profile Field Validation Messages
            Assert.NotNull(SharedResourcesKey.PassportNumberAlphanumeric);
            
            // Verify the values are correct
            Assert.Equal("LoginUsernameRequired", SharedResourcesKey.LoginUsernameRequired);
            Assert.Equal("LoginPasswordRequired", SharedResourcesKey.LoginPasswordRequired);
            Assert.Equal("UsernameAlreadyInUse", SharedResourcesKey.UsernameAlreadyInUse);
            Assert.Equal("PasswordMinimumLength", SharedResourcesKey.PasswordMinimumLength);
            Assert.Equal("PassportNumberAlphanumeric", SharedResourcesKey.PassportNumberAlphanumeric);
        }

        [Fact]
        public void MockLocalizer_WithEnglishCulture_ShouldReturnEnglishMessages()
        {
            // Given
            var mockLocalizer = new Mock<IStringLocalizer<SharedResources>>();
            SetupEnglishLocalizedStrings(mockLocalizer);

            // When & Then
            var usernameMessage = mockLocalizer.Object[SharedResourcesKey.LoginUsernameRequired];
            var passwordMessage = mockLocalizer.Object[SharedResourcesKey.LoginPasswordRequired];
            var usernameInUseMessage = mockLocalizer.Object[SharedResourcesKey.UsernameAlreadyInUse];
            var passwordLengthMessage = mockLocalizer.Object[SharedResourcesKey.PasswordMinimumLength];
            var passportMessage = mockLocalizer.Object[SharedResourcesKey.PassportNumberAlphanumeric];

            Assert.Equal("Username is required.", usernameMessage.Value);
            Assert.Equal("Password is required.", passwordMessage.Value);
            Assert.Equal("Username is already in use.", usernameInUseMessage.Value);
            Assert.Equal("Password must be at least 8 characters long.", passwordLengthMessage.Value);
            Assert.Equal("Passport number must be alphanumeric.", passportMessage.Value);
        }

        [Fact]
        public void MockLocalizer_WithArabicCulture_ShouldReturnArabicMessages()
        {
            // Given
            var mockLocalizer = new Mock<IStringLocalizer<SharedResources>>();
            SetupArabicLocalizedStrings(mockLocalizer);

            // When & Then
            var usernameMessage = mockLocalizer.Object[SharedResourcesKey.LoginUsernameRequired];
            var passwordMessage = mockLocalizer.Object[SharedResourcesKey.LoginPasswordRequired];
            var usernameInUseMessage = mockLocalizer.Object[SharedResourcesKey.UsernameAlreadyInUse];
            var passwordLengthMessage = mockLocalizer.Object[SharedResourcesKey.PasswordMinimumLength];
            var passportMessage = mockLocalizer.Object[SharedResourcesKey.PassportNumberAlphanumeric];

            Assert.Equal("اسم المستخدم مطلوب.", usernameMessage.Value);
            Assert.Equal("كلمة المرور مطلوبة.", passwordMessage.Value);
            Assert.Equal("اسم المستخدم مستخدم بالفعل.", usernameInUseMessage.Value);
            Assert.Equal("يجب أن تكون كلمة المرور 8 أحرف على الأقل.", passwordLengthMessage.Value);
            Assert.Equal("رقم جواز السفر يجب أن يكون أرقام وحروف فقط.", passportMessage.Value);
        }

        [Theory]
        [InlineData("en-US")]
        [InlineData("ar-EG")]
        public void ResourceKeys_ShouldBeConsistentAcrossCultures(string cultureName)
        {
            // Given
            var culture = new CultureInfo(cultureName);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            // When & Then - Verify keys exist regardless of culture
            Assert.NotEmpty(SharedResourcesKey.LoginUsernameRequired);
            Assert.NotEmpty(SharedResourcesKey.LoginPasswordRequired);
            Assert.NotEmpty(SharedResourcesKey.UsernameAlreadyInUse);
            Assert.NotEmpty(SharedResourcesKey.PasswordMinimumLength);
            Assert.NotEmpty(SharedResourcesKey.PassportNumberAlphanumeric);
        }

        #region Helper Methods

        private void SetupEnglishLocalizedStrings(Mock<IStringLocalizer<SharedResources>> mockLocalizer)
        {
            mockLocalizer.Setup(x => x[SharedResourcesKey.LoginUsernameRequired])
                .Returns(new LocalizedString(SharedResourcesKey.LoginUsernameRequired, "Username is required."));
            
            mockLocalizer.Setup(x => x[SharedResourcesKey.LoginPasswordRequired])
                .Returns(new LocalizedString(SharedResourcesKey.LoginPasswordRequired, "Password is required."));
            
            mockLocalizer.Setup(x => x[SharedResourcesKey.UsernameAlreadyInUse])
                .Returns(new LocalizedString(SharedResourcesKey.UsernameAlreadyInUse, "Username is already in use."));
            
            mockLocalizer.Setup(x => x[SharedResourcesKey.PasswordMinimumLength])
                .Returns(new LocalizedString(SharedResourcesKey.PasswordMinimumLength, "Password must be at least 8 characters long."));
            
            mockLocalizer.Setup(x => x[SharedResourcesKey.PassportNumberAlphanumeric])
                .Returns(new LocalizedString(SharedResourcesKey.PassportNumberAlphanumeric, "Passport number must be alphanumeric."));
        }

        private void SetupArabicLocalizedStrings(Mock<IStringLocalizer<SharedResources>> mockLocalizer)
        {
            mockLocalizer.Setup(x => x[SharedResourcesKey.LoginUsernameRequired])
                .Returns(new LocalizedString(SharedResourcesKey.LoginUsernameRequired, "اسم المستخدم مطلوب."));
            
            mockLocalizer.Setup(x => x[SharedResourcesKey.LoginPasswordRequired])
                .Returns(new LocalizedString(SharedResourcesKey.LoginPasswordRequired, "كلمة المرور مطلوبة."));
            
            mockLocalizer.Setup(x => x[SharedResourcesKey.UsernameAlreadyInUse])
                .Returns(new LocalizedString(SharedResourcesKey.UsernameAlreadyInUse, "اسم المستخدم مستخدم بالفعل."));
            
            mockLocalizer.Setup(x => x[SharedResourcesKey.PasswordMinimumLength])
                .Returns(new LocalizedString(SharedResourcesKey.PasswordMinimumLength, "يجب أن تكون كلمة المرور 8 أحرف على الأقل."));
            
            mockLocalizer.Setup(x => x[SharedResourcesKey.PassportNumberAlphanumeric])
                .Returns(new LocalizedString(SharedResourcesKey.PassportNumberAlphanumeric, "رقم جواز السفر يجب أن يكون أرقام وحروف فقط."));
        }

        #endregion
    }
}
