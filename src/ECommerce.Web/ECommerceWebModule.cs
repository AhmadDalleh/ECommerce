//using System;
//using System.IO;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Extensions.DependencyInjection;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using ECommerce.EntityFrameworkCore;
//using ECommerce.Localization;
//using ECommerce.MultiTenancy;
//using ECommerce.Web.Menus;
//using Microsoft.OpenApi.Models;
//using OpenIddict.Validation.AspNetCore;
//using Volo.Abp;
//using Volo.Abp.Account.Web;
//using Volo.Abp.AspNetCore.Mvc;
//using Volo.Abp.AspNetCore.Mvc.Localization;
//using Volo.Abp.AspNetCore.Mvc.UI;
//using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
//using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
//using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
//using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
//using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
//using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
//using Volo.Abp.AspNetCore.Serilog;
//using Volo.Abp.Autofac;
//using Volo.Abp.AutoMapper;
//using Volo.Abp.FeatureManagement;
//using Volo.Abp.Identity.Web;
//using Volo.Abp.Localization;
//using Volo.Abp.Modularity;
//using Volo.Abp.PermissionManagement.Web;
//using Volo.Abp.Security.Claims;
//using Volo.Abp.SettingManagement.Web;
//using Volo.Abp.Swashbuckle;
//using Volo.Abp.TenantManagement.Web;
//using Volo.Abp.OpenIddict;
//using Volo.Abp.UI.Navigation.Urls;
//using Volo.Abp.UI;
//using Volo.Abp.UI.Navigation;
//using Volo.Abp.VirtualFileSystem;
//using Volo.Abp.Identity.Web;
//using Volo.Abp.Account.Web;

//namespace ECommerce.Web;

//[DependsOn(
//    typeof(AbpAspNetCoreMvcUiThemeSharedModule),
//    typeof(ECommerceHttpApiModule),
//    typeof(ECommerceApplicationModule),
//    typeof(ECommerceEntityFrameworkCoreModule),
//    typeof(AbpAutofacModule),
//    typeof(AbpAccountWebIdentityServerModule),
//    typeof(AbpIdentityWebModule),
//    typeof(AbpSettingManagementWebModule),
//    typeof(AbpAccountWebOpenIddictModule),
//    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
//    typeof(AbpTenantManagementWebModule),
//    typeof(AbpAspNetCoreSerilogModule),
//    typeof(AbpSwashbuckleModule)
//    )]
//public class ECommerceWebModule : AbpModule
//{
//    public override void PreConfigureServices(ServiceConfigurationContext context)
//    {
//        var hostingEnvironment = context.Services.GetHostingEnvironment();
//        var configuration = context.Services.GetConfiguration();

//        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
//        {
//            options.AddAssemblyResource(
//                typeof(ECommerceResource),
//                typeof(ECommerceDomainModule).Assembly,
//                typeof(ECommerceDomainSharedModule).Assembly,
//                typeof(ECommerceApplicationModule).Assembly,
//                typeof(ECommerceApplicationContractsModule).Assembly,
//                typeof(ECommerceWebModule).Assembly
//            );
//        });

//        PreConfigure<OpenIddictBuilder>(builder =>
//        {
//            builder.AddValidation(options =>
//            {
//                options.AddAudiences("ECommerce");
//                options.UseLocalServer();
//                options.UseAspNetCore();
//            });
//        });

//        if (!hostingEnvironment.IsDevelopment())
//        {
//            PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
//            {
//                options.AddDevelopmentEncryptionAndSigningCertificate = false;
//            });

//            PreConfigure<OpenIddictServerBuilder>(serverBuilder =>
//            {
//                serverBuilder.AddProductionEncryptionAndSigningCertificate("openiddict.pfx", "4dec57d8-dfd6-405b-b118-225638d7f695");
//            });
//        }
//    }

//    public override void ConfigureServices(ServiceConfigurationContext context)
//    {
//        var hostingEnvironment = context.Services.GetHostingEnvironment();
//        var configuration = context.Services.GetConfiguration();

//        Configure<AbpJwtBearerOptions>(options =>
//        {
//            options.Authority = configuration["AuthServer:Authority"];
//            options.Audience = "ECommerce";
//        });

//        ConfigureAuthentication(context);
//        ConfigureUrls(configuration);
//        ConfigureBundles();
//        ConfigureAutoMapper();
//        ConfigureVirtualFileSystem(hostingEnvironment);
//        ConfigureNavigationServices();
//        ConfigureAutoApiControllers();
//        ConfigureSwaggerServices(context.Services);
//    }

