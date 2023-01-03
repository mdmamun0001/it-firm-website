using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Zay.ViewModel.CustomValidation;

namespace Zay.ViewModel
{
    public class ClientCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [DataType(DataType.Upload)]
        [MaxFileSize(1 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg", ".gif", ".svg" })]
        [ImageDimensionAttribute(600, 500, 400, 350)]
        public  IFormFile? Image { get; set; }
        [DisplayName("Display Order")]
        [Required]
        public int DisplayOrder { get; set; }
        [Required]
        public Boolean IsActive { get; set; }
    }
}
