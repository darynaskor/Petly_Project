using System.ComponentModel.DataAnnotations;

namespace AnimalShelter.DAL.Models
{
    public class AdoptionRequest
    {
        [Key]  // 🔹 позначаємо головний ключ
        public int request_id { get; set; }

        public int user_id { get; set; }
        public int pet_id { get; set; }
        public DateTime request_date { get; set; }
        public string status { get; set; }
    }
}