using Abstraction.Base.Response;
using Abstraction.Constants;
using Abstraction.Contract.Service;
using Abstraction.Contracts.Identity;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Application.Base.Abstracts;
using Application.Common.Helpers;
using AutoMapper;
using Domain.Entities.FundManagement;
using Domain.Entities.Notifications;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Resources;
using Core.Abstraction.Contract.Service.Notifications;
using Abstraction.Enums;
using System.Security.Claims;
using Abstraction.Contract.Service.Notifications;
using System.Text;
using DocumentFormat.OpenXml.Spreadsheet;



namespace Application.Features.BoardMembers.Commands.Add
{
    /// <summary>
    /// Handler for AddBoardMemberCommand
    /// Implements business logic for adding board members with validation
    /// Follows Clean Architecture and CQRS patterns
    /// </summary>
    public class AddBoardMemberCommandHandler : BaseResponseHandler, ICommandHandler<AddBoardMemberCommand, BaseResponse<string>>
    {
        #region Fields

        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ILoggerManager _logger;
        private readonly IIdentityServiceManager _identityService;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;
        private readonly IWhatsAppNotificationService _whatsAppService;

        #endregion

        #region Constructor

        public AddBoardMemberCommandHandler(
            IRepositoryManager repository,
            IMapper mapper,
            IStringLocalizer<SharedResources> localizer,
            ILoggerManager logger,
            IIdentityServiceManager identityService,
            ICurrentUserService currentUserService,
            UserManager<User> userManager,
            IWhatsAppNotificationService whatsAppService)
        {
            _repository = repository;
            _mapper = mapper;
            _localizer = localizer;
            _logger = logger;
            _identityService = identityService;
            _currentUserService = currentUserService;
            _userManager = userManager;
            _whatsAppService = whatsAppService;
        }

        #endregion

        #region Handler Implementation

