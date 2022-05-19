using Duende.IdentityServer.Validation;
using IdentityModel;
using Xceleration.RSBusiness.IdentityServer.Contracts;

namespace Xceleration.RSBusiness.IdentityServer.Domain.Validators;

public class JwtBearerSettings
{
    private readonly ValidatedTokenRequest _request;

    public JwtBearerSettings(ValidatedTokenRequest request)
    {
        _request = request;
    }

    public string Token => _request.Raw.Get(OidcConstants.TokenRequest.Assertion);
    public string TokenUse => _request.Raw.Get(Constants.JwtBearerKeys.RequestTokenUseKey);
    public string Subject { get; set; }
    public string CorporateAccountId { get; set; }
}