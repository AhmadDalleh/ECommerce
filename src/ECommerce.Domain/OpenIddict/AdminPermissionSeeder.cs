using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Identity;

public class YourDataSeeder : IDataSeedContributor, ITransientDependency
{
    private readonly IPermissionManager _permissionManager;

    public YourDataSeeder(IPermissionManager permissionManager)
    {
        _permissionManager = permissionManager;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        // Define permission names
        var identityPermissions = new[]
        {
            "AbpIdentity.Users.Default",
            "AbpIdentity.Users.Create",
            "AbpIdentity.Users.Update",
            "AbpIdentity.Users.Delete",
            "AbpIdentity.Roles.Default",
            "AbpIdentity.Roles.Create",
            "AbpIdentity.Roles.Update",
            "AbpIdentity.Roles.Delete",
            "AbpIdentity.UserLookup.Default"
        };

        // Assign permissions to "admin" and "superadmin" roles
        foreach (var permission in identityPermissions)
        {
            await _permissionManager.SetAsync(
                permission,
                RolePermissionValueProvider.ProviderName,
                "admin",  // role name
                true
            );

            await _permissionManager.SetAsync(
                permission,
                RolePermissionValueProvider.ProviderName,
                "superadmin",
                true
            );
        }
    }
}
