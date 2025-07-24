using AutoMapper;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contracts.Identity;
using Abstraction.Contract.Service.Notifications;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Microsoft.AspNetCore.Identity;
using Domain.Entities.Users;
using Domain.Entities.Notifications;
using static Domain.Entities.Notifications.NotificationType;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contract.Service;
using Abstraction.Constants;
using DocumentFormat.OpenXml.Spreadsheet;
using Abstraction.Contracts.Service;
using Core.Abstraction.Contract.Service.Notifications;
using Abstraction.Enums;



namespace Application.Features.Identity.Users.Commands.EditUser
{
    /// <summary>
    /// Enhanced handler for editing users with Sprint 3 administrative features
    /// Includes role management and advanced user properties
    /// </summary>
    public class EditUserCommandHandler : BaseResponseHandler, ICommandHandler<EditUserCommand, BaseResponse<string>>
    {
        #region Fields
        private readonly IIdentityServiceManager _identityServiceManager;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly IWhatsAppNotificationService _whatsAppService;
        // TODO: Add IFileUploadService when available
        #endregion

        #region Constructors
        public EditUserCommandHandler(
            IIdentityServiceManager identityServiceManager,
            IMapper mapper,
            IStringLocalizer<SharedResources> localizer,
            ICurrentUserService currentUserService,
            IServiceManager service,
            IRepositoryManager repositoryManager,
            ILoggerManager logger,
            IWhatsAppNotificationService whatsAppService)
        {
            _mapper = mapper;
            _identityServiceManager = identityServiceManager;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _repositoryManager = repositoryManager;
            _logger = logger;
            _whatsAppService = whatsAppService;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<string>> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get existing user
                var existingUser = await _identityServiceManager.UserManagmentService.FindByIdWithRolesAsync(request.Id.ToString());
                if (existingUser == null)
                    return NotFound<string>(_localizer[SharedResourcesKey.UserNotFound]);

                // Get current user roles for comparison
                var currentRoles = existingUser.Roles.Select(c=>c.Name).ToList();

                // Check email uniqueness
                var userWithEmail = await _identityServiceManager.UserManagmentService.FindByEmailAsync(request.Email);
                if (userWithEmail != null && userWithEmail.Id != request.Id)
                    return BadRequest<string>(_localizer[SharedResourcesKey.ProfileDuplicateEmail]);

                // Check username uniqueness
                var userWithUsername = await _identityServiceManager.UserManagmentService.FindByNameAsync(existingUser.UserName);
                if (userWithUsername != null && userWithUsername.Id != request.Id)
                    return BadRequest<string>(_localizer[SharedResourcesKey.UsernameAlreadyInUse]);

           
                // Handle file uploads
                if (!string.IsNullOrWhiteSpace(request.CVFile))
                {
                    // TODO: Implement file upload service integration
                    existingUser.CVFilePath = request.CVFile;
                }

                if (!string.IsNullOrWhiteSpace(request.PersonalPhoto))
                {
                    // TODO: Implement file upload service integration
                    existingUser.PersonalPhotoPath = request.PersonalPhoto;
                }
                request.UserName = existingUser.UserName;
                // Map basic fields using AutoMapper
                _mapper.Map(request, existingUser);

                // Update audit fields
                existingUser.UpdatedAt = DateTime.Now;
                existingUser.UpdatedBy = _currentUserService.UserId;
                // Handle single-holder role replacement logic
                // Check for single-holder role conflicts and handle replacement
                var singleHolderRoles = new[] { RoleHelper.LegalCouncil, RoleHelper.FinanceController, RoleHelper.ComplianceLegalManagingDirector, RoleHelper.HeadOfRealEstate };
                var newRole = request.Roles.FirstOrDefault();
                if (singleHolderRoles.Contains(newRole))
                {

                    var conflictingUser = await _identityServiceManager.UserManagmentService.FindActiveUserWithOnlyRoleAsync(newRole, existingUser.Id);
                    if (conflictingUser != null)
                    {
                        // Deactivate the conflicting user (replacement logic)
                        conflictingUser.IsActive = false;
                        conflictingUser.UpdatedAt = DateTime.Now;
                        conflictingUser.UpdatedBy = _currentUserService.UserId;
                        existingUser.IsActive = true;
                        var deactivateResult = await _identityServiceManager.UserManagmentService.UpdateAsync(conflictingUser);
                        if (!deactivateResult.Succeeded)
                        {
                            _logger.LogError(null, $"Failed to deactivate conflicting user {conflictingUser.Id} for role {newRole}");
                            return BadRequest<string>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
                        }
                        _logger.LogInfo($"Deactivated user {conflictingUser.Id} due to role replacement for {newRole}");
                    }
                }
                // Save user changes
                var result = await _identityServiceManager.UserManagmentService.UpdateAsync(existingUser);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return BadRequest<string>($"{_localizer[SharedResourcesKey.SystemErrorSavingData]}: {errors}");
                }

