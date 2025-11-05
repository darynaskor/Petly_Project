using System.ComponentModel.DataAnnotations;

namespace Petly.Maui.Models
{
    public class AdoptionRequest
    {
        [Key]
        public int adopt_id { get; set; }

        [Required]
        public int user_id { get; set; }

        [Required]
        public int pet_id { get; set; }

        [Required]
        public DateTime date { get; set; } = DateTime.Now;

        [Required]
        [RegularExpression("pending|approved|rejected",
            ErrorMessage = "Статус має бути: pending, approved або rejected.")]
        public string status { get; set; } = "pending";
    }
}
