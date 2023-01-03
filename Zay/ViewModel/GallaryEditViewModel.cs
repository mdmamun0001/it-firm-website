using System.ComponentModel.DataAnnotations;
using Zay.ViewModel.CustomValidation;

namespace Zay.ViewModel
{
    public class GallaryEditViewModel:GallaryCreateViewModel
    {
        public string ExistingImage { get; set; }
        [DataType(DataType.Upload)]
        [MaxFileSize(1 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg", ".gif", ".svg" })]
        [ImageDimensionAttribute(1200, 800, 400, 350)]
        public new  IFormFile? Image { get; set; }
        public int Id { get; set; }
    }
}
