using Azure.Identity;
using Microsoft.AspNetCore.DataProtection;
using Xceleration.RSBusiness.IdentityServer.Contracts;
using Xceleration.RSBusiness.IdentityServer.Contracts.Options;

namespace Xceleration.RSBusiness.IdentityServer.Extensions;

public static class DataProtectionExtensions
{
    public static IServiceCollection AddPeopleCartDataProtection(this IServiceCollection services,
        IConfiguration configuration)
    {
        var cred = configuration.GetRequiredSection(nameof(AzureCredentials)).Get<AzureCredentials>();
        var options = configuration.GetRequiredSection(nameof(AzureOptions)).Get<AzureOptions>();
        var credentials = new ClientSecretCredential(cred.Tenant, cred.ClientId, cred.Secret);
        var vaultUri = $"https://{options.VaultName}.vault.azure.net";
        var keyIdentifier = $"{vaultUri}/keys/{options.DataProtection.KeyName}";
        var blobConnectionString =
            configuration.LookupConnectionString(Constants.Database.ConnectionStrings.RewardStationStorage);
        services.AddDataProtection()
            .PersistKeysToAzureBlobStorage(blobConnectionString,
                options.DataProtection.Container, options.DataProtection.BlobName)
            .ProtectKeysWithAzureKeyVault(
                new Uri(keyIdentifier),
                credentials);
        return services;
    }
}