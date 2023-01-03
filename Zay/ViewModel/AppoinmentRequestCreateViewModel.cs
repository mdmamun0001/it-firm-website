using System.ComponentModel.DataAnnotations;

namespace Zay.ViewModel
{
    public class AppoinmentRequestCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        [Required]
        public string ServiceType { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CustomerFlexibleDateTime { get; set; }
    }
}
