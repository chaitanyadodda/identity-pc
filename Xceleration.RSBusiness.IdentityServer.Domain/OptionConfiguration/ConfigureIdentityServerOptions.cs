using Duende.IdentityServer.Configuration;
using Microsoft.Extensions.Options;
using Xceleration.RSBusiness.IdentityServer.Contracts;
using Xceleration.RSBusiness.IdentityServer.Contracts.Options;

namespace Xceleration.RSBusiness.IdentityServer.Domain.OptionConfiguration;

public class ConfigureIdentityServerOptions : IConfigureOptions<IdentityServerOptions>
{
    private readonly DefaultOptions _options;

    public ConfigureIdentityServerOptions(DefaultOptions options)
    {
        _options = options;
    }

    public void Configure(IdentityServerOptions options)
    {
        // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
        options.EmitStaticAudienceClaim = true;
        options.Discovery.CustomEntries.Add("host_uri",
            $"~{Constants.ProtocolRoutePaths.DiscoveryHost}");
        options.InputLengthRestrictions.Scope = int.MaxValue;
        options.Cors.CorsPaths.Add(Constants.ProtocolRoutePaths.DiscoveryHost);
        options.Caching.IdentityProviderCacheDuration = _options.CacheTimeSpan;
    }
}