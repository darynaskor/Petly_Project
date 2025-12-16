namespace Petly.Maui;

public partial class App : Application
{
    public App(AppShell shell)
    {
        InitializeComponent();
        UserAppTheme = AppTheme.Light; // force light theme so text resources stay dark
        MainPage = shell; // Shell сам покаже логін при старті
    }
}