        public async Task<BaseResponse<string>> Handle(AddBoardMemberCommand request, CancellationToken cancellationToken)
        {
            try
            {

                _logger.LogInfo($"Starting AddBoardMember operation for FundId: {request.FundId}, UserId: {request.UserId}");
                if (request == null)
                    return BadRequest<string>(_localizer[SharedResourcesKey.AnErrorIsOccurredWhileSavingData]);
                // 1. Validate fund exists and is active

                var fund = await _repository.Funds.ViewFundUsers(request.FundId, trackChanges: false);
                if (fund == null)
                {
                    _logger.LogWarn($"Fund not found with ID: {request.FundId}");
                    return NotFound<string>(_localizer[SharedResourcesKey.FundNotFound]);
                }
                // 2. Create new board member entity using AutoMapper
                var boardMember = _mapper.Map<BoardMember>(request);
                boardMember.IsActive = true; // Ensure new members are active
                // 3. Add to repository
                var addedBoardMember = await _repository.BoardMembers.AddAsync(boardMember, cancellationToken);
                // 4. Send notifications
                await AddNotification(fund, addedBoardMember);

                // 4.1. Send WhatsApp notification to the newly added member
                var temporaryPassword = GenerateTemporaryPassword();
                await SendWhatsAppNotification(fund, addedBoardMember, temporaryPassword);

                // 5. Check if fund should be activated (2 independent members)
                
                await CheckAndActivateFund(fund);

                _logger.LogInfo($"Board member added successfully. ID: {addedBoardMember.Id}");
                return Success<string>(_localizer[SharedResourcesKey.BoardMemberAddedSuccessfully]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while adding board member for FundId: {request.FundId}, UserId: {request.UserId}");
                return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
            }
        }
        #endregion
        #region Helper Methods
        /// <summary>
        /// Adds notifications for board member addition following AddResolutionCommandHandler pattern
        /// Based on Sprint.md MSG002 and MSG007 notification requirements
        /// </summary>
        private async Task AddNotification(Fund fund, BoardMember boardMember)
        {
            var notifications = new List<Domain.Entities.Notifications.Notification>();

            var user = await _userManager.FindByIdAsync(boardMember.UserId.ToString() ?? "");
            var userRole = await GetUserFundRole(fund, _currentUserService.UserId.Value);


            // MSG002: Notify the new board member that they were added
            notifications.Add(new Domain.Entities.Notifications.Notification
            {
                Title = string.Empty,
                Body = $"{LocalizationHelper.GetBoardMemberTypeDisplay(boardMember.MemberType, _localizer)}|{fund.Name}|{userRole}|{_currentUserService.UserName}",
                FundId = fund.Id,
                UserId = boardMember.UserId,
                NotificationType = (int)NotificationType.BoardMemberAdded, // MSG002
            });

            // MSG007: Notify fund stakeholders about the new board member
            var stakeholderUserIds = new List<int>();

            // Add Fund Managers
            if (fund.FundManagers != null)
            {
                stakeholderUserIds.AddRange(fund.FundManagers.Select(fm => fm.UserId));
            }

            // Add Legal Council
            if (fund.LegalCouncilId > 0)
            {
                stakeholderUserIds.Add(fund.LegalCouncilId);
            }

            // Add Board Secretaries
            if (fund.FundBoardSecretaries != null)
            {
                stakeholderUserIds.AddRange(fund.FundBoardSecretaries.Select(bs => bs.UserId));
            }

            // Remove duplicates and the new member (they already get MSG002)
            stakeholderUserIds = stakeholderUserIds.Distinct().Where(id => id != boardMember.UserId).ToList();

            foreach (var userId in stakeholderUserIds)
            {
                notifications.Add(new Domain.Entities.Notifications.Notification
                {
                    Title = string.Empty,
                    Body = $"{user?.FullName}|{LocalizationHelper.GetBoardMemberTypeDisplay(boardMember.MemberType, _localizer)}|{fund.Name}|{userRole}|{_currentUserService.UserName}",
                    FundId = fund.Id,
                    UserId = userId,
                    NotificationType = (int)NotificationType.BoardMemberAddedToFund, // MSG007
                });
            }

            if (notifications.Count > 0)
            {
                await _repository.Notifications.AddRangeAsync(notifications);
                _logger.LogInfo($"Board member notifications added for BoardMember ID: {boardMember.Id}, Count: {notifications.Count}");
            }
        }

        /// <summary>
        /// Checks if fund should be activated when 2 independent members are added
        /// Uses enhanced State Pattern with FundStateContext for comprehensive audit logging and notifications
        /// Based on Sprint.md MSG008 notification requirements
        /// </summary>
        private async Task CheckAndActivateFund(Fund fund)
        {
            try
            {
                // Check if fund is not already active
                var currentStatus = fund.GetCurrentStatusEnum();
                if (currentStatus == FundStatusEnum.Active)
                    return;

                // Count active independent board members using repository method
                var independentMembersCount = await _repository.BoardMembers.IndependentMembersCountAsync(fund.Id);

                // Activate fund if we have 2 or more independent members
                if (independentMembersCount >= 2)
                {
                    // Use basic state transition for now
                    // TODO: Integrate with FundStateContext for enhanced audit logging
                    var transitionSuccessful = fund.TransitionToStatus(FundStatusEnum.Active);

                    if (transitionSuccessful)
                    {
                        await _repository.Funds.UpdateAsync(fund);

                        // Send fund activation notifications (MSG008)
                        await SendFundActivationNotifications(fund);

                        _logger.LogInfo($"Fund {fund.Id} activated with {independentMembersCount} independent members");
                    }
                    else
                    {
                        _logger.LogWarn($"Failed to transition Fund {fund.Id} to Active status. Current status: {currentStatus}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking and activating fund {fund.Id}");
            }
        }

        /// <summary>
        /// Sends fund activation notifications to all fund stakeholders
        /// Based on Sprint.md MSG008 notification requirements
        /// </summary>
        private async Task SendFundActivationNotifications(Fund fund)
        {
            var notifications = new List<Domain.Entities.Notifications.Notification>();

            // Get fund details to access all stakeholders

            var stakeholderUserIds = new List<int>();

            // Add Fund Managers
            if (fund.FundManagers != null)
            {
                stakeholderUserIds.AddRange(fund.FundManagers.Select(fm => fm.UserId));
            }

            // Add Legal Council
            if (fund.LegalCouncilId > 0)
            {
                stakeholderUserIds.Add(fund.LegalCouncilId);
            }

            // Add Board Secretaries
            if (fund.FundBoardSecretaries != null)
            {
                stakeholderUserIds.AddRange(fund.FundBoardSecretaries.Select(bs => bs.UserId));
            }

            // Add Board Members
            if (fund.BoardMembers != null)
            {
                stakeholderUserIds.AddRange(fund.BoardMembers.Where(bm => bm.IsActive).Select(bm => bm.UserId));
            }

            // Remove duplicates
            stakeholderUserIds = stakeholderUserIds.Distinct().ToList();

            foreach (var userId in stakeholderUserIds)
            {
                notifications.Add(new Domain.Entities.Notifications.Notification
                {
                    Title = string.Empty,
                    Body = fund.Name, // MSG008: Fund name only
                    FundId = fund.Id,
                    UserId = userId,
                    NotificationType = (int)NotificationType.FundActivated, // MSG008
                });
            }

            if (notifications.Count > 0)
            {
                await _repository.Notifications.AddRangeAsync(notifications);
                _logger.LogInfo($"Fund activation notifications sent for Fund ID: {fund.Id}, Count: {notifications.Count}");
            }
        }
        /// <summary>
        /// Determines the current user's role within a specific fund context
        /// Checks FundBoardSecretary table, Fund.LegalCouncilId field, and FundManager table
        /// Follows Clean Architecture and CQRS patterns with proper repository usage
        /// </summary>
        /// <param name="fundId">The fund ID to check roles for</param>
        /// <param name="currentUserId">The current user ID to check roles for</param>
        /// <returns>Comma-separated string of roles the user has in the fund, or empty string if no roles</returns>
        private async Task<Roles> GetUserFundRole(Fund fundDetails, int currentUserId)
        {
            try
            {
                _logger.LogInfo($"Checking fund roles for User ID: {currentUserId} in Fund ID: {fundDetails.Id}");

                var userRole = Roles.None;
                // Get fund details with all related entities

                // 1. Check if user is Legal Council for the fund
                if (fundDetails.LegalCouncilId == currentUserId)
                {
                    userRole = Roles.LegalCouncil;
                    _logger.LogInfo($"User ID: {currentUserId} is Legal Council for Fund ID: {fundDetails.Id}");
                }

                // 2. Check if user is a Fund Manager for the fund
                if (fundDetails.FundManagers != null && fundDetails.FundManagers.Count > 0)
                {
                    var isFundManager = fundDetails.FundManagers.Any(fm => fm.UserId == currentUserId);
                    if (isFundManager)
                    {
                        userRole = Roles.FundManager;
                        _logger.LogInfo($"User ID: {currentUserId} is Fund Manager for Fund ID: {fundDetails.Id}");
                    }
                }

                // 3. Check if user is a Board Secretary for the fund
                if (fundDetails.FundBoardSecretaries != null && fundDetails.FundBoardSecretaries.Count > 0)
                {
                    var isBoardSecretary = fundDetails.FundBoardSecretaries.Any(bs => bs.UserId == currentUserId);
                    if (isBoardSecretary)
                    {
                        userRole = Roles.BoardSecretary;
                        _logger.LogInfo($"User ID: {currentUserId} is Board Secretary for Fund ID: {fundDetails.Id}");
                    }
                }

                // 4. Check if user is a Board Member for the fund
                if (fundDetails.BoardMembers != null && fundDetails.BoardMembers.Count > 0)
                {
                    var isBoardMember = fundDetails.BoardMembers.Any(bs => bs.UserId == currentUserId);
                    if (isBoardMember)
                    {
                        userRole = Roles.BoardMember;
                        _logger.LogInfo($"User ID: {currentUserId} is Board Member for Fund ID: {fundDetails.Id}");
                    }
                }

                // Return comma-separated roles or empty string if no roles found

                _logger.LogInfo($"User ID: {currentUserId} has roles in Fund ID: {fundDetails.Id}: '{userRole}'");

                return userRole;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking fund roles for User ID: {currentUserId} in Fund ID: {fundDetails.Id}");
                return Roles.None;
            }
        }

        /// <summary>
        /// Sends WhatsApp notification to the newly added board member
        /// Implements Sprint 3 JDWA-1258 conditional registration notification logic:
        /// - If user has RegistrationMessageIsSent = 0, sends registration message (MSG-ADD-008)
        /// - Otherwise, sends standard fund member addition notification
        /// </summary>
        private async Task SendWhatsAppNotification(Fund fund, BoardMember boardMember,string temporaryPassword)
        {
            try
            {
                _logger.LogInfo($"Starting WhatsApp notification for board member {boardMember.UserId} in fund {fund.Id}");

                // Get the user details
                var user = await _userManager.FindByIdAsync(boardMember.UserId.ToString());
                if (user == null)
                {
                    _logger.LogWarn($"User not found for board member notification. UserId: {boardMember.UserId}");
                    return;
                }

                // Check if user is eligible for registration notification (Sprint 3 JDWA-1258)
                var phoneNumber = user.CountryCode + user.PhoneNumber;

                if (string.IsNullOrEmpty(phoneNumber))
                {
                    _logger.LogWarn($"No phone number found for user {boardMember.UserId}. WhatsApp notification skipped.");
                    return;
                }

                // Prepare message parameters: fund name and member role
                var memberRoleDisplay = LocalizationHelper.GetBoardMemberTypeDisplay(boardMember.MemberType, _localizer);
                var parameters = new object[] { fund.Name, memberRoleDisplay };

                // Send standard WhatsApp fund member addition notification
                var response = await _whatsAppService.SendLocalizedMessageAsync(
                    user.Id,
                    phoneNumber,
                    WhatsAppMessageType.FundMemberAdded,
                    new object[] { temporaryPassword },
                    CancellationToken.None);

                if (response.IsSuccess)
                {
                    _logger.LogInfo($"Standard fund member WhatsApp notification sent successfully to user {boardMember.UserId}. MessageId: {response.MessageId}");
                }
                else
                {
                    _logger.LogError(null, $"Failed to send standard fund member WhatsApp notification to user {boardMember.UserId}. Error: {response.ErrorMessage}");
                }
        }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending WhatsApp notification for board member {boardMember.UserId} in fund {fund.Id}");
                // Don't throw - WhatsApp notification failure shouldn't break the main flow
            }
        }
     
        /// <summary>
        /// Validates if a digit string looks like a valid international phone number
        /// </summary>
        private bool IsValidInternationalFormat(string digits)
        {
            // Common country codes and their expected lengths
            var countryCodePatterns = new Dictionary<string, (int minLength, int maxLength)>
            {
                { "1", (11, 11) },      // US/Canada: +1XXXXXXXXXX
                { "20", (11, 13) },     // Egypt: +20XXXXXXXXX
                { "966", (12, 12) },    // Saudi Arabia: +966XXXXXXXXX
                { "971", (12, 12) },    // UAE: +971XXXXXXXXX
                { "965", (11, 11) },    // Kuwait: +965XXXXXXXX
                { "973", (11, 11) },    // Bahrain: +973XXXXXXXX
                { "974", (11, 11) },    // Qatar: +974XXXXXXXX
                { "968", (11, 11) },    // Oman: +968XXXXXXXX
                { "44", (12, 13) },     // UK: +44XXXXXXXXXXX
                { "49", (11, 12) },     // Germany: +49XXXXXXXXXX
                { "33", (11, 12) },     // France: +33XXXXXXXXX
            };

            foreach (var pattern in countryCodePatterns)
            {
                if (digits.StartsWith(pattern.Key) &&
                    digits.Length >= pattern.Value.minLength &&
                    digits.Length <= pattern.Value.maxLength)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Generate a secure temporary password
        /// Following the same pattern used in other handlers for consistency
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

