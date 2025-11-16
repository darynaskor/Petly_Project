using Petly.Maui.Models;
using Petly.Maui.Services;

namespace Petly.Maui.Views
{
    public partial class AdminAdoptionListPage : ContentPage
    {
        private readonly IAdoptionService _adoptionService;
        private readonly UserContext _userCtx;

        public AdminAdoptionListPage(IAdoptionService adoptionService)
        {
            InitializeComponent();
            _adoptionService = adoptionService;

            // Отримуємо UserContext через DI
            _userCtx = GetService<UserContext>()!;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // 🔐 Перевірка доступу — тільки для адміна
            await _userCtx.LoadCurrentUserAsync();
            if (!_userCtx.IsAdmin)
            {
                await DisplayAlert("Доступ", "Ця сторінка доступна тільки адміністратору.", "OK");
                await Shell.Current.GoToAsync("//home");
                return;
            }

            // Якщо адмін — показуємо всі заявки
            RequestsList.ItemsSource = await _adoptionService.GetAllAsync();
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//home");
        }

        private async void OnRequestSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is AdoptionRequest req)
            {
                await Shell.Current.GoToAsync($"admin/requests/details?id={req.Id}");
                ((CollectionView)sender).SelectedItem = null;
            }
        }

        // Допоміжний метод для DI
        private static T? GetService<T>() where T : class =>
            Application.Current?.Handler?.MauiContext?.Services?.GetService<T>();
    }
}
