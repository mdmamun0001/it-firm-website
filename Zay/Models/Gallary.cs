using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Zay.Models
{
    public class Gallary
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
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
