using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Zay.Models
{
    public class Employe
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string JobTitle { get; set; }
        public String? Responsibility { get; set;}
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
