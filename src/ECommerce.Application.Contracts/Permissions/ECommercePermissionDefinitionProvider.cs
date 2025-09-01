using ECommerce.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ECommerce.Permissions;

public class ECommercePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var ecommerceGroup = context.AddGroup(ECommercePermissions.GroupName, L("ECommerce"));

        // Products
        var products = ecommerceGroup.AddPermission(ECommercePermissions.Products.Default, L("Permission:Products"));
        products.AddChild(ECommercePermissions.Products.Create, L("Permission:Create"));
        products.AddChild(ECommercePermissions.Products.Update, L("Permission:Update"));
        products.AddChild(ECommercePermissions.Products.Delete, L("Permission:Delete"));

        // Orders
        var orders = ecommerceGroup.AddPermission(ECommercePermissions.Orders.Default, L("Permission:Orders"));
        orders.AddChild(ECommercePermissions.Orders.Manage, L("Permission:Manage"));

        // Customers
        var customers = ecommerceGroup.AddPermission(ECommercePermissions.Customers.Default, L("Permission:Customers"));
        customers.AddChild(ECommercePermissions.Customers.Manage, L("Permission:Manage"));

        // Categories
        var categories = ecommerceGroup.AddPermission(ECommercePermissions.Categories.Default, L("Permission:Categories"));
        categories.AddChild(ECommercePermissions.Categories.Manage, L("Permission:Manage"));

    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ECommerceResource>(name);
    }
}
