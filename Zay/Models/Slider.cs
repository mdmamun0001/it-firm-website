using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Zay.Models
{
    public class Slider
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public String Image { get; set; }
        [DisplayName("Display Order")]
        [Required]
        public int DisplayOrder { get; set; }
        [Required]
        public Boolean IsActive { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
