using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Zay.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public String Image { get; set; }
        [DisplayName("Display Order")]
        [Required]
        public int DisplayOrder { get; set; }
        [Required]
        public Boolean IsActive { get; set; }
        public ICollection<ServiceCategory>? ServiceCategories { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
