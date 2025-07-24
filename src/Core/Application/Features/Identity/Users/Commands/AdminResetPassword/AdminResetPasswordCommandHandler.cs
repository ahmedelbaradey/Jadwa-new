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

namespace Application.Features.Identity.Users.Commands.AdminResetPassword
{
    /// <summary>
    /// Handler for administrative password reset
    /// Implements Clean Architecture and CQRS patterns with temporary password generation
    /// </summary>
    public class AdminResetPasswordCommandHandler : BaseResponseHandler, ICommandHandler<AdminResetPasswordCommand, BaseResponse<string>>
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
        public AdminResetPasswordCommandHandler(
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
        public async Task<BaseResponse<string>> Handle(AdminResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get the user whose password should be reset
                var user = await _identityServiceManager.UserManagmentService.FindByIdAsync(request.UserId.ToString());
                if (user == null)
                {
                    return NotFound<string>(_localizer[SharedResourcesKey.UserNotFound]);
                }

                // Enhanced eligibility validation (JDWA-1257)
                if (!IsUserEligibleForPasswordReset(user))
                {
                    return BadRequest<string>(_localizer[SharedResourcesKey.UserNotEligibleForPasswordReset]);
                }

                // Generate temporary password if not provided
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

                 
                user.RegistrationIsCompleted = false;
                // Reset failed login attempts
                user.LastFailedLoginAttempt = null;
                user.AccessFailedCount = 0;

                // Update security stamp to invalidate existing tokens
                await _identityServiceManager.UserManagmentService.UpdateSecurityStampAsync(user);

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

                // TODO: Add audit logging when service is available
                // await _auditLogService.LogUserActionAsync(
                //     _currentUserService.UserId.GetValueOrDefault(),
                //     "Admin Password Reset",
                //     $"Password reset for user {user.UserName} (ID: {user.Id}). Reason: {request.Reason}",
                //     "PasswordReset");


                // Send WhatsApp notification with new password (MSG-RESET-006)
                await SendWhatsAppPasswordResetNotificationAsync(user, temporaryPassword);

                return Success<string>(_localizer[SharedResourcesKey.UserPasswordResetSuccessfully]);
            }
            catch (Exception ex)
            {
                return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
            }
        }

        #region Helper Methods

        /// <summary>
        /// Checks if user is eligible for password reset based on enhanced criteria (JDWA-1257)
        /// User must be Active AND Registration Completed AND Registration Message Sent
        /// </summary>
        private bool IsUserEligibleForPasswordReset(User user)
        {
            return user.IsActive &&
                   user.RegistrationIsCompleted &&
                   user.RegistrationMessageIsSent;
        }

        #endregion
        #endregion

        #region Private Methods
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
        /// Sends WhatsApp password reset notification to user
        /// Implements MSG-RESET-006 from Sprint 3 requirements
        /// </summary>
        private async Task SendWhatsAppPasswordResetNotificationAsync(User user, string temporaryPassword)
        {
            try
            {
                _logger.LogInfo($"Sending WhatsApp password reset notification to user {user.Id}");

                // Format phone number for WhatsApp
                var phoneNumber = (user.PhoneNumber);
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    _logger.LogWarn($"Invalid phone number for user {user.Id}. WhatsApp notification skipped.");
                    return;
                }

                // Send WhatsApp notification with temporary password
                var response = await _whatsAppService.SendPasswordResetMessageAsync(
                    user.Id,
                    phoneNumber,
                    temporaryPassword,
                    CancellationToken.None);

                if (response.IsSuccess)
                {
                    _logger.LogInfo($"WhatsApp password reset notification sent successfully to user {user.Id}. MessageId: {response.MessageId}");
                }
                else
                {
                    _logger.LogWarn($"WhatsApp password reset notification failed for user {user.Id}. Error: {response.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                // Log WhatsApp failure but don't fail the entire operation (MSG-RESET-003)
                _logger.LogError(ex, $"Failed to send WhatsApp password reset notification to user {user.Id}");
            }
        }

     
        #endregion
    }
}
