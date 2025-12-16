using System;

namespace Petly.Maui.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();

        BindingContext = GetService<Petly.Maui.ViewModels.LoginViewModel>()
            ?? throw new InvalidOperationException("LoginViewModel is not registered.");

        AnimatePage();
        StartPawAnimations();
    }

    private void StartPawAnimations()
    {
        AnimatePaw(Paw1, 6000);
        AnimatePaw(Paw2, 6500);
        AnimatePaw(Paw3, 7000);
        AnimatePaw(Paw4, 7500);

        AnimatePaw(Paw5, 6200);
        AnimatePaw(Paw6, 6800);
        AnimatePaw(Paw7, 7400);
        AnimatePaw(Paw8, 8000);
    }

    private async void AnimatePaw(Image paw, uint speed)
    {
        while (true)
        {
            paw.TranslationY = 1100;

            await paw.TranslateTo(paw.TranslationX, -200, speed, Easing.Linear);

            paw.TranslationY = 1100;
        }
    }

    private async void AnimatePage()
    {
        await Task.Delay(200);

        await Logo.FadeTo(1, 700, Easing.CubicOut);
        await Logo.TranslateTo(0, 0, 500, Easing.CubicOut);

        WelcomeText.FadeTo(1, 500);
        WelcomeText.TranslateTo(0, 0, 500);

        await EmailBlock.FadeTo(1, 300);
        await PasswordBlock.FadeTo(1, 300);
        await RememberBlock.FadeTo(1, 300);

        await LoginButton.ScaleTo(1, 400, Easing.SpringOut);
        await RegisterBlock.FadeTo(1, 400);

        
    }

    private static T? GetService<T>() where T : class =>
        Application.Current?.Handler?.MauiContext?.Services?.GetService<T>();
}
