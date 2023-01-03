using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Zay.ViewModel.CustomValidation;

namespace Zay.ViewModel
{
    public class SliderCreateViewModel
    {
       
        public string? Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        [DataType(DataType.Upload)]
        [MaxFileSize(1 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg", ".gif", ".svg" })]
        [ImageDimensionAttribute(1200, 600, 500, 400)]
        public IFormFile Image { get; set; }
        [DisplayName("Display Order")]
        [Required]
        public int DisplayOrder { get; set; }
        [Required]
        public Boolean IsActive { get; set; }
    }
}
