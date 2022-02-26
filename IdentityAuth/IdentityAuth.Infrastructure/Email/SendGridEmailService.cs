using IdentityAuth.Core;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace IdentityAuth.Infrastructure.Email
{
    public class SendGridEmailService : IEmailService
    {
        private readonly SendGridConfiguration _configuration;
        private ITemplateRepo _templateRepo;

        public SendGridEmailService(IOptions<SendGridConfiguration> configuration, ITemplateRepo templateRepo)
        {
            _configuration = configuration.Value ?? throw new ArgumentNullException(nameof(configuration));
            _templateRepo = templateRepo ?? throw new ArgumentNullException(nameof(templateRepo));
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var client = new SendGridClient(_configuration.ApiKey);

            var fromAddress = new EmailAddress(_configuration.FromEmailAddress, _configuration.FromName);
            var toAddress = new EmailAddress(to);

            var message = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, null, body);

            await client.SendEmailAsync(message);
        }

        public async Task SendEmailAsync(string to, string subject, string templateKey, IDictionary<string, string> parameters)
        {
            var template = await _templateRepo.Get(templateKey);
            var content = template.Content;

            foreach (var key in parameters.Keys)
            {
                content = content.Replace($"{{{key}}}", parameters[key]);
            }

            var client = new SendGridClient(_configuration.ApiKey);

            var fromAddress = new EmailAddress(_configuration.FromEmailAddress, _configuration.FromName);
            var toAddress = new EmailAddress(to);

            var message = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, null, content);

            await client.SendEmailAsync(message);
        }
    }
}
