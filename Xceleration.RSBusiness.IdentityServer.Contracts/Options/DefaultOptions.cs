namespace Xceleration.RSBusiness.IdentityServer.Contracts.Options;

public class DefaultOptions
{
    public TimeSpan CacheTimeSpan { get; set; } = TimeSpan.FromHours(1);
    public TimeSpan UserCacheTimeSpan { get; set; } = TimeSpan.FromMinutes(5);
}