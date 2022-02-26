namespace IdentityAuth.Core
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);

        Task SendEmailAsync(string to, string subject, string templateKey, IDictionary<string, string> parameters);
    }
}
