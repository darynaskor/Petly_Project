using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using Petly.Maui.Services;
using Microsoft.Maui.Controls;
using Petly.Maui.Models;

namespace Petly.Maui.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly UserContext _ctx;

    public event PropertyChangedEventHandler? PropertyChanged;

    private string _first = "";
    private string _last = "";
    private string _email = "";
    private ImageSource? _avatarImage;

    public string FirstName
    {
        get => _first;
        set { _first = value; OnChanged(); OnChanged(nameof(FullName)); }
    }

    public string LastName
    {
        get => _last;
        set { _last = value; OnChanged(); OnChanged(nameof(FullName)); }
    }

    public string Email
    {
        get => _email;
        set { _email = value; OnChanged(); }
    }

    public string FullName => string.Join(" ", new[] { FirstName, LastName }.Where(s => !string.IsNullOrWhiteSpace(s)));

    // 🔹 властивість для аватара
    public ImageSource? AvatarImage
    {
        get => _avatarImage;
        set { _avatarImage = value; OnChanged(); }
    }

    // Запасний аватар
    private const string DefaultAvatar = "https://uxwing.com/wp-content/themes/uxwing/download/peoples-avatars/man-user-circle-icon.png";

    public MainViewModel(UserContext ctx) => _ctx = ctx;

    public async Task InitializeAsync()
    {
        await _ctx.LoadCurrentUserAsync();
        var u = _ctx.CurrentUser;

        FirstName = u?.FirstName ?? "Гість";
        LastName  = u?.LastName ?? "";
        Email     = u?.Email ?? "";

        if (!string.IsNullOrEmpty(u?.AvatarPath) && File.Exists(u.AvatarPath))
        {
            AvatarImage = ImageSource.FromFile(u.AvatarPath);
        }
        else
        {
            AvatarImage = ImageSource.FromUri(new Uri(DefaultAvatar));
        }
    }

    private void OnChanged([CallerMemberName] string? n = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
}
