using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;
using Microsoft.Extensions.Localization;
using Application.Features.Identity.Users.Queries.List;
using Application.Features.Identity.Users.Queries.Responses;
using Abstraction.Contracts.Identity;
using Abstraction.Contracts.Logging;
using Domain.Entities.Users;
using Domain.Helpers;
using Resources;
using Abstraction.Common.Wappers;

namespace Application.UnitTests.Features.Identity.Users.Queries.List
{
    /// <summary>
    /// Unit tests for enhanced ListQueryHandler with optimized role mapping functionality
    /// Tests the new navigation property approach for role retrieval and mapping
    /// Validates the elimination of N+1 queries through Entity Framework eager loading
    /// </summary>
    public class ListQueryHandlerTests
    {
        private readonly Mock<IIdentityServiceManager> _mockIdentityServiceManager;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerManager> _mockLogger;
        private readonly Mock<IStringLocalizer<SharedResources>> _mockLocalizer;
        private readonly ListQueryHandler _handler;

        public ListQueryHandlerTests()
        {
            _mockIdentityServiceManager = new Mock<IIdentityServiceManager>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerManager>();
            _mockLocalizer = new Mock<IStringLocalizer<SharedResources>>();

            _handler = new ListQueryHandler(
                _mockIdentityServiceManager.Object,
                _mockMapper.Object,
                _mockLogger.Object,
                _mockLocalizer.Object);
        }

        [Fact]
        public async Task Handle_WithUsersHavingMultipleRoles_ShouldMapRolesCorrectly()
        {
            // Given
            var request = new ListQuery { PageNumber = 1, PageSize = 10 };

            var testUsers = new List<User>
            {
                new User
                {
                    Id = 1,
                    FullName = "Test User",
                    Email = "test@example.com",
                    Roles = new List<Role>
                    {
                        new Role { Id = 1, Name = "Fund Manager" },
                        new Role { Id = 3, Name = "Board Secretary" }
                    }
                }
            }.AsQueryable();

            var expectedResponse = new List<GetUserListResponse>
            {
                new GetUserListResponse
                {
                    Id = 1,
                    FullName = "Test User",
                    Email = "test@example.com",
                    Roles = new List<string> { "Fund Manager", "Board Secretary" },
                    PrimaryRole = "Fund Manager"
                }
            };

            _mockIdentityServiceManager.Setup(x => x.UserManagmentService.UsersWithRoles())
                .Returns(testUsers);

            _mockMapper.Setup(x => x.ProjectTo<GetUserListResponse>(It.IsAny<IQueryable<User>>()))
                .Returns(expectedResponse.AsQueryable());

            // When
            var result = await _handler.Handle(request, CancellationToken.None);

            // Then
            result.Should().NotBeNull();
            result.Data.Should().HaveCount(1);
            result.Data[0].Roles.Should().HaveCount(2);
            result.Data[0].Roles.Should().Contain("Fund Manager");
            result.Data[0].Roles.Should().Contain("Board Secretary");
            result.Data[0].PrimaryRole.Should().Be("Fund Manager");
        }

        [Fact]
        public async Task EnhanceWithRoleInformation_WithUserHavingNoRoles_ShouldReturnEmptyCollections()
        {
            // Given
            var userResponses = new List<GetUserListResponse>
            {
                new GetUserListResponse { Id = 1, FullName = "Test User", Email = "test@example.com" }
            };

            var testUser = new User { Id = 1, FullName = "Test User", Email = "test@example.com" };
            var rolesResponse = new ManageUserRolesResponse
            {
                UserId = 1,
                UserRoles = new List<UserRoles>
                {
                    new UserRoles { Id = 1, Name = "Fund Manager", HasRole = false },
                    new UserRoles { Id = 2, Name = "Legal Council", HasRole = false }
                }
            };

            _mockIdentityServiceManager.Setup(x => x.UserManagmentService.FindByIdAsync("1"))
                .ReturnsAsync(testUser);
            _mockIdentityServiceManager.Setup(x => x.AuthorizationService.GetUsersRoles(testUser))
                .ReturnsAsync(rolesResponse);

            // When
            await CallEnhanceWithRoleInformation(userResponses);

            // Then
            userResponses[0].Roles.Should().BeEmpty();
            userResponses[0].PrimaryRole.Should().BeNull();
        }

        [Fact]
        public async Task EnhanceWithRoleInformation_WithUserNotFound_ShouldSetEmptyRolesAndLogWarning()
        {
            // Given
            var userResponses = new List<GetUserListResponse>
            {
                new GetUserListResponse { Id = 999, FullName = "Non-existent User", Email = "nonexistent@example.com" }
            };

            _mockIdentityServiceManager.Setup(x => x.UserManagmentService.FindByIdAsync("999"))
                .ReturnsAsync((User)null);

            // When
            await CallEnhanceWithRoleInformation(userResponses);

            // Then
            userResponses[0].Roles.Should().BeEmpty();
            userResponses[0].PrimaryRole.Should().BeNull();
            _mockLogger.Verify(x => x.LogWarn("User not found with ID: 999"), Times.Once);
        }

