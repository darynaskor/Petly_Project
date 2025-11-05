namespace Petly.Maui.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(Petly.Maui.ViewModels.RegisterViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
