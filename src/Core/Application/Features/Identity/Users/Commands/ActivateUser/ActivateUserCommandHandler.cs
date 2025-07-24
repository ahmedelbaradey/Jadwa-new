using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Microsoft.AspNetCore.Identity;
using Domain.Entities.Users;
using Microsoft.Extensions.Localization;
using Resources;
using System.Linq;
using Abstraction.Contract.Service;
using Abstraction.Contracts.Identity;
using Abstraction.Constants;
using Abstraction.Contracts.Repository;
using Abstraction.Contracts.Logger;
using Core.Abstraction.Contract.Service.Notifications;
using Domain.Entities.FundManagement;
 

namespace Application.Features.Identity.Users.Commands.ActivateUser
{
    /// <summary>
    /// Handler for activating/deactivating user accounts with complex fund-related business rules (JDWA-1253)
    /// Implements Clean Architecture and CQRS patterns with enhanced validation
    /// </summary>
    public class ActivateDeActivateUserCommandHandler : BaseResponseHandler, ICommandHandler<ActivateUserCommand, BaseResponse<string>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IIdentityServiceManager _identityServiceManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IWhatsAppNotificationService _whatsAppService;
        private readonly ILoggerManager _logger;

        #endregion

        #region Constructor
        public ActivateDeActivateUserCommandHandler(
            IStringLocalizer<SharedResources> localizer,
            IIdentityServiceManager identityServiceManager,
            ICurrentUserService currentUserService,
            IRepositoryManager repositoryManager,
            IWhatsAppNotificationService whatsAppService,
            ILoggerManager logger)
        {
            _localizer = localizer;
            _currentUserService = currentUserService;
            _identityServiceManager = identityServiceManager;
            _repositoryManager = repositoryManager;
            _whatsAppService = whatsAppService;
            _logger = logger;
        }
        #endregion

        #region Handler
        public async Task<BaseResponse<string>> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get the user to activate/deactivate
                var user = await  _identityServiceManager.AuthorizationService.GetUserByRoles(request.UserId);
                if (user == null)
                {
                    return NotFound<string>(_localizer[SharedResourcesKey.UserNotFound]);
                }

                
                var roleNames = user.Roles.Select(c=>c.Name).ToList();

