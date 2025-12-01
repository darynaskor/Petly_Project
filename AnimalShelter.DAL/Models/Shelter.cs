using System.ComponentModel.DataAnnotations;

namespace AnimalShelter.DAL.Models
{
    public class Shelter
    {
        [Key]
        public int shelter_id { get; set; }
        public string shelter_name { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }
}
