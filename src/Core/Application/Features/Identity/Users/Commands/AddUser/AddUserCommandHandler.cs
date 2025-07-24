using AutoMapper;
using Application.Base.Abstracts;
using Domain.Entities.Users;
using Abstraction.Base.Response;
using Abstraction.Contracts.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contract.Service;
using Abstraction.Constants;
using Abstraction.Contracts.Logger;
using Core.Abstraction.Contract.Service.Notifications;
using System.Linq;
using System.Text;


namespace Application.Features.Identity.Users.Commands.AddUser
{
    /// <summary>
    /// Enhanced handler for adding users with Sprint 3 administrative features
    /// Includes role assignment and registration message integration
    /// </summary>
    public class AddUserCommandHandler : BaseResponseHandler, ICommandHandler<AddUserCommand, BaseResponse<string>>
    {
        #region Fields
        private readonly IIdentityServiceManager _identityServiceManager;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IWhatsAppNotificationService _whatsAppService;
        private readonly ILoggerManager _logger;
        // TODO: Add IFileUploadService when available
        #endregion

        #region Constructors
        public AddUserCommandHandler(
            IIdentityServiceManager identityServiceManager,
            IMapper mapper,
            IStringLocalizer<SharedResources> localizer,
            ICurrentUserService currentUserService,
            IWhatsAppNotificationService whatsAppService,
            ILoggerManager logger)
        {
            _mapper = mapper;
            _identityServiceManager = identityServiceManager;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _whatsAppService = whatsAppService;
            _logger = logger;
        }
        #endregion

        #region Private Methods
        public async Task<BaseResponse<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if user exists by email
                var existingUserByEmail = await _identityServiceManager.UserManagmentService.FindByEmailAsync(request.Email);
                if (existingUserByEmail != null)
                    return BadRequest<string>(_localizer[SharedResourcesKey.ProfileDuplicateEmail]);

                // Check if user exists by username
                var existingUserByUserName = await _identityServiceManager.UserManagmentService.FindByNameAsync(request.UserName);
                if (existingUserByUserName != null)
                    return BadRequest<string>(_localizer[SharedResourcesKey.UsernameAlreadyInUse]);

                // Handle file uploads
                string? cvFilePath = null;
                string? photoFilePath = null;

                if (request.CVFile != null)
                {
                    // TODO: Implement file upload service integration
                    cvFilePath = request.CVFile;
                }

                if (request.PersonalPhoto != null)
                {
                    // TODO: Implement file upload service integration
                    photoFilePath = request.PersonalPhoto;
                }

                // Map to user entity
                var user = _mapper.Map<User>(request);
                var password = GenerateTemporaryPassword();
                // Set Sprint 3 specific fields
                user.CVFilePath = cvFilePath;
                user.CountryCode = "+20";
                user.PreferredLanguage = "ar-EG"; // Default to Arabic
                user.PhoneNumber = request.UserName;
                user.PersonalPhotoPath = photoFilePath;
                user.RegistrationMessageIsSent = request.Roles.All(c => c == "Board Member") ? false : true;
                user.RegistrationIsCompleted = false;
                user.CreatedAt = DateTime.Now;
                user.CreatedBy = _currentUserService.UserId;
                // Create user
                var result = await _identityServiceManager.UserManagmentService.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return BadRequest<string>($"{_localizer[SharedResourcesKey.SystemErrorSavingData]}: {errors}");
                }

                // Assign roles
                var assignedRoles = new List<string>();
                if (request.Roles.Any())
                {
                    foreach (var role in request.Roles)
                    {
                        var roleExists = await _identityServiceManager.AuthorizationService.IsRoleNameExist(role);
                        if (roleExists)
                        {
                            await _identityServiceManager.UserManagmentService.AddToRoleAsync(user, role);
                            assignedRoles.Add(role);
                        }
                    }
                }
                else
                {
                    // Default role assignment
                    var defaultRoleExists = await _identityServiceManager.AuthorizationService.IsRoleNameExist("USER");
                    if (!defaultRoleExists)
                    {
                        await _identityServiceManager.AuthorizationService.AddRoleAsync("USER", null);
                    }
                    await _identityServiceManager.UserManagmentService.AddToRoleAsync(user, "USER");
                    assignedRoles.Add("USER");
                }
                if (request.Roles.Contains(RoleHelper.LegalCouncil) || request.Roles.Contains(RoleHelper.HeadOfRealEstate) || request.Roles.Contains(RoleHelper.FinanceController) || request.Roles.Contains(RoleHelper.ComplianceLegalManagingDirector))
                {
                    foreach (var role in request.Roles)
                    {
                        var updatedUser = await _identityServiceManager.UserManagmentService.FindActiveUserWithOnlyRoleAsync(role, user.Id);
                        if(updatedUser!=null )
                        {
                            updatedUser.IsActive = false;
                            await _identityServiceManager.UserManagmentService.UpdateAsync(updatedUser);
                        }
                    }
                }

                // Prepare response


                // Send registration message if eligible (not only Board Member)
                if (user.RegistrationMessageIsSent)
                {
                    await SendWhatsAppRegistrationNotificationAsync(user, assignedRoles, password);
                }

                return Success<string>(_localizer[SharedResourcesKey.UserAddedSuccessfully]);
            }
            catch (Exception ex)
            {
                return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
            }
        }

        /// <summary>
        /// Sends WhatsApp registration notification to newly created user
        /// Implements MSG-ADD-008 from Sprint 3 requirements
        /// Only sends if user is eligible (not only Board Member role)
        /// </summary>
        private async Task SendWhatsAppRegistrationNotificationAsync(User user, List<string> userRoles, string temporaryPassword)
        {
            try
            {
                _logger.LogInfo($"Sending WhatsApp registration notification to user {user.Id}");

                // Format phone number for WhatsApp
                var phoneNumber = user.PhoneNumber;
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    _logger.LogWarn($"Invalid phone number for user {user.Id}. WhatsApp notification skipped.");
                    return;
                }

                // Get primary role for message
                var primaryRole = userRoles.FirstOrDefault() ?? "User";
                var loginUrl = "https://jadwa-fund-management.com/login"; // TODO: Get from configuration

                // Send WhatsApp registration notification
                var response = await _whatsAppService.SendUserRegistrationMessageAsync(
                    user.Id,
                    phoneNumber,
                    user.UserName,
                    loginUrl,
                    CancellationToken.None);

                if (response.IsSuccess)
                {
                    _logger.LogInfo($"WhatsApp registration notification sent successfully to user {user.Id}. MessageId: {response.MessageId}");
                }
                else
                {
                    _logger.LogWarn($"WhatsApp registration notification failed for user {user.Id}. Error: {response.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                // Log WhatsApp failure but don't fail the entire operation (MSG-ADD-009)
                _logger.LogError(ex, $"Failed to send WhatsApp registration notification to user {user.Id}");
            }
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
        #endregion
    }
}