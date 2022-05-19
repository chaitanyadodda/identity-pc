using Azure.Security.KeyVault.Secrets;
using Newtonsoft.Json;

namespace Xceleration.RsBusiness.AzureKeyVault;

public static class StringExtensions
{
    public static T FromJson<T>(this KeyVaultSecret secret) where T : class
    {
        var jsonSettings = new JsonSerializerSettings();
        return JsonConvert.DeserializeObject<T>(secret.Value, jsonSettings);
    }
}