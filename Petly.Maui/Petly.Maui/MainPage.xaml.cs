using Petly.Maui.ViewModels;

namespace Petly.Maui;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _vm;

    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.InitializeAsync();
    }

    private async void EditProfileButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("editprofile");
    }
}
