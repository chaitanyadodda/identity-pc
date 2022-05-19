using Duende.IdentityServer.Models;

namespace Xceleration.RSBusiness.IdentityServer.Contracts.ExternalProviders;

public class Saml2IdentityProvider : IdentityProvider
{
    public Saml2IdentityProvider() : base("saml2")
    {
    }

    public Saml2IdentityProvider(IdentityProvider other) : base("saml2", other)
    {
    }

    public string ModulePath
    {
        get => this[nameof(ModulePath)];
        set => this[nameof(ModulePath)] = value;
    }

    public string EntityId
    {
        get => this[nameof(EntityId)];
        set => this[nameof(EntityId)] = value;
    }

    public string MinIncomingSigningAlgorithm
    {
        get => this[nameof(MinIncomingSigningAlgorithm)];
        set => this[nameof(MinIncomingSigningAlgorithm)] = value;
    }

    public string IdentityProviderEntityId
    {
        get => this[nameof(IdentityProviderEntityId)];
        set => this[nameof(IdentityProviderEntityId)] = value;
    }

    public string IdentityProviderMetadataLoc
    {
        get => this[nameof(IdentityProviderMetadataLoc)];
        set => this[nameof(IdentityProviderMetadataLoc)] = value;
    }
}