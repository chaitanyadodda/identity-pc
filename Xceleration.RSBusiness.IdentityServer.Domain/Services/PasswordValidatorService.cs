using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Xceleration.RSBusiness.IdentityServer.Contracts.Services;

namespace Xceleration.RSBusiness.IdentityServer.Domain.Services;

public class PasswordValidatorService : IPasswordValidatorService
{
    private const string AesPassword = "!Aufrt#3819tcyDau";
    private readonly byte[] _byteSaltBytes = { 22, 15, 23, 01, 23, 98, 53, 41, 183, 188, 167, 189 };
    private readonly ILogger<PasswordValidatorService> _logger;

    public PasswordValidatorService(ILogger<PasswordValidatorService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> IsPasswordValid(string password, string storedPassword)
    {
        if (string.IsNullOrWhiteSpace(password)) return false;

        var encryptedPassword = await ComputeHash(password);
        return encryptedPassword == storedPassword;
    }

    #region Private Methods

    private async Task<string> ComputeHash(string password)
    {
        try
        {
            var bytesToEncrypt = Encoding.UTF8.GetBytes(password);
            var passwordBytes = Encoding.UTF8.GetBytes(AesPassword);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            using var ms = new MemoryStream();
            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            var key = new Rfc2898DeriveBytes(passwordBytes, _byteSaltBytes, 1000);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CBC;
            await using var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            await cs.WriteAsync(bytesToEncrypt, 0, bytesToEncrypt.Length);
            cs.Close();
            var encryptedBytes = ms.ToArray();
            return Convert.ToBase64String(encryptedBytes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to encrypt password");
            return string.Empty;
        }
    }

    #endregion Private Methods
}