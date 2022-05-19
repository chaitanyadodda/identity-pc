using Duende.IdentityServer.Models;

namespace Xceleration.RSBusiness.IdentityServer.Contracts.ExternalProviders;

public class PcOidcProvider : OidcProvider
{
    public PcOidcProvider()
    {
    }

    /// <summary>Ctor</summary>
    public PcOidcProvider(IdentityProvider other)
        : base(other)
    {
    }

    /// <summary>The base address of the OIDC provider.</summary>
    public string CallbackPath
    {
        get => this[nameof(CallbackPath)];
        set => this[nameof(CallbackPath)] = value;
    }

    /// <summary>The response type. Defaults to "id_token".</summary>
    public string SignedOutCallbackPath
    {
        get => this[nameof(SignedOutCallbackPath)];
        set => this[nameof(SignedOutCallbackPath)] = value;
    }
}