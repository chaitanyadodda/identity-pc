using MediatR;

namespace Xceleration.RSBusiness.IdentityServer.Contracts.Commands;

public class ValidateUser : IRequest<bool>
{
    public ValidateUser(string clientId, string userName, string password)
    {
        ClientId = clientId;
        UserName = userName;
        Password = password;
    }

    public string ClientId { get; }
    public string UserName { get; }
    public string Password { get; }
}