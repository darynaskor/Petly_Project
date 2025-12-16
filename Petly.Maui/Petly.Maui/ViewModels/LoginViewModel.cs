using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Extensions.Logging; // ⬅️ Додано для логування
using Microsoft.Maui.Storage;
using Petly.Maui.Services;

namespace Petly.Maui.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly IAuthService _auth;
    private readonly ILogger<LoginViewModel> _logger; // ⬅️ Поле логера

    private const string PrefRemember = "auth/remember";
    private const string PrefEmail = "auth/email";

    private string _email = "";
    private string _password = "";
    private bool _rememberMe;
    private bool _busy;
    private string _error = "";

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Email { get => _email; set { _email = value; OnChanged(); } }
    public string Password { get => _password; set { _password = value; OnChanged(); } }
    public bool RememberMe { get => _rememberMe; set { _rememberMe = value; OnChanged(); } }
    public bool IsBusy { get => _busy; set { _busy = value; OnChanged(); ((Command)LoginCommand).ChangeCanExecute(); } }
    public string Error { get => _error; set { _error = value; OnChanged(); OnChanged(nameof(HasError)); } }
    public bool HasError => !string.IsNullOrEmpty(Error);

    public ICommand LoginCommand { get; }
    public ICommand GoToRegisterCommand { get; }

    // ⬅️ Оновлений конструктор: додано logger
    public LoginViewModel(IAuthService auth, ILogger<LoginViewModel> logger)
    {
        _auth = auth;
        _logger = logger;

        // Підставляємо email, якщо користувач відмічав "Запам'ятати мене"
        RememberMe = Preferences.Get(PrefRemember, false);
        if (RememberMe)
        {
            var savedEmail = Preferences.Get(PrefEmail, string.Empty);
            Email = savedEmail;
            _logger.LogDebug("Відновлено email із пам'яті: {Email}", savedEmail);
        }

        LoginCommand = new Command(async () => await LoginAsync(), () => !IsBusy);

        GoToRegisterCommand = new Command(async () =>
        {
            _logger.LogInformation("Користувач переходить на сторінку реєстрації.");
            await Shell.Current.GoToAsync("//register");
        });
    }

    private async Task LoginAsync()
    {
        if (IsBusy) return;

        // Логуємо спробу (НІКОЛИ не логуйте пароль!)
        _logger.LogInformation("Спроба входу через UI. Email: {Email}", Email);

        IsBusy = true;
        Error = "";

        var ok = await _auth.SignInAsync(Email, Password);

        // оновити “Запам’ятати мене”
        if (ok && RememberMe)
        {
            Preferences.Set(PrefRemember, true);
            Preferences.Set(PrefEmail, Email);
            _logger.LogDebug("Оновлено налаштування 'Запам'ятати мене'.");
        }
        else
        {
            Preferences.Set(PrefRemember, RememberMe);
            if (!RememberMe) Preferences.Remove(PrefEmail);
        }

        IsBusy = false;

        if (ok)
        {
            _logger.LogInformation("Вхід успішний. Перехід на головну.");

            // ✅ після логіну — на головну з профілем
            if (Shell.Current is AppShell sh) await sh.GoToMainAsync();
        }
        else
        {
            // Помилку логуємо як Warning, бо це очікувана поведінка при помилці юзера
            _logger.LogWarning("Вхід не вдався для користувача {Email}. Невірні дані.", Email);

            Error = "Невірні email або пароль";
        }
    }

    private void OnChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}