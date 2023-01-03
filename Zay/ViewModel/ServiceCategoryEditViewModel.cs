using System.ComponentModel.DataAnnotations;
using Zay.ViewModel.CustomValidation;

namespace Zay.ViewModel
{
    public class ServiceCategoryEditViewModel : ServiceCategoryViewModel
    {
        public int Id { get; set; }
        public string ExistingImage { get; set; }
        [DataType(DataType.Upload)]
        [MaxFileSize(1 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg", ".gif", ".svg" })]
        [ImageDimensionAttribute(1400, 800, 500, 40)]
        public new IFormFile? Image { get; set; }
    }
}
