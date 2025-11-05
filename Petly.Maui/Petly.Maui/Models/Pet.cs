using System.ComponentModel.DataAnnotations;

namespace Petly.Maui.Models
{
    public class Pet
    {
        [Key]
        public int pet_id { get; set; }

        [Required(ErrorMessage = "Ім’я тварини є обов’язковим.")]
        [StringLength(50)]
        public string pet_name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Тип тварини є обов’язковим.")]
        public string type { get; set; } = string.Empty;

        [Range(0, 30, ErrorMessage = "Вік має бути від 0 до 30 років.")]
        public int age { get; set; }

        [Required]
        public string gender { get; set; } = string.Empty;

        public string? description { get; set; }
        public string? health { get; set; }
        public string? photourl { get; set; }

        [Required]
        public int shelter_id { get; set; }

        [Required]
        [RegularExpression("available|adopted|treatment|reserved",
            ErrorMessage = "Статус має бути: available, adopted, treatment або reserved.")]
        public string status { get; set; } = "available";
    }
}
