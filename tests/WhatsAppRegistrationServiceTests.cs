using Abstraction.Contract.Service.Notifications;
using Abstraction.Contracts.Identity;
using Abstraction.Contracts.Logger;
using Abstraction.Base.Response;
using Core.Abstraction.Contract.Service.Notifications;
using Domain.Entities.Users;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Enums;
using Microsoft.Extensions.Configuration;
using Abstraction.Contract.Service;
using Infrastructure.Service.Notifications;
using Moq;
using Xunit;
using Abstraction.Base.Dto;

namespace Tests.Infrastructure.Services.Notifications
{
    /// <summary>
    /// Unit tests for WhatsAppRegistrationService
    /// Tests the standardized WhatsApp registration notification functionality
    /// </summary>
    public class WhatsAppRegistrationServiceTests
    {
        private readonly Mock<IWhatsAppNotificationService> _whatsAppNotificationServiceMock;
        private readonly Mock<IIdentityServiceManager> _identityServiceManagerMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly Mock<ILoggerManager> _loggerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly WhatsAppRegistrationService _service;

        public WhatsAppRegistrationServiceTests()
        {
            _whatsAppNotificationServiceMock = new Mock<IWhatsAppNotificationService>();
            _identityServiceManagerMock = new Mock<IIdentityServiceManager>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _localizerMock = new Mock<IStringLocalizer<SharedResources>>();
            _loggerMock = new Mock<ILoggerManager>();
            _configurationMock = new Mock<IConfiguration>();

            // Setup configuration
            _configurationMock.Setup(c => c["AppSettings:LoginUrl"]).Returns("https://jadwa-fund-management.com/login");

            _service = new WhatsAppRegistrationService(
                _whatsAppNotificationServiceMock.Object,
                _identityServiceManagerMock.Object,
                _currentUserServiceMock.Object,
                _localizerMock.Object,
                _loggerMock.Object,
                _configurationMock.Object);
        }

        [Fact]
        public void IsUserEligibleForRegistrationNotification_WithEligibleUser_ReturnsTrue()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                RegistrationMessageIsSent = false,
                PhoneNumber = "0501234567",
                CountryCode = "+966"
            };

