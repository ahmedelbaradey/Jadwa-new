using Application.Features.Identity.Users.Queries.CheckSingleHolderRoleAvailability;
using Abstraction.Constants;
using Abstraction.Contract.Service.Identity;
using Abstraction.Contracts.Logger;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Moq;
using Resources;
using Xunit;

namespace Application.Tests.Features.Identity.Users.Queries
{
    /// <summary>
    /// Unit tests for CheckSingleHolderRoleAvailabilityQueryHandler
    /// Tests the business logic for checking single-holder role availability
    /// </summary>
    public class CheckSingleHolderRoleAvailabilityQueryHandlerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<ISingleHolderRoleService> _mockSingleHolderRoleService;
        private readonly Mock<IStringLocalizer<SharedResources>> _mockLocalizer;
        private readonly Mock<ILoggerManager> _mockLogger;
        private readonly CheckSingleHolderRoleAvailabilityQueryHandler _handler;

        public CheckSingleHolderRoleAvailabilityQueryHandlerTests()
        {
            // Setup UserManager mock
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _mockSingleHolderRoleService = new Mock<ISingleHolderRoleService>();
            _mockLocalizer = new Mock<IStringLocalizer<SharedResources>>();
            _mockLogger = new Mock<ILoggerManager>();

            _handler = new CheckSingleHolderRoleAvailabilityQueryHandler(
                _mockUserManager.Object,
                _mockSingleHolderRoleService.Object,
                _mockLocalizer.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_WhenAllRolesHaveActiveUsers_ShouldReturnAllTrue()
        {
            // Arrange
            var query = new CheckSingleHolderRoleAvailabilityQuery();
            
            var activeUser = new User { Id = 1, IsActive = true, UserName = "testuser" };
            var usersInRole = new List<User> { activeUser };

            _mockUserManager.Setup(x => x.GetUsersInRoleAsync(It.IsAny<string>()))
                           .ReturnsAsync(usersInRole);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.LegalCouncilHasActiveUser);
            Assert.True(result.Data.FinanceControllerHasActiveUser);
            Assert.True(result.Data.ComplianceLegalManagingDirectorHasActiveUser);
            Assert.True(result.Data.HeadOfRealEstateHasActiveUser);
            Assert.Equal(4, result.Data.Summary.AssignedRoles);
            Assert.Equal(0, result.Data.Summary.AvailableRoles);
            Assert.Equal(100m, result.Data.Summary.AssignmentPercentage);
        }

        [Fact]
        public async Task Handle_WhenNoRolesHaveActiveUsers_ShouldReturnAllFalse()
        {
            // Arrange
            var query = new CheckSingleHolderRoleAvailabilityQuery();
            
            var emptyUserList = new List<User>();

            _mockUserManager.Setup(x => x.GetUsersInRoleAsync(It.IsAny<string>()))
                           .ReturnsAsync(emptyUserList);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.NotNull(result.Data);
            Assert.False(result.Data.LegalCouncilHasActiveUser);
            Assert.False(result.Data.FinanceControllerHasActiveUser);
            Assert.False(result.Data.ComplianceLegalManagingDirectorHasActiveUser);
            Assert.False(result.Data.HeadOfRealEstateHasActiveUser);
            Assert.Equal(0, result.Data.Summary.AssignedRoles);
            Assert.Equal(4, result.Data.Summary.AvailableRoles);
            Assert.Equal(0m, result.Data.Summary.AssignmentPercentage);
        }

        [Fact]
        public async Task Handle_WhenSomeRolesHaveActiveUsers_ShouldReturnMixedResults()
        {
            // Arrange
            var query = new CheckSingleHolderRoleAvailabilityQuery();
            
            var activeUser = new User { Id = 1, IsActive = true, UserName = "testuser" };
            var inactiveUser = new User { Id = 2, IsActive = false, UserName = "inactiveuser" };

            _mockUserManager.Setup(x => x.GetUsersInRoleAsync(RoleHelper.LEGAL_COUNCIL))
                           .ReturnsAsync(new List<User> { activeUser });
            
            _mockUserManager.Setup(x => x.GetUsersInRoleAsync(RoleHelper.FinanceController))
                           .ReturnsAsync(new List<User>());
            
            _mockUserManager.Setup(x => x.GetUsersInRoleAsync(RoleHelper.ComplianceLegalManagingDirector))
                           .ReturnsAsync(new List<User> { inactiveUser });
            
            _mockUserManager.Setup(x => x.GetUsersInRoleAsync(RoleHelper.HeadOfRealEstate))
                           .ReturnsAsync(new List<User> { activeUser });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.LegalCouncilHasActiveUser);
            Assert.False(result.Data.FinanceControllerHasActiveUser);
            Assert.False(result.Data.ComplianceLegalManagingDirectorHasActiveUser); // Inactive user
            Assert.True(result.Data.HeadOfRealEstateHasActiveUser);
            Assert.Equal(2, result.Data.Summary.AssignedRoles);
            Assert.Equal(2, result.Data.Summary.AvailableRoles);
            Assert.Equal(50m, result.Data.Summary.AssignmentPercentage);
        }

