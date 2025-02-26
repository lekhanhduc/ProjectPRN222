using SendGrid;
using SendGrid.Helpers.Mail;

namespace E_Learning.Servies.Impl
{
    public class EmailService : IEmailService
    {
        private readonly string _sendGridApiKey;
        private readonly string _templateId;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _sendGridApiKey = configuration["SendGrid:ApiKey"];
            _templateId = configuration["SendGrid:TemplateId"];
            _logger = logger;
        }

        public async Task SendEmailVerification(string to, string name, string verificationLink, string otp)
        {
            var client = new SendGridClient(_sendGridApiKey);
            var from = new EmailAddress("lekhanhduccc@gmail.com", "ELearning");
            var toEmail = new EmailAddress(to);
            var msg = new SendGridMessage
            {
                TemplateId = _templateId,
                From = from,
                Subject = "Email Verification for E-Learning"
            };

            msg.AddTo(toEmail);
            msg.SetTemplateData(new
            {
                name = name,
                verification_link = verificationLink,
            });

            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                _logger.LogInformation("Email sent successfully to {Email}", to);
            }
            else
            {
                _logger.LogError("Failed to send email to {Email}. Status: {StatusCode} {Body}", to, response.StatusCode, response.Body);
            }
        }
    }
}

