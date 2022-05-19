using Duende.IdentityServer;
using Microsoft.Extensions.Primitives;
using Xceleration.RSBusiness.IdentityServer.Contracts;
using Xceleration.RSBusiness.IdentityServer.Contracts.Store;
using Xceleration.RSBusiness.IdentityServer.Models;

namespace Xceleration.RSBusiness.IdentityServer.Endpoints;

public class DiscoveryHostEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet(Constants.ProtocolRoutePaths.DiscoveryHost, GetHostDefinition);
    }

    public void DefineServices(IServiceCollection services)
    {
    }

    #region Internal Methods

    internal async Task<IResult> GetHostDefinition(IHttpContextAccessor contextAccessor,
        IInternalClientStore internalClientStore, ILogger<DiscoveryHostEndpoint> logger,
        CancellationToken cancellationToken = default)
    {
        var context = contextAccessor.HttpContext;
        var origin = GetOriginByOrigin(context) ?? GetOriginByReferer(context);
        if (string.IsNullOrWhiteSpace(origin))
        {
            logger.LogWarning("Unable to determine origin");
            return Results.NotFound();
        }

        var client = await internalClientStore.GetClientByOrigin(origin, cancellationToken);
        if (client == null)
        {
            logger.LogWarning("Discover Api Host: origin {origin} not found.", origin);
            return Results.NotFound();
        }

        var apiHost = new ApiHostModel(client.ClientId, client.AllowedScopes, client.AllowedGrantTypes);

        if (client.AllowOfflineAccess)
            apiHost.ClientScopes.Add(IdentityServerConstants.StandardScopes.OfflineAccess);

        return Results.Ok(apiHost);
    }

    #endregion Internal Methods

    #region Private Methods

    private string GetOriginByOrigin(HttpContext context)
    {
        const string originHeaderKey = "origin";
        context.Request.Headers.TryGetValue(originHeaderKey, out var origins);

        if (StringValues.IsNullOrEmpty(origins) || origins.Count == 0) return null;
        var origin = origins.ToArray()[0]; //get first
        return !string.IsNullOrWhiteSpace(origin) ? origin : null;
    }

    private string GetOriginByReferer(HttpContext context)
    {
        const string refererHeaderKey = "referer";
        context.Request.Headers.TryGetValue(refererHeaderKey, out var referers);

        if (StringValues.IsNullOrEmpty(referers) || referers.Count == 0) return null;
        var referer = referers.ToArray()[0]; //get first referer
        if (string.IsNullOrWhiteSpace(referer)) return null;

        var url = new Uri(referer);
        var origin = $"{url.Scheme}://{url.Host}";

        if (url.Port > 0 && url.Port != 80 && url.Port != 443) origin = $"{origin}:{url.Port}";

        return origin;
    }

    #endregion Private Methods
}