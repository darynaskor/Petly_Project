namespace Petly.Maui.Views;

public partial class VolunteerPage : ContentPage
{
    public VolunteerPage()
    {
        InitializeComponent();
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Готово",
            "Вашу заявку прийнято, ми вам зателефонуємо.",
            "OK");

        await Shell.Current.GoToAsync("//pets");
    }
}
