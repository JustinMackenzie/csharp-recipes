using System.ComponentModel.DataAnnotations;

namespace IdentityAuth.Requests
{
    public class ForgotPasswordRequest
    {
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}