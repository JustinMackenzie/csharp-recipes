using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuth.Core.Auth
{
    public interface IAuthService
    {
        Task ForgotPasswordAsync(string email);
        Task<LoginResult> LoginAsync(string username, string password);
        Task RegisterAsync(RegisterUserRequest request);
        Task ResetPasswordAsync(ResetPasswordRequest request);
        Task VerifyEmailAsync(VerifyEmailRequest request);
    }
}
