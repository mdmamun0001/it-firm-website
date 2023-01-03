using System.ComponentModel.DataAnnotations;

namespace Zay.ViewModel
{
    public class CustomerMessageCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
