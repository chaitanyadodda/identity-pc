using Microsoft.Extensions.Options;
using RewardStation.Utility.KeyVault;
using Xceleration.RSBusiness.IdentityServer.Contracts.Options;

namespace Xceleration.RSBusiness.IdentityServer.Domain.AzureAd;

public class ConfigureRsKeyVaultServiceOptions : IConfigureOptions<RSKeyVaultServiceOptions>
{
    private readonly AzureOptions _azureOptions;
    private readonly AzureCredentials _cred;

    public ConfigureRsKeyVaultServiceOptions(AzureCredentials cred, AzureOptions azureOptions)
    {
        _cred = cred;
        _azureOptions = azureOptions;
    }

    public void Configure(RSKeyVaultServiceOptions options)
    {
        options.ClientID = _cred.ClientId;
        options.Secret = _cred.Secret;
        options.TenantID = _cred.Tenant;
        options.VaultUrl = $"https://{_azureOptions.VaultName}.vault.azure.net";
    }
}