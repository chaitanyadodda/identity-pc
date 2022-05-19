namespace Xceleration.RSBusiness.IdentityServer.Contracts.Options;

public class AzureOptions
{
    public string VaultName { get; set; }
    public DataProtectionOptions DataProtection { get; set; }
}