using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Storage;
using Petly.Maui.Services;

namespace Petly.Maui.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly IAuthService _auth;

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

    public LoginViewModel(IAuthService auth)
    {
        _auth = auth;

        // Підставляємо email, якщо користувач відмічав "Запам'ятати мене"
        RememberMe = Preferences.Get(PrefRemember, false);
        if (RememberMe)
            Email = Preferences.Get(PrefEmail, string.Empty);

        LoginCommand = new Command(async () => await LoginAsync(), () => !IsBusy);
        GoToRegisterCommand = new Command(async () => await Shell.Current.GoToAsync("//register"));
    }

    private async Task LoginAsync()
    {
        if (IsBusy) return;
        IsBusy = true; Error = "";

        var ok = await _auth.SignInAsync(Email, Password);

        // оновити “Запам’ятати мене”
        if (ok && RememberMe)
        {
            Preferences.Set(PrefRemember, true);
            Preferences.Set(PrefEmail, Email);
        }
        else
        {
            Preferences.Set(PrefRemember, RememberMe);
            if (!RememberMe) Preferences.Remove(PrefEmail);
        }

        IsBusy = false;

        if (ok)
        {
            // ✅ після логіну — на головну з профілем
            if (Shell.Current is AppShell sh) await sh.GoToMainAsync();
        }
        else
        {
            Error = "Невірні email або пароль";
        }
    }

    private void OnChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
