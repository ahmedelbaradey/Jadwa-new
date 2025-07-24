using Core.Abstraction.Contract.Service.Notifications;
using Domain.Exceptions;
using Abstraction.Contracts.Logger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Resources;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text;
using System.Net.Http.Headers;
using System.Globalization;
using Abstraction.Contract.Service.Notifications;
using Abstraction.Base.Dto;
using Abstraction.Enums;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Infrastructure.Service.Notifications
{
    /// <summary>
    /// WhatsApp notification service implementation
    /// Integrates with WhatsApp Business API for sending messages to Saudi users
    /// Follows existing notification service patterns in the codebase
    /// </summary>
    public class WhatsAppNotificationService : IWhatsAppNotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly INotificationLocalizationService _localizationService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ILoggerManager _logger;
        private readonly IConfiguration _configuration;

        // WhatsApp API configuration keys
        private const string WhatsAppApiUrlKey = "WhatsApp:ApiUrl";
        private const string WhatsAppApiTokenKey = "WhatsApp:ApiToken";
        private const string WhatsAppPhoneNumberIdKey = "WhatsApp:PhoneNumberId";
        private const string WhatsAppVersionKey = "WhatsApp:Version";
        private const string WhatsAppEnabledKey = "WhatsApp:Enabled";

        // Saudi phone number regex pattern
        private static readonly Regex SaudiPhoneRegex = new(@"^\+966[5][0-9]{8}$", RegexOptions.Compiled);

        public WhatsAppNotificationService(
            HttpClient httpClient,
            INotificationLocalizationService localizationService,
            IStringLocalizer<SharedResources> localizer,
            ILoggerManager logger,
            IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            ConfigureHttpClient();
        }

        public async Task<WhatsAppMessageResponseDto> SendMessageAsync(WhatsAppMessageRequestDto request, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInfo($"Sending WhatsApp message to user {request.UserId}, phone: {request.PhoneNumber}");



                // Prepare message content
               // var messageContent =  await PrepareMessageContentAsync(request, cancellationToken);
               
               
                // Send message via WhatsApp API
                var apiResponse = await SendToWhatsAppApiAsync(request.PhoneNumber, request.Parameters, request.MessageType, request.Culture, cancellationToken);

                var response = new WhatsAppMessageResponseDto
                {
                    IsSuccess = true,
                    MessageId = apiResponse.MessageId,
                    DeliveryStatus = WhatsAppDeliveryStatus.Sent,
                    SentAt = DateTime.Now,
                    PhoneNumber = request.PhoneNumber,
                    UserId = request.UserId,
                    MessageType = request.MessageType
                };

                _logger.LogInfo($"WhatsApp message sent successfully. MessageId: {response.MessageId}");
                return response;
            }
            catch (WhatsAppNotificationException)
            {
                throw; // Re-throw WhatsApp-specific exceptions
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending WhatsApp message to user {request.UserId}");

                return new WhatsAppMessageResponseDto
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    DeliveryStatus = WhatsAppDeliveryStatus.Failed,
                    SentAt = DateTime.Now,
                    PhoneNumber = request.PhoneNumber,
                    UserId = request.UserId,
                    MessageType = request.MessageType
                };
            }
        }

        public async Task<WhatsAppMessageResponseDto> SendLocalizedMessageAsync(
            int userId,
            string phoneNumber,
            WhatsAppMessageType messageType,
            object[]? parameters = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Check if WhatsApp service is enabled
                if (!IsWhatsAppServiceEnabled())
                {
                    _logger.LogInfo($"WhatsApp service is disabled. Skipping message for user {userId}");
                    return new WhatsAppMessageResponseDto
                    {
                        IsSuccess = false,
                        ErrorMessage = "WhatsApp service is disabled",
                        UserId = userId,
                        PhoneNumber = phoneNumber,
                        MessageType = messageType
                    };
                }

                // Get user's preferred language
                var userLanguage = await _localizationService.GetUserPreferredLanguageAsync(userId);

                // Get localized message content
                var localizedContent = GetLocalizedMessageContent(userLanguage, messageType, parameters);

                var request = new WhatsAppMessageRequestDto
                {
                    UserId = userId,
                    PhoneNumber = phoneNumber,
                    MessageType = messageType,
                    Culture = userLanguage,
                    Parameters = parameters,
                    Priority = GetMessagePriority(messageType)
                };

                return await SendMessageAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending localized WhatsApp message to user {userId}");
                throw;
            }
        }

        public bool ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            return SaudiPhoneRegex.IsMatch(phoneNumber);
        }

        public async Task<WhatsAppDeliveryStatus> GetDeliveryStatusAsync(string messageId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInfo($"Checking delivery status for message: {messageId}");

                var apiUrl = _configuration[WhatsAppApiUrlKey];
                var version = _configuration[WhatsAppVersionKey] ?? "v17.0";
                var statusUrl = $"{apiUrl}/{version}/messages/{messageId}";

                var response = await _httpClient.GetAsync(statusUrl, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync(cancellationToken);
                    var statusResponse = JsonSerializer.Deserialize<WhatsAppStatusResponse>(content);

                    return MapApiStatusToDeliveryStatus(statusResponse?.Status);
                }

                _logger.LogWarn($"Failed to get delivery status for message {messageId}. Status: {response.StatusCode}");
                return WhatsAppDeliveryStatus.Failed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting delivery status for message {messageId}");
                return WhatsAppDeliveryStatus.Failed;
            }
        }

        public async Task<WhatsAppMessageResponseDto> SendPasswordResetMessageAsync(
            int userId,
            string phoneNumber,
            string temporaryPassword,
            CancellationToken cancellationToken = default)
        {
            return await SendLocalizedMessageAsync(
                userId,
                phoneNumber,
                WhatsAppMessageType.PasswordReset,
                new object[] { temporaryPassword },
                cancellationToken);
        }

        public async Task<WhatsAppMessageResponseDto> SendUserRegistrationMessageAsync(
            int userId,
            string phoneNumber,
            string username,
            string loginUrl,
            CancellationToken cancellationToken = default)
        {
            return await SendLocalizedMessageAsync(
                userId,
                phoneNumber,
                WhatsAppMessageType.UserRegistration,
                new object[] { username, loginUrl },
                cancellationToken);
        }

        public async Task<WhatsAppMessageResponseDto> SendAccountActivationMessageAsync(
            int userId,
            string phoneNumber,
            CancellationToken cancellationToken = default)
        {
            return await SendLocalizedMessageAsync(
                userId,
                phoneNumber,
                WhatsAppMessageType.AccountActivation,
                null,
                cancellationToken);
        }

        public async Task<WhatsAppMessageResponseDto> SendAccountDeactivationMessageAsync(
            int userId,
            string phoneNumber,
            CancellationToken cancellationToken = default)
        {
            return await SendLocalizedMessageAsync(
                userId,
                phoneNumber,
                WhatsAppMessageType.AccountDeactivation,
                null,
                cancellationToken);
        }

        private void ConfigureHttpClient()
        {
            try
            {
                // Check if WhatsApp service is enabled
                var isEnabled = _configuration.GetValue<bool>(WhatsAppEnabledKey, true);
                if (!isEnabled)
                {
                    _logger.LogInfo("WhatsApp service is disabled in configuration");
                    return;
                }

                // Validate required configuration values
                var apiUrl = _configuration[WhatsAppApiUrlKey];
                var apiToken = _configuration[WhatsAppApiTokenKey];
                var phoneNumberId = _configuration[WhatsAppPhoneNumberIdKey];
                var version = _configuration[WhatsAppVersionKey] ?? "v17.0";

                var missingConfigs = new List<string>();

                if (string.IsNullOrEmpty(apiUrl))
                    missingConfigs.Add("WhatsApp:ApiUrl");

                if (string.IsNullOrEmpty(apiToken))
                    missingConfigs.Add("WhatsApp:ApiToken");

                if (string.IsNullOrEmpty(phoneNumberId))
                    missingConfigs.Add("WhatsApp:PhoneNumberId");

                if (missingConfigs.Any())
                {
                    var missingConfigsStr = string.Join(", ", missingConfigs);
                    var errorMessage = $"WhatsApp API configuration is incomplete. Missing required settings: {missingConfigsStr}. " +
                                     "Please add the WhatsApp configuration section to your appsettings.json file. " +
                                     "See WhatsAppConfiguration.md for setup instructions.";

                    _logger.LogError(null, errorMessage);
                    throw new WhatsAppAuthenticationException(errorMessage);
                }

                // Configure HTTP client with authentication
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                _logger.LogInfo($"WhatsApp service configured successfully. API URL: {apiUrl}, Version: {version}");
            }
            catch (WhatsAppAuthenticationException)
            {
                throw; // Re-throw WhatsApp-specific exceptions
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to configure WhatsApp HTTP client. Please check your WhatsApp configuration settings. Error: {ex.Message}";
                _logger.LogError(ex, errorMessage);
                throw new WhatsAppAuthenticationException(errorMessage);
            }
        }

        /// <summary>
        /// Checks if WhatsApp service is enabled in configuration
        /// </summary>
        /// <returns>True if enabled, false otherwise</returns>
        private bool IsWhatsAppServiceEnabled()
        {
            return _configuration.GetValue<bool>(WhatsAppEnabledKey, true);
        }

        private async Task<string> PrepareMessageContentAsync(WhatsAppMessageRequestDto request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.Message))
            {
                return request.Message;
            }

            // If no message provided, get localized content
            var userLanguage = request.Culture ?? await _localizationService.GetUserPreferredLanguageAsync(request.UserId);
            return GetLocalizedMessageContent(userLanguage, request.MessageType, request.Parameters);
        }

        private string GetLocalizedMessageContent(string culture, WhatsAppMessageType messageType, object[]? parameters)
        {
            try
            {
                // Set culture for localization
                var originalCulture = CultureInfo.CurrentCulture;
                var originalUICulture = CultureInfo.CurrentUICulture;

                try
                {
                    var cultureInfo = new CultureInfo(culture);
                    CultureInfo.CurrentCulture = cultureInfo;
                    CultureInfo.CurrentUICulture = cultureInfo;

                    var messageKey = GetMessageResourceKey(messageType);
                    var template = _localizer[messageKey].Value;

                    // Format message with parameters if provided
                    if (parameters?.Length > 0)
                    {
                        return string.Format(template, parameters);
                    }

                    return template;
                }
                finally
                {
                    CultureInfo.CurrentCulture = originalCulture;
                    CultureInfo.CurrentUICulture = originalUICulture;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting localized message for type {messageType}");
                return GetFallbackMessage(messageType, parameters);
            }
        }

        private string GetMessageResourceKey(WhatsAppMessageType messageType)
        {
            return messageType switch
            {
                WhatsAppMessageType.PasswordReset => SharedResourcesKey.WhatsAppPasswordResetMessage,
                WhatsAppMessageType.UserRegistration => SharedResourcesKey.WhatsAppUserRegistrationMessage,
                WhatsAppMessageType.AccountActivation => SharedResourcesKey.WhatsAppAccountActivationMessage,
                WhatsAppMessageType.AccountDeactivation => SharedResourcesKey.WhatsAppAccountDeactivationMessage,
                WhatsAppMessageType.RegistrationMessageResend => SharedResourcesKey.WhatsAppRegistrationResendMessage,
                WhatsAppMessageType.FundMemberAdded => SharedResourcesKey.WhatsAppFundMemberAddedMessage,
                _ => throw new WhatsAppTemplateException(messageType.ToString(), "Unknown message type")
            };
        }

        private string GetFallbackMessage(WhatsAppMessageType messageType, object[]? parameters)
        {
            var fallbackMessage = messageType switch
            {
                WhatsAppMessageType.PasswordReset => "Your temporary password is: {0}",
                WhatsAppMessageType.UserRegistration => "Welcome! Your username is {0}. Access the system at: {1}",
                WhatsAppMessageType.AccountActivation => "Your account has been activated successfully.",
                WhatsAppMessageType.AccountDeactivation => "Your account has been deactivated.",
                WhatsAppMessageType.RegistrationMessageResend => "Registration message resent. Username: {0}, Login: {1}",
                _ => "You have a new notification from Jadwa system."
            };

            if (parameters?.Length > 0)
            {
                try
                {
                    return string.Format(fallbackMessage, parameters);
                }
                catch
                {
                    return fallbackMessage;
                }
            }

            return fallbackMessage;
        }

        private MessagePriority GetMessagePriority(WhatsAppMessageType messageType)
        {
            return messageType switch
            {
                WhatsAppMessageType.PasswordReset => MessagePriority.High,
                WhatsAppMessageType.UserRegistration => MessagePriority.High,
                WhatsAppMessageType.AccountActivation => MessagePriority.Normal,
                WhatsAppMessageType.AccountDeactivation => MessagePriority.High,
                WhatsAppMessageType.RegistrationMessageResend => MessagePriority.Normal,
                _ => MessagePriority.Normal
            };
        }

        private string GetMessageTemplate(WhatsAppMessageType messageType, string cultureInfo)
        {
            return messageType switch
            {
                WhatsAppMessageType.RegistrationMessageResend => string.Concat("tempforjadwa"),
                WhatsAppMessageType.FundMemberAdded => string.Concat("tempforjadwa"),
                _ => ""
            };
        }

        private async Task<WhatsAppApiResponse> SendToWhatsAppApiAsync(string phoneNumber, object[] _params, WhatsAppMessageType messageType,string cultureInfo, CancellationToken cancellationToken)
        {
            try
            {
                var apiUrl = _configuration[WhatsAppApiUrlKey];
                var version = _configuration[WhatsAppVersionKey] ?? "v17.0";
                var phoneNumberId = _configuration[WhatsAppPhoneNumberIdKey];

                if (string.IsNullOrEmpty(apiUrl) || string.IsNullOrEmpty(phoneNumberId))
                {
                    throw new WhatsAppAuthenticationException("WhatsApp API configuration is incomplete");
                }

                var requestUrl = $"{apiUrl}/{version}/{phoneNumberId}/messages";

                // Clean phone number (remove + sign for API)
                var cleanPhoneNumber = phoneNumber.StartsWith("+") ? phoneNumber.Substring(1) : phoneNumber;

                var requestBody = new
                {
                    messaging_product = "whatsapp",
                    to = cleanPhoneNumber,
                    type = "template",
                    template = new
                    {
                        name = GetMessageTemplate(messageType, cultureInfo),
                        language = new { code = "en_US" }, // Default to English, can be localized later
                        components = new object[]
                        {
                            new
                            {
                                type = "body",
                                parameters = _params.Select(p => new { type = "text", text = p }).ToArray()
                            },
                            new
                            {
                                type = "button",
                                sub_type = "url",
                                index = 0,
                                parameters = _params.Select(p => new { type = "text", text = p }).ToArray()
                            }
                        }
                    }
                };

                var jsonContent = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                _logger.LogInfo($"Sending WhatsApp API request to: {requestUrl}");
                _logger.LogInfo($"Request payload: {jsonContent}");

                var response = await _httpClient.PostAsync(requestUrl, content, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogInfo($"WhatsApp API response: {responseContent}");

                    var apiResponse = JsonSerializer.Deserialize<WhatsAppApiResponse>(responseContent);

                    if (apiResponse?.Messages?.Length > 0)
                    {
                        _logger.LogInfo($"WhatsApp message sent successfully. MessageId: {apiResponse.Messages[0].Id}");
                        return apiResponse;
                    }

                    _logger.LogError(null, $"Invalid API response format. Response content: {responseContent}");
                    throw new WhatsAppDeliveryException("", $"Invalid API response format. Response: {responseContent}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    var retryAfter = response.Headers.RetryAfter?.Delta ?? TimeSpan.FromMinutes(1);
                    throw new WhatsAppRateLimitException(DateTime.Now.Add(retryAfter));
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogError(null, $"WhatsApp API error: {response.StatusCode} - {errorContent}");

                    throw new WhatsAppDeliveryException("", $"API request failed: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error while calling WhatsApp API");
                throw new WhatsAppDeliveryException("", "Network error while sending message", "NETWORK_ERROR");
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "WhatsApp API request timeout");
                throw new WhatsAppDeliveryException("", "Request timeout", "TIMEOUT");
            }
        }

        private WhatsAppDeliveryStatus MapApiStatusToDeliveryStatus(string? apiStatus)
        {
            return apiStatus?.ToLower() switch
            {
                "sent" => WhatsAppDeliveryStatus.Sent,
                "delivered" => WhatsAppDeliveryStatus.Delivered,
                "read" => WhatsAppDeliveryStatus.Read,
                "failed" => WhatsAppDeliveryStatus.Failed,
                "rejected" => WhatsAppDeliveryStatus.Rejected,
                _ => WhatsAppDeliveryStatus.Queued
            };
        }

    }

    // Internal classes for WhatsApp API responses
    internal class WhatsAppApiResponse
    {
        public WhatsAppMessage[]? Messages { get; set; }
        public string MessageId => Messages?.FirstOrDefault()?.Id ?? string.Empty;
    }

    internal class WhatsAppMessage
    {
        public string Id { get; set; } = string.Empty;
    }

    internal class WhatsAppStatusResponse
    {
        public string? Status { get; set; }
    }
}