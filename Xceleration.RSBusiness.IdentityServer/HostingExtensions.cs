using Serilog;
using Xceleration.RSBusiness.IdentityServer.Endpoints;
using Xceleration.RSBusiness.IdentityServer.Extensions;

namespace Xceleration.RSBusiness.IdentityServer;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // uncomment if you want to add a UI
        builder.Services.AddRazorPages();
        builder.Services.AddPeopleCartIdentityServer(builder.Configuration)
            .AddPeopleCartCommon()
            .AddPeopleCartStores(builder.Configuration)
            .AddPeopleCartAuthentication()
            .AddPeopleCartMediatr()
            .AddEndpointDefinitions(typeof(DiscoveryHostEndpoint).Assembly)
            .AddPeopleCartDataProtection(builder.Configuration);

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();
        // uncomment if you want to add a UI
        app.UseStaticFiles();
        app.UseRouting();

        app.UseIdentityServer();

        // uncomment if you want to add a UI
        app.UseAuthorization();
        app.MapRazorPages().RequireAuthorization();
        app.UseEndpointDefinitions();

        return app;
    }
}