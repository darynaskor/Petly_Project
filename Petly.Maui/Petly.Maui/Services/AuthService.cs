// Services/AuthService.cs
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Microsoft.Maui.Storage;
using Microsoft.Extensions.Logging; // ⬅️ Додано для логування
using Petly.Maui.Models;

namespace Petly.Maui.Services;

public class AuthService : IAuthService
{
    private const string AccountsFile = "accounts.json";
    private const string PrefLoggedIn = "auth/isLoggedIn";
    private const string PrefEmail = "auth/email";
    private const string PrefIsAdmin = "auth/isAdmin";

    // Логер
    private readonly ILogger<AuthService> _logger;

    private readonly JsonRepository<UserAccount> _repo = new(AccountsFile);

    public bool IsLoggedIn => Preferences.Get(PrefLoggedIn, false);
    public string? CurrentEmail => Preferences.Get(PrefEmail, null);
    public bool IsAdmin => Preferences.Get(PrefIsAdmin, false);

    // ⬅️ Додаємо конструктор для ін'єкції логера
    public AuthService(ILogger<AuthService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> SignInAsync(string email, string password)
    {
        _logger.LogInformation("Спроба входу користувача: {Email}", email);

        var accounts = await _repo.LoadAsync();

        // 🔐 Спец-випадок: адмінські креденшали
        if (email.Equals("admin@gmail.com", StringComparison.OrdinalIgnoreCase) &&
            password == "admin")
        {
            _logger.LogInformation("Виявлено вхід адміністратора.");

            // Створюємо/оновлюємо акаунт адміна в локальному JSON
            var existing = accounts.FirstOrDefault(a =>
                a.Email.Equals("admin@gmail.com", StringComparison.OrdinalIgnoreCase));

            if (existing is null)
            {
                _logger.LogInformation("Створення локального запису адміністратора.");
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
                bool changed = false;
                if (!existing.IsAdmin)
                {
                    existing.IsAdmin = true;
                    changed = true;
                }
                if (existing.PasswordHash != Hash("admin"))
                {
                    existing.PasswordHash = Hash("admin");
                    changed = true;
                }

                if (changed) await _repo.SaveAsync(accounts);
            }

            SetSession(email: "admin@gmail.com", isAdmin: true);
            _logger.LogInformation("Адміністратор успішно авторизований.");
            return true;
        }

        // 👤 Звичайний користувач
        var normalized = NormalizeEmail(email);
        var user = accounts.FirstOrDefault(a =>
            a.Email.Equals(normalized, StringComparison.OrdinalIgnoreCase));

        if (user is null)
        {
            _logger.LogWarning("Вхід не вдався: Користувача {Email} не знайдено.", email);
            return false;
        }

        if (user.PasswordHash != Hash(password))
        {
            _logger.LogWarning("Вхід не вдався: Невірний пароль для {Email}.", email);
            return false;
        }

        SetSession(email: user.Email, isAdmin: user.IsAdmin);
        _logger.LogInformation("Користувач {Email} успішно увійшов.", user.Email);
        return true;
    }

    public Task SignOutAsync()
    {
        var email = CurrentEmail ?? "Unknown";
        _logger.LogInformation("Вихід користувача: {Email}", email);

        Preferences.Set(PrefLoggedIn, false);
        Preferences.Remove(PrefEmail);
        Preferences.Remove(PrefIsAdmin);   
        return Task.CompletedTask;
    }

    public async Task<(bool ok, string error)> RegisterAsync(
        string firstName, string lastName,
        string email, string password,
        bool acceptTerms)
    {
        _logger.LogInformation("Спроба реєстрації: {Email}", email);

        if (string.IsNullOrWhiteSpace(firstName)) return LogAndReturnError("Вкажіть ім'я.");
        if (string.IsNullOrWhiteSpace(lastName)) return LogAndReturnError("Вкажіть прізвище.");
        if (string.IsNullOrWhiteSpace(email)) return LogAndReturnError("Вкажіть email.");
        if (!email.Contains("@")) return LogAndReturnError("Невалідний email.");
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            return LogAndReturnError("Пароль має містити щонайменше 6 символів.");
        if (!acceptTerms) return LogAndReturnError("Потрібно погодитися з умовами.");

        var e = NormalizeEmail(email);
        var accounts = await _repo.LoadAsync();

        if (accounts.Any(a => a.Email.Equals(e, StringComparison.OrdinalIgnoreCase)))
        {
            _logger.LogWarning("Реєстрація не вдалася: Email {Email} вже зайнятий.", e);
            return (false, "Користувач з таким email вже існує.");
        }

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

        _logger.LogInformation("Новий користувач зареєстрований: {Email}", e);

        // авторизуємо одразу як звичайного
        SetSession(e, isAdmin: false);
        return (true, "");
    }

    // Допоміжний метод для логування помилок реєстрації
    private (bool, string) LogAndReturnError(string error)
    {
        _logger.LogWarning("Валідація реєстрації не пройшла: {Error}", error);
        return (false, error);
    }

    // ===== ДОДАТКОВІ МЕТОДИ API =====

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