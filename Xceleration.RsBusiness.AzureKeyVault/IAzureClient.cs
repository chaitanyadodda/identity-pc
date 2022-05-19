using Azure.Security.KeyVault.Secrets;

namespace Xceleration.RsBusiness.AzureKeyVault;

public interface IAzureClient
{
    Task<(bool canGetSecret, KeyVaultSecret secret)> TryGetSecret(string name, string version = null,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<KeyVaultSecret>> GetAllSecrets(string startsWith = null,
        CancellationToken cancellationToken = default);
}