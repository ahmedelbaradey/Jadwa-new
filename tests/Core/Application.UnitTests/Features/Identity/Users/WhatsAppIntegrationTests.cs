using Xunit;
using Moq;
using Microsoft.Extensions.Localization;
using AutoMapper;
using Application.Features.Identity.Users.Commands.AddUser;
using Application.Features.Identity.Users.Commands.AdminResetPassword;
using Application.Features.Identity.Users.Commands.ResendRegistrationMessage;
using Application.Features.Identity.Users.Commands.ActivateUser;
using Abstraction.Contracts.Identity;
using Abstraction.Contract.Service;
using Abstraction.Contracts.Logger;
using Core.Abstraction.Contract.Service.Notifications;
using Resources;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Abstraction.Base.Response;
using Abstraction.Contracts.Repository;

namespace Tests.Core.Application.UnitTests.Features.Identity.Users
{
    /// <summary>
    /// Unit tests for WhatsApp integration in user management command handlers
    /// Tests the Sprint 3 WhatsApp notification requirements
    /// </summary>
    public class WhatsAppIntegrationTests
    {
        private readonly Mock<IIdentityServiceManager> _mockIdentityService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IStringLocalizer<SharedResources>> _mockLocalizer;
        private readonly Mock<ICurrentUserService> _mockCurrentUserService;
        private readonly Mock<IWhatsAppNotificationService> _mockWhatsAppService;
        private readonly Mock<ILoggerManager> _mockLogger;
        private readonly Mock<IRepositoryManager> _mockRepository;

        public WhatsAppIntegrationTests()
        {
            _mockIdentityService = new Mock<IIdentityServiceManager>();
            _mockMapper = new Mock<IMapper>();
            _mockLocalizer = new Mock<IStringLocalizer<SharedResources>>();
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _mockWhatsAppService = new Mock<IWhatsAppNotificationService>();
            _mockLogger = new Mock<ILoggerManager>();
            _mockRepository = new Mock<IRepositoryManager>();
        }

