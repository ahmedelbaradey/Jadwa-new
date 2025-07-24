using Abstraction.Contracts.Repository;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data.Config
{
    public static class Seed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            var repositoryManager = serviceProvider.GetRequiredService<IRepositoryManager>();
            await RoleConfig.SeedAsync(roleManager);
            await UserConfig.SeedBasicUserAsync(userManager, roleManager);
            await UserConfig.SeedSuperAdminAsync(userManager, roleManager);
            await UserConfig.SeedFundManagerAsync(userManager, roleManager);
            await UserConfig.SeedLegalCouncilAsync(userManager, roleManager);
            await UserConfig.SeedBoardSecretaryAsync(userManager, roleManager);
            await StatusHistoryConfig.SeedStatusHistoryAsync(repositoryManager.StatusHistory);
        }
    }
}
