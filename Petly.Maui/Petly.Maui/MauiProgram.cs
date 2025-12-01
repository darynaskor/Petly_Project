using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Petly.Maui.Services;
using Petly.Maui.ViewModels;
using Petly.Maui.Views;
using AnimalShelter.DAL;
using AnimalShelter.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using BllPetService = AnimalShelter.BLL.Services.PetService;

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
            const string connectionString = "Host=localhost;Port=5432;Database=animal_shelter;Username=postgres;Password=u30zd9CX";

            builder.Services.AddDbContextFactory<AnimalShelterContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddScoped(typeof(GenericRepository<>));
            builder.Services.AddScoped<BllPetService>();

            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<UserContext>();
            builder.Services.AddSingleton<ShelterService>();
            builder.Services.AddSingleton<IAdoptionService, AdoptionService>();
            builder.Services.AddSingleton<IVolunteerService, VolunteerService>(); 

            // ========= DI: ViewModels =========
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<PetsListViewModel>();
            builder.Services.AddTransient<EditProfileViewModel>();

            // ========= DI: Pages (користувач) =========
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<PetsListPage>();
            builder.Services.AddTransient<EditProfilePage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MapPage>();
            builder.Services.AddTransient<VolunteerPage>(); // сторінка користувача
            builder.Services.AddTransient<Views.PetEditPage>();
            builder.Services.AddTransient<AdoptionPage>();

            // ========= DI: Admin Pages (singleton для Shell) =========
            builder.Services.AddSingleton<AdminAdoptionListPage>();
            builder.Services.AddSingleton<AdminAdoptionDetailsPage>();
            builder.Services.AddSingleton<AdminVolunteerListPage>();
            builder.Services.AddSingleton<AdminVolunteerDetailsPage>();
            builder.Services.AddSingleton<IDonationService, DonationService>();


            // Shell через DI
            builder.Services.AddSingleton<AppShell>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
