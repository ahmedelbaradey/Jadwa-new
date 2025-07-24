using Abstraction.Contracts.Logger;
using Domain.Entities.Users;
using Infrastructure.Identity.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using FluentAssertions;

namespace Application.UnitTests.Features.Identity.Users.Services
{
    /// <summary>
    /// Unit tests for UserManagmentIdentityService role validation methods
    /// Tests unique role checking functionality with various scenarios
    /// </summary>
    public class UserManagmentServiceTests : IDisposable
    {
        private readonly Mock<ILoggerManager> _mockLogger;
        private readonly UserManager<User> _userManager;
        private readonly UserManagmentIdentityService _service;
        private readonly DbContextOptions<TestDbContext> _dbOptions;
        private readonly TestDbContext _context;

        public UserManagmentServiceTests()
        {
            _mockLogger = new Mock<ILoggerManager>();
            
            // Setup in-memory database
            _dbOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            _context = new TestDbContext(_dbOptions);
            
            // Setup UserManager with in-memory store
            var userStore = new Mock<IUserStore<User>>();
            var userManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);
            
            // Setup UserManager to use our test context
            userManager.Setup(x => x.Users).Returns(_context.Users);
            userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .Returns<User>(user => Task.FromResult(GetUserRoles(user.Id)));
            
            _userManager = userManager.Object;
            _service = new UserManagmentIdentityService(_userManager, _mockLogger.Object);
        }

        #region FindActiveUserWithOnlyRoleAsync Tests

        [Fact]
        public async Task FindActiveUserWithOnlyRoleAsync_WithNoUsersHavingRole_ShouldReturnNull()
        {
            // Given
            await SeedTestUsersAsync();
            
            // When
            var result = await _service.FindActiveUserWithOnlyRoleAsync("Non-Existent Role");
            
            // Then
            result.Should().BeNull();
        }

        [Fact]
        public async Task FindActiveUserWithOnlyRoleAsync_WithUserHavingOnlyTargetRole_ShouldReturnUser()
        {
            // Given
            await SeedTestUsersAsync();
            var targetUser = _context.Users.First(u => u.Id == 1);
            
            // When
            var result = await _service.FindActiveUserWithOnlyRoleAsync("Legal Counsel");
            
            // Then
            result.Should().NotBeNull();
            result.Id.Should().Be(targetUser.Id);
            result.FullName.Should().Be(targetUser.FullName);
        }

        [Fact]
        public async Task FindActiveUserWithOnlyRoleAsync_WithUserHavingMultipleRoles_ShouldReturnNull()
        {
            // Given
            await SeedTestUsersAsync();
            
            // When - User 3 has both "Board Member" and "Finance Controller" roles
            var result = await _service.FindActiveUserWithOnlyRoleAsync("Finance Controller");
            
            // Then
            result.Should().BeNull(); // Should not return user with multiple roles
        }

        [Fact]
        public async Task FindActiveUserWithOnlyRoleAsync_WithExcludedUser_ShouldExcludeFromResults()
        {
            // Given
            await SeedTestUsersAsync();
            
            // When - Exclude user 1 who has only "Legal Counsel" role
            var result = await _service.FindActiveUserWithOnlyRoleAsync("Legal Counsel", excludeUserId: 1);
            
            // Then
            result.Should().BeNull(); // Should exclude user 1 from results
        }

        [Fact]
        public async Task FindActiveUserWithOnlyRoleAsync_WithInactiveUser_ShouldNotReturnInactiveUser()
        {
            // Given
            await SeedTestUsersAsync();
            
            // When - User 4 is inactive but has only "Head of Real Estate" role
            var result = await _service.FindActiveUserWithOnlyRoleAsync("Head of Real Estate");
            
            // Then
            result.Should().BeNull(); // Should not return inactive user
        }

        [Fact]
        public async Task FindActiveUserWithOnlyRoleAsync_WithDeletedUser_ShouldNotReturnDeletedUser()
        {
            // Given
            await SeedTestUsersAsync();
            
            // When - User 5 is deleted but has only "Compliance and Legal Managing Director" role
            var result = await _service.FindActiveUserWithOnlyRoleAsync("Compliance and Legal Managing Director");
            
            // Then
            result.Should().BeNull(); // Should not return deleted user
        }

