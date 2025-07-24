using Core.Abstraction.Contract.Service.Notifications;
using Application.Features.Notifications.Dtos;
using Application.Features.Notifications.Enums;
using Domain.Exceptions;
using Infrastructure.Service.Notifications;
using Abstraction.Contracts.Logger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Resources;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using Xunit;

namespace Tests.Infrastructure.Services.Notifications
{
    public class WhatsAppNotificationServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly Mock<INotificationLocalizationService> _localizationServiceMock;
        private readonly Mock<IStringLocalizer<SharedResources>> _localizerMock;
        private readonly Mock<ILoggerManager> _loggerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly HttpClient _httpClient;
        private readonly WhatsAppNotificationService _service;

        public WhatsAppNotificationServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _localizationServiceMock = new Mock<INotificationLocalizationService>();
            _localizerMock = new Mock<IStringLocalizer<SharedResources>>();
            _loggerMock = new Mock<ILoggerManager>();
            _configurationMock = new Mock<IConfiguration>();

            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

            // Setup configuration
            _configurationMock.Setup(c => c["WhatsApp:ApiUrl"]).Returns("https://graph.facebook.com");
            _configurationMock.Setup(c => c["WhatsApp:ApiToken"]).Returns("test-token");
            _configurationMock.Setup(c => c["WhatsApp:PhoneNumberId"]).Returns("test-phone-id");
            _configurationMock.Setup(c => c["WhatsApp:Version"]).Returns("v17.0");

            _service = new WhatsAppNotificationService(
                _httpClient,
                _localizationServiceMock.Object,
                _localizerMock.Object,
                _loggerMock.Object,
                _configurationMock.Object);
        }

        [Fact]
        public void ValidatePhoneNumber_WithValidSaudiNumber_ReturnsTrue()
        {
            // Arrange
            var validPhoneNumber = "+966501234567";

            // Act
            var result = _service.ValidatePhoneNumber(validPhoneNumber);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("+1234567890")]
        [InlineData("966501234567")]
        [InlineData("+966401234567")] // Invalid prefix (should be 5)
        [InlineData("+96650123456")] // Too short
        [InlineData("+9665012345678")] // Too long
        public void ValidatePhoneNumber_WithInvalidNumbers_ReturnsFalse(string phoneNumber)
        {
            // Act
            var result = _service.ValidatePhoneNumber(phoneNumber);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task SendMessageAsync_WithValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new WhatsAppMessageRequestDto
            {
                UserId = 123,
                PhoneNumber = "+966501234567",
                MessageType = WhatsAppMessageType.PasswordReset,
                Message = "Test message"
            };

            var apiResponse = new
            {
                messages = new[] { new { id = "test-message-id" } }
            };

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(apiResponse))
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _service.SendMessageAsync(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("test-message-id", result.MessageId);
            Assert.Equal(WhatsAppDeliveryStatus.Sent, result.DeliveryStatus);
            Assert.Equal(request.PhoneNumber, result.PhoneNumber);
            Assert.Equal(request.UserId, result.UserId);
        }

        [Fact]
        public async Task SendMessageAsync_WithInvalidPhoneNumber_ThrowsInvalidPhoneNumberException()
        {
            // Arrange
            var request = new WhatsAppMessageRequestDto
            {
                UserId = 123,
                PhoneNumber = "invalid-phone",
                MessageType = WhatsAppMessageType.PasswordReset,
                Message = "Test message"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidPhoneNumberException>(() => _service.SendMessageAsync(request));
        }

        [Fact]
        public async Task SendLocalizedMessageAsync_WithValidParameters_CallsLocalizationService()
        {
            // Arrange
            var userId = 123;
            var phoneNumber = "+966501234567";
            var messageType = WhatsAppMessageType.UserRegistration;
            var parameters = new object[] { "testuser", "https://example.com" };

            _localizationServiceMock
                .Setup(l => l.GetUserPreferredLanguageAsync(userId))
                .ReturnsAsync("ar-EG");

            var localizedString = new Mock<IStringLocalizer<SharedResources>>();
            localizedString.Setup(l => l[It.IsAny<string>()]).Returns(new LocalizedString("key", "Localized message: {0}, {1}"));
            
            _localizerMock.Setup(l => l[It.IsAny<string>()]).Returns(new LocalizedString("key", "Localized message: {0}, {1}"));

            var apiResponse = new
            {
                messages = new[] { new { id = "test-message-id" } }
            };

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(apiResponse))
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _service.SendLocalizedMessageAsync(userId, phoneNumber, messageType, parameters);

            // Assert
            Assert.True(result.IsSuccess);
            _localizationServiceMock.Verify(l => l.GetUserPreferredLanguageAsync(userId), Times.Once);
        }

        [Fact]
        public async Task SendPasswordResetMessageAsync_WithValidParameters_SendsCorrectMessage()
        {
            // Arrange
            var userId = 123;
            var phoneNumber = "+966501234567";
            var temporaryPassword = "TempPass123";

            _localizationServiceMock
                .Setup(l => l.GetUserPreferredLanguageAsync(userId))
                .ReturnsAsync("en-US");

            _localizerMock
                .Setup(l => l[SharedResourcesKey.WhatsAppPasswordResetMessage])
                .Returns(new LocalizedString(SharedResourcesKey.WhatsAppPasswordResetMessage, "Your temporary password is: {0}"));

            var apiResponse = new
            {
                messages = new[] { new { id = "test-message-id" } }
            };

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(apiResponse))
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _service.SendPasswordResetMessageAsync(userId, phoneNumber, temporaryPassword);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(WhatsAppMessageType.PasswordReset, result.MessageType);
        }

        [Fact]
        public async Task SendMessageAsync_WithApiError_ReturnsFailureResponse()
        {
            // Arrange
            var request = new WhatsAppMessageRequestDto
            {
                UserId = 123,
                PhoneNumber = "+966501234567",
                MessageType = WhatsAppMessageType.PasswordReset,
                Message = "Test message"
            };

            var httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("API Error")
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _service.SendMessageAsync(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(WhatsAppDeliveryStatus.Failed, result.DeliveryStatus);
            Assert.NotNull(result.ErrorMessage);
        }

        [Fact]
        public async Task SendMessageAsync_WithRateLimitError_ThrowsRateLimitException()
        {
            // Arrange
            var request = new WhatsAppMessageRequestDto
            {
                UserId = 123,
                PhoneNumber = "+966501234567",
                MessageType = WhatsAppMessageType.PasswordReset,
                Message = "Test message"
            };

            var httpResponse = new HttpResponseMessage(HttpStatusCode.TooManyRequests);
            httpResponse.Headers.RetryAfter = new System.Net.Http.Headers.RetryConditionHeaderValue(TimeSpan.FromMinutes(1));

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            // Act & Assert
            await Assert.ThrowsAsync<WhatsAppRateLimitException>(() => _service.SendMessageAsync(request));
        }

        [Fact]
        public async Task GetDeliveryStatusAsync_WithValidMessageId_ReturnsStatus()
        {
            // Arrange
            var messageId = "test-message-id";
            var statusResponse = new { status = "delivered" };

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(statusResponse))
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _service.GetDeliveryStatusAsync(messageId);

            // Assert
            Assert.Equal(WhatsAppDeliveryStatus.Delivered, result);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
