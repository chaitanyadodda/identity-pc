using FluentValidation;
using Microsoft.Extensions.Options;
using RewardStation.Utility.KeyVault;
using Xceleration.RsBusiness.AzureKeyVault;
using Xceleration.RSBusiness.IdentityServer.Contracts.Options;
using Xceleration.RSBusiness.IdentityServer.Contracts.Services;
using Xceleration.RSBusiness.IdentityServer.Domain.AzureAd;
using Xceleration.RSBusiness.IdentityServer.Domain.Services;
using Xceleration.RSBusiness.IdentityServer.Domain.Validators;
using Xceleration.RSBusiness.IdentityServer.Stores.Profiles;

namespace Xceleration.RSBusiness.IdentityServer.Extensions;

public static class CommonExtensions
{
    public static IServiceCollection AddPeopleCartCommon(this IServiceCollection services)
    {
        services.AddOptions<AzureCredentials>()
            .BindConfiguration(nameof(AzureCredentials));
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<AzureCredentials>>().Value);
        services.AddOptions<AzureOptions>()
            .BindConfiguration(nameof(AzureOptions));
        services.AddOptions<DefaultOptions>()
            .BindConfiguration(nameof(DefaultOptions));

        services.AddSingleton(sp => sp.GetRequiredService<IOptions<AzureOptions>>().Value);
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<DefaultOptions>>().Value);

        services.AddRSKeyVaultService(o => { });
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<RSKeyVaultServiceOptions>>().Value);
        services.ConfigureOptions<ConfigureRsKeyVaultServiceOptions>();
        services.AddTransient<IAzureClient, AzureClient>();
        services.AddTransient<IPasswordValidatorService, PasswordValidatorService>();

        services.AddValidatorsFromAssemblies(new[]
        {
            typeof(JwtBearerSettingsValidator).Assembly
        });

        services.AddAutoMapper((sp, config) => { config.ConstructServicesUsing(sp.GetService); },
            typeof(SettingsProfile));

        return services;
    }
}