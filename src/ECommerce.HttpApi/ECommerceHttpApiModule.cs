using Localization.Resources.AbpUi;
using ECommerce.Localization;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Caching;

namespace ECommerce;

[DependsOn(
    typeof(ECommerceApplicationContractsModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpTenantManagementHttpApiModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(AbpSettingManagementHttpApiModule),
    typeof(AbpCachingStackExchangeRedisModule)
    )]
public class ECommerceHttpApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureLocalization();

        Configure<AbpExceptionHandlingOptions>(options =>
        {
            options.SendExceptionsDetailsToClients = true;
        });

        Configure<AbpDistributedCacheOptions>(options =>
        {
            options.KeyPrefix = "ECommerce:"; // keep your cache keys tidy
        });
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ECommerceResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    
    }
}
