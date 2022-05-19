using System.Security.Claims;
using Duende.IdentityServer.Models;

namespace Xceleration.RSBusiness.IdentityServer.Domain.Extensions;

internal static class ProfileDataRequestContextExtensions
{
    public static List<Claim> FilterCaseInsensitiveClaims(
        this ProfileDataRequestContext context,
        IEnumerable<Claim> claims)
    {
        return context == null
            ? throw new ArgumentNullException(nameof(context))
            : claims == null
                ? throw new ArgumentNullException(nameof(claims))
                : claims.Where(x =>
                        context.RequestedClaimTypes.Contains(x.Type, StringComparer.OrdinalIgnoreCase))
                    .ToList();
    }

    public static void AddRequestedClaims(
        this ProfileDataRequestContext context,
        IEnumerable<Claim> claims, bool casInsensitive)
    {
        if (!context.RequestedClaimTypes.Any())
            return;
        context.RequestedClaimTypes = context.RequestedClaimTypes.Select(c => c.Trim()).ToList();
        context.IssuedClaims.AddRange(casInsensitive
            ? context.FilterCaseInsensitiveClaims(claims)
            : context.FilterClaims(claims));
    }
}