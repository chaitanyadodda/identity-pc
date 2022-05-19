using MediatR;
using Xceleration.RSBusiness.IdentityServer.Contracts.Dtos;

namespace Xceleration.RSBusiness.IdentityServer.Contracts.Queries;

public class GetSettings : IRequest<SettingsDto>
{
    public GetSettings(string clientId)
    {
        ClientId = clientId;
    }

    public string ClientId { get; }
}