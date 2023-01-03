using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Zay.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public String Image { get; set; }
        
        public string? ClientName { get; set; }
        public string? ClientUrl { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public string Description { get; set; }
        public string? ProjectUrl { get; set; }
    
        public string? BackgroundColor { get; set; }
        [Required]
        public int DisplayOrder { get; set; }
        [Required]
        public Boolean IsActive { get; set; }
        [Required]
        public string Status { get; set; }
        public DateTime? PublishedDate { get; set; }
        public ICollection<Tool>? Tools { get; set; }
    }
}
