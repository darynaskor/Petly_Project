using Microsoft.Maui.Controls;
using Petly.Maui.Services;

namespace Petly.Maui.Controls;

public partial class TopNavBar : ContentView
{
    private readonly IAuthService _auth;

    public TopNavBar()
    {
        InitializeComponent();

        _auth = GetService<IAuthService>()!;

        AdminBadge.IsVisible = _auth?.IsAdmin ?? false;
        LogoutBtn.IsVisible = _auth?.IsLoggedIn ?? false;

        HighlightActive();
        if (Shell.Current is not null)
            Shell.Current.Navigated += (_, __) => HighlightActive();
    }

    void HighlightActive()
    {
        var all = new[] { PetsBtn, MapBtn, AboutBtn, DonationBtn, VolBtn };
        foreach (var b in all) b.FontAttributes = FontAttributes.None;

        var path = Shell.Current?.CurrentState.Location.ToString() ?? string.Empty;
        if (path.Contains("/pets")) PetsBtn.FontAttributes = FontAttributes.Bold;
        else if (path.Contains("/map")) MapBtn.FontAttributes = FontAttributes.Bold;
        else if (path.Contains("/about")) AboutBtn.FontAttributes = FontAttributes.Bold;
        else if (path.Contains("/donation")) DonationBtn.FontAttributes = FontAttributes.Bold;
        else if (path.Contains("/volunteer")) VolBtn.FontAttributes = FontAttributes.Bold;
    }

    // Навігація
    async void OnPets(object s, EventArgs e) => await Shell.Current.GoToAsync("//pets");
    async void OnMap(object s, EventArgs e) => await Shell.Current.GoToAsync("//map");
    async void OnAbout(object s, EventArgs e) => await Shell.Current.GoToAsync("//about");
    async void OnDonation(object s, EventArgs e) => await Shell.Current.GoToAsync("//donation");
    async void OnVol(object s, EventArgs e) => await Shell.Current.GoToAsync("//volunteer");
    async void OnProfile(object s, EventArgs e) => await Shell.Current.GoToAsync("//main"); 

    async void OnLogout(object s, EventArgs e)
    {
        await _auth.SignOutAsync();
        await Shell.Current.GoToAsync("//login");
    }

    static T? GetService<T>() where T : class =>
        Application.Current?.Handler?.MauiContext?.Services?.GetService<T>();
}
