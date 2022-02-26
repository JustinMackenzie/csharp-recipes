namespace IdentityAuth.Infrastructure.Auth
{
    public class JwtConfiguration
    {
        public string ValidIssuer { get; internal set; }
        public string ValidAudience { get; internal set; }
        public char[] Secret { get; internal set; }
    }
}