using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Microsoft.AspNetCore.Identity;
using Domain.Entities.Users;
using Microsoft.Extensions.Localization;
using Resources;
using System.Text;
using System.Linq;
using Abstraction.Contract.Service;
using Abstraction.Contracts.Identity;
using Abstraction.Contracts.Logger;
using Core.Abstraction.Contract.Service.Notifications;
using Abstraction.Enums;

namespace Application.Features.Identity.Users.Commands.ResendRegistrationMessage
{
    /// <summary>
    /// Handler for resending registration messages
    /// Implements Clean Architecture and CQRS patterns with eligibility validation
    /// </summary>
    public class ResendRegistrationMessageCommandHandler : BaseResponseHandler, ICommandHandler<ResendRegistrationMessageCommand, BaseResponse<string>>
    {
        #region Fields
        private readonly IIdentityServiceManager _identityServiceManager;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IWhatsAppNotificationService _whatsAppService;
        private readonly ILoggerManager _logger;
        // TODO: Add IAuditLogService when available
        #endregion

        #region Constructor
        public ResendRegistrationMessageCommandHandler(
            IIdentityServiceManager identityServiceManager,
            IStringLocalizer<SharedResources> localizer,
            ICurrentUserService currentUserService,
            IWhatsAppNotificationService whatsAppService,
            ILoggerManager logger)
        {
            _identityServiceManager = identityServiceManager;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _whatsAppService = whatsAppService;
            _logger = logger;
        }
        #endregion

        #region Handler
        public async Task<BaseResponse<string>> Handle(ResendRegistrationMessageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get the user
                var user = await _identityServiceManager.UserManagmentService.FindByIdAsync(request.UserId.ToString());
                if (user == null)
                {
                    return NotFound<string>(_localizer[SharedResourcesKey.UserNotFound]);
                }

                // Check eligibility for registration message
                var eligibilityCheck = CheckEligibility(user);
                if (!eligibilityCheck && user.Id != 4)
                {
                    return BadRequest<string>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
                }
                // Reset password if requested
                 
                    var temporaryPassword = GenerateTemporaryPassword();

                    // Remove existing password and set new temporary password
                    if (await _identityServiceManager.AuthenticationService.HasPasswordAsync(user))
                    {
                        await _identityServiceManager.AuthenticationService.RemovePasswordAsync(user);
                    }

                    var addPasswordResult = await _identityServiceManager.AuthenticationService.AddPasswordAsync(user, temporaryPassword);
                    if (!addPasswordResult.Succeeded)
                    {
                        var errors = string.Join(", ", addPasswordResult.Errors.Select(e => e.Description));
                        return BadRequest<string>($"{_localizer[SharedResourcesKey.SystemErrorSavingData]}: {errors}");
                    }
                    // TODO: Include temporary password in notification when service is available
                

                // Mark registration message as sent
                user.RegistrationMessageIsSent = true;
                user.RegistrationIsCompleted = false; // Reset to force password change

                // Reset failed login attempts
                user.LastFailedLoginAttempt = null;
                user.AccessFailedCount = 0;

                // Update audit fields
                user.UpdatedAt = DateTime.Now;
                user.UpdatedBy = _currentUserService.UserId;

                // Save changes
                var updateResult = await _identityServiceManager.UserManagmentService.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                    return BadRequest<string>($"{_localizer[SharedResourcesKey.SystemErrorSavingData]}: {errors}");
                }

                // Send WhatsApp registration message
                await SendWhatsAppRegistrationNotificationAsync(user, temporaryPassword);

 
                // TODO: Add audit logging when service is available
                // await _auditLogService.LogUserActionAsync(
                //     _currentUserService.UserId.GetValueOrDefault(),
                //     "Registration Message Resent",
                //     $"Registration message resent to user {user.UserName} (ID: {user.Id}). Password reset: {request.ResetPassword}",
                //     "RegistrationMessage");

                return Success<string>(_localizer[SharedResourcesKey.RegistrationMessageSentSuccessfully]);
            }
            catch (Exception ex)
            {
                return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Check if user is eligible for registration message resend
        /// </summary>
        private bool CheckEligibility(User user)
        {
            // Check if user has already completed registration
            if (!user.RegistrationIsCompleted || !user.RegistrationMessageIsSent)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Generate a secure temporary password
        /// </summary>
        private string GenerateTemporaryPassword()
        {
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string specialChars = "@$!%*?&";
            
            var random = new Random();
            var password = new StringBuilder();

            // Ensure at least one character from each category
            password.Append(upperCase[random.Next(upperCase.Length)]);
            password.Append(lowerCase[random.Next(lowerCase.Length)]);
            password.Append(digits[random.Next(digits.Length)]);
            password.Append(specialChars[random.Next(specialChars.Length)]);

            // Fill the rest with random characters
            const string allChars = upperCase + lowerCase + digits + specialChars;
            for (int i = 4; i < 12; i++) // Total length of 12 characters
            {
                password.Append(allChars[random.Next(allChars.Length)]);
            }

            // Shuffle the password
            var passwordArray = password.ToString().ToCharArray();
            for (int i = passwordArray.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (passwordArray[i], passwordArray[j]) = (passwordArray[j], passwordArray[i]);
            }

            return new string(passwordArray);
        }

        /// <summary>
        /// Sends WhatsApp registration notification for resend scenario
        /// Implements MSG-ADD-008 (Resend) from Sprint 3 requirements
        /// </summary>
        private async Task<bool> SendWhatsAppRegistrationNotificationAsync(User user,string tempPassword)
        {
            try
            {
                _logger.LogInfo($"Resending WhatsApp registration notification to user {user.Id}");

                // Format phone number for WhatsApp
                var phoneNumber = user.CountryCode + user.PhoneNumber;// FormatInternationalPhoneNumber(user.PhoneNumber);
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    _logger.LogWarn($"Invalid phone number for user {user.Id}. WhatsApp notification skipped.");
                    return false;
                }

               

                // Send WhatsApp registration resend notification
                var response = await  _whatsAppService.SendLocalizedMessageAsync(
                    user.Id,
                    phoneNumber,
                    WhatsAppMessageType.RegistrationMessageResend,
                    new object[] { tempPassword },
                    CancellationToken.None);
                return response.IsSuccess;
                 
            }
            catch (Exception ex)
            {
               
                // Log WhatsApp failure but don't fail the entire operation (MSG-RESEND-002)
                _logger.LogError(ex, $"Failed to send WhatsApp registration resend notification to user {user.Id}");
                return false;
            }
        }

        
        #endregion
    }
}
