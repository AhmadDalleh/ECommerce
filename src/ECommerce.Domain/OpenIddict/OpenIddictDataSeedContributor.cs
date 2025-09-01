using OpenIddict.Abstractions;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.OpenIddict.Applications;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IDataSeedContributor))]
public class OpenIddictClientDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IOpenIddictApplicationManager _applicationManager;

    public OpenIddictClientDataSeedContributor(IOpenIddictApplicationManager applicationManager)
    {
        _applicationManager = applicationManager;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        var clientId = "ECommerce_Swagger";
        var clientSecret = "123qwe!@#";

        var client = await _applicationManager.FindByClientIdAsync(clientId);
        if (client == null)
        {
            await _applicationManager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = clientId,
                ClientSecret = clientSecret, // ABP will hash it automatically
                DisplayName = "Swagger Client",
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.GrantTypes.Password,
                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.Roles,
                    "ECommerce"
                }
            });
        }
    }
}