        #endregion

        #region UserHasMultipleRolesAsync Tests

        [Fact]
        public async Task UserHasMultipleRolesAsync_WithUserHavingMultipleRoles_ShouldReturnTrue()
        {
            // Given
            await SeedTestUsersAsync();
            
            // When - User 3 has multiple roles
            var result = await _service.UserHasMultipleRolesAsync(3);
            
            // Then
            result.Should().BeTrue();
        }

        [Fact]
        public async Task UserHasMultipleRolesAsync_WithUserHavingSingleRole_ShouldReturnFalse()
        {
            // Given
            await SeedTestUsersAsync();
            
            // When - User 1 has only one role
            var result = await _service.UserHasMultipleRolesAsync(1);
            
            // Then
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UserHasMultipleRolesAsync_WithNonExistentUser_ShouldReturnFalse()
        {
            // Given
            await SeedTestUsersAsync();
            
            // When
            var result = await _service.UserHasMultipleRolesAsync(999);
            
            // Then
            result.Should().BeFalse();
        }

        #endregion

        #region GetUsersWithOnlyRoleAsync Tests

        [Fact]
        public async Task GetUsersWithOnlyRoleAsync_WithMultipleUsersHavingOnlyRole_ShouldReturnAllUsers()
        {
            // Given
            await SeedTestUsersAsync();
            
            // When
            var result = await _service.GetUsersWithOnlyRoleAsync("Board Member");
            
            // Then
            result.Should().HaveCount(1); // Only User 2 has only "Board Member" role
            result.First().Id.Should().Be(2);
        }

        [Fact]
        public async Task GetUsersWithOnlyRoleAsync_WithNoUsersHavingOnlyRole_ShouldReturnEmptyList()
        {
            // Given
            await SeedTestUsersAsync();
            
            // When
            var result = await _service.GetUsersWithOnlyRoleAsync("Non-Existent Role");
            
            // Then
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetUsersWithOnlyRoleAsync_ShouldExcludeInactiveAndDeletedUsers()
        {
            // Given
            await SeedTestUsersAsync();
            
            // When
            var result = await _service.GetUsersWithOnlyRoleAsync("Head of Real Estate");
            
            // Then
            result.Should().BeEmpty(); // User 4 is inactive, so should not be included
        }

        #endregion

        #region Helper Methods

        private async Task SeedTestUsersAsync()
        {
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    UserName = "05123456789",
                    Email = "user1@test.com",
                    FullName = "User One",
                    PhoneNumber = "05123456789",
                    IsActive = true,
                    IsDeleted = false
                },
                new User
                {
                    Id = 2,
                    UserName = "05123456790",
                    Email = "user2@test.com",
                    FullName = "User Two",
                    PhoneNumber = "05123456790",
                    IsActive = true,
                    IsDeleted = false
                },
                new User
                {
                    Id = 3,
                    UserName = "05123456791",
                    Email = "user3@test.com",
                    FullName = "User Three",
                    PhoneNumber = "05123456791",
                    IsActive = true,
                    IsDeleted = false
                },
                new User
                {
                    Id = 4,
                    UserName = "05123456792",
                    Email = "user4@test.com",
                    FullName = "User Four",
                    PhoneNumber = "05123456792",
                    IsActive = false, // Inactive
                    IsDeleted = false
                },
                new User
                {
                    Id = 5,
                    UserName = "05123456793",
                    Email = "user5@test.com",
                    FullName = "User Five",
                    PhoneNumber = "05123456793",
                    IsActive = true,
                    IsDeleted = true // Deleted
                }
            };

            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();
        }

        private IList<string> GetUserRoles(int userId)
        {
            // Mock role assignments for testing
            return userId switch
            {
                1 => new List<string> { "Legal Counsel" }, // Only one role
                2 => new List<string> { "Board Member" }, // Only one role
                3 => new List<string> { "Board Member", "Finance Controller" }, // Multiple roles
                4 => new List<string> { "Head of Real Estate" }, // Only one role but user is inactive
                5 => new List<string> { "Compliance and Legal Managing Director" }, // Only one role but user is deleted
                _ => new List<string>()
            };
        }

        #endregion

        public void Dispose()
        {
            _context?.Dispose();
        }
    }

    // Test DbContext for in-memory testing
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
