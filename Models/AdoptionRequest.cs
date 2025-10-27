namespace Petly.Maui.Models
{
    public class AdoptionRequest
    {
        public int adopt_id { get; set; }
        public int user_id { get; set; }
        public int pet_id { get; set; }
        public DateTime date { get; set; }
        public string status { get; set; } = string.Empty;
    }
}
