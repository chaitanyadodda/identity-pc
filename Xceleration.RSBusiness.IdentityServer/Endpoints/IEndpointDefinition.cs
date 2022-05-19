namespace Xceleration.RSBusiness.IdentityServer.Endpoints;

public interface IEndpointDefinition
{
    void DefineEndpoints(WebApplication app);
    void DefineServices(IServiceCollection services);
}