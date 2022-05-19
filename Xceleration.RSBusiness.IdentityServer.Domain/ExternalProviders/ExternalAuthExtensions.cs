using Azure.Security.KeyVault.Secrets;
using Xceleration.RsBusiness.AzureKeyVault;

namespace Xceleration.RSBusiness.IdentityServer.Domain.ExternalProviders;

public static class ExternalAuthExtensions
{
    private const string ExternalAuthsKey = "ExternalAuths-";

    public static string ExtractScheme(this KeyVaultSecret secret)
    {
        return secret.Name.Replace($"{ExternalAuthsKey}", string.Empty);
    }

    public static string GetSecretName(this string scheme)
    {
        return $"{ExternalAuthsKey}{scheme}";
    }

    public static Task<IEnumerable<KeyVaultSecret>> GetAllExternalAuths(this IAzureClient client)
    {
        return client.GetAllSecrets(ExternalAuthsKey);
    }
}