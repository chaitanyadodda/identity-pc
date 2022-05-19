using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Logging;
using RewardStation.Utility.KeyVault;

namespace Xceleration.RsBusiness.AzureKeyVault;

public class AzureClient : IAzureClient
{
    private readonly IRSKeyVaultServices _keyVaultServices;
    private readonly ILogger<AzureClient> _logger;
    private readonly RSKeyVaultServiceOptions _options;


    public AzureClient(ILogger<AzureClient> logger, IRSKeyVaultServices keyVaultServices,
        RSKeyVaultServiceOptions options)
    {
        _logger = logger;
        _keyVaultServices = keyVaultServices;
        _options = options;
    }

    public async Task<(bool canGetSecret, KeyVaultSecret secret)> TryGetSecret(string name, string version = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _keyVaultServices.GetRSSecret(name);
            if (string.IsNullOrWhiteSpace(response.Value))
            {
                _logger.LogInformation("Key - {key} not found in vault - {vault}", name, _options.VaultUrl);
                return (false, null);
            }

            if (!(!response.Properties.Enabled ?? false)) return (true, response);
            _logger.LogInformation("Key - {key} not enabled in vault - {vault}", name, _options.VaultUrl);
            return (false, null);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to get secret key - {key} from vault - {vault}", name, _options.VaultUrl);
            return (false, null);
        }
    }

    public async Task<IEnumerable<KeyVaultSecret>> GetAllSecrets(string startsWith = null,
        CancellationToken cancellationToken = default)
    {
        startsWith ??= "ValueNotPassed";

        var names = await _keyVaultServices.GetAllRSSecrets(startsWith, cancellationToken);
        var secrets = new List<KeyVaultSecret>();
        foreach (var name in names)
        {
            var secret = await _keyVaultServices.GetRSSecret(name.Name);
            secrets.Add(secret);
        }

        return secrets;
    }
}