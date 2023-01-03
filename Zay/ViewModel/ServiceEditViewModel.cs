using System.ComponentModel.DataAnnotations;
using Zay.ViewModel.CustomValidation;

namespace Zay.ViewModel
{
    public class ServiceEditViewModel: ServiceCreateViewModel
    {
        public int ServiceId { get; set; }
        public List<int>? ExistingServiceCategoriesIds { get; set; }
        public List<int>? NewServiceCategoriesIds { get; set; }
        public string ExistingImage { get; set; }
        [DataType(DataType.Upload)]
        [MaxFileSize(1 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg", ".gif", ".svg" })]
        [ImageDimensionAttribute(1000, 600, 500, 400)]
        public new IFormFile? Image { get; set; }
    }
}
