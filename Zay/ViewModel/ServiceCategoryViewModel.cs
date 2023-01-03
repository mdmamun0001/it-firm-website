using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Zay.ViewModel.CustomValidation;

namespace Zay.ViewModel
{
    public class ServiceCategoryViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Upload)]
        [MaxFileSize(1 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg", ".gif", ".svg" })]
        [ImageDimensionAttribute(1400, 800, 500, 400)]
        public IFormFile Image { get; set; } 
        [Required]
        public int DisplayOrder { get; set; }
        [Required]
        public Boolean IsActive { get; set; }
    }
}
