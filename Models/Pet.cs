namespace Petly.Maui.Models
{
    public class Pet
    {
        public int pet_id { get; set; }
        public string? pet_name { get; set; } 
        public string? type { get; set; } 
        public int age { get; set; }
        public string? gender { get; set; }
        public string? description { get; set; }
        public string? health { get; set; }
        public string? photourl { get; set; }
        public int shelter_id { get; set; }
        public bool status { get; set; }
    }
}