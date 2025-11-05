namespace Petly.Maui.Controls;

public partial class TopNavBar : ContentView
{
    public TopNavBar()
    {
        InitializeComponent();
        HighlightActive();
        if (Shell.Current is not null)
            Shell.Current.Navigated += (_, __) => HighlightActive();
    }

    void HighlightActive()
    {
        var path = Shell.Current?.CurrentState?.Location.ToString() ?? string.Empty;

        foreach (var b in new[] { PetsBtn, MapBtn, AboutBtn, DonationBtn, VolBtn })
            b.FontAttributes = FontAttributes.None;

        if (path.Contains("/pets")) PetsBtn.FontAttributes = FontAttributes.Bold;
        else if (path.Contains("/map")) MapBtn.FontAttributes = FontAttributes.Bold;
        else if (path.Contains("/about")) AboutBtn.FontAttributes = FontAttributes.Bold;
        else if (path.Contains("/donation")) DonationBtn.FontAttributes = FontAttributes.Bold;
        else if (path.Contains("/volunteer")) VolBtn.FontAttributes = FontAttributes.Bold;
    }

    async void OnPets(object s, EventArgs e) => await Shell.Current.GoToAsync("//pets");
    async void OnMap(object s, EventArgs e) => await Shell.Current.GoToAsync("//map");
    async void OnAbout(object s, EventArgs e) => await Shell.Current.GoToAsync("//about");
    async void OnDonation(object s, EventArgs e) => await Shell.Current.GoToAsync("//donation");
    async void OnVol(object s, EventArgs e) => await Shell.Current.GoToAsync("//volunteer");
    async void OnProfileClicked(object s, EventArgs e) => await Shell.Current.GoToAsync("//home");


}