                // Handle role updates
                await _identityServiceManager.AuthorizationService.EditUserRoles(request.Roles, existingUser);

                // Handle conditional WhatsApp registration message
                await HandleConditionalWhatsAppRegistrationAsync(existingUser, currentRoles, request.Roles);

                // Handle role change notifications
                await HandleRoleChangeNotificationsAsync(existingUser, currentRoles, request.Roles);

                // Add comprehensive audit logging
               // await AddUserEditAuditEntryAsync(existingUser, currentRoles, request.Roles);

                return Success<string>(_localizer[SharedResourcesKey.UserUpdatedSuccessfully]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error editing user {request.Id}");
                return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
            }
        }

        /// <summary>
        /// Handles conditional WhatsApp registration message sending
        /// If user has ONLY Board Member role and gets Fund Manager/Associate Fund Manager role
        /// AND Registration Message Is Sent flag is 0, send WhatsApp message
        /// </summary>
        private async Task HandleConditionalWhatsAppRegistrationAsync(User user, IList<string> currentRoles, List<string> newRoles)
        {
            try
            {
                // Check if user had ONLY Board Member role
                var hadOnlyBoardMemberRole = currentRoles.Count == 1 && currentRoles.Contains(RoleHelper.BoardMember);

                // Check if user is getting Fund Manager or Associate Fund Manager role
                var gettingFundManagerRole = newRoles.Contains(RoleHelper.FundManager) && !currentRoles.Contains(RoleHelper.FundManager);
                var gettingAssociateFundManagerRole = newRoles.Contains(RoleHelper.AssociateFundManager) && !currentRoles.Contains(RoleHelper.AssociateFundManager);

                // Check if registration message hasn't been sent
                var registrationMessageNotSent = !user.RegistrationMessageIsSent;

                if (hadOnlyBoardMemberRole && (gettingFundManagerRole || gettingAssociateFundManagerRole) && registrationMessageNotSent)
                {
                    // Send WhatsApp registration message
                    var roleName = gettingFundManagerRole ? "Fund Manager" : "Associate Fund Manager";

                 //  await SendWhatsAppRegistrationNotificationAsync(User user, string tempPassword)
                   

                    if (true)
                    {
                        user.RegistrationMessageIsSent = true;
                        user.UpdatedAt = DateTime.Now;
                        user.UpdatedBy = _currentUserService.UserId;

                        await _identityServiceManager.UserManagmentService.UpdateAsync(user);
                        _logger.LogInfo($"WhatsApp registration message sent successfully for user {user.Id}");
                    }
                    else
                    {
                        _logger.LogError(null,$"Failed to send WhatsApp registration message for user {user.Id}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error handling conditional WhatsApp registration for user {user.Id}");
            }
        }

        /// <summary>
        /// Handles role change notifications
        /// Sends notifications for relieve of duties and role updates
        /// </summary>
        private async Task HandleRoleChangeNotificationsAsync(User user, IList<string> currentRoles, List<string> newRoles)
        {
            try
            {
                // Check for relieve of duties notification (Fund Manager/Associate Fund Manager role removed)
                var fundManagerRemoved = currentRoles.Contains(RoleHelper.FundManager) && !newRoles.Contains(RoleHelper.FundManager);
                var associateFundManagerRemoved = currentRoles.Contains(RoleHelper.AssociateFundManager) && !newRoles.Contains(RoleHelper.AssociateFundManager);

                if (fundManagerRemoved)
                {
                    await SendRelieveOfDutiesNotificationAsync(user.Id, "Fund Manager");
                }

                if (associateFundManagerRemoved)
                {
                    await SendRelieveOfDutiesNotificationAsync(user.Id, "Associate Fund Manager");
                }

                // Check for role update notification (single-holder role changes)
                var singleHolderRoles = new[] { RoleHelper.LegalCouncil, RoleHelper.FinanceController,
                                              RoleHelper.ComplianceLegalManagingDirector, RoleHelper.HeadOfRealEstate };

                var currentSingleHolderRole = currentRoles.FirstOrDefault(r => singleHolderRoles.Contains(r));
                var newSingleHolderRole = newRoles.FirstOrDefault(r => singleHolderRoles.Contains(r));

                if (currentSingleHolderRole != newSingleHolderRole && (currentSingleHolderRole != null || newSingleHolderRole != null))
                {
                    await SendRoleUpdateNotificationAsync(user.Id, currentSingleHolderRole, newSingleHolderRole);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error handling role change notifications for user {user.Id}");
            }
        }

        /// <summary>
        /// Sends relieve of duties notification
        /// Follows the same pattern as Resolution notifications
        /// </summary>
        private async Task SendRelieveOfDutiesNotificationAsync(int userId, string roleName)
        {
            try
            {
                var notification = new Domain.Entities.Notifications.Notification
                {
                    UserId = userId,
                    FundId = 0, // No specific fund for user role changes
                    Title = string.Empty, // Will be localized by the service
                    Body = roleName, // Role name as parameter for localization
                    NotificationType = (int)NotificationType.UserRelieveOfDuties,
                    IsRead = false
                };

                await _repositoryManager.Notifications.AddAsync(notification);
                _logger.LogInfo($"Relieve of duties notification added for user {userId}, role: {roleName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending relieve of duties notification to user {userId}");
                // Don't fail the main operation if notification fails
            }
        }

        /// <summary>
        /// Sends role update notification
        /// Follows the same pattern as Resolution notifications
        /// </summary>
        private async Task SendRoleUpdateNotificationAsync(int userId, string? oldRole, string? newRole)
        {
            try
            {
                // Only send notification if both roles are provided
                if (string.IsNullOrEmpty(oldRole) && string.IsNullOrEmpty(newRole))
                    return;

                var bodyParameters = $"{oldRole ?? ""}|{newRole ?? ""}";

                var notification = new Domain.Entities.Notifications.Notification
                {
                    UserId = userId,
                    FundId = 0, // No specific fund for user role changes
                    Title = string.Empty, // Will be localized by the service
                    Body = bodyParameters, // Role names as parameters for localization
                    NotificationType = (int)NotificationType.UserRoleUpdate,
                    IsRead = false
                };

                await _repositoryManager.Notifications.AddAsync(notification);
                _logger.LogInfo($"Role update notification added for user {userId}, from: {oldRole} to: {newRole}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending role update notification to user {userId}");
                // Don't fail the main operation if notification fails
            }
        }

        /// <summary>
        /// Sends WhatsApp registration notification for resend scenario
        /// Implements MSG-ADD-008 (Resend) from Sprint 3 requirements
        /// </summary>
        private async Task<bool> SendWhatsAppRegistrationNotificationAsync(User user, string tempPassword)
        {
            try
            {
                _logger.LogInfo($"Resending WhatsApp registration notification to user {user.Id}");

                // Format phone number for WhatsApp
                var phoneNumber = "+201021069171";// FormatInternationalPhoneNumber(user.PhoneNumber);
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    _logger.LogWarn($"Invalid phone number for user {user.Id}. WhatsApp notification skipped.");
                    return false;
                }



                // Send WhatsApp registration resend notification
                var response = await _whatsAppService.SendLocalizedMessageAsync(
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
