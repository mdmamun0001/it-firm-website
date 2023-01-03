using System.ComponentModel.DataAnnotations;

namespace Zay.Services
{
    public class MailRequest
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile>? Attachments { get; set; }
    }
}
