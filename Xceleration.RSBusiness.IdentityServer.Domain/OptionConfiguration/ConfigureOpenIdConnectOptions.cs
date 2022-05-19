using Duende.IdentityServer;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Xceleration.RSBusiness.IdentityServer.Contracts.ExternalProviders;

namespace Xceleration.RSBusiness.IdentityServer.Domain.OptionConfiguration;

public class ConfigureOpenIdConnectOptions : IConfigureNamedOptions<OpenIdConnectOptions>
{
    private const string ResponseType = "id_token";
    private readonly IHostEnvironment _environment;
    private readonly IIdentityProviderStore _store;

    public ConfigureOpenIdConnectOptions(IIdentityProviderStore store,
        IHostEnvironment environment)
    {
        _store = store;
        _environment = environment;
    }

    public void Configure(OpenIdConnectOptions options)
    {
    }

    public void Configure(string name, OpenIdConnectOptions options)
    {
        var provider = _store.GetBySchemeAsync(name).ConfigureAwait(false).GetAwaiter().GetResult();
        if (provider == null) return;
        var pcOidcProvider = new PcOidcProvider(provider);
        SetupOidcConnection(options, pcOidcProvider);
    }


    #region Private Methods

    private void SetupOidcConnection(OpenIdConnectOptions options, PcOidcProvider oidcProvider)
    {
        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
        options.SignOutScheme = IdentityServerConstants.SignoutScheme;
        options.ClientId = oidcProvider.ClientId.Trim();
        options.Authority = oidcProvider.Authority.Trim();
        options.SaveTokens = true;
        if (!string.IsNullOrWhiteSpace(oidcProvider.ClientSecret))
            options.ClientSecret = oidcProvider.ClientSecret.Trim();

        if (!string.IsNullOrWhiteSpace(oidcProvider.CallbackPath))
            options.CallbackPath = oidcProvider.CallbackPath.Trim();

        if (!string.IsNullOrWhiteSpace(oidcProvider.SignedOutCallbackPath))
            options.SignedOutCallbackPath = oidcProvider.SignedOutCallbackPath.Trim();

        if (oidcProvider.Scopes != null && oidcProvider.Scopes.Any())
        {
            foreach (var scope in oidcProvider.Scopes) options.Scope.Add(scope);

            if (!options.Scope.Contains(IdentityServerConstants.StandardScopes.OpenId))
                options.Scope.Add(IdentityServerConstants.StandardScopes.OpenId);
        }

        options.RequireHttpsMetadata = !_environment.IsDevelopment();
        options.ResponseType = ResponseType;
    }

    #endregion Private Methods
}