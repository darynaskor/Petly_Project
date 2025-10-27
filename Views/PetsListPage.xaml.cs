using Microsoft.Maui.Controls;

namespace Petly.Maui.Views
{
    public partial class PetsListPage : ContentPage
    {
        public PetsListPage()
        {
            InitializeComponent();
        }
        private async void OnMoreDetailsClicked(object sender, EventArgs e)
            {
                // Не забудьте перевірити, чи зареєстрований маршрут 'petdetails' в AppShell!
                await Shell.Current.GoToAsync("petdetails");
            }
    }
}