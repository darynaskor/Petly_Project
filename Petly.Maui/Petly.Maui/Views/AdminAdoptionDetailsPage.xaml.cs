using Petly.Maui.Services;
using Petly.Maui.Models;

namespace Petly.Maui.Views
{
    [QueryProperty(nameof(RequestId), "id")]
    public partial class AdminAdoptionDetailsPage : ContentPage
    {
        private readonly IAdoptionService _adoptionService;
        private readonly UserContext _userCtx;

        private AdoptionRequest? _request;

        public string? RequestId { get; set; }

        public AdminAdoptionDetailsPage(IAdoptionService adoptionService)
        {
            InitializeComponent();
            _adoptionService = adoptionService;

            // Отримуємо UserContext через DI
            _userCtx = GetService<UserContext>()!;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // 🔐 Перевірка доступу тільки для адміна
            await _userCtx.LoadCurrentUserAsync();
            if (!_userCtx.IsAdmin)
            {
                await DisplayAlert("Доступ", "Ця сторінка доступна тільки адміністратору.", "OK");
                await Shell.Current.GoToAsync("//pets");
                return;
            }

            if (!string.IsNullOrEmpty(RequestId))
            {
                _request = await _adoptionService.GetByIdAsync(RequestId);

                if (_request != null)
                {
                    // Фото тварини
                    PetPhoto.Source = _request.pet_photoUrl;

                    // Дані користувача (підлаштуй під свої поля в AdoptionRequest)
                    // Якщо в моделі є UserName — можна замінити:
                    // UserName.Text = _request.UserName;
                    UserName.Text = $"{_request.first_name} {_request.last_name}".Trim();

                    UserEmail.Text = _request.email;
                    UserPhone.Text = _request.phone;
                    UserDescription.Text = _request.user_description;

                    // Інфо про тваринку
                    PetInfo.Text = $"{_request.pet_type}, {_request.pet_name}, {_request.pet_age}, {_request.pet_gender}";
                }
            }
        }

        private async void OnApprove(object sender, EventArgs e)
        {
            if (_request != null)
            {
                await _adoptionService.UpdateStatusAsync(_request.adopt_id, AdoptionStatus.Підтверджено);
                await DisplayAlert("Успіх", "Заявку підтверджено", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void OnReject(object sender, EventArgs e)
        {
            if (_request != null)
            {
                await _adoptionService.UpdateStatusAsync(_request.adopt_id, AdoptionStatus.Відхилено);
                await DisplayAlert("Готово", "Заявку відхилено", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void OnMarkAdopted(object sender, EventArgs e)
        {
            if (_request != null)
            {
                await _adoptionService.MarkPetAsAdoptedAsync(_request.pet_id);
                await DisplayAlert("Успіх", "Тваринку позначено як прилаштовану", "OK");
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        // Допоміжний метод для DI
        private static T? GetService<T>() where T : class =>
            Application.Current?.Handler?.MauiContext?.Services?.GetService<T>();
    }
}
