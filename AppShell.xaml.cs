using Petly.Maui.Views; 

namespace Petly.Maui; // AppShell знаходиться тут

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // 1. MainPage знаходиться в тому ж просторі імен (Petly.Maui), тому працює
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));

        // 2. PetsListPage знаходиться у Petly.Maui.Views. 
        //    Потрібно вказати повну назву типу, або використати 'Views.PetsListPage'
        //    якщо 'using Petly.Maui.Views;' присутній.

        Routing.RegisterRoute("petlist", typeof(Views.PetsListPage));

        Routing.RegisterRoute("petdetails", typeof(Views.PetDetailsPage));
    }
}