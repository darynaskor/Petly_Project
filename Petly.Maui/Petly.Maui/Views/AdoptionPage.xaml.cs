namespace Petly.Maui.Views
{
    public partial class AdoptionPage : ContentPage
    {
        public AdoptionPage()
        {
            InitializeComponent();
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Дякуємо 💛", "Ваша заявка на адопцію надіслана! Ми зв’яжемось із вами найближчим часом.", "OK");
            await Shell.Current.GoToAsync("//petlist");
        }
    }
}
