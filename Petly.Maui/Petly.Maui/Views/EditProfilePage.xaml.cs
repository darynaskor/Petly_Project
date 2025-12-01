using System;
using Petly.Maui.ViewModels;

namespace Petly.Maui.Views;

public partial class EditProfilePage : ContentPage
{
    private readonly EditProfileViewModel _vm;

    public EditProfilePage()
    {
        InitializeComponent();
        _vm = GetService<EditProfileViewModel>()
            ?? throw new InvalidOperationException("EditProfileViewModel is not registered.");
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.InitializeAsync();   // ← підтягуємо дані поточного користувача
    }

    private static T? GetService<T>() where T : class =>
        Application.Current?.Handler?.MauiContext?.Services?.GetService<T>();
}
