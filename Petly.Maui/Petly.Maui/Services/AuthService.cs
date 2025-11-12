// Services/AuthService.cs
using System.Security.Cryptography;
using System.Text;
using System.Linq;                    // ⬅️ для FirstOrDefault/Any
using Microsoft.Maui.Storage;
using Petly.Maui.Models;

namespace Petly.Maui.Services;

public class AuthService : IAuthService
{
    private const string AccountsFile = "accounts.json";
    private const string PrefLoggedIn = "auth/isLoggedIn";
    private const string PrefEmail = "auth/email";
    private const string PrefIsAdmin = "auth/isAdmin";

    private readonly JsonRepository<UserAccount> _repo = new(AccountsFile);

    public bool IsLoggedIn => Preferences.Get(PrefLoggedIn, false);
    public string? CurrentEmail => Preferences.Get(PrefEmail, null);
    public bool IsAdmin => Preferences.Get(PrefIsAdmin, false);

    public async Task<bool> SignInAsync(string email, string password)
    {
        var accounts = await _repo.LoadAsync();

        // 🔐 Спец-випадок: адмінські креденшали
        if (email.Equals("admin@gmail.com", StringComparison.OrdinalIgnoreCase) &&
            password == "admin")
        {
            // Створюємо/оновлюємо акаунт адміна в локальному JSON
            var existing = accounts.FirstOrDefault(a =>
                a.Email.Equals("admin@gmail.com", StringComparison.OrdinalIgnoreCase));

            if (existing is null)
            {
                accounts.Add(new UserAccount
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@gmail.com",
                    PasswordHash = Hash("admin"),
                    AcceptedTerms = true,
                    IsAdmin = true
                });
                await _repo.SaveAsync(accounts);
            }
            else
            {
                // гарантуємо флаг і актуальний пароль
                if (!existing.IsAdmin) existing.IsAdmin = true;
                if (existing.PasswordHash != Hash("admin"))
                {
                    existing.PasswordHash = Hash("admin");
                }
                await _repo.SaveAsync(accounts);
            }

            SetSession(email: "admin@gmail.com", isAdmin: true);
            return true;
        }

        // 👤 Звичайний користувач
        var normalized = NormalizeEmail(email);
        var user = accounts.FirstOrDefault(a =>
            a.Email.Equals(normalized, StringComparison.OrdinalIgnoreCase));
        if (user is null) return false;
        if (user.PasswordHash != Hash(password)) return false;

        SetSession(email: user.Email, isAdmin: user.IsAdmin);
        return true;
    }

    public Task SignOutAsync()
    {
        Preferences.Set(PrefLoggedIn, false);
        Preferences.Remove(PrefEmail);
        Preferences.Remove(PrefIsAdmin);   // ⬅️ чистимо роль
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
            AcceptedTerms = true,
            IsAdmin = false       // реєстрація -> не адмін
        });

        await _repo.SaveAsync(accounts);

        // авторизуємо одразу як звичайного
        SetSession(e, isAdmin: false);
        return (true, "");
    }

    // ===== ДОДАТКОВІ МЕТОДИ API (збережені для сумісності) =====

    // Тримай як публічний, якщо вже десь використовується:
    public void SetCurrentEmail(string email)
    {
        // за замовчуванням без підвищення ролі
        SetSession(NormalizeEmail(email), isAdmin: false);
    }

    public async Task<UserAccount?> GetCurrentUserAsync()
    {
        var e = CurrentEmail;
        if (string.IsNullOrWhiteSpace(e)) return null;
        var list = await _repo.LoadAsync();
        return list.FirstOrDefault(a => a.Email.Equals(e, StringComparison.OrdinalIgnoreCase));
    }

    // якщо десь очікується саме така назва (зручно для ViewModel)
    public Task<UserAccount?> GetCurrentAccountAsync() => GetCurrentUserAsync();

    // ===== helpers =====

    private static string NormalizeEmail(string email) => (email ?? "").Trim().ToLowerInvariant();

    private static string Hash(string s)
    {
        using var sha = SHA256.Create();
        return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(s)));
    }

    private static void SetSession(string email, bool isAdmin)
    {
        Preferences.Set(PrefLoggedIn, true);
        Preferences.Set(PrefEmail, email);
        Preferences.Set(PrefIsAdmin, isAdmin);
    }
}
