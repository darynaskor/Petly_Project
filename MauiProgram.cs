using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using System.Diagnostics;
using Petly.Maui.Views;
using Petly.Maui;

namespace Petly.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>() // Головний клас застосунку (App.xaml.cs)
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // =========================================================
            //  РЕЄСТРАЦІЯ СТОРІНОК (ОБОВ'ЯЗКОВО ДЛЯ SHELL ТА НАВІГАЦІЇ)
            // =========================================================

            // Реєструємо сторінки як Transient (створюються щоразу при запиті).
            // Це необхідно, щоб Shell міг їх створювати при навігації.
            // Я припускаю, що PetsListPage знаходиться у просторі імен Petly.Maui.Views 
            // (або Petly.Maui, якщо ви ще не створили папку Views).

            builder.Services.AddTransient<MainPage>();
            
            builder.Services.AddTransient<Views.PetsListPage>();
            builder.Services.AddTransient<Views.PetDetailsPage>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}