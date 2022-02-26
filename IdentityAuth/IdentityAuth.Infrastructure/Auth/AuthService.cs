using IdentityAuth.Core;
using IdentityAuth.Core.Auth;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityAuth.Infrastructure.Auth
{
    public class AuthService : IAuthService
    {
        private const string ResetPasswordTemplateKey = @"ResetPassword";
        private const string VerifyEmailTemplateKey = @"VerifyEmail";

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IEmailService _emailService;

        public AuthService(UserManager<IdentityUser> userManager, IJwtGenerator jwtGenerator, IEmailService emailService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _jwtGenerator = jwtGenerator ?? throw new ArgumentNullException(nameof(jwtGenerator));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task<LoginResult> LoginAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username cannot be null or empty.", nameof(username));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be null or empty.", nameof(username));
            }

            var user = await _userManager.FindByNameAsync(username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                return LoginResult.FailedLogin();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var jwt = _jwtGenerator.GenerateJwt(authClaims);

            return LoginResult.SuccessfulLogin(jwt);
        }

        public async Task ForgotPasswordAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (token == null)
            {
                return;
            }

            await _emailService.SendEmailAsync(user.Email, "Reset your Password", ResetPasswordTemplateKey, new Dictionary<string, string>
            {
                { "token", token },
                { "username", user.UserName },
            });
        }

        public async Task ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user == null)
            {
                return;
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
        }

        public async Task VerifyEmailAsync(VerifyEmailRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            var purpose = UserManager<IdentityUser>.ConfirmEmailTokenPurpose;
            await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultEmailProvider, purpose, request.Token);
        }

        public async Task RegisterAsync(RegisterUserRequest request)
        {
            var user = new IdentityUser
            { 
                UserName = request.Email,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return;
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            if (token == null)
            {
                return;
            }

            await _emailService.SendEmailAsync(user.Email, "Verify Email", VerifyEmailTemplateKey,
                new Dictionary<string, string>
                {
                    { "token", token },
                    { "username", user.UserName },
                });
        }
    }
}
