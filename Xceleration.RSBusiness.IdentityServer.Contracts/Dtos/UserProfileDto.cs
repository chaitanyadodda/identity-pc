using System.Security.Claims;

namespace Xceleration.RSBusiness.IdentityServer.Contracts.Dtos;

public record UserProfileDto(IEnumerable<Claim> Claims);