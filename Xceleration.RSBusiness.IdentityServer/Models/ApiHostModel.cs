namespace Xceleration.RSBusiness.IdentityServer.Models;

public record ApiHostModel(string ClientId, ICollection<string> ClientScopes, ICollection<string> AllowedGrantTypes);