        [Fact]
        public async Task Handle_WhenUserManagerThrowsException_ShouldReturnServerError()
        {
            // Arrange
            var query = new CheckSingleHolderRoleAvailabilityQuery();
            
            _mockUserManager.Setup(x => x.GetUsersInRoleAsync(It.IsAny<string>()))
                           .ThrowsAsync(new Exception("Database error"));

            _mockLocalizer.Setup(x => x[SharedResourcesKey.SystemErrorRetrievingData])
                         .Returns(new LocalizedString(SharedResourcesKey.SystemErrorRetrievingData, "System error retrieving data"));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("System error retrieving data", result.Message);
            _mockLogger.Verify(x => x.LogError(It.IsAny<Exception>(), "Error checking single-holder role availability"), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenSpecificRoleCheckFails_ShouldDefaultToFalseForThatRole()
        {
            // Arrange
            var query = new CheckSingleHolderRoleAvailabilityQuery();
            
            var activeUser = new User { Id = 1, IsActive = true, UserName = "testuser" };

            _mockUserManager.Setup(x => x.GetUsersInRoleAsync(RoleHelper.LEGAL_COUNCIL))
                           .ReturnsAsync(new List<User> { activeUser });
            
            _mockUserManager.Setup(x => x.GetUsersInRoleAsync(RoleHelper.FinanceController))
                           .ThrowsAsync(new Exception("Role check error"));
            
            _mockUserManager.Setup(x => x.GetUsersInRoleAsync(RoleHelper.ComplianceLegalManagingDirector))
                           .ReturnsAsync(new List<User>());
            
            _mockUserManager.Setup(x => x.GetUsersInRoleAsync(RoleHelper.HeadOfRealEstate))
                           .ReturnsAsync(new List<User>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Succeeded);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.LegalCouncilHasActiveUser);
            Assert.False(result.Data.FinanceControllerHasActiveUser); // Should default to false on error
            Assert.False(result.Data.ComplianceLegalManagingDirectorHasActiveUser);
            Assert.False(result.Data.HeadOfRealEstateHasActiveUser);
            Assert.Equal(1, result.Data.Summary.AssignedRoles);
            Assert.Equal(3, result.Data.Summary.AvailableRoles);
            Assert.Equal(25m, result.Data.Summary.AssignmentPercentage);
        }

        [Fact]
        public async Task Handle_ShouldLogInfoMessages()
        {
            // Arrange
            var query = new CheckSingleHolderRoleAvailabilityQuery();
            
            _mockUserManager.Setup(x => x.GetUsersInRoleAsync(It.IsAny<string>()))
                           .ReturnsAsync(new List<User>());

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockLogger.Verify(x => x.LogInfo("Checking single-holder role availability status"), Times.Once);
            _mockLogger.Verify(x => x.LogInfo(It.Is<string>(s => s.Contains("Single-holder role availability check completed"))), Times.Once);
        }
    }
}
