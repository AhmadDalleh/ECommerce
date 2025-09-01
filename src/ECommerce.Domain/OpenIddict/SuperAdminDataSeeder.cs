using AutoFixture;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq; // <-- needed for .Any(...)
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;
using Volo.Abp.Users;

public class SuperAdminDataSeeder : IDataSeedContributor, ITransientDependency
{
    private readonly IIdentityUserRepository _userRepository;
    private readonly IdentityUserManager _userManager;
    private readonly IdentityRoleManager _roleManager;
    private readonly IPermissionManager _permissionManager;

    public SuperAdminDataSeeder(
        IIdentityUserRepository userRepository,
        IdentityUserManager userManager,
        IdentityRoleManager roleManager,
        IPermissionManager permissionManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _roleManager = roleManager;
        _permissionManager = permissionManager;
    }

    [UnitOfWork]
    public async Task SeedAsync(DataSeedContext context)
    {
        // 1) Ensure SuperAdmin role exists
        var superAdminRole = await _roleManager.FindByNameAsync("SuperAdmin");
        if (superAdminRole == null)
        {
            superAdminRole = new IdentityRole(GuidGenerator.Create(), "SuperAdmin", context.TenantId);
            (await _roleManager.CreateAsync(superAdminRole)).CheckErrors();
        }

        // 2) Grant ALL role-compatible permissions to SuperAdmin
        var allPermissions = await _permissionManager.GetAllAsync(
            RolePermissionValueProvider.ProviderName, // provider name = "R"
            superAdminRole.Name                        // provider key = role name
        );

        foreach (var permission in allPermissions)
        {
            // Providers is a collection of PermissionValueProviderInfo, so compare by .Name
            var supportsRoleProvider = permission.Providers != null &&
                                       permission.Providers.Any(p => p.Name == RolePermissionValueProvider.ProviderName);

            if (!supportsRoleProvider)
                continue;

            await _permissionManager.SetForRoleAsync(
                superAdminRole.Name,
                permission.Name,
                true
            );
        }

        // 3) Ensure SuperAdmin user exists
        var superAdminUser = await _userRepository.FindByNormalizedUserNameAsync("SUPERADMIN");
        if (superAdminUser == null)
        {
            superAdminUser = new IdentityUser(
                GuidGenerator.Create(),
                "superadmin",
                "superadmin@ecom.local",
                context.TenantId
            );

            (await _userManager.CreateAsync(superAdminUser, "1q2w3E*")).CheckErrors();

            // Confirm email/phone via domain methods (property setters may be protected)
            superAdminUser.SetEmailConfirmed(true);
            superAdminUser.SetPhoneNumberConfirmed(true);

            await _userManager.UpdateAsync(superAdminUser);
        }

        // 4) Assign user to SuperAdmin role
        if (!await _userManager.IsInRoleAsync(superAdminUser, "SuperAdmin"))
        {
            (await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin")).CheckErrors();
        }
    }
}
