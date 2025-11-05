using Microsoft.Maui.Controls;

namespace Petly.Maui.Views
{
    public partial class PetsListPage : ContentPage
    {
        public PetsListPage()
        {
            InitializeComponent();
        }

        private async void OnPetsClicked(object sender, EventArgs e)
        {
            // переходимо на ту саму сторінку (оновлення)
            await Shell.Current.GoToAsync("petlist");
        }
        private async void OnAboutClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("about");
        }
        private async void DonationButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("donation");
        }

    }
}
