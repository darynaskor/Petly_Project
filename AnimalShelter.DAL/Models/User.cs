using System.ComponentModel.DataAnnotations;

namespace AnimalShelter.DAL.Models
{
    public class User
    {
        [Key]
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string user_surname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public string role { get; set; }
    }
}
