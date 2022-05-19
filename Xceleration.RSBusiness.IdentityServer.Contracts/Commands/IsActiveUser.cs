using MediatR;

namespace Xceleration.RSBusiness.IdentityServer.Contracts.Commands;

public class IsActiveUser : IRequest<bool>
{
    public IsActiveUser(string clientId, string subjectId, int? corporateAccountId)
    {
        ClientId = clientId;
        SubjectId = subjectId;
        CorporateAccountId = corporateAccountId;
    }

    public string ClientId { get; }
    public string SubjectId { get; }
    public int? CorporateAccountId { get; }
}