namespace Petly.Maui.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(Petly.Maui.ViewModels.LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
