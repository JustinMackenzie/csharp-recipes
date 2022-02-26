using System.Security.Claims;

namespace IdentityAuth.Core.Auth
{
    public interface IJwtGenerator
    {
        Jwt GenerateJwt(IEnumerable<Claim> claims);
    }
}
