using Duende.IdentityServer.Validation;
using FluentValidation;
using Xceleration.RSBusiness.IdentityServer.Contracts;
using Xceleration.RSBusiness.IdentityServer.Domain.Extensions;

namespace Xceleration.RSBusiness.IdentityServer.Domain.Validators;

public class JwtBearerSettingsValidator : AbstractValidator<JwtBearerSettings>
{
    public JwtBearerSettingsValidator(ITokenValidator tokenValidator)
    {
        RuleFor(m => m.TokenUse).NotEmpty()
            .Equal(Constants.TokenUse.OnBehalfOf, StringComparer.CurrentCultureIgnoreCase);
        RuleFor(m => m.Token).MustAsync(async (settings, token, cancel) =>
            {
                var result = await tokenValidator.ValidateAccessTokenAsync(token);
                if (result.IsError) return false;
                settings.Subject = result.Claims.ClaimValue("sub");
                settings.CorporateAccountId = result.Claims.ClaimValue("corporateAccountId");
                return true;
            }).WithMessage("Invalid Token")
            .DependentRules(() =>
                Transform(m => m.CorporateAccountId, value => int.TryParse(value, out var val) ? (int?)val : null)
                    .NotEmpty()
                    .GreaterThan(0)
                    .WithMessage("{PropertyName} must be greater than 0"));
    }
}