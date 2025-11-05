using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Petly.Maui.Services;
using Petly.Maui.ViewModels;
using Petly.Maui.Views;

namespace Petly.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // ========= DI: Services =========
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<UserContext>();

            // ========= DI: ViewModels =========
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<EditProfileViewModel>();

            // ========= DI: Pages =========
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<PetsListPage>();
            builder.Services.AddTransient<EditProfilePage>();
            builder.Services.AddTransient<MainPage>();

            // Shell через DI
            builder.Services.AddSingleton<AppShell>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