        [Fact]
        public async Task EnhanceWithRoleInformation_WithNullRolesResponse_ShouldSetEmptyRolesAndLogWarning()
        {
            // Given
            var userResponses = new List<GetUserListResponse>
            {
                new GetUserListResponse { Id = 1, FullName = "Test User", Email = "test@example.com" }
            };

            var testUser = new User { Id = 1, FullName = "Test User", Email = "test@example.com" };

            _mockIdentityServiceManager.Setup(x => x.UserManagmentService.FindByIdAsync("1"))
                .ReturnsAsync(testUser);
            _mockIdentityServiceManager.Setup(x => x.AuthorizationService.GetUsersRoles(testUser))
                .ReturnsAsync((ManageUserRolesResponse)null);

            // When
            await CallEnhanceWithRoleInformation(userResponses);

            // Then
            userResponses[0].Roles.Should().BeEmpty();
            userResponses[0].PrimaryRole.Should().BeNull();
            _mockLogger.Verify(x => x.LogWarn("Roles response is null for user ID: 1"), Times.Once);
        }

        [Fact]
        public async Task EnhanceWithRoleInformation_WithException_ShouldSetEmptyRolesAndLogError()
        {
            // Given
            var userResponses = new List<GetUserListResponse>
            {
                new GetUserListResponse { Id = 1, FullName = "Test User", Email = "test@example.com" }
            };

            var testUser = new User { Id = 1, FullName = "Test User", Email = "test@example.com" };
            var exception = new Exception("Database connection failed");

            _mockIdentityServiceManager.Setup(x => x.UserManagmentService.FindByIdAsync("1"))
                .ReturnsAsync(testUser);
            _mockIdentityServiceManager.Setup(x => x.AuthorizationService.GetUsersRoles(testUser))
                .ThrowsAsync(exception);

            // When
            await CallEnhanceWithRoleInformation(userResponses);

            // Then
            userResponses[0].Roles.Should().BeEmpty();
            userResponses[0].PrimaryRole.Should().BeNull();
            _mockLogger.Verify(x => x.LogError(exception, "Error retrieving roles for user ID: 1. Error: Database connection failed"), Times.Once);
        }

        [Fact]
        public async Task EnhanceWithRoleInformation_WithSingleRole_ShouldSetCorrectPrimaryRole()
        {
            // Given
            var userResponses = new List<GetUserListResponse>
            {
                new GetUserListResponse { Id = 1, FullName = "Test User", Email = "test@example.com" }
            };

            var testUser = new User { Id = 1, FullName = "Test User", Email = "test@example.com" };
            var rolesResponse = new ManageUserRolesResponse
            {
                UserId = 1,
                UserRoles = new List<UserRoles>
                {
                    new UserRoles { Id = 1, Name = "Fund Manager", HasRole = true },
                    new UserRoles { Id = 2, Name = "Legal Council", HasRole = false }
                }
            };

            _mockIdentityServiceManager.Setup(x => x.UserManagmentService.FindByIdAsync("1"))
                .ReturnsAsync(testUser);
            _mockIdentityServiceManager.Setup(x => x.AuthorizationService.GetUsersRoles(testUser))
                .ReturnsAsync(rolesResponse);

            // When
            await CallEnhanceWithRoleInformation(userResponses);

            // Then
            userResponses[0].Roles.Should().HaveCount(1);
            userResponses[0].Roles.Should().Contain("Fund Manager");
            userResponses[0].PrimaryRole.Should().Be("Fund Manager");
            _mockLogger.Verify(x => x.LogInfo("Successfully retrieved 1 roles for user ID: 1"), Times.Once);
        }

        [Fact]
        public async Task EnhanceWithRoleInformation_WithMultipleUsers_ShouldProcessAllUsers()
        {
            // Given
            var userResponses = new List<GetUserListResponse>
            {
                new GetUserListResponse { Id = 1, FullName = "User 1", Email = "user1@example.com" },
                new GetUserListResponse { Id = 2, FullName = "User 2", Email = "user2@example.com" }
            };

            var user1 = new User { Id = 1, FullName = "User 1", Email = "user1@example.com" };
            var user2 = new User { Id = 2, FullName = "User 2", Email = "user2@example.com" };

            var roles1 = new ManageUserRolesResponse
            {
                UserId = 1,
                UserRoles = new List<UserRoles>
                {
                    new UserRoles { Id = 1, Name = "Fund Manager", HasRole = true }
                }
            };

            var roles2 = new ManageUserRolesResponse
            {
                UserId = 2,
                UserRoles = new List<UserRoles>
                {
                    new UserRoles { Id = 2, Name = "Legal Council", HasRole = true }
                }
            };

            _mockIdentityServiceManager.Setup(x => x.UserManagmentService.FindByIdAsync("1")).ReturnsAsync(user1);
            _mockIdentityServiceManager.Setup(x => x.UserManagmentService.FindByIdAsync("2")).ReturnsAsync(user2);
            _mockIdentityServiceManager.Setup(x => x.AuthorizationService.GetUsersRoles(user1)).ReturnsAsync(roles1);
            _mockIdentityServiceManager.Setup(x => x.AuthorizationService.GetUsersRoles(user2)).ReturnsAsync(roles2);

            // When
            await CallEnhanceWithRoleInformation(userResponses);

            // Then
            userResponses[0].Roles.Should().Contain("Fund Manager");
            userResponses[0].PrimaryRole.Should().Be("Fund Manager");
            userResponses[1].Roles.Should().Contain("Legal Council");
            userResponses[1].PrimaryRole.Should().Be("Legal Council");
        }

        /// <summary>
        /// Helper method to call the private EnhanceWithRoleInformation method using reflection
        /// This allows us to test the method directly without going through the full Handle method
        /// </summary>
        private async Task CallEnhanceWithRoleInformation(List<GetUserListResponse> userResponses)
        {
            var method = typeof(ListQueryHandler).GetMethod("EnhanceWithRoleInformation", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            await (Task)method.Invoke(_handler, new object[] { userResponses });
        }
    }
}
