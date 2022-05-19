using MediatR;
using Xceleration.RSBusiness.IdentityServer.Contracts.Dtos;

namespace Xceleration.RSBusiness.IdentityServer.Contracts.Queries;

public class GetUserByUserName : IRequest<UserDto>
{
    public GetUserByUserName(string clientId, string userName)
    {
        ClientId = clientId;
        UserName = userName;
    }

    public string ClientId { get; }
    public string UserName { get; }
}