using System.ComponentModel.DataAnnotations;

namespace Petly.Maui.Models
{
    public class User
    {
        [Key]
        public int user_id { get; set; }

        [Required]
        [StringLength(50)]
        public string user_name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string user_surname { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль має містити щонайменше 6 символів.")]
        public string password { get; set; } = string.Empty;

        [Phone]
        public string? phone { get; set; }

        public string? role { get; set; }
    }
}
