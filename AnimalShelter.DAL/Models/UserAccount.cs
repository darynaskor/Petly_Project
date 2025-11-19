using System.ComponentModel.DataAnnotations;

namespace AnimalShelter.DAL.Models
{
    public class UserAccount
    {
        [Key]
        public int id { get; set; }

        [Required, StringLength(50)]
        public string first_name { get; set; }

        [Required, StringLength(50)]
        public string last_name { get; set; }

        [Required, EmailAddress, StringLength(100)]
        public string email { get; set; }

        [Required]
        public string password_hash { get; set; }

        public bool accepted_terms { get; set; } = false;

        public bool is_admin { get; set; } = false;

        public bool is_volunteer { get; set; } = false;
    }
}