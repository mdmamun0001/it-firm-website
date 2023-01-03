using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Zay.Models
{
    public class CustomerReview
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public String Message   { get; set; }
        public String? Image { get; set; }
        [DisplayName("Display Order")]
        [Required]
        public int DisplayOrder { get; set; }
        [Required]
        public Boolean IsActive { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
