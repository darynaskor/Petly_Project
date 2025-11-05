// Services/IAuthService.cs
using Petly.Maui.Models;

namespace Petly.Maui.Services
{
    public interface IAuthService
    {
        bool IsLoggedIn { get; }
        string? CurrentEmail { get; }

        Task<bool> SignInAsync(string email, string password);

        // Імена елементів кортежу мають збігатися з реалізацією!
        Task<(bool ok, string error)> RegisterAsync(
            string firstName, string lastName,
            string email, string password,
            bool acceptTerms);

        Task SignOutAsync();

        // Додаємо ці 2 методи, щоб можна було оновлювати «поточного» користувача
        void SetCurrentEmail(string email);
        Task<UserAccount?> GetCurrentUserAsync();
    }
}
