namespace Petly.Maui;

public partial class App : Application
{
    public App(AppShell shell)
    {
        InitializeComponent();
        MainPage = shell; // Shell сам покаже логін при старті
    }
}
