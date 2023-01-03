using System.ComponentModel.DataAnnotations;

namespace Zay.ViewModel
{
    public class MailAddressEditViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public Boolean IsActive { get; set; }
    }
}
