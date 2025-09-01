using ECommerce.Localization;
using ECommerce.MultiTenancy;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace ECommerce.Web.Menus;

public class ECommerceMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var administration = context.Menu.GetAdministration();
        var l = context.GetLocalizer<ECommerceResource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                ECommerceMenus.Home,
                l["Menu:Home"],
                "~/",
                icon: "fas fa-home",
                order: 0
            )
        );
        context.Menu.AddItem(
    new ApplicationMenuItem(
        "IdentityManagement",
        l["Menu:IdentityManagement"],
        icon: "fas fa-users"
    )
    .AddItem(new ApplicationMenuItem(
        IdentityMenuNames.Users,
        l["Users"],
        url: "/Identity/Users"
    ).RequirePermissions(IdentityPermissions.Users.Default)) // 👈 check AbpIdentity.Users permission
    .AddItem(new ApplicationMenuItem(
        IdentityMenuNames.Roles,
        l["Roles"],
        url: "/Identity/Roles"
    ).RequirePermissions(IdentityPermissions.Roles.Default)) // 👈 check AbpIdentity.Roles permission
);

        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 3);

        return Task.CompletedTask;
    }
}
