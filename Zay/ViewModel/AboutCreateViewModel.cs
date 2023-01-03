using System.ComponentModel.DataAnnotations;
using Zay.ViewModel.CustomValidation;

namespace Zay.ViewModel
{
    public class AboutCreateViewModel
    {
        [Required]
        public string CompanyName { get; set; }
        public string? ShortDescription { get; set; }
        [Required]
        public string Description { get; set; }
        [DataType(DataType.Upload)]
        [MaxFileSize(1 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg", ".gif", ".svg" })]
        [ImageDimensionAttribute(250, 250, 150, 100)]
        public IFormFile? Logo { get; set; }
        [DataType(DataType.Upload)]
        [MaxFileSize(1 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg", ".gif", ".svg" })]
        [ImageDimensionAttribute(1500, 800, 500, 400)]
        public IFormFile? Image { get; set; }
        [Required]
        [Phone]
        public string PhoneNnmber { get; set; }
        [Phone]
        public string? PhoneNumber2 { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [EmailAddress]
        public string? Email2 { get; set; }
        [Required]
        public string Address { get; set; }
        public string? AddressLink { get; set; }
        [Required]
        public string OfficeTime { get; set; }
        public string OffDay { get; set; }
        public string? Vission { get; set; }
        public string? Mission { get; set; }
        public string? Strength { get; set; }
    }
}
