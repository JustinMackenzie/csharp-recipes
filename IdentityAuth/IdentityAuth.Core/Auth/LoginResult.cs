namespace IdentityAuth.Core.Auth
{
    public class LoginResult
    {
        public bool Success { get; }
        public Jwt? Token { get; }

        public static LoginResult SuccessfulLogin(Jwt token)
        {
            return new LoginResult(true, token);
        }

        public static LoginResult FailedLogin()
        {
            return new LoginResult(false, null);
        }

        private LoginResult(bool success, Jwt? token)
        {
            Success = success;
            Token = token;
        }
    }
}