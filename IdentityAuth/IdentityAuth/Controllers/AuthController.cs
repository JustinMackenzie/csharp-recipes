using AutoMapper;
using IdentityAuth.Core.Auth;
using IdentityAuth.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, ILogger<AuthController> logger, IMapper mapper)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request.Username, request.Password);

            if (result.Success)
            {
                return Ok(result.Token);
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            await _authService.RegisterAsync(_mapper.Map<RegisterUserRequest>(request));
            return Ok();
        }

        [HttpPost("resetpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] Requests.ResetPasswordRequest request)
        {
            await _authService.ResetPasswordAsync(_mapper.Map<Core.Auth.ResetPasswordRequest>(request));
            return Ok();
        }

        [HttpPost("verifyemail")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmailAsync([FromBody] Requests.VerifyEmailRequest request)
        {
            await _authService.VerifyEmailAsync(_mapper.Map<Core.Auth.VerifyEmailRequest>(request));
            return Ok();
        }

        [HttpPost("forgotpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordRequest request)
        {
            await _authService.ForgotPasswordAsync(request.Email);
            return Ok();
        }
    }
}
