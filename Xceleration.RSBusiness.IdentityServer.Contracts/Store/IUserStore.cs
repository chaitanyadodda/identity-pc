using Xceleration.RSBusiness.IdentityServer.Contracts.Dtos;

namespace Xceleration.RSBusiness.IdentityServer.Contracts.Store;

public interface IUserStore
{
    Task<UserDto> GetUserByUserId(int corporateAccountId, string userId, CancellationToken cancellationToken);
}