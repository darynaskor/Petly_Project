using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Petly.Maui.Models;
using Petly.Maui.Services;

namespace Petly.Maui.ViewModels;

public class EditProfileViewModel : INotifyPropertyChanged
{
    private readonly UserContext _ctx;
    private readonly JsonRepository<UserAccount> _repo = new("accounts.json");
    private readonly IAuthService _auth;

    public event PropertyChangedEventHandler? PropertyChanged;

    private string _first = "";
    private string _last = "";
    private string _email = "";

    public string FirstName { get => _first; set { _first = value; OnChanged(); } }
    public string LastName { get => _last; set { _last = value; OnChanged(); } }
    public string Email { get => _email; set { _email = value; OnChanged(); } }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public EditProfileViewModel(UserContext ctx, IAuthService auth)
    {
        _ctx = ctx; _auth = auth;
        SaveCommand = new Command(async () => await SaveAsync());
        CancelCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
    }

    public async Task InitializeAsync()
    {
        await _ctx.LoadCurrentUserAsync();
        var u = _ctx.CurrentUser;
        FirstName = u?.FirstName ?? "";
        LastName = u?.LastName ?? "";
        Email = u?.Email ?? "";
    }

    private async Task SaveAsync()
    {
        // читаємо всіх і оновлюємо саме поточного, зберігаємо хеш паролю як був
        var all = await _repo.LoadAsync();
        var current = all.FirstOrDefault(a =>
            a.Email.Equals(_auth.CurrentEmail ?? "", StringComparison.OrdinalIgnoreCase))
            ?? new UserAccount();

        current.FirstName = FirstName.Trim();
        current.LastName = LastName.Trim();

        // якщо змінюємо email — переносимо акаунт
        var emailNew = Email.Trim();
        if (!emailNew.Equals(current.Email, StringComparison.OrdinalIgnoreCase))
        {
            // видаляємо старий запис (якщо був)
            all.RemoveAll(a => a.Email.Equals(current.Email, StringComparison.OrdinalIgnoreCase));
            current.Email = emailNew;
            all.Add(current);
        }

        await _repo.SaveAsync(all);

        // оновимо контекст і AuthService (щоб MainPage побачила новий email)
        await _ctx.LoadCurrentUserAsync();
        if (!_auth.CurrentEmail?.Equals(emailNew, StringComparison.OrdinalIgnoreCase) ?? true)
            await _auth.SignInAsync(emailNew, ""); // якщо паролі не перевіряєш тут — ок; інакше прибери

        await Shell.Current.GoToAsync("..");
    }

    private void OnChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
