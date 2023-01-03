using System.Net.Mail;

namespace Zay.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest, List<string> Mailtolist);
    }
}
