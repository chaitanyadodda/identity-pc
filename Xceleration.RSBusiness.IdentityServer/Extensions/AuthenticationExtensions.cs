using Xceleration.RSBusiness.IdentityServer.Domain.OptionConfiguration;

namespace Xceleration.RSBusiness.IdentityServer.Extensions;

internal static class AuthenticationExtensions
{
    public static IServiceCollection AddPeopleCartAuthentication(this IServiceCollection services)
    {
        services.ConfigureOptions<ConfigureOpenIdConnectOptions>();
        services.ConfigureOptions<ConfigureSaml2Options>();
        services.ConfigureOptions<ConfigureAuthenticationOptions>();
        return services;
    }
}