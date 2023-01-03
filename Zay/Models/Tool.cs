using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Zay.Models
{
    public class Tool
    {
        [Key]
        public int ToolId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public String Image { get; set; }
        [DisplayName("Display Order")]
        [Required]
        public int DisplayOrder { get; set; }
        [Required]
        public Boolean IsActive { get; set; }
        public ICollection<Project> Projects { get; set; }
    }
}