            // Act
            var result = _service.IsUserEligibleForRegistrationNotification(user);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsUserEligibleForRegistrationNotification_WithIneligibleUser_ReturnsFalse()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                RegistrationMessageIsSent = true,
                PhoneNumber = "0501234567",
                CountryCode = "+966"
            };

            // Act
            var result = _service.IsUserEligibleForRegistrationNotification(user);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("0501234567", "+966", "+966501234567")]
        [InlineData("501234567", "+966", "+966501234567")]
        [InlineData("+966501234567", "+966", "+966501234567")]
        [InlineData("05012345678", "+966", null)] // Too long
        [InlineData("050123456", "+966", null)] // Too short
        [InlineData("", "+966", null)] // Empty
        [InlineData(null, "+966", null)] // Null
        public void FormatPhoneNumberForWhatsApp_WithVariousInputs_ReturnsExpectedResult(string phoneNumber, string countryCode, string expected)
        {
            // Act
            var result = _service.FormatPhoneNumberForWhatsApp(phoneNumber, countryCode);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task SendWhatsAppRegistrationNotificationAsync_WithEligibleUser_SendsNotificationAndUpdatesFlag()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                RegistrationMessageIsSent = false,
                PhoneNumber = "0501234567",
                CountryCode = "+966",
                UserName = "testuser"
            };

            var temporaryPassword = "TempPass123";

            // Setup mocks
            var mockUserService = new Mock<IUserManagmentService>();
            var mockIdentityResult = Microsoft.AspNetCore.Identity.IdentityResult.Success;
            
            mockUserService.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(mockIdentityResult);
            _identityServiceManagerMock.Setup(x => x.UserManagmentService).Returns(mockUserService.Object);

            _whatsAppNotificationServiceMock.Setup(x => x.SendLocalizedMessageAsync(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<WhatsAppMessageType>(), It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new WhatsAppMessageResponseDto { IsSuccess = true, MessageId = "msg123" });

            _localizerMock.Setup(x => x[It.IsAny<string>()]).Returns(new LocalizedString("key", "Localized message"));

            // Act
            var result = await _service.SendWhatsAppRegistrationNotificationAsync(
                user, temporaryPassword, WhatsAppRegistrationMessageType.NewUser);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            
            // Verify WhatsApp service was called
            _whatsAppNotificationServiceMock.Verify(x => x.SendUserRegistrationMessageAsync(
                user.Id, "+966501234567", user.UserName, It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            
            // Verify user was updated
            mockUserService.Verify(x => x.UpdateAsync(It.Is<User>(u => u.RegistrationMessageIsSent == true)), Times.Once);
        }

        [Fact]
        public async Task SendBoardMemberAddedNotificationAsync_WithValidParameters_CallsMainMethod()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                RegistrationMessageIsSent = false,
                PhoneNumber = "0501234567",
                CountryCode = "+966"
            };

            var temporaryPassword = "TempPass123";
            var fundName = "Test Fund";
            var memberType = "Independent";

            // Setup mocks
            var mockUserService = new Mock<IUserManagmentService>();
            var mockIdentityResult = Microsoft.AspNetCore.Identity.IdentityResult.Success;
            
            mockUserService.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(mockIdentityResult);
            _identityServiceManagerMock.Setup(x => x.UserManagmentService).Returns(mockUserService.Object);

            _whatsAppNotificationServiceMock.Setup(x => x.SendLocalizedMessageAsync(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<WhatsAppMessageType>(), It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new WhatsAppMessageResponseDto { IsSuccess = true, MessageId = "msg123" });

            _localizerMock.Setup(x => x[It.IsAny<string>()]).Returns(new LocalizedString("key", "Localized message"));

            // Act
            var result = await _service.SendBoardMemberAddedNotificationAsync(user, temporaryPassword, fundName, memberType);

            // Assert
            Assert.True(result.IsSuccess);
            
            // Verify WhatsApp service was called with correct message type
            _whatsAppNotificationServiceMock.Verify(x => x.SendLocalizedMessageAsync(
                user.Id, "+966501234567", WhatsAppMessageType.UserRegistration, It.IsAny<object[]>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SendWhatsAppRegistrationNotificationAsync_WithIneligibleUser_ReturnsFailure()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                RegistrationMessageIsSent = true, // Already sent
                PhoneNumber = "0501234567",
                CountryCode = "+966"
            };

            var temporaryPassword = "TempPass123";

            _localizerMock.Setup(x => x[It.IsAny<string>()]).Returns(new LocalizedString("key", "User not eligible"));

            // Act
            var result = await _service.SendWhatsAppRegistrationNotificationAsync(
                user, temporaryPassword, WhatsAppRegistrationMessageType.NewUser);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            
            // Verify WhatsApp service was not called
            _whatsAppNotificationServiceMock.Verify(x => x.SendUserRegistrationMessageAsync(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task SendWhatsAppRegistrationNotificationAsync_WithInvalidPhoneNumber_ReturnsFailure()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                RegistrationMessageIsSent = false,
                PhoneNumber = "invalid-phone",
                CountryCode = "+966"
            };

            var temporaryPassword = "TempPass123";

            _localizerMock.Setup(x => x[It.IsAny<string>()]).Returns(new LocalizedString("key", "Invalid phone number"));

            // Act
            var result = await _service.SendWhatsAppRegistrationNotificationAsync(
                user, temporaryPassword, WhatsAppRegistrationMessageType.NewUser);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            
            // Verify WhatsApp service was not called
            _whatsAppNotificationServiceMock.Verify(x => x.SendUserRegistrationMessageAsync(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
