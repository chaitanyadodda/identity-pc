using System.Reflection;
using System.Security.Claims;
using MediatR;
using Xceleration.RSBusiness.IdentityServer.Contracts.Dtos;
using Xceleration.RSBusiness.IdentityServer.Contracts.Queries;
using Xceleration.RSBusiness.IdentityServer.Contracts.Store;
using Xceleration.RSBusiness.IdentityServer.Domain.Extensions;

namespace Xceleration.RSBusiness.IdentityServer.Domain.Queries;

public class UserQueries :
    IRequestHandler<GetUserByUserName, UserDto>,
    IRequestHandler<GetUserProfile, UserProfileDto>
{
    private readonly ISettingsStore _settingsStore;
    private readonly IUserStore _userStore;

    public UserQueries(IUserStore userStore, ISettingsStore settingsStore)
    {
        _userStore = userStore;
        _settingsStore = settingsStore;
    }

    public Task<UserDto> Handle(GetUserByUserName request, CancellationToken cancellationToken)
    {
        return GetUser(request.ClientId, request.UserName, null, cancellationToken);
    }

    public async Task<UserProfileDto> Handle(GetUserProfile request, CancellationToken cancellationToken)
    {
        var user = await GetUser(request.ClientId, request.SubjectName, request.CorporateAccountId, cancellationToken);
        return user == null
            ? new UserProfileDto(Array.Empty<Claim>())
            : new UserProfileDto(ToClaims(user));
    }

    #region Private Methods

    private async Task<UserDto> GetUser(string clientId, string userName, int? corporateAccountId,
        CancellationToken cancellationToken)
    {
        if (!corporateAccountId.HasValue)
        {
            var settings = await _settingsStore.GetSettingsByClientId(clientId, cancellationToken);
            corporateAccountId = settings.OrganizationId;
        }

        return await _userStore.GetUserByUserId(corporateAccountId.Value, userName, cancellationToken);
    }

    private IEnumerable<Claim> ToClaims(UserDto user)
    {
        return user.GetType()
            .GetProperties()
            .Select(pi => CreateClaim(user, pi))
            .Where(c => c != null);
    }

    private Claim CreateClaim(UserDto user, PropertyInfo property)
    {
        var value = property.GetValue(user, null);
        if (value == null) return null;

        var claimValueType = value switch
        {
            int _ => ClaimValueTypes.Integer32,
            long _ => ClaimValueTypes.Integer64,
            bool _ => ClaimValueTypes.Boolean,
            _ => ClaimValueTypes.String
        };

        return new Claim(property.Name.ToCamelCase(), value.ToString() ?? string.Empty, claimValueType);
    }

    #endregion Private Methods
}