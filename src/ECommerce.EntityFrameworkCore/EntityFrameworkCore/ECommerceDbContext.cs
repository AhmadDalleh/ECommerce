using ECommerce.Catalog;
using ECommerce.Customers;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace ECommerce.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class ECommerceDbContext :
    AbpDbContext<ECommerceDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    #region Customer Entities

    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerPassword> CustomersPassword { get; set; }

    public DbSet<CustomerRole> CustomerRoles { get; set; }
    public DbSet<CustomerCustomerRole> CustomerCustomerRoles { get; set; }

    public DbSet<Address> Addresses { get; set; }

    public DbSet<CustomerAddress> CustomerAddresses { get; set; }

    #endregion

    #region Catalog Entities
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductPhoto> ProductPhotos { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }

    #endregion

    public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(ECommerceConsts.DbTablePrefix + "YourEntities", ECommerceConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});

        #region Customer Tables Configurations
        builder.ApplyConfiguration(new Customers.CustomerEntityTypeConfiguration());
        builder.ApplyConfiguration(new Customers.CustomerPasswordEntityTypeConfiguration());
        builder.ApplyConfiguration(new Customers.CustomerRoleConfiguration());
        builder.ApplyConfiguration(new Customers.CustomerCustomerRoleConfiguration());
        builder.ApplyConfiguration(new Customers.AddressEntityConfiguration());
        builder.ApplyConfiguration(new Customers.CustomerAddressEntityConfiguration());

        #endregion

        #region Catlog Tables Configurations
        builder.ApplyConfiguration(new Catalog.CategoryConfiguration());
        builder.ApplyConfiguration(new Catalog.ProductConfiguration());
        builder.ApplyConfiguration(new Catalog.ProductPhotoConfiguration());
        builder.ApplyConfiguration(new Catalog.ProductCategoryConfiguration());
        #endregion

    }
}
