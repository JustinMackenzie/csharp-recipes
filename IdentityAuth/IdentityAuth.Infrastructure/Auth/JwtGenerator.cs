using IdentityAuth.Core.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityAuth.Infrastructure.Auth
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly JwtConfiguration _jwtConfiguration;

        public JwtGenerator(IOptions<JwtConfiguration> jwtConfiguration)
        {
            _jwtConfiguration = jwtConfiguration.Value ?? throw new ArgumentNullException(nameof(jwtConfiguration));
        }

        public Jwt GenerateJwt(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret));

            var token = new JwtSecurityToken(
                issuer: _jwtConfiguration.ValidIssuer,
                audience: _jwtConfiguration.ValidAudience,
                expires: DateTime.Now.AddHours(3),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return new Jwt { Expires = token.ValidTo, Token = new JwtSecurityTokenHandler().WriteToken(token) };
        }
    }
}
