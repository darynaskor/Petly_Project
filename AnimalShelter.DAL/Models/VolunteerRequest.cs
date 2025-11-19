using System.ComponentModel.DataAnnotations;

namespace AnimalShelter.DAL.Models
{
    public class VolunteerRequest
    {
        [Key]
        public int id { get; set; }

        public string? user_id { get; set; }

        public string? first_name { get; set; }

        public string? last_name { get; set; }

        [EmailAddress]
        public string? email { get; set; }

        [Phone]
        public string? phone { get; set; }

        public string? shelter { get; set; }

        public string? type { get; set; }

        public string? status_enum { get; set; }
    }
}