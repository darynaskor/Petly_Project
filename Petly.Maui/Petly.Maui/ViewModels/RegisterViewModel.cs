using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Extensions.Logging; 
using Petly.Maui.Services;

namespace Petly.Maui.ViewModels;

public class RegisterViewModel : INotifyPropertyChanged
{
    private readonly IAuthService _auth;
    private readonly ILogger<RegisterViewModel> _logger; 

    private string _firstName = "";
    private string _lastName = "";
    private string _email = "";
    private string _password = "";
    private bool _agree;
    private bool _busy;
    private string _error = "";

    public event PropertyChangedEventHandler? PropertyChanged;

    public string FirstName { get => _firstName; set { _firstName = value; OnChanged(); } }
    public string LastName { get => _lastName; set { _lastName = value; OnChanged(); } }
    public string Email { get => _email; set { _email = value; OnChanged(); } }
    public string Password { get => _password; set { _password = value; OnChanged(); } }
    public bool Agree { get => _agree; set { _agree = value; OnChanged(); } }
    public bool IsBusy { get => _busy; set { _busy = value; OnChanged(); ((Command)RegisterCommand).ChangeCanExecute(); } }
    public string Error { get => _error; set { _error = value; OnChanged(); } }

    public ICommand RegisterCommand { get; }
    public ICommand GoToLoginCommand { get; }

    public RegisterViewModel(IAuthService auth, ILogger<RegisterViewModel> logger)
    {
        _auth = auth;
        _logger = logger;

        RegisterCommand = new Command(async () => await RegisterAsync(), () => !IsBusy);

        GoToLoginCommand = new Command(async () =>
        {
            _logger.LogInformation("Користувач переходить на сторінку входу (Login).");
            await Shell.Current.GoToAsync("//login");
        });
    }

    private async Task RegisterAsync()
    {
        if (IsBusy) return;

        _logger.LogInformation("Початок процесу реєстрації. Email: {Email}, Згода з умовами: {Agree}", Email, Agree);

        IsBusy = true;
        Error = "";

        var (ok, msg) = await _auth.RegisterAsync(FirstName, LastName, Email, Password, Agree);

        IsBusy = false;

        if (!ok)
        {
            _logger.LogWarning("Реєстрація не вдалася: {Error}", msg);
            Error = msg;
            return;
        }

        _logger.LogInformation("Реєстрація успішна для {Email}. Перехід на головну сторінку.", Email);

        if (Shell.Current is AppShell sh) await sh.GoToMainAsync();
    }

    private void OnChanged([CallerMemberName] string? n = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
}