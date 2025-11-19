using System.ComponentModel.DataAnnotations;

namespace AnimalShelter.DAL.Models
{
    public class User
    {
        [Key]
        public int user_id { get; set; }

        [Required, StringLength(100)]
        public string user_name { get; set; }

        [Required, StringLength(100)]
        public string user_surname { get; set; }

        [Required, EmailAddress, StringLength(100)]
        public string email { get; set; }

        [Required, StringLength(200)]
        public string password { get; set; }

        [Phone, StringLength(20)]
        public string? phone { get; set; }

        [StringLength(20)]
        public string? role { get; set; }

        public string? user_description { get; set; }
    }
}