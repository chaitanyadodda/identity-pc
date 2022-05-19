using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using FluentValidation;
using IdentityModel;
using Microsoft.Extensions.Logging;
using Xceleration.RSBusiness.IdentityServer.Domain.Extensions;
using Xceleration.RSBusiness.IdentityServer.Domain.Validators;

namespace Xceleration.RSBusiness.IdentityServer.Domain.ExtensionGrants;

public class JwtBearerExtensionGrant : IExtensionGrantValidator
{
    private readonly ILogger<JwtBearerExtensionGrant> _logger;
    private readonly IValidator<JwtBearerSettings> _validator;

    public JwtBearerExtensionGrant(ILogger<JwtBearerExtensionGrant> logger, IValidator<JwtBearerSettings> validator)
    {
        _logger = logger;
        _validator = validator;
    }

    public async Task ValidateAsync(ExtensionGrantValidationContext context)
    {
        try
        {
            var settings = new JwtBearerSettings(context.Request);

            var result = await _validator.ValidateAsync(settings);
            if (result.IsValid)
            {
                var claim = new Claim(nameof(settings.CorporateAccountId).ToCamelCase(), settings.CorporateAccountId,
                    ClaimValueTypes.Integer32);
                context.Result = string.IsNullOrWhiteSpace(settings.Subject)
                    ? new GrantValidationResult()
                    : new GrantValidationResult(settings.Subject, "obo", new[] { claim });
            }
            else
            {
                _logger.LogError("Error for TokenExchange - error = {errorMessage} for client {clientId}",
                    result.ToString(), context.Request.ClientId);
                context.Result =
                    new GrantValidationResult(TokenRequestErrors.UnauthorizedClient, result.ToString());
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error for TokenExchange - error = {errorMessage} for client {clientId}", e.Message,
                context.Request.ClientId);

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
        }
    }

    public string GrantType => OidcConstants.GrantTypes.JwtBearer;
}