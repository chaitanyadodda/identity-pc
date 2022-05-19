using Microsoft.EntityFrameworkCore;
using Xceleration.RSBusiness.IdentityServer.Contracts;
using Xceleration.RSBusiness.IdentityServer.Contracts.Store;
using Xceleration.RSBusiness.IdentityServer.Domain.Caches;
using Xceleration.RSBusiness.IdentityServer.Stores;
using Xceleration.RSBusiness.IdentityServer.Stores.DbContexts;

namespace Xceleration.RSBusiness.IdentityServer.Extensions;

internal static class StoreExtensions
{
    public static IServiceCollection AddPeopleCartStores(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IUserStore, UserStore>();
        services.AddTransient<IInternalClientStore, InternalClientStore>();
        services.AddTransient<ISettingsStore, SettingsStore>();

        services.AddDbContext<UserDbContext>(o =>
            o.UseSqlServer(configuration.LookupConnectionString(Constants.Database.ConnectionStrings.PeopleCart)));

        services.Decorate<IUserStore, UserStoreCache>();
        services.Decorate<ISettingsStore, SettingsStoreCache>();

        return services;
    }
}