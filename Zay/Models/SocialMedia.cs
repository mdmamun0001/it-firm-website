using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Zay.Models
{
    public class SocialMedia
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        [DisplayName("Class Name FontAwesome v:6.2")]
        public String ClassName { get; set; }
        [Required]
        [DisplayName("Background Color")]
        public String BgColor { get; set; }
        [Required]
        public String Color { get; set; }
        [Required]
        [DisplayName("Display Order")]
       
        public int DisplayOrder { get; set; }
        [Required]
        public Boolean IsActive { get; set; }
    }
}
