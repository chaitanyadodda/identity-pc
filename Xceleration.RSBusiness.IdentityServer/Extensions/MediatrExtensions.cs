using MediatR;
using Xceleration.RSBusiness.IdentityServer.Domain.Commands;

namespace Xceleration.RSBusiness.IdentityServer.Extensions;

internal static class MediatrExtensions
{
    public static IServiceCollection AddPeopleCartMediatr(this IServiceCollection services)
    {
        services.AddMediatR(typeof(UserCommands).Assembly);
        return services;
    }
}