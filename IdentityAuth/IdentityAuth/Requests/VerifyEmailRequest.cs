using System.ComponentModel.DataAnnotations;

namespace IdentityAuth.Requests
{
    public class VerifyEmailRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; } = string.Empty;
    }
}