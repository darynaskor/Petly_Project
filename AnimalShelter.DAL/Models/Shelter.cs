using System.ComponentModel.DataAnnotations;

namespace AnimalShelter.DAL.Models
{
    public class Shelter
    {
        [Key]
        public int shelter_id { get; set; }

        [Required, StringLength(100)]
        public string shelter_name { get; set; }

        [StringLength(200)]
        public string? address { get; set; }

        [Phone, StringLength(20)]
        public string? phone { get; set; }

        [EmailAddress, StringLength(100)]
        public string? email { get; set; }

        public decimal? px { get; set; }
        public decimal? py { get; set; }
    }
}