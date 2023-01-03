using System.ComponentModel.DataAnnotations;

namespace Zay.ViewModel
{
    public class UserEditViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? NewPassword { get; set; }
    }
}
