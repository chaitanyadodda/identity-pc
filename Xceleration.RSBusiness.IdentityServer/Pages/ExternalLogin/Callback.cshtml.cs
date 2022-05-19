using System.Security.Claims;
using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Xceleration.RSBusiness.IdentityServer.Contracts.Queries;

namespace Xceleration.RSBusiness.IdentityServer.Pages.ExternalLogin;

[AllowAnonymous]
[SecurityHeaders]
public class Callback : PageModel
{
    private readonly IEventService _events;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly ILogger<Callback> _logger;
    private readonly IMediator _mediator;

    public Callback(
        IIdentityServerInteractionService interaction,
        IEventService events,
        ILogger<Callback> logger,
        IMediator mediator)
    {
        _interaction = interaction;
        _logger = logger;
        _mediator = mediator;
        _events = events;
    }

    public async Task<IActionResult> OnGet()
    {
        // read external identity from the temporary cookie
        var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
        // retrieve return URL
        var returnUrl = result.Properties?.Items["returnUrl"] ?? "~/";
        // check if external login is in the context of an OIDC request
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

        if (result.Succeeded != true)
        {
            _logger.LogError(result.Failure, "External authentication error");
            await _events.RaiseAsync(new UserLoginFailureEvent(string.Empty, "invalid credentials"));
            await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);
            return Redirect(returnUrl);
        }

        var externalUser = result.Principal;

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            var externalClaims = externalUser.Claims.Select(c => $"{c.Type}: {c.Value}");
            _logger.LogDebug("External claims: {@claims}", externalClaims);
        }

        var userIdClaim = GetUserIdClaim(externalUser);

        var provider = result.Properties.Items["scheme"];
        var subject = userIdClaim.Value;

        // find external user
        var user = await _mediator.Send(new GetUserByUserName(context.Client.ClientId, subject));
        if (user == null)
        {
            _logger.LogError(
                "Unable to locate user with subject - {subject} from Provider {provider}, clientId = {clientId}, claims = {claims}",
                subject, provider, context.Client.ClientId, result.Principal?.Claims?.ToList());

            await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied,
                "Unable to locate user");

            return Redirect(returnUrl);
        }

        // this allows us to collect any additional claims or properties
        // for the specific protocols used and store them in the local auth cookie.
        // this is typically used to store data needed for signout from those protocols.
        var additionalLocalClaims = new List<Claim>();
        var localSignInProps = new AuthenticationProperties();
        CaptureExternalLoginContext(result, additionalLocalClaims, localSignInProps);

        // issue authentication cookie for user
        var isuser = new IdentityServerUser(user.SubjectId)
        {
            DisplayName = user.SubjectId,
            IdentityProvider = provider,
            AdditionalClaims = additionalLocalClaims
        };

        await HttpContext.SignInAsync(isuser, localSignInProps);

        // delete temporary cookie used during external authentication
        await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);


        await _events.RaiseAsync(new UserLoginSuccessEvent(provider, subject, user.SubjectId, user.SubjectId,
            true, context?.Client.ClientId));

        if (context != null)
            if (context.IsNativeClient())
                // The client is native, so this change in how to
                // return the response is for better UX for the end user.
                return this.LoadingPage(returnUrl);

        return Redirect(returnUrl);
    }

    #region Private Methods

    private static Claim GetUserIdClaim(ClaimsPrincipal externalUser)
    {
        // lookup our user and external provider info
        // try to determine the unique id of the external user (issued by the provider)
        // the most common claim type for that are the sub claim and the NameIdentifier
        // depending on the external provider, some other claim type might be used

        var userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject) ??
                          externalUser.FindFirst(ClaimTypes.Name) ??
                          externalUser.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim;
    }

    // if the external login is OIDC-based, there are certain things we need to preserve to make logout work
    // this will be different for WS-Fed, SAML2p or other protocols
    private void CaptureExternalLoginContext(AuthenticateResult externalResult, List<Claim> localClaims,
        AuthenticationProperties localSignInProps)
    {
        // if the external system sent a session id claim, copy it over
        // so we can use it for single sign-out
        var sid = externalResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
        if (sid != null) localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));

        // if the external provider issued an id_token, we'll keep it for signout
        var idToken = externalResult.Properties.GetTokenValue("id_token");
        if (idToken != null)
            localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = idToken } });
    }

    #endregion Private Methods
}