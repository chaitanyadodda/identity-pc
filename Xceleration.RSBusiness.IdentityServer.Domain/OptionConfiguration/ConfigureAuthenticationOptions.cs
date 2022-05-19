using Duende.IdentityServer.Configuration;
using Microsoft.Extensions.Options;
using Xceleration.RsBusiness.AzureKeyVault;
using Xceleration.RSBusiness.IdentityServer.Contracts.Dtos;
using Xceleration.RSBusiness.IdentityServer.Domain.ExternalProviders;
using AuthenticationOptions = Microsoft.AspNetCore.Authentication.AuthenticationOptions;

namespace Xceleration.RSBusiness.IdentityServer.Domain.OptionConfiguration;

public class ConfigureAuthenticationOptions : IConfigureOptions<AuthenticationOptions>
{
    private readonly IAzureClient _client;
    private readonly DynamicProviderOptions _options;

    public ConfigureAuthenticationOptions(IAzureClient client, DynamicProviderOptions options)
    {
        _client = client;
        _options = options;
    }

    public void Configure(AuthenticationOptions options)
    {
        var secrets = _client.GetAllExternalAuths().ConfigureAwait(false).GetAwaiter().GetResult();
        foreach (var secret in secrets)
        {
            var identity = secret.FromJson<IdentityProviderDto>();
            var providerType = _options.FindProviderType(identity.Type);
            if (providerType != null)
                options.AddScheme(secret.ExtractScheme(),
                    bldr =>
                    {
                        bldr.DisplayName = identity.DisplayName;
                        bldr.HandlerType = providerType.HandlerType;
                    });
        }
    }
}