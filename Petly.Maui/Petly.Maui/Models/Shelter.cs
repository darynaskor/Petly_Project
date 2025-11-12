using System.ComponentModel.DataAnnotations;

namespace Petly.Maui.Models
{
    public class Shelter
    {
        // --- Поля даних (як у БД/EF) ---
        [Key] public int shelter_id { get; set; }

        [Required, StringLength(100)]
        public string shelter_name { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string address { get; set; } = string.Empty;

        [Phone]
        public string phone { get; set; } = string.Empty;

        [EmailAddress]
        public string email { get; set; } = string.Empty;

        // --- Допоміжні поля для карти/байндингів ---
        public string Id { get; set; } = Guid.NewGuid().ToString();   // унікальний id для UI
        public double Px { get; set; }                                 // 0..1 по ширині
        public double Py { get; set; }                                 // 0..1 по висоті

        // Зручні аліаси, щоб у XAML можна було писати {Binding Name}, {Binding Address}, {Binding Phone}
        public string Name
        {
            get => shelter_name;
            set => shelter_name = value ?? "";
        }

        public string Address
        {
            get => address;
            set => address = value ?? "";
        }

        public string Phone
        {
            get => phone;
            set => phone = value ?? "";
        }
    }
}
