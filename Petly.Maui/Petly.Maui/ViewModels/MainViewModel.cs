using System.ComponentModel;
using System.Runtime.CompilerServices;
using Petly.Maui.Services;

namespace Petly.Maui.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly UserContext _ctx;

    public event PropertyChangedEventHandler? PropertyChanged;

    private string _first = "";
    private string _last = "";
    private string _email = "";

    public string FirstName { get => _first; set { _first = value; OnChanged(); OnChanged(nameof(FullName)); } }
    public string LastName { get => _last; set { _last = value; OnChanged(); OnChanged(nameof(FullName)); } }
    public string Email { get => _email; set { _email = value; OnChanged(); } }
    public string FullName => string.Join(" ", new[] { FirstName, LastName }.Where(s => !string.IsNullOrWhiteSpace(s)));

    public string AvatarUrl { get; } =
        "https://uxwing.com/wp-content/themes/uxwing/download/peoples-avatars/man-user-circle-icon.png";

    public MainViewModel(UserContext ctx) => _ctx = ctx;

    public async Task InitializeAsync()
    {
        await _ctx.LoadCurrentUserAsync();
        var u = _ctx.CurrentUser;
        FirstName = u?.FirstName ?? "Гість";
        LastName = u?.LastName ?? "";
        Email = u?.Email ?? "";
    }

    private void OnChanged([CallerMemberName] string? n = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
}
