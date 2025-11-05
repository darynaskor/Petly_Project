namespace Petly.Maui.Views
{
    public partial class DonationPage : ContentPage
    {
        public DonationPage()
        {
            InitializeComponent();
        }

        private async void OnDonateClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Дякуємо 💛", "Ваш внесок прийнято! Ви робите цей світ кращим.", "OK");
            await Shell.Current.GoToAsync("//petlist");
        }
    }
}
