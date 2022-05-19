using System.Security.Claims;
using Duende.IdentityServer;
using Duende.IdentityServer.Stores;
using Duende.IdentityServer.Validation;
using IdentityModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Xceleration.RSBusiness.IdentityServer.Contracts.Store;

namespace Xceleration.RSBusiness.IdentityServer.Domain.CustomRequest;

public class CustomRequestValidator : ICustomTokenRequestValidator
{
    private const string CorporateAccountId = "corporateAccountId";

    private readonly IClientStore _clientStore;
    private readonly ILogger<CustomRequestValidator> _logger;
    private readonly ISettingsStore _settingsStore;
    private readonly ITokenValidator _tokenValidator;

    public CustomRequestValidator(ILogger<CustomRequestValidator> logger, ITokenValidator tokenValidator,
        ISettingsStore settingsStore, IClientStore clientStore)
    {
        _logger = logger;
        _tokenValidator = tokenValidator;
        _settingsStore = settingsStore;
        _clientStore = clientStore;
    }

    public async Task ValidateAsync(CustomTokenRequestValidationContext context)
    {
        switch (context.Result.ValidatedRequest.GrantType)
        {
            case OidcConstants.GrantTypes.ClientCredentials:
                await ProcessClientCredentials(context);
                break;
            case OidcConstants.GrantTypes.JwtBearer:
                var assertion = context.Result.ValidatedRequest.Raw.Get(OidcConstants.TokenRequest.Assertion);
                await ProcessToken(context, assertion);
                break;
        }
    }

    #region Private Methods

    private async Task ProcessClientCredentials(CustomTokenRequestValidationContext context,
        CancellationToken token = default)
    {
        var client = context.Result.ValidatedRequest.Client;
        var setting = await _settingsStore.GetSettingsByClientId(client.ClientId, token);
        var corporateAccountId = setting.OrganizationId;
        _logger.LogDebug("Adding the PeopleCart CorporationAccount Id {corporateAccountId} to Client {clientId}",
            corporateAccountId,
            client.ClientId);

        context.Result.ValidatedRequest.ClientClaims.Add(new Claim(CorporateAccountId, corporateAccountId.ToString(),
            ClaimValueTypes.Integer32));
    }

    private async Task ProcessToken(CustomTokenRequestValidationContext context, string token)
    {
        var result = await _tokenValidator.ValidateAccessTokenAsync(token);
        if (!result.IsError)
            try
            {
                var clientId = result.Claims.FirstOrDefault(c => c.Type == OidcConstants.TokenRequest.ClientId)
                    ?.Value;
                var act = new
                {
                    client_id = clientId
                };
                context.Result.ValidatedRequest.ClientClaims.Add(new Claim("act", JsonConvert.SerializeObject(act),
                    IdentityServerConstants.ClaimValueTypes.Json));

                var client = await _clientStore.FindClientByIdAsync(clientId);
                foreach (var clientClaim in client.Claims)
                    context.Result.ValidatedRequest.ClientClaims.Add(new Claim(clientClaim.Type,
                        clientClaim.Value, clientClaim.ValueType));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in {className} error = {errorMessage}", nameof(CustomRequestValidator),
                    e.Message);
            }
    }

    #endregion Private Methods
}