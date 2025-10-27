// App.xaml.cs

using Petly.Maui.Views;
using Petly.Maui;

namespace Petly.Maui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}