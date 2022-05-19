using Duende.IdentityServer.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Sustainsys.Saml2.AspNetCore2;
using Xceleration.RSBusiness.IdentityServer.Contracts;
using Xceleration.RSBusiness.IdentityServer.Contracts.ExternalProviders;
using Xceleration.RSBusiness.IdentityServer.Domain.CustomRequest;
using Xceleration.RSBusiness.IdentityServer.Domain.ExtensionGrants;
using Xceleration.RSBusiness.IdentityServer.Domain.ExternalProviders;
using Xceleration.RSBusiness.IdentityServer.Domain.OptionConfiguration;
using Xceleration.RSBusiness.IdentityServer.Domain.Services;
using Xceleration.RSBusiness.IdentityServer.Stores.DbContexts;

namespace Xceleration.RSBusiness.IdentityServer.Extensions;

internal static class IdentityServerExtensions
{
    public static IServiceCollection AddPeopleCartIdentityServer(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.LookupConnectionString(Constants.Database.ConnectionStrings.IdentityServer);

        services.AddSingleton<IConfigureOptions<IdentityServerOptions>, ConfigureIdentityServerOptions>();

        services.AddIdentityServer()
            .AddConfigurationStore<PcConfigurationDbContext>(options =>
            {
                options.DefaultSchema = "config";
                options.ConfigureDbContext = b =>
                    b.UseSqlServer(connectionString);
            })
            .AddConfigurationStoreCache()
            .AddOperationalStore(options =>
            {
                options.DefaultSchema = "ids";
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 3600;
                options.RemoveConsumedTokens = true;
                options.ConfigureDbContext = b => b.UseSqlServer(connectionString);
            })
            .AddInMemoryCaching()
            .AddProfileService<ProfileService>()
            .AddIdentityProviderStoreCache<AzureIdentityProviderStore>()
            .AddSaml2DynamicProvider()
            .AddExtensionGrantValidator<JwtBearerExtensionGrant>()
            .AddCustomTokenRequestValidator<CustomRequestValidator>();

        return services;
    }

    #region Private Methods

    private static IIdentityServerBuilder AddSaml2DynamicProvider(this IIdentityServerBuilder builder)
    {
        builder.Services.Configure<IdentityServerOptions>(options =>
        {
            options.DynamicProviders.AddProviderType<Saml2Handler, Saml2Options, Saml2IdentityProvider>("saml2");
        });

        // these are services from ASP.NET Core and are added manually since we're not using the 
        // Saml2 helper that we'd normally use statically on the AddAuthentication.
        builder.Services.TryAddEnumerable(ServiceDescriptor
            .Singleton<IPostConfigureOptions<Saml2Options>, PostConfigureSaml2Options>());
        builder.Services.TryAddTransient<Saml2Handler>();

        return builder;
    }

    #endregion Private Methods
}