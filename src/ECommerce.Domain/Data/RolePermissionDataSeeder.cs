using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

public class RolePermissionDataSeeder : IDataSeedContributor, ITransientDependency
{
    private readonly IdentityRoleManager _roleManager;
    private readonly IPermissionDataSeeder _permissionDataSeeder;
    private readonly IPermissionDefinitionManager _permissionDefinitionManager;

    public RolePermissionDataSeeder(
        IdentityRoleManager roleManager,
        IPermissionDataSeeder permissionDataSeeder,
        IPermissionDefinitionManager permissionDefinitionManager)
    {
        _roleManager = roleManager;
        _permissionDataSeeder = permissionDataSeeder;
        _permissionDefinitionManager = permissionDefinitionManager;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        // 1. Create roles if they don't exist
        var adminRole = await _roleManager.FindByNameAsync("Admin");
        if (adminRole == null)
        {
            adminRole = new Volo.Abp.Identity.IdentityRole(Guid.NewGuid(), "Admin");
            (await _roleManager.CreateAsync(adminRole)).CheckErrors();
        }

        var customerRole = await _roleManager.FindByNameAsync("Customer");
        if (customerRole == null)
        {
            customerRole = new Volo.Abp.Identity.IdentityRole(Guid.NewGuid(), "Customer");
            (await _roleManager.CreateAsync(customerRole)).CheckErrors();
        }

        var guestRole = await _roleManager.FindByNameAsync("Guest");
        if (guestRole == null)
        {
            guestRole = new Volo.Abp.Identity.IdentityRole(Guid.NewGuid(), "Guest");
            (await _roleManager.CreateAsync(guestRole)).CheckErrors();
        }

        // 2. Get all permissions (names as string)
        var allPermissionNames = (await _permissionDefinitionManager.GetPermissionsAsync())
                                 .Select(p => p.Name)
                                 .ToArray();

        // 3. Seed Admin => all permissions
        await _permissionDataSeeder.SeedAsync(
            RolePermissionValueProvider.ProviderName,
            "Admin",
            allPermissionNames,
            null
        );

        // 4. Customer => limited permissions (strings only)
        await _permissionDataSeeder.SeedAsync(
            RolePermissionValueProvider.ProviderName,
            "Customer",
            new[]
            {
                "ECommerce.Products.Default",
                "ECommerce.Orders.Default",
                "ECommerce.Orders.Manage",
                "ECommerce.Categories.Default"
            },
            null
        );

        // 5. Guest => read-only
        await _permissionDataSeeder.SeedAsync(
            RolePermissionValueProvider.ProviderName,
            "Guest",
            new[]
            {
                "ECommerce.Products.Default",
                "ECommerce.Categories.Default"
            },
            null
        );
    }
}
