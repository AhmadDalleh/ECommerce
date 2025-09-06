namespace ECommerce.Permissions;

public static class ECommercePermissions
{
    public const string GroupName = "ECommerce";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";

    //public static class Roles
    //{
    //    public const string Default = GroupName + ".Roles";
    //    public const string Create = Default + ".Create";
    //    public const string Edit = Default + ".Edit";
    //    public const string Delete = Default + ".Delete";

    //}

    //public static class Users
    //{
    //    public const string Default = GroupName + ".Users";
    //    public const string Create = Default + ".Create";
    //    public const string Edit = Default + ".Edit";
    //    public const string Delete = Default + ".Delete";
    //}



    //public static class Categories
    //{
    //    public const string Default = GroupName + ".Categories";
    //    public const string Create = Default + ".Create";
    //    public const string Edit = Default + ".Edit";
    //    public const string Delete = Default + ".Delete";
    //}


    //public static class Products
    //{
    //    public const string Default = GroupName + ".Products";
    //    public const string Create = Default + ".Create";
    //    public const string Edit = Default + ".Edit";
    //    public const string Delete = Default + ".Delete";
    //}

    //public static class ProductPhotos
    //{
    //    public const string Default = GroupName + ".ProductsPhotos";
    //    public const string Create = Default + ".Create";
    //    public const string Edit = Default + ".Edit";
    //    public const string Delete = Default + ".Delete";
    //}



    //public static class Customers
    //{
    //    public const string Default = GroupName + ".Customers";
    //    public const string Create = Default + ".Create";
    //    public const string Edit = Default + ".Edit";
    //    public const string Delete = Default + ".Delete";
    //}


    //public static class Orders
    //{
    //    public const string Default = GroupName + ".Orders";
    //    public const string Create = Default + ".Create";
    //    public const string Edit = Default + ".Edit";
    //    public const string Delete = Default + ".Delete";
    //}

    public static class Products
    {
        public const string Default = GroupName + ".Products";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Orders
    {
        public const string Default = GroupName + ".Orders";
        public const string Manage = Default + ".Manage";
    }

    public static class Customers
    {
        public const string Default = GroupName + ".Customers";
        public const string Manage = Default + ".Manage";
    }

    public static class Categories
    {
        public const string Default = GroupName + ".Categories";
        public const string Manage = Default + ".Manage";
    }


    public static string[] GetAll()
    {
        return new[]
        {
                Products.Default, Products.Create, Products.Update, Products.Delete,
                Orders.Default, Orders.Manage,
                Customers.Default, Customers.Manage,
                Categories.Default, Categories.Manage
            };
    }
}
