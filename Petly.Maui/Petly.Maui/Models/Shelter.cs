using System.ComponentModel.DataAnnotations;

namespace Petly.Maui.Models
{
    public class Shelter
    {
        [Key]
        public int shelter_id { get; set; }

        [Required]
        [StringLength(100)]
        public string shelter_name { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string address { get; set; } = string.Empty;

        [Phone]
        public string phone { get; set; } = string.Empty;

        [EmailAddress]
        public string email { get; set; } = string.Empty;
    }
}
