using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Zay.ViewModel
{
    public class UserCreateViewModel
    {
        [Required]
       public string Name { get; set; }
        [Required]
        [EmailAddress]
        [DisplayName("User Email")]
      public  string UserEmail { get; set; }
        [Required]
        [PasswordPropertyText]
        public  string Password { get; set; }
        [Required]
        [PasswordPropertyText]
        [Compare("Password", ErrorMessage = "Both Password and Confirm Password Must be Same")]
        [DisplayName("Confirm Password")]
        public  string ConfirmPassword { get; set; }
    }
}
