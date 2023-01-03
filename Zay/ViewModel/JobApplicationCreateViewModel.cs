using System.ComponentModel.DataAnnotations;

namespace Zay.ViewModel
{
    public class JobApplicationCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public string? Message { get; set; }
        [Required]
        
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.pdf)$", ErrorMessage = " pdf only.")]
        public IFormFile Cv { get; set; }
        [Required]
        public string JobTitle { get; set; }
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.pdf)$", ErrorMessage = " pdf only.")]
        public IFormFile? CoverLetter { get; set; }
    }
}
