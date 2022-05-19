using System.Security.Claims;
using System.Security.Principal;

namespace Xceleration.RSBusiness.IdentityServer.Domain.Extensions;

public static class ClaimExtensions
{
    public static string ClaimValue(this IEnumerable<Claim> claims, string key)
    {
        return claims.FirstOrDefault(c => string.Equals(c.Type, key, StringComparison.OrdinalIgnoreCase))?.Value;
    }

    public static int? GetCorporateAccountId(this IPrincipal principal)
    {
        var identity = principal.Identity as ClaimsIdentity;
        var corporateAccountId = identity?.Claims.ClaimValue("corporateAccountId");
        return int.TryParse(corporateAccountId, out var id) ? id : null;
    }
}