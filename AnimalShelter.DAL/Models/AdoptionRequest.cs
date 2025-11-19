using System.ComponentModel.DataAnnotations;


namespace AnimalShelter.DAL.Models
{
    public class AdoptionRequest
    {
        [Key]
        public int adopt_id { get; set; }


        public int user_id { get; set; }
        public int pet_id { get; set; }


        public string status_enum { get; set; }


        public bool is_pet_adopted { get; set; } = false;
    }
}