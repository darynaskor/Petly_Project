using Petly.Maui.Services;

namespace Petly.Maui.Views;

public partial class PetsListPage : ContentPage
{
    private readonly IAuthService _auth;
    private bool _isAdmin;

    public PetsListPage()
    {
        InitializeComponent();
        _auth = GetService<IAuthService>()!;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _isAdmin = _auth?.IsAdmin ?? false;

        // кнопка "Додати" тільки для адміна
        AddPetButton.IsVisible = _isAdmin;
    }

    private async void OnAddPetClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("petedit");
    }

    private static T? GetService<T>() where T : class =>
        Application.Current?.Handler?.MauiContext?.Services?.GetService<T>();
}
