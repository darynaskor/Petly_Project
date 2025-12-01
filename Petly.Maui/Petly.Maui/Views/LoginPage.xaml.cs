using System;

namespace Petly.Maui.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = GetService<Petly.Maui.ViewModels.LoginViewModel>()
            ?? throw new InvalidOperationException("LoginViewModel is not registered.");
    }

    private static T? GetService<T>() where T : class =>
        Application.Current?.Handler?.MauiContext?.Services?.GetService<T>();
}
