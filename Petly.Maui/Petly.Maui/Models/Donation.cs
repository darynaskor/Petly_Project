namespace Petly.Maui.Models
{
    public class Donation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // Email користувача, який зробив внесок
        public string? UserEmail { get; set; }

        public string OwnerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;

        // останні 4 цифри картки (для адміну)
        public string CardLast4 { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
