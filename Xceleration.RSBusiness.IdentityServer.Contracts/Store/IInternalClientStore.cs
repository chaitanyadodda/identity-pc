using Duende.IdentityServer.Models;

namespace Xceleration.RSBusiness.IdentityServer.Contracts.Store;

public interface IInternalClientStore
{
    Task<Client> GetClientByOrigin(string origin, CancellationToken cancellationToken);
}