        [Fact]
        public async Task AddUserCommandHandler_ShouldSendWhatsAppNotification_WhenUserIsEligible()
        {
            // Arrange
            var handler = new AddUserCommandHandler(
                _mockIdentityService.Object,
                _mockMapper.Object,
                _mockLocalizer.Object,
                _mockCurrentUserService.Object,
                _mockWhatsAppService.Object,
                _mockLogger.Object);

            var command = new AddUserCommand
            {
                Email = "test@example.com",
                UserName = "201500294221",
                Password = "TempPass123!",
                ConfirmPassword = "TempPass123!",
                FullName = "Test User",
                Roles = new List<string> { "Fund Manager" } // Not Board Member only
            };

            var user = new User
            {
                Id = 1,
                Email = command.Email,
                UserName = command.UserName,
                FullName = command.FullName,
                PhoneNumber = command.UserName,
                RegistrationMessageIsSent = true
            };

            // Setup mocks
            _mockIdentityService.Setup(x => x.UserManagmentService.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);
            _mockIdentityService.Setup(x => x.UserManagmentService.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);
            _mockMapper.Setup(x => x.Map<User>(It.IsAny<AddUserCommand>()))
                .Returns(user);
            _mockIdentityService.Setup(x => x.UserManagmentService.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _mockIdentityService.Setup(x => x.AuthorizationService.IsRoleNameExist(It.IsAny<string>()))
                .ReturnsAsync(true);
            _mockIdentityService.Setup(x => x.UserManagmentService.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _mockWhatsAppService.Setup(x => x.SendUserRegistrationMessageAsync(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new WhatsAppMessageResponseDto { IsSuccess = true, MessageId = "msg123" });

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            _mockWhatsAppService.Verify(x => x.SendUserRegistrationMessageAsync(
                It.IsAny<int>(), 
                "+201500294221", // Should format phone number correctly
                It.IsAny<string>(), 
                It.IsAny<string>(), 
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddUserCommandHandler_ShouldNotSendWhatsAppNotification_WhenUserIsBoardMemberOnly()
        {
            // Arrange
            var handler = new AddUserCommandHandler(
                _mockIdentityService.Object,
                _mockMapper.Object,
                _mockLocalizer.Object,
                _mockCurrentUserService.Object,
                _mockWhatsAppService.Object,
                _mockLogger.Object);

            var command = new AddUserCommand
            {
                Email = "test@example.com",
                UserName = "201500294221",
                Password = "TempPass123!",
                ConfirmPassword = "TempPass123!",
                FullName = "Test User",
                Roles = new List<string> { "Board Member" } // Board Member only
            };

            var user = new User
            {
                Id = 1,
                Email = command.Email,
                UserName = command.UserName,
                FullName = command.FullName,
                PhoneNumber = command.UserName,
                RegistrationMessageIsSent = false // Should be false for Board Member only
            };

            // Setup mocks
            _mockIdentityService.Setup(x => x.UserManagmentService.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);
            _mockIdentityService.Setup(x => x.UserManagmentService.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);
            _mockMapper.Setup(x => x.Map<User>(It.IsAny<AddUserCommand>()))
                .Returns(user);
            _mockIdentityService.Setup(x => x.UserManagmentService.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _mockIdentityService.Setup(x => x.AuthorizationService.IsRoleNameExist(It.IsAny<string>()))
                .ReturnsAsync(true);
            _mockIdentityService.Setup(x => x.UserManagmentService.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            _mockWhatsAppService.Verify(x => x.SendUserRegistrationMessageAsync(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), 
                Times.Never);
        }

        [Theory]
        [InlineData("+0201500294221", "+201500294221")] // Egyptian number with extra 0
        [InlineData("0201500294221", "+201500294221")]  // Egyptian number without +
        [InlineData("01500294221", "+201500294221")]    // Egyptian mobile without country code
        [InlineData("1500294221", "+201500294221")]     // Egyptian mobile without leading 0
        [InlineData("+966501234567", "+966501234567")]  // Saudi number (correct)
        [InlineData("0501234567", "+966501234567")]     // Saudi mobile with leading 0
        [InlineData("501234567", "+966501234567")]      // Saudi mobile without leading 0
        public async Task PhoneNumberFormatting_ShouldFormatCorrectly_InAllHandlers(string input, string expected)
        {
            // Test AddUserCommandHandler phone formatting
            var addUserHandler = new AddUserCommandHandler(
                _mockIdentityService.Object,
                _mockMapper.Object,
                _mockLocalizer.Object,
                _mockCurrentUserService.Object,
                _mockWhatsAppService.Object,
                _mockLogger.Object);

            // Use reflection to test the private method
            var method = typeof(AddUserCommandHandler)
                .GetMethod("FormatInternationalPhoneNumber", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            var result = method.Invoke(addUserHandler, new object[] { input }) as string;
            
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task AdminResetPasswordCommandHandler_ShouldSendWhatsAppNotification()
        {
            // Arrange
            var handler = new AdminResetPasswordCommandHandler(
                _mockIdentityService.Object,
                _mockLocalizer.Object,
                _mockCurrentUserService.Object,
                _mockWhatsAppService.Object,
                _mockLogger.Object);

            var command = new AdminResetPasswordCommand { UserId = "1" };
            var user = new User
            {
                Id = 1,
                PhoneNumber = "201500294221",
                UserName = "testuser"
            };

            // Setup mocks
            _mockIdentityService.Setup(x => x.UserManagmentService.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _mockIdentityService.Setup(x => x.UserManagmentService.RemovePasswordAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);
            _mockIdentityService.Setup(x => x.UserManagmentService.AddPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _mockWhatsAppService.Setup(x => x.SendPasswordResetMessageAsync(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new WhatsAppMessageResponseDto { IsSuccess = true, MessageId = "msg123" });

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            _mockWhatsAppService.Verify(x => x.SendPasswordResetMessageAsync(
                It.IsAny<int>(), 
                "+201500294221", // Should format phone number correctly
                It.IsAny<string>(), 
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ActivateDeActivateUserCommandHandler_ShouldSendWhatsAppNotifications()
        {
            // Arrange
            var handler = new ActivateDeActivateUserCommandHandler(
                _mockLocalizer.Object,
                _mockIdentityService.Object,
                _mockCurrentUserService.Object,
                _mockRepository.Object,
                _mockWhatsAppService.Object,
                _mockLogger.Object);

            var activateCommand = new ActivateDeActivateUserCommand { UserId = 1, IsActive = true };
            var user = new User
            {
                Id = 1,
                PhoneNumber = "201500294221",
                UserName = "testuser",
                IsActive = false
            };

            // Setup mocks
            _mockIdentityService.Setup(x => x.UserManagmentService.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);
            _mockIdentityService.Setup(x => x.UserManagmentService.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);
            _mockWhatsAppService.Setup(x => x.SendAccountActivationMessageAsync(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new WhatsAppMessageResponseDto { IsSuccess = true, MessageId = "msg123" });

            // Act
            var result = await handler.Handle(activateCommand, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            _mockWhatsAppService.Verify(x => x.SendAccountActivationMessageAsync(
                It.IsAny<int>(), 
                "+201500294221", // Should format phone number correctly
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
