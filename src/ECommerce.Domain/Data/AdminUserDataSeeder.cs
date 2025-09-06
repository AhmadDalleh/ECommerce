using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace ECommerce.Data
{
    public class AdminUserDataSeeder : IDataSeedContributor ,ITransientDependency
    {
        private readonly IdentityUserManager _userManager;
        private readonly IdentityRoleManager _roleManager;
        private readonly IPermissionDataSeeder _permissionDataSeeder;
        private readonly IPermissionDefinitionManager _permissionDefinitionManager;

        public AdminUserDataSeeder(IdentityUserManager userManager, IdentityRoleManager roleManager, IPermissionDataSeeder permissionDataSeeder, IPermissionDefinitionManager permissionDefinitionManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _permissionDataSeeder = permissionDataSeeder;
            _permissionDefinitionManager = permissionDefinitionManager;
        }

        [UnitOfWork]
        public async Task SeedAsync(DataSeedContext context)
        {
            // 1. Get admin user
            var adminUser = await _userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                throw new Exception("Admin user not found!");
            }

            // 2. Ensure admin role exists
            var adminRole = await _roleManager.FindByNameAsync("admin");
            if (adminRole == null)
            {
                adminRole = new IdentityRole(Guid.NewGuid(), "admin");
                (await _roleManager.CreateAsync(adminRole)).CheckErrors();
            }

            // 3. Assign role to user
            if (!await _userManager.IsInRoleAsync(adminUser, "admin"))
            {
                (await _userManager.AddToRoleAsync(adminUser, "admin")).CheckErrors();
            }

            // 4. Get ALL permissions
            var allPermissions = (await _permissionDefinitionManager
                .GetPermissionsAsync())
                .Select(p => p.Name)
                .ToArray();

            // 5. Seed permissions for role
            await _permissionDataSeeder.SeedAsync(
                RolePermissionValueProvider.ProviderName,
                "admin",
                allPermissions
            );

            // (Optional) also seed for the user directly
            await _permissionDataSeeder.SeedAsync(
                UserPermissionValueProvider.ProviderName,
                adminUser.Id.ToString(),
                allPermissions
            );
        }
    }
}
