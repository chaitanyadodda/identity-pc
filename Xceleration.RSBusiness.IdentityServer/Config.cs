using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Xceleration.RSBusiness.IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new[]
            { new ApiScope("api1", "Api1") };

    public static IEnumerable<Client> Clients =>
        new[]
        {
            new Client
            {
                ClientId = "web",
                AllowedGrantTypes = GrantTypes.Implicit,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = { "api1", IdentityServerConstants.StandardScopes.OpenId },
                RedirectUris = { "https://bing.com" }
            }
        };
}