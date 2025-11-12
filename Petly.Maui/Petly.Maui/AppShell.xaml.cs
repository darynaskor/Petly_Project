using Petly.Maui.Views;
using Petly.Maui.Services;

namespace Petly.Maui;

public partial class AppShell : Shell
{
    private readonly IAuthService _auth;

    public AppShell(IAuthService auth)
    {
        InitializeComponent();
        _auth = auth;

        // Реєстрація додаткових маршрутів
        Routing.RegisterRoute("petdetails", typeof(PetDetailsPage));
        Routing.RegisterRoute("editprofile", typeof(EditProfilePage));
        Routing.RegisterRoute("map", typeof(MapPage));
        Routing.RegisterRoute("volunteer", typeof(VolunteerPage));
        Routing.RegisterRoute("petedit", typeof(Petly.Maui.Views.PetEditPage));

        // Старт з логіну
        FlyoutBehavior = FlyoutBehavior.Disabled;
        GoToAsync("//login");
    }

    // Перехід у головну після входу
    public async Task GoToMainAsync()
    {
        FlyoutBehavior = FlyoutBehavior.Flyout;
        await GoToAsync("//home");
    }
}
