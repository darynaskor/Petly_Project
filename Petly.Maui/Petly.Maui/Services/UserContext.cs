// Services/UserContext.cs
using System.Linq;
using Petly.Maui.Models;

namespace Petly.Maui.Services
{
    public class UserContext
    {
        private readonly IAuthService _auth;
        private readonly JsonRepository<UserAccount> _repo = new("accounts.json");

        public UserAccount? CurrentUser { get; private set; }

        public bool IsLoggedIn => _auth.IsLoggedIn;
        public bool IsAdmin => _auth.IsAdmin;
        public string? Email => _auth.CurrentEmail;

        // зручно перевіряти статус волонтера
        public bool IsVolunteer => CurrentUser?.IsVolunteer == true;

        public UserContext(IAuthService auth)
        {
            _auth = auth;
        }

        public void Clear() => CurrentUser = null;

        // Завантажує поточного користувача з JSON за збереженим email
        public async Task LoadCurrentUserAsync()
        {
            var email = _auth.CurrentEmail;
            if (string.IsNullOrWhiteSpace(email))
            {
                CurrentUser = null;
                return;
            }

            var list = await _repo.LoadAsync();
            CurrentUser = list.FirstOrDefault(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        // Зберігає зміни користувача в JSON
        public async Task SaveCurrentUserAsync(UserAccount updated)
        {
            var list = await _repo.LoadAsync();
            var idx = list.FindIndex(u =>
                u.Email.Equals(updated.Email, StringComparison.OrdinalIgnoreCase));

            if (idx >= 0)
                list[idx] = updated;
            else
                list.Add(updated);

            await _repo.SaveAsync(list);
            CurrentUser = updated;
        }

        // Метод для швидкого отримання поточного користувача
        public async Task<UserAccount?> GetCurrentAccountAsync()
        {
            if (CurrentUser != null)
                return CurrentUser;

            await LoadCurrentUserAsync();
            return CurrentUser;
        }

        // Позначити користувача як волонтера (викликаємо після approve заявки)
        public async Task MarkAsVolunteerAsync(string email)
        {
            var list = await _repo.LoadAsync();
            var u = list.FirstOrDefault(x =>
                x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (u == null)
                return;

            if (!u.IsVolunteer)
            {
                u.IsVolunteer = true;
                await _repo.SaveAsync(list);
            }

            // оновлюємо кеш, якщо це поточний користувач
            if (CurrentUser != null &&
                CurrentUser.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
            {
                CurrentUser = u;
            }
        }
    }
}
