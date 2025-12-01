using Petly.Maui.Services;
using Petly.Maui.ViewModels;

namespace Petly.Maui.Views;

public partial class PetsListPage : ContentPage
{
    private readonly IAuthService _auth;
    private readonly PetsListViewModel _viewModel;
    private bool _isAdmin;

    public PetsListPage()
    {
        InitializeComponent();
        _auth = GetService<IAuthService>()!;
        _viewModel = GetService<PetsListViewModel>()!;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel is not null)
        {
            await _viewModel.InitializeAsync();
        }

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