//    private void ConfigureAuthentication(ServiceConfigurationContext context)
//    {
//        //context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
//        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
//        {
//            options.IsDynamicClaimsEnabled = true;
//        });
//    }

//    private void ConfigureUrls(IConfiguration configuration)
//    {
//        Configure<AppUrlOptions>(options =>
//        {
//            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
//        });
//    }

//    private void ConfigureBundles()
//    {
//        Configure<AbpBundlingOptions>(options =>
//        {
//            options.StyleBundles.Configure(
//                LeptonXLiteThemeBundles.Styles.Global,
//                bundle =>
//                {
//                    bundle.AddFiles("/global-styles.css");
//                }
//            );
//        });
//    }

//    private void ConfigureAutoMapper()
//    {
//        Configure<AbpAutoMapperOptions>(options =>
//        {
//            options.AddMaps<ECommerceWebModule>();
//        });
//    }

//    private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
//    {
//        if (hostingEnvironment.IsDevelopment())
//        {
//            Configure<AbpVirtualFileSystemOptions>(options =>
//            {
//                options.FileSets.ReplaceEmbeddedByPhysical<ECommerceDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}ECommerce.Domain.Shared"));
//                options.FileSets.ReplaceEmbeddedByPhysical<ECommerceDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}ECommerce.Domain"));
//                options.FileSets.ReplaceEmbeddedByPhysical<ECommerceApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}ECommerce.Application.Contracts"));
//                options.FileSets.ReplaceEmbeddedByPhysical<ECommerceApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}ECommerce.Application"));
//                options.FileSets.ReplaceEmbeddedByPhysical<ECommerceWebModule>(hostingEnvironment.ContentRootPath);
//            });
//        }
//    }

//    private void ConfigureNavigationServices()
//    {
//        Configure<AbpNavigationOptions>(options =>
//        {
//            options.MenuContributors.Add(new ECommerceMenuContributor());
//        });
//    }

//    private void ConfigureAutoApiControllers()
//    {
//        Configure<AbpAspNetCoreMvcOptions>(options =>
//        {
//            options.ConventionalControllers.Create(typeof(ECommerceApplicationModule).Assembly);
//        });
//    }

//    private void ConfigureSwaggerServices(IServiceCollection services)
//    {
//        services.AddAbpSwaggerGen(
//            options =>
//            {
//                options.SwaggerDoc("v1", new OpenApiInfo { Title = "ECommerce API", Version = "v1" });
//                options.DocInclusionPredicate((docName, description) => true);
//                options.CustomSchemaIds(type => type.FullName);
//                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//                {
//                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
//                    Name = "Authorization",
//                    In = ParameterLocation.Header,
//                    Type = SecuritySchemeType.ApiKey,
//                    Scheme = "Bearer"
//                });
//                options.AddSecurityRequirement(new OpenApiSecurityRequirement
//            {
//                {
//                    new OpenApiSecurityScheme
//                    {
//                        Reference = new OpenApiReference
//                        {
//                            Type = ReferenceType.SecurityScheme,
//                            Id = "Bearer"
//                        }
//                    },
//                    new string[] {}
//                }
//                });
//            }
//        );
//    }

//    public override void OnApplicationInitialization(ApplicationInitializationContext context)
//    {
//        var app = context.GetApplicationBuilder();
//        var env = context.GetEnvironment();

//        if (env.IsDevelopment())
//        {
//            app.UseDeveloperExceptionPage();
//        }

//        app.UseAbpRequestLocalization();

//        if (!env.IsDevelopment())
//        {
//            app.UseErrorPage();
//        }

//        app.UseCorrelationId();
//        app.MapAbpStaticAssets();
//        app.UseRouting();
//        app.UseAuthentication();
//        //app.UseAbpOpenIddictValidation();
//        app.UseAuthorization();
//        if (MultiTenancyConsts.IsEnabled)
//        {
//            app.UseMultiTenancy();
//        }

//        app.UseUnitOfWork();
//        app.UseDynamicClaims();
//        app.UseAuthorization();

//        app.UseSwagger();
//        app.UseAbpSwaggerUI(options =>
//        {
//            options.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce API");
//        });

//        app.UseAuditing();
//        app.UseAbpSerilogEnrichers();
//        app.UseConfiguredEndpoints();
//    }
//}

using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ECommerce.EntityFrameworkCore;
using ECommerce.Localization;
using ECommerce.MultiTenancy;
using ECommerce.Web.Menus;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Web;
using Volo.Abp.Security.Claims;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.Web;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.UI;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ECommerce.Permissions;
using OpenIddict.Validation.AspNetCore;

namespace ECommerce.Web;

[DependsOn(
    typeof(AbpAspNetCoreMvcUiThemeSharedModule),
    typeof(ECommerceHttpApiModule),
    typeof(ECommerceApplicationModule),
    typeof(ECommerceEntityFrameworkCoreModule),
    typeof(AbpAutofacModule),
    typeof(AbpIdentityWebModule),
    typeof(AbpSettingManagementWebModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpTenantManagementWebModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAccountWebOpenIddictModule)
)]
public class ECommerceWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(ECommerceResource),
                typeof(ECommerceDomainModule).Assembly,
                typeof(ECommerceDomainSharedModule).Assembly,
                typeof(ECommerceApplicationModule).Assembly,
                typeof(ECommerceApplicationContractsModule).Assembly,
                typeof(ECommerceWebModule).Assembly
            );
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureUrls(configuration);
        ConfigureAuthentication(context);
        ConfigureBundles();
        ConfigureAutoMapper();
        ConfigureVirtualFileSystem(hostingEnvironment);
        ConfigureNavigationServices();
        ConfigureAutoApiControllers();
        ConfigureSwaggerServices(context.Services);
        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/ECommerce/Index", ECommercePermissions.Customers.Default);
            options.Conventions.AuthorizePage("/ECommerce/CreateModal", ECommercePermissions.Customers.Manage);
            options.Conventions.AuthorizePage("/ECommerce/EditModal", ECommercePermissions.Customers.Manage);
        });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        //var configuration = context.Services.GetConfiguration();
        //var key = configuration["Jwt:SecurityKey"];
        //var issuer = configuration["Jwt:Issuer"];
        //var audience = configuration["Jwt:Audience"];

        //context.Services.AddAuthentication("Bearer")
        //    .AddJwtBearer("Bearer", options =>
        //    {
        //        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        //        {
        //            ValidateIssuer = true,
        //            ValidateAudience = true,
        //            ValidateLifetime = true,
        //            ValidateIssuerSigningKey = true,
        //            ValidIssuer = issuer,
        //            ValidAudience = audience,
        //            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
        //                System.Text.Encoding.UTF8.GetBytes(key))
        //        };
        //    });
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);

        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
        });
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            
            // CSS Bundle (you already have)
            options.StyleBundles.Configure(
                LeptonXLiteThemeBundles.Styles.Global,
                bundle => bundle.AddFiles("/global-styles.css")
            );
        });
    }

    private void ConfigureAutoMapper()
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<ECommerceWebModule>();
        });
    }

    private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
    {
        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<ECommerceDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}ECommerce.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<ECommerceDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}ECommerce.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<ECommerceApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}ECommerce.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<ECommerceApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}ECommerce.Application"));
                options.FileSets.ReplaceEmbeddedByPhysical<ECommerceWebModule>(hostingEnvironment.ContentRootPath);
            });
        }
    }

    private void ConfigureNavigationServices()
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new ECommerceMenuContributor());
        });
    }

    private void ConfigureAutoApiControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(ECommerceApplicationModule).Assembly);
        });
    }

    private void ConfigureSwaggerServices(IServiceCollection services)
    {
        services.AddAbpSwaggerGen(options =>
        {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "ECommerce API", Version = "v1" });
        options.DocInclusionPredicate((docName, description) => true);
        options.CustomSchemaIds(type => type.FullName);
        //    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        //    {
        //        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        //        Name = "Authorization",
        //        In = ParameterLocation.Header,
        //        Type = SecuritySchemeType.ApiKey,
        //        Scheme = "Bearer"
        //    });
        //    options.AddSecurityRequirement(new OpenApiSecurityRequirement
        //    {
        //        {
        //            new OpenApiSecurityScheme
        //            {
        //                Reference = new OpenApiReference
        //                {
        //                    Type = ReferenceType.SecurityScheme,
        //                    Id = "Bearer"
        //                }
        //            },
        //            Array.Empty<string>()
        //        }
        //    });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.MapAbpStaticAssets();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseDynamicClaims();

        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce API");
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
