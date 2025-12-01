using System;
using Petly.Maui.ViewModels;

namespace Petly.Maui;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _vm;

    public MainPage()
    {
        InitializeComponent();
        _vm = GetService<MainViewModel>() ?? throw new InvalidOperationException("MainViewModel is not registered.");
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

    private static T? GetService<T>() where T : class =>
        Application.Current?.Handler?.MauiContext?.Services?.GetService<T>();
}
