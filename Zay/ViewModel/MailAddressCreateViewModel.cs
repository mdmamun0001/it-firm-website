using System.ComponentModel.DataAnnotations;

namespace Zay.ViewModel
{
    public class MailAddressCreateViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public Boolean IsActive { get; set; }
    }
}
