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
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // Дані користувача
        public string UserId { get; set; } = string.Empty;
        [Required] public string FirstName { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required] public string Phone { get; set; } = string.Empty;

        // Дані тварини
        public string PetId { get; set; } = string.Empty;
        [Required] public string PetName { get; set; } = string.Empty;
        [Required] public string PetType { get; set; } = string.Empty;
        [Required] public string PetAge { get; set; } = string.Empty;
        [Required] public string PetGender { get; set; } = string.Empty;

        [Required] public string UserDescription { get; set; } = string.Empty;
        public string PetPhotoUrl { get; set; } = string.Empty;

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
