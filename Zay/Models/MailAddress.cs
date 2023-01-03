using System.ComponentModel.DataAnnotations;

namespace Zay.Models
{
    public class MailAddress
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public Boolean IsActive { get; set; }
    }
}
