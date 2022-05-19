using MediatR;
using Xceleration.RSBusiness.IdentityServer.Contracts.Dtos;

namespace Xceleration.RSBusiness.IdentityServer.Contracts.Queries;

public class GetUserProfile : IRequest<UserProfileDto>
{
    public GetUserProfile(string clientId, string subjectName, int? corporateAccountId)
    {
        ClientId = clientId;
        SubjectName = subjectName;
        CorporateAccountId = corporateAccountId;
    }

    public string ClientId { get; }
    public string SubjectName { get; }
    public int? CorporateAccountId { get; }
}