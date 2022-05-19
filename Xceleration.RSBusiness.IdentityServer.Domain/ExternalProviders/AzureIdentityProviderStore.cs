using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using Xceleration.RsBusiness.AzureKeyVault;
using Xceleration.RSBusiness.IdentityServer.Contracts.Dtos;

namespace Xceleration.RSBusiness.IdentityServer.Domain.ExternalProviders;

public class AzureIdentityProviderStore : IIdentityProviderStore
{
    private readonly IAzureClient _azureClient;

    public AzureIdentityProviderStore(IAzureClient azureClient)
    {
        _azureClient = azureClient;
    }

    public async Task<IEnumerable<IdentityProviderName>> GetAllSchemeNamesAsync()
    {
        var external = await _azureClient.GetAllExternalAuths();
        return external.Select(secret =>
        {
            var identity = secret.FromJson<IdentityProviderDto>();
            return new IdentityProviderName
            {
                Scheme = secret.ExtractScheme(),
                DisplayName = identity.DisplayName,
                Enabled = identity.Enabled
            };
        });
    }

    public async Task<IdentityProvider> GetBySchemeAsync(string scheme)
    {
        var (canGet, secret) =
            await _azureClient.TryGetSecret(scheme.GetSecretName());
        if (!canGet) return null;
        var identity = secret.FromJson<IdentityProviderDto>();
        return identity.ConvertTo(scheme);
    }
}