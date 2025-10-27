namespace Petly.Maui.Models
{
    public class Shelter
    {
        public int shelter_id { get; set; }
        public string shelter_name { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
    }
}