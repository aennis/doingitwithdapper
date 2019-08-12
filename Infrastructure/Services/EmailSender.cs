
using System.Threading.Tasks;

namespace Infrastructure.Services
{

    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            // TODO: Wire this up to actual email sending logic local SMTP, etc.
            return Task.CompletedTask;
        }
    }
}
