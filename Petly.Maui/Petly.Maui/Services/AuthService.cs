// Services/AuthService.cs
using System.Security.Cryptography;
using System.Text;
using Microsoft.Maui.Storage;
using Petly.Maui.Models;

namespace Petly.Maui.Services;

public class AuthService : IAuthService
{
    private const string AccountsFile = "accounts.json";
    private const string PrefLoggedIn = "auth/isLoggedIn";
    private const string PrefEmail = "auth/email";

    private readonly JsonRepository<UserAccount> _repo = new(AccountsFile);

    public bool IsLoggedIn => Preferences.Get(PrefLoggedIn, false);
    public string? CurrentEmail => Preferences.Get(PrefEmail, null);

    public async Task<bool> SignInAsync(string email, string password)
    {
        var e = NormalizeEmail(email);
        var accounts = await _repo.LoadAsync();
        var user = accounts.FirstOrDefault(a => a.Email.Equals(e, StringComparison.OrdinalIgnoreCase));
        if (user is null) return false;
        if (user.PasswordHash != Hash(password)) return false;

        SetCurrentEmail(e);
        return true;
    }

    public Task SignOutAsync()
    {
        Preferences.Set(PrefLoggedIn, false);
        Preferences.Remove(PrefEmail);
        return Task.CompletedTask;
    }

    public async Task<(bool ok, string error)> RegisterAsync(
        string firstName, string lastName,
        string email, string password,
        bool acceptTerms)
    {
        if (string.IsNullOrWhiteSpace(firstName)) return (false, "Вкажіть ім'я.");
        if (string.IsNullOrWhiteSpace(lastName)) return (false, "Вкажіть прізвище.");
        if (string.IsNullOrWhiteSpace(email)) return (false, "Вкажіть email.");
        if (!email.Contains("@")) return (false, "Невалідний email.");
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            return (false, "Пароль має містити щонайменше 6 символів.");
        if (!acceptTerms) return (false, "Потрібно погодитися з умовами.");

        var e = NormalizeEmail(email);
        var accounts = await _repo.LoadAsync();

        if (accounts.Any(a => a.Email.Equals(e, StringComparison.OrdinalIgnoreCase)))
            return (false, "Користувач з таким email вже існує.");

        accounts.Add(new UserAccount
        {
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            Email = e,
            PasswordHash = Hash(password),
            AcceptedTerms = true
        });

        await _repo.SaveAsync(accounts);

        SetCurrentEmail(e); // авторизуємо одразу
        return (true, "");
    }

    // ← реалізація методів, яких бракувало
    public void SetCurrentEmail(string email)
    {
        var e = NormalizeEmail(email);
        Preferences.Set(PrefLoggedIn, true);
        Preferences.Set(PrefEmail, e);
    }

    public async Task<UserAccount?> GetCurrentUserAsync()
    {
        var e = CurrentEmail;
        if (string.IsNullOrWhiteSpace(e)) return null;
        var list = await _repo.LoadAsync();
        return list.FirstOrDefault(a => a.Email.Equals(e, StringComparison.OrdinalIgnoreCase));
    }

    private static string NormalizeEmail(string email) => (email ?? "").Trim().ToLowerInvariant();

    private static string Hash(string s)
    {
        using var sha = SHA256.Create();
        return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(s)));
    }
}
