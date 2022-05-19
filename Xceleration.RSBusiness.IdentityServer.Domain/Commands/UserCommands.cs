using MediatR;
using Xceleration.RSBusiness.IdentityServer.Contracts.Commands;
using Xceleration.RSBusiness.IdentityServer.Contracts.Dtos;
using Xceleration.RSBusiness.IdentityServer.Contracts.Services;
using Xceleration.RSBusiness.IdentityServer.Contracts.Store;

namespace Xceleration.RSBusiness.IdentityServer.Domain.Commands;

public class UserCommands :
    IRequestHandler<ValidateUser, bool>,
    IRequestHandler<IsActiveUser, bool>
{
    private readonly IPasswordValidatorService _passwordValidator;
    private readonly ISettingsStore _settingsStore;
    private readonly IUserStore _userStore;

    public UserCommands(IUserStore userStore, ISettingsStore settingsStore,
        IPasswordValidatorService passwordValidator)
    {
        _userStore = userStore;
        _settingsStore = settingsStore;
        _passwordValidator = passwordValidator;
    }

    public async Task<bool> Handle(IsActiveUser request, CancellationToken cancellationToken)
    {
        var user = await GetUser(request.ClientId, request.SubjectId, request.CorporateAccountId, cancellationToken);
        return user != null;
    }

    public async Task<bool> Handle(ValidateUser request, CancellationToken cancellationToken)
    {
        var user = await GetUser(request.ClientId, request.UserName, null, cancellationToken);
        return user != null && await _passwordValidator.IsPasswordValid(request.Password, user.Password);
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

    #endregion Private Methods
}