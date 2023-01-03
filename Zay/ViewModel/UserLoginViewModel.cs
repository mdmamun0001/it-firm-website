using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Zay.ViewModel
{
    public class UserLoginViewModel
    {
        [Required]
        [EmailAddress]
        [DisplayName("User Email")]
        public string UserEmail { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
        [DisplayName("RememberMe")]
        public Boolean RememberMe { get; set; }
    }
}