                if (!user.IsActive)
                {
                    return await HandleUserActivationAsync(user, roleNames);
                }
                else
                {
                    return await HandleUserDeactivationAsync(user, roleNames);
                }
            }
            catch (Exception ex)
            {
                return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
            }
        }

        private async Task<BaseResponse<string>> HandleUserActivationAsync(User user, List<string> roleNames)
        {
            // Check for single-holder role replacement logic
            var singleHolderRoles = new[] { RoleHelper.LegalCouncil, RoleHelper.FinanceController,RoleHelper.ComplianceLegalManagingDirector, RoleHelper.HeadOfRealEstate , RoleHelper.LegalCouncil};

            var userSingleHolderRoles = roleNames.Where(r => singleHolderRoles.Contains(r)).ToList();

            if (userSingleHolderRoles.Count == 1 && roleNames.Count == 1)
            {
                // User has only one single-holder role, check for existing active user with same role
                var existingUser = await _identityServiceManager.UserManagmentService.FindActiveUserWithOnlyRoleAsync(userSingleHolderRoles.First(), user.Id);

                if (existingUser != null)
                {
                    // Deactivate existing user (replacement workflow)
                    existingUser.IsActive = false;
                    existingUser.LockoutEnabled = true;
                    existingUser.LockoutEnd = DateTimeOffset.MaxValue;
                    existingUser.UpdatedAt = DateTime.Now;
                    existingUser.UpdatedBy = _currentUserService.UserId;

                    await _identityServiceManager.UserManagmentService.UpdateAsync(existingUser);
                }
            }

            // Activate the user
            user.IsActive = true;
            user.LockoutEnabled = false;
            user.LockoutEnd = null;
            user.AccessFailedCount = 0;
            user.LastFailedLoginAttempt = null;
            user.UpdatedAt = DateTime.Now;
            user.UpdatedBy = _currentUserService.UserId;

            var result = await _identityServiceManager.UserManagmentService.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest<string>($"{_localizer[SharedResourcesKey.SystemErrorSavingData]}: {errors}");
            }

            // Send WhatsApp notification for activation
            await SendWhatsAppActivationNotificationAsync(user);
            // TODO: Send in-app notification

            return Success<string>(_localizer[SharedResourcesKey.UserActivatedSuccessfully]);
        }

        private async Task<BaseResponse<string>> HandleUserDeactivationAsync(User user, List<string> roleNames)
        {
            try { 
            // Check deactivation restrictions for Independent Board Members
            if (roleNames.Contains(RoleHelper.BoardMember))
            {
                var boardMemberships =  _repositoryManager.BoardMembers.GetByCondition<BoardMember>(bm => bm.UserId == user.Id && bm.IsActive, false);
                  

                foreach (var membership in boardMemberships)
                {
                    if (membership.MemberType == BoardMemberType.Independent)
                    {
                        var fundIndependentMembersCount = await _repositoryManager.BoardMembers.GetActiveIndependentMemberCountAsync(membership.FundId);
                        if (fundIndependentMembersCount  <= 2)
                        {
                            return BadRequest<string>(_localizer[SharedResourcesKey.CannotDeactivateIndependentBoardMember]);
                        }
                    }
                }
            }

            // Check deactivation restrictions for Fund Managers
            if (roleNames.Contains(RoleHelper.FundManager))
            {
                var isSoleFundManager = await _repositoryManager.FundManagers.IsUserSoleFundManagerForAnyFundAsync(user.Id);
                if (isSoleFundManager)
                {
                    return BadRequest<string>(_localizer[SharedResourcesKey.CannotDeactivateSoleFundManager]);
                }
            }

            // Check deactivation restrictions for Associate Fund Managers
            //if (roleNames.Contains(RoleHelper.AssociateFundManager))
            //{
            //    var isSoleAssociateManager = await _repositoryManager.FundMembers.IsUserSoleAssociateFundManagerForAnyFundAsync(user.Id);
            //    if (isSoleAssociateManager)
            //    {
            //        return BadRequest<string>(_localizer[SharedResourcesKey.CannotDeactivateSoleFundManager]);
            //    }
            //}

            // Check deactivation restrictions for single-holder roles
            var singleHolderRoles = new[] { RoleHelper.LegalCouncil, RoleHelper.FinanceController,RoleHelper.ComplianceLegalManagingDirector, RoleHelper.HeadOfRealEstate };

            var userSingleHolderRoles = roleNames.Where(r => singleHolderRoles.Contains(r)).ToList();

            if (userSingleHolderRoles.Count == 1 && roleNames.Count == 1)
            {
                // Check if there's another active user with the same role
                var otherActiveUser = await _identityServiceManager.UserManagmentService.FindActiveUserWithOnlyRoleAsync(userSingleHolderRoles.First(), user.Id);
                if (otherActiveUser == null)
                {
                    return BadRequest<string>(_localizer[SharedResourcesKey.CannotDeactivateSingleHolderRole]);
                }
            }

            // Deactivate the user
            user.IsActive = false;
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.MaxValue;
            user.UpdatedAt = DateTime.Now;
            user.UpdatedBy = _currentUserService.UserId;

            var result = await _identityServiceManager.UserManagmentService.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest<string>($"{_localizer[SharedResourcesKey.SystemErrorSavingData]}: {errors}");
            }

            // TODO: Terminate active sessions
            // Send WhatsApp notification for deactivation
            await SendWhatsAppDeactivationNotificationAsync(user);
            // TODO: Send in-app notification

         //   return Success<string>(isCurrentlyActive ? _localizer[SharedResourcesKey.UserActivatedSuccessfully] : _localizer[SharedResourcesKey.UserDeactivatedSuccessfully]);
        
                // TODO: Add audit logging when service is available
                // await _auditLogService.LogUserActionAsync(
                //     _currentUserService.UserId.GetValueOrDefault(),
                //     "User Activation",
                //     $"User {user.UserName} (ID: {user.Id}) was activated. Reason: {request.Reason ?? "Not specified"}",
                //     "Activation");

                // TODO: Send notification when service is available
                // if (request.SendNotification)
                // {
                //     await _notificationService.SendUserActivationNotificationAsync(user.Id, request.Reason);
                // }

                return Success<string>( _localizer[SharedResourcesKey.UserDeactivatedSuccessfully]);
            }
            catch (Exception ex)
            {
                return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
            }
        }

        /// <summary>
        /// Sends WhatsApp activation notification to user
        /// Implements MSG-ACTDEACT-009 from Sprint 3 requirements
        /// </summary>
        private async Task SendWhatsAppActivationNotificationAsync(User user)
        {
            try
            {
                _logger.LogInfo($"Sending WhatsApp activation notification to user {user.Id}");

                // Format phone number for WhatsApp
                var phoneNumber = user.PhoneNumber;
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    _logger.LogWarn($"Invalid phone number for user {user.Id}. WhatsApp notification skipped.");
                    return;
                }

                // Send WhatsApp activation notification
                var response = await _whatsAppService.SendAccountActivationMessageAsync(
                    user.Id,
                    phoneNumber,
                    CancellationToken.None);

                if (response.IsSuccess)
                {
                    _logger.LogInfo($"WhatsApp activation notification sent successfully to user {user.Id}. MessageId: {response.MessageId}");
                }
                else
                {
                    _logger.LogWarn($"WhatsApp activation notification failed for user {user.Id}. Error: {response.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                // Log WhatsApp failure but don't fail the entire operation
                _logger.LogError(ex, $"Failed to send WhatsApp activation notification to user {user.Id}");
            }
        }

        /// <summary>
        /// Sends WhatsApp deactivation notification to user
        /// Implements MSG-ACTDEACT-010 from Sprint 3 requirements
        /// </summary>
        private async Task SendWhatsAppDeactivationNotificationAsync(User user)
        {
            try
            {
                _logger.LogInfo($"Sending WhatsApp deactivation notification to user {user.Id}");

                // Format phone number for WhatsApp
                var phoneNumber =user.PhoneNumber;
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    _logger.LogWarn($"Invalid phone number for user {user.Id}. WhatsApp notification skipped.");
                    return;
                }

                // Send WhatsApp deactivation notification
                var response = await _whatsAppService.SendAccountDeactivationMessageAsync(
                    user.Id,
                    phoneNumber,
                    CancellationToken.None);

                if (response.IsSuccess)
                {
                    _logger.LogInfo($"WhatsApp deactivation notification sent successfully to user {user.Id}. MessageId: {response.MessageId}");
                }
                else
                {
                    _logger.LogWarn($"WhatsApp deactivation notification failed for user {user.Id}. Error: {response.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                // Log WhatsApp failure but don't fail the entire operation
                _logger.LogError(ex, $"Failed to send WhatsApp deactivation notification to user {user.Id}");
            }
        }

       
        #endregion
    }
}
