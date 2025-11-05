using Petly.Maui.ViewModels;

namespace Petly.Maui.Views;

public partial class EditProfilePage : ContentPage
{
    private readonly EditProfileViewModel _vm;

    public EditProfilePage(EditProfileViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.InitializeAsync();   // ← підтягуємо дані поточного користувача
    }
}
