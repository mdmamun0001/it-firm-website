using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Zay.ViewModel.CustomValidation;

namespace Zay.ViewModel
{
    public class SliderEditViewModel: SliderCreateViewModel
    {
        [Required]
        public int Id { get; set; }
        [DataType(DataType.Upload)]
        [MaxFileSize(1 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg", ".gif", ".svg" })]
        [ImageDimensionAttribute(1200, 600, 500, 400)]
        public new IFormFile? Image { get; set; }
        [Required]
        public string ExistingImage { get; set; }
    }
}
