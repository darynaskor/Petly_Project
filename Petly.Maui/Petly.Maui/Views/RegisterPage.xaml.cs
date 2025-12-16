using System;

namespace Petly.Maui.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
        BindingContext = GetService<Petly.Maui.ViewModels.RegisterViewModel>()
            ?? throw new InvalidOperationException("RegisterViewModel is not registered.");
    }

    private static T? GetService<T>() where T : class =>
        Application.Current?.Handler?.MauiContext?.Services?.GetService<T>();
}
