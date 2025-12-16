using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Petly.Maui.Models;
using Petly.Maui.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;          
using System.IO;

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
    private ImageSource? _avatarImage;

    public string FirstName { get => _first; set { _first = value; OnChanged(); } }
    public string LastName  { get => _last;  set { _last = value; OnChanged(); } }
    public string Email     { get => _email; set { _email = value; OnChanged(); } }
    public ImageSource? AvatarImage { get => _avatarImage; set { _avatarImage = value; OnChanged(); } }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand ChangePhotoCommand { get; }

    public EditProfileViewModel(UserContext ctx, IAuthService auth)
    {
        _ctx = ctx;
        _auth = auth;

        SaveCommand = new Command(async () => await SaveAsync());
        CancelCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        ChangePhotoCommand = new Command(async () => await ChangePhotoAsync());
    }

    public async Task InitializeAsync()
    {
        await _ctx.LoadCurrentUserAsync();
        var u = _ctx.CurrentUser;
        FirstName = u?.FirstName ?? "";
        LastName = u?.LastName ?? "";
        Email = u?.Email ?? "";

        if (!string.IsNullOrEmpty(u?.AvatarPath) && File.Exists(u.AvatarPath))
            AvatarImage = ImageSource.FromFile(u.AvatarPath);
    }

    private async Task ChangePhotoAsync()
    {
        try
        {
            var result = await MediaPicker.PickPhotoAsync();
            if (result != null)
            {
                var fileName = Path.Combine(FileSystem.AppDataDirectory, result.FileName);
                using var stream = await result.OpenReadAsync();
                using var fileStream = File.OpenWrite(fileName);
                await stream.CopyToAsync(fileStream);

                AvatarImage = ImageSource.FromFile(fileName);

                // Оновлення користувача
                var user = _ctx.CurrentUser;
                if (user != null)
                {
                    user.AvatarPath = fileName;
                    var all = await _repo.LoadAsync();
                    var current = all.FirstOrDefault(a => a.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase));
                    if (current != null)
                    {
                        current.AvatarPath = fileName;
                        await _repo.SaveAsync(all);
                    }
                }
            }

        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Помилка", ex.Message, "OK");
        }
    }

    private async Task SaveAsync()
    {
        var all = await _repo.LoadAsync();
        var current = all.FirstOrDefault(a =>
            a.Email.Equals(_auth.CurrentEmail ?? "", StringComparison.OrdinalIgnoreCase))
            ?? new UserAccount();

        current.FirstName = FirstName.Trim();
        current.LastName  = LastName.Trim();

        var emailNew = Email.Trim();
        if (!emailNew.Equals(current.Email, StringComparison.OrdinalIgnoreCase))
        {
            all.RemoveAll(a => a.Email.Equals(current.Email, StringComparison.OrdinalIgnoreCase));
            current.Email = emailNew;
            all.Add(current);
        }

        await _repo.SaveAsync(all);
        await _ctx.LoadCurrentUserAsync();

        if (!_auth.CurrentEmail?.Equals(emailNew, StringComparison.OrdinalIgnoreCase) ?? true)
            await _auth.SignInAsync(emailNew, "");

        await Shell.Current.GoToAsync("..");
    }

    private void OnChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
