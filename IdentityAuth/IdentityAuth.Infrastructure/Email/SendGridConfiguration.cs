using System.Net;

namespace IdentityAuth.Infrastructure.Email
{
    public class SendGridConfiguration
    {
        public string ApiKey { get; set; }
        public string FromName { get; set; }
        public string FromEmailAddress { get; set; }
    }
}