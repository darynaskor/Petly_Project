namespace Petly.Maui.Models
{
    public class UserAccount
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public bool AcceptedTerms { get; set; }

        // позначка адміністратора
        public bool IsAdmin { get; set; } = false;
    }
}
