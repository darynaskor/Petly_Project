namespace Petly.Maui.Models;

public class UserAccount
{
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";

    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public bool AcceptedTerms { get; set; } = false;
}
