using Duende.IdentityServer.Services;
using Xceleration.RSBusiness.IdentityServer.Contracts.Dtos;
using Xceleration.RSBusiness.IdentityServer.Contracts.Options;
using Xceleration.RSBusiness.IdentityServer.Contracts.Store;

namespace Xceleration.RSBusiness.IdentityServer.Domain.Caches;

public class UserStoreCache : IUserStore
{
    private readonly ICache<UserDto> _cache;
    private readonly IUserStore _innerStore;
    private readonly DefaultOptions _options;

    public UserStoreCache(ICache<UserDto> cache, IUserStore innerStore, DefaultOptions options)
    {
        _cache = cache;
        _innerStore = innerStore;
        _options = options;
    }

    public async Task<UserDto> GetUserByUserId(int corporateAccountId, string userId,
        CancellationToken cancellationToken)
    {
        var key = $"{nameof(GetUserByUserId)}_{corporateAccountId}_{userId}";
        var user = await _cache.GetOrAddAsync(key, _options.UserCacheTimeSpan,
            () => _innerStore.GetUserByUserId(corporateAccountId, userId, cancellationToken));
        return user;
    }
}