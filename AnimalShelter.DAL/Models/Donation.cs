using System.ComponentModel.DataAnnotations;

namespace AnimalShelter.DAL.Models
{
    public class Donation
    {
        [Key]
        public int id { get; set; }

        public string? user_email { get; set; }

        [Required]
        public string owner_name { get; set; }

        [Required, EmailAddress]
        public string email { get; set; }

        [Required, Phone]
        public string phone { get; set; }

        public string? purpose { get; set; }

        public string? card_last4 { get; set; }

        public decimal? amount { get; set; }

        public DateTime created_at { get; set; } = DateTime.UtcNow;
    }
}
