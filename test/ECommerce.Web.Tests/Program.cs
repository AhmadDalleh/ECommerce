using Microsoft.AspNetCore.Builder;
using ECommerce;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();

builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("ECommerce.Web.csproj");
await builder.RunAbpModuleAsync<ECommerceWebTestModule>(applicationName: "ECommerce.Web" );

public partial class Program
{
}
