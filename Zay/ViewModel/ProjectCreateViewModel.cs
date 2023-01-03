using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Zay.Models;
using Zay.ViewModel.CustomValidation;

namespace Zay.ViewModel
{
    public class ProjectCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        [MaxFileSize(1 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg", ".gif", ".svg" })]
        [ImageDimensionAttribute(1400, 800, 800, 400)] 
        public IFormFile Image { get; set; }
        public string? ClientName { get; set; }
        public string? ClientUrl { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public string Description { get; set; }
        public string? ProjectUrl { get; set; }
        [Required]
        public int DisplayOrder { get; set; }
        [Required]
        public Boolean IsActive { get; set; }
        public string? BackgroundColor { get; set; }
        [Required]
        public string Status { get; set; }
        public DateTime? PublishedDate { get; set; }
        public List<int>? ToolIds { get; set; }
    }
}
