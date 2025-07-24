
using Abstraction.Constants;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Infrastructure.Data.Config
{
    public static class UserConfig
    {
        public static async Task SeedBasicUserAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            //Seed Default User
            var defaultUser = new User
            {
                UserName = "basicuser",
                Email = "basicuser@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                FullName = "Basic User",
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                }
            }
        }

        public static async Task SeedSuperAdminAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            //Seed Default User
            var defaultUser = new User
            {
                UserName = "superadmin",
                Email = "superadmin@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                FullName = "System Admin"
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
                }
                await roleManager.SeedClaimsForSuperAdmin();
            }
        }

        private async static Task SeedClaimsForSuperAdmin(this RoleManager<Role> roleManager)
        {
            var role = await roleManager.FindByNameAsync("SuperAdmin");
            var claims = await roleManager.GetClaimsAsync(role);

            foreach (var permission in Claims.GenerateAdminModules())
            {
                if (!claims.Any(c => c.Type == CustomClaimTypes.Permission && c.Value == permission))
                    await roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, permission));
            }
        }

        private async static Task SeedClaimsForFundManager(this RoleManager<Role> roleManager)
        {
            var role = await roleManager.FindByNameAsync("FundManager");
            var claims = await roleManager.GetClaimsAsync(role);
            foreach (var permission in Claims.GenerateFundManagerModules())
            {
                if (!claims.Any(c => c.Type == CustomClaimTypes.Permission && c.Value == permission))
                    await roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, permission));
            }
        }
        private async static Task SeedClaimsForBoardSecertary(this RoleManager<Role> roleManager)
        {
            var role = await roleManager.FindByNameAsync("BoardSecretary");
            var claims = await roleManager.GetClaimsAsync(role);
            foreach (var permission in Claims.GenerateBoardSecertaryModules())
            {
                if (!claims.Any(c => c.Type == CustomClaimTypes.Permission && c.Value == permission))
                    await roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, permission));
            }
        }

        private async static Task SeedClaimsForLegal(this RoleManager<Role> roleManager)
        {
            var role = await roleManager.FindByNameAsync("LegalCouncil");
            var claims = await roleManager.GetClaimsAsync(role);
            foreach (var permission in Claims.GenerateLegalCouncilModules())
            {
                if (!claims.Any(c => c.Type == CustomClaimTypes.Permission && c.Value == permission))
                    await roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, permission));
            }
        }

        public static async Task AddPermissionClaim(this RoleManager<Role> roleManager, Role role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Claims.GeneratePermissions(module);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == CustomClaimTypes.Permission && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, permission));
                }
            }
        }

        public static async Task SeedFundManagerAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            //Seed Fund Manager User
            var fundManagers = new List<User>
            {
                new User
                {
                    UserName = "omniafundmanager",
                    Email = "omniafundmanager@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    FullName = "امنية شوقي محمد"
                },
                new User
                {
                    UserName = "omniatwofundmanager",
                    Email = "omniatwofundmanager@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    FullName = "ابراهيم محمد محمد"
                },
                new User
                {
                    UserName = "firstfundmanager",
                    Email = "firstfundmanager@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    FullName = "مدير الصندوق 1"
                },
                new User
                {
                    UserName = "secondfundmanager",
                    Email = "secondfundmanager@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    FullName = "مدير الصندوق 2"
                },
                new User
                {
                    UserName = "thirdfundmanager",
                    Email = "thirdfundmanager@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    FullName = "مدير الصندوق 3"
                },
                new User
                {
                    UserName = "fourthfundmanager",
                    Email = "fourthfundmanager@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    FullName = "مدير الصندوق 4"
                },
                new User
                {
                    UserName = "fivefundmanager",
                    Email = "fivefundmanager@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    FullName = "مدير الصندوق 5"
                },
                new User
                {
                    UserName = "sixfundmanager",
                    Email = "sixfundmanager@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    FullName ="مدير الصندوق 6"
                },
            };

            foreach (var fundManager in fundManagers)
            {
                if (userManager.Users.All(u => u.Id != fundManager.Id))
                {
                    var user = await userManager.FindByEmailAsync(fundManager.Email);
                    if (user == null)
                    {
                        await userManager.CreateAsync(fundManager, "123Pa$$word!");
                        await userManager.AddToRoleAsync(fundManager, Roles.FundManager.ToString());
                    }
                }
            }
            await roleManager.SeedClaimsForFundManager();

        }

        public static async Task SeedLegalCouncilAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            //Seed Legal Council User
            var legalCouncil = new User
            {
                UserName = "legalcouncil",
                Email = "legalcouncil@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                FullName = "المستشار القانوني"
            };
            if (userManager.Users.All(u => u.Id != legalCouncil.Id))
            {
                var user = await userManager.FindByEmailAsync(legalCouncil.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(legalCouncil, "123Pa$$word!");
                    await userManager.AddToRoleAsync(legalCouncil, Roles.LegalCouncil.ToString());
                }
            }
            await roleManager.SeedClaimsForLegal();

        }

        public static async Task SeedBoardSecretaryAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            //Seed Board Secretary User
            var boardSecretaries = new List<User>
            {
                new User
                {
                    UserName = "omniaboardsecretary",
                    Email = "omniaboardsecretary@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    FullName = "Omnia Board Secretary"
                },
                new User
                {
                    UserName = "omniatwoboardsecretary",
                    Email = "omniatwoboardsecretary@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    FullName = "Omnia Two Board Secretary"
                },
                new User
                {
                    UserName = "firstboardsecretary",
                    Email = "firstboardsecretary@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    FullName = "سكرتير المجلس 1"
                },
                new User
                {
                    UserName = "secondboardsecretary",
                    Email = "secondboardsecretary@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    FullName = "سكرتير المجلس 2"
                },
                new User
                {
                    UserName = "thirdboardsecretary",
                    Email = "thirdboardsecretary@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    FullName = "سكرتير المجلس 3"
                },
                new User
                {
                    UserName = "fourthboardsecretary",
                    Email = "fourthboardsecretary@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    FullName = "سكرتير المجلس 4"
                },
                new User
                {
                    UserName = "fiveboardsecretary",
                    Email = "fiveboardsecretary@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    FullName = "سكرتير المجلس 5"
                }
            };
            foreach (var boardSecretary in boardSecretaries)
            {
                if (userManager.Users.All(u => u.Id != boardSecretary.Id))
                {
                    var user = await userManager.FindByEmailAsync(boardSecretary.Email);
                    if (user == null)
                    {
                        await userManager.CreateAsync(boardSecretary, "123Pa$$word!");
                        await userManager.AddToRoleAsync(boardSecretary, Roles.BoardSecretary.ToString());
                    }
                }
            }
            await roleManager.SeedClaimsForBoardSecertary();
        }
    }
}