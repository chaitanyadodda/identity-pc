using Duende.IdentityServer.Models;

namespace Xceleration.RSBusiness.IdentityServer.Contracts.Dtos;

public class IdentityProviderDto
{
    public string DisplayName { get; set; }

    public bool Enabled { get; set; } = true;

    public string Type { get; set; }

    public Dictionary<string, string> Properties { get; set; } = new();

    public IdentityProvider ConvertTo(string scheme)
    {
        return new IdentityProvider(Type)
        {
            Enabled = Enabled,
            Properties = Properties,
            Scheme = scheme,
            DisplayName = DisplayName
        };
    }
}