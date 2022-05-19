namespace Xceleration.RSBusiness.IdentityServer.Contracts.Services;

public interface IPasswordValidatorService
{
    Task<bool> IsPasswordValid(string password, string storedPassword);
}