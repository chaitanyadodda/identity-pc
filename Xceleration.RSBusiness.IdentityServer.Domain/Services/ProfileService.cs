using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Xceleration.RSBusiness.IdentityServer.Contracts.Commands;
using Xceleration.RSBusiness.IdentityServer.Contracts.Queries;
using Xceleration.RSBusiness.IdentityServer.Domain.Extensions;

namespace Xceleration.RSBusiness.IdentityServer.Domain.Services;

public class ProfileService : IProfileService
{
    private readonly ILogger<ProfileService> _logger;
    private readonly IMediator _mediator;

    public ProfileService(IMediator mediator, ILogger<ProfileService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var subjId = context.Subject.GetSubjectId();
        var corpId = context.Subject.GetCorporateAccountId();
        var profile =
            await _mediator.Send(new GetUserProfile(context.Client.ClientId, subjId, corpId));
        if (profile == null)
        {
            _logger.LogWarning(
                "There aren't any profiles found in {profileService}. Caller - {caller},  ClientId - {clientId}",
                nameof(ProfileService), context.Caller, context.Client.ClientId);
            return;
        }

        context.AddRequestedClaims(profile.Claims, true);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var subjId = context.Subject.GetSubjectId();
        var corpId = context.Subject.GetCorporateAccountId();

        var isActive = await _mediator.Send(new IsActiveUser(context.Client.ClientId, subjId, corpId));
        if (isActive)
        {
            context.IsActive = true;
        }
        else
        {
            _logger.LogWarning("{subject} is not active for {clientId}", context.Subject.GetSubjectId(),
                context.Client.ClientId);
            context.IsActive = false;
        }
    }
}