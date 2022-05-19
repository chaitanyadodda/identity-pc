using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Xceleration.RSBusiness.IdentityServer.Contracts.Options;

namespace Xceleration.RSBusiness.IdentityServer.Extensions;

public static class ConfigurationExtensions
{
    public static string LookupConnectionString(this IConfiguration configuration, string key)
    {
        var cred = configuration.GetRequiredSection(nameof(AzureCredentials)).Get<AzureCredentials>();
        var options = configuration.GetRequiredSection(nameof(AzureOptions)).Get<AzureOptions>();
        var credentials = new ClientSecretCredential(cred.Tenant, cred.ClientId, cred.Secret);
        var vaultUri = $"https://{options.VaultName}.vault.azure.net";
        var client = new SecretClient(new Uri(vaultUri), credentials);
        return client.GetSecret($"ConnectionStrings-{key}")?.Value?.Value ?? string.Empty;
    }
}