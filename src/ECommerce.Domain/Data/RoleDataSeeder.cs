using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

public class RoleDataSeeder : IDataSeedContributor, ITransientDependency
{
    private readonly IdentityRoleManager _roleManager;

    public RoleDataSeeder(IdentityRoleManager roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await EnsureRoleExistsAsync("Admin");
        await EnsureRoleExistsAsync("Customer");
        await EnsureRoleExistsAsync("Guest");
    }

    private async Task EnsureRoleExistsAsync(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            role = new IdentityRole(Guid.NewGuid(), roleName);
            (await _roleManager.CreateAsync(role)).CheckErrors();
        }
    }
}
