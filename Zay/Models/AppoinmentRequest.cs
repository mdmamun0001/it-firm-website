using System.ComponentModel.DataAnnotations;

namespace Zay.Models
{
    public class AppoinmentRequest
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        [Required]
        public string ServiceType { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public DateTime CustomerFlexibleDateTime { get; set; }
        [Required]
        public Boolean IsSeen { get; set; }
        [Required]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
