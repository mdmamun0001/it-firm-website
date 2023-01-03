using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Zay.Models
{
    public class ServiceCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public String Images { get; set; }
        [DisplayName("Display Order")]
        [Required]
        public int DisplayOrder { get; set; }
        [Required]
        public Boolean IsActive { get; set; }
        public ICollection<Service>? Services { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
