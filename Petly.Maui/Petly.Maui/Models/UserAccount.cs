using System.ComponentModel.DataAnnotations;

namespace Petly.Maui.Models
{
    public class UserAccount
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public bool AcceptedTerms { get; set; }

        // адміністратор
        public bool IsAdmin { get; set; }

        // 🔹 нова властивість – статус волонтера
        public bool IsVolunteer { get; set; } = false;
    }
}

