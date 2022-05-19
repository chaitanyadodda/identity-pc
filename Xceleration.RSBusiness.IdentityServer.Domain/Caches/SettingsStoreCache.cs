using Duende.IdentityServer.Services;
using Xceleration.RSBusiness.IdentityServer.Contracts.Dtos;
using Xceleration.RSBusiness.IdentityServer.Contracts.Options;
using Xceleration.RSBusiness.IdentityServer.Contracts.Store;

namespace Xceleration.RSBusiness.IdentityServer.Domain.Caches;

public class SettingsStoreCache : ISettingsStore
{
    private readonly ICache<SettingsDto> _cache;
    private readonly ISettingsStore _innerStore;
    private readonly DefaultOptions _options;

    public SettingsStoreCache(ICache<SettingsDto> cache, ISettingsStore innerStore, DefaultOptions options)
    {
        _cache = cache;
        _innerStore = innerStore;
        _options = options;
    }

    public async Task<SettingsDto> GetSettingsByClientId(string clientId, CancellationToken cancellationToken)
    {
        var key = $"{nameof(GetSettingsByClientId)}_{clientId}";
        var user = await _cache.GetOrAddAsync(key, _options.CacheTimeSpan,
            () => _innerStore.GetSettingsByClientId(clientId, cancellationToken));
        return user;
    }
}