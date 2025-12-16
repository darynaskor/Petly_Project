using System.ComponentModel.DataAnnotations;

namespace Petly.Maui.Models
{
    public enum AdoptionStatus
    {
        Нова,
        Розглядається,
        Підтверджено,
        Відхилено
    }

    public class AdoptionRequest
    {
        public string adopt_id { get; set; } = Guid.NewGuid().ToString();

        // Дані користувача
        public string user_id { get; set; } = string.Empty;
        [Required] public string first_name { get; set; } = string.Empty;
        [Required] public string last_name { get; set; } = string.Empty;
        [Required, EmailAddress] public string email { get; set; } = string.Empty;
        [Required] public string phone { get; set; } = string.Empty;

        // Дані тварини
        public string pet_id { get; set; } = string.Empty;
        [Required] public string pet_name { get; set; } = string.Empty;
        [Required] public string pet_type { get; set; } = string.Empty;
        [Required] public string pet_age { get; set; } = string.Empty;
        [Required] public string pet_gender { get; set; } = string.Empty;

        [Required] public string user_description { get; set; } = string.Empty;
        public string pet_photoUrl { get; set; } = string.Empty;

        // Для позначки тваринки як прилаштованої
        public bool IsPetAdopted { get; set; } = false;
        // Enum статус
        public AdoptionStatus StatusEnum { get; set; } = AdoptionStatus.Нова;

        public string Status
        {
            get => StatusEnum.ToString();
            set
            {
                if (Enum.TryParse(value, out AdoptionStatus parsed))
                    StatusEnum = parsed;
            }
        }
    }

}
