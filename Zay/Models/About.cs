using System.ComponentModel.DataAnnotations;

namespace Zay.Models
{
    public class About
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string CompanyName { get; set; }
        public string? ShortDescription { get; set; }
        [Required]
        public string Description { get; set; }
        public String? Image { get; set; }
        [Required]
        public String PhoneNnmber { get; set; }
        public string? PhoneNumber2 { get; set; }
        [Required]
        public string  Email { get; set; }
        public string?  Email2 { get; set; }
        [Required]
        public string  Address { get; set; }
        public string?  AddressLink { get; set; }
        [Required]
        public string OfficeTime { get; set; }
        public string OffDay { get; set; }

        public string? Logo  { get; set; }
      
        public string? Vission { get; set; }
       
        public string? Mission { get; set; }
      
        public string? Strength { get; set; }
    }
}
