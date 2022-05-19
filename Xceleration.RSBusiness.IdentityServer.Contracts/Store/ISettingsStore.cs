using Xceleration.RSBusiness.IdentityServer.Contracts.Dtos;

namespace Xceleration.RSBusiness.IdentityServer.Contracts.Store;

public interface ISettingsStore
{
    Task<SettingsDto> GetSettingsByClientId(string clientId, CancellationToken cancellationToken);
}