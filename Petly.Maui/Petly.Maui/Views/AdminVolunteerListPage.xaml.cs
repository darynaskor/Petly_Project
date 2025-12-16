using Petly.Maui.Models;
using Petly.Maui.Services;
using Microsoft.Maui.Controls;

namespace Petly.Maui.Views
{
    public partial class AdminVolunteerListPage : ContentPage
    {
        private readonly IVolunteerService _volunteerService;
        private readonly IAuthService _auth;

        public AdminVolunteerListPage(IVolunteerService volunteerService, IAuthService auth)
        {
            InitializeComponent();
            _volunteerService = volunteerService;
            _auth = auth;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // ❗ Доступ лише для адміна
            if (!_auth.IsAdmin)
            {
                await DisplayAlert("Доступ", "Ця сторінка доступна лише адміністраторам.", "OK");
                await Shell.Current.GoToAsync("//home");
                return;
            }

            // Завантажуємо всі заявки (для адміна)
            RequestsList.ItemsSource = await _volunteerService.GetAllAsync();
        }

        private async void OnRequestSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is VolunteerRequest req)
            {
                await Shell.Current.GoToAsync($"admin/volunteer/details?id={req.Id}");
                ((CollectionView)sender).SelectedItem = null;
            }
        }
    }
}
