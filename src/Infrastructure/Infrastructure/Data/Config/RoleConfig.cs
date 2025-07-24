
using Abstraction.Constants;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;



namespace Infrastructure.Data.Config
{
    public static class RoleConfig
    {
        public static async Task SeedAsync(RoleManager<Role> roleManager)
        {
            await roleManager.CreateAsync(new Role() { Name = Roles.SuperAdmin.ToString().ToLower() });
            await roleManager.CreateAsync(new Role() { Name = Roles.Admin.ToString().ToLower() });
            await roleManager.CreateAsync(new Role() { Name = Roles.Basic.ToString().ToLower() });
            await roleManager.CreateAsync(new Role() { Name = Roles.FundManager.ToString().ToLower() });
            await roleManager.CreateAsync(new Role() { Name = Roles.LegalCouncil.ToString().ToLower() });
            await roleManager.CreateAsync(new Role() { Name = Roles.BoardSecretary.ToString().ToLower() });
            await roleManager.CreateAsync(new Role() { Name = Roles.BoardMember.ToString().ToLower() });

            await roleManager.CreateAsync(new Role() { Name = Roles.FinanceController.ToString().ToLower() });
            await roleManager.CreateAsync(new Role() { Name = Roles.ComplianceLegalManagingDirector.ToString().ToLower() });
            await roleManager.CreateAsync(new Role() { Name = Roles.HeadOfRealEstate.ToString().ToLower() });
            await roleManager.CreateAsync(new Role() { Name = Roles.AssociatedFundManager.ToString().ToLower() });

        }
    }
}