using brevo_csharp.Api;
using ClientConfig = brevo_csharp.Client.Configuration;
using brevo_csharp.Model;
using Task = System.Threading.Tasks.Task;

namespace ComingHereServer.Services
{
    public class EmailService
    {
        private readonly string _apiKey;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(IConfiguration configuration)
        {
            _apiKey = configuration["Brevo:ApiKey"] ?? throw new ArgumentNullException(nameof(_apiKey));
            _fromEmail = configuration["Brevo:FromEmail"] ?? throw new ArgumentNullException(nameof(_fromEmail));
            _fromName = configuration["Brevo:FromName"] ?? throw new ArgumentNullException(nameof(_fromName));
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            var apiInstance = new TransactionalEmailsApi();

            ClientConfig.Default.ApiKey["api-key"] = _apiKey;

            var sendSmtpEmail = new SendSmtpEmail(
                sender: new SendSmtpEmailSender(_fromName, _fromEmail),
                to: new List<SendSmtpEmailTo> { new SendSmtpEmailTo(toEmail) },
                subject: subject,
                htmlContent: htmlContent
            );

            try
            {
                var result = await apiInstance.SendTransacEmailAsync(sendSmtpEmail);
                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error sending email: " + e.Message);
            }
        }

        public async Task SendConfirmationEmailAsync(string toEmail, string confirmationCode)
        {
            var subject = "Подтверждение регистрации на ComingHere";
            var htmlContent = $@"
                <h2>Подтверждение Email</h2>
                <p>Спасибо за регистрацию! Ваш код подтверждения:</p>
                <h3 style=""color: #2e6c80;"">{confirmationCode}</h3>
                <p>Введите этот код на сайте, чтобы подтвердить ваш Email.</p>";

            await SendEmailAsync(toEmail, subject, htmlContent);
        }

        public Task SendResetCodeAsync(string email, string code)
        {
            var subject = "Your password reset code";
            var body = $"Use this code to reset your password: {code}";
            return SendEmailAsync(email, subject, body);
        }
    }
}