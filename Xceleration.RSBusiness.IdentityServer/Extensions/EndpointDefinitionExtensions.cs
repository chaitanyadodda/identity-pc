using System.Reflection;
using Xceleration.RSBusiness.IdentityServer.Endpoints;

namespace Xceleration.RSBusiness.IdentityServer.Extensions;

public static class EndpointDefinitionExtensions
{
    public static IServiceCollection AddEndpointDefinitions(this IServiceCollection services,
        params Assembly[] anchorAssemblies)
    {
        var definitions = anchorAssemblies
            .SelectMany(a => a.ExportedTypes.Where(t =>
                typeof(IEndpointDefinition).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface))
            .Select(Activator.CreateInstance).Cast<IEndpointDefinition>()
            .ToList();

        foreach (var endpointDefinition in definitions) endpointDefinition.DefineServices(services);

        services.AddSingleton((IReadOnlyCollection<IEndpointDefinition>)definitions);
        return services;
    }

    public static WebApplication UseEndpointDefinitions(this WebApplication app)
    {
        var definitions = app.Services.GetRequiredService<IReadOnlyCollection<IEndpointDefinition>>();
        foreach (var endpointDefinition in definitions) endpointDefinition.DefineEndpoints(app);

        return app;
    }
}