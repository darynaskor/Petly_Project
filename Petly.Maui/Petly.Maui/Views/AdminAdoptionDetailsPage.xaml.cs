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
                    PetPhoto.Source = _request.PetPhotoUrl;

                    // Дані користувача (підлаштуй під свої поля в AdoptionRequest)
                    // Якщо в моделі є UserName — можна замінити:
                    // UserName.Text = _request.UserName;
                    UserName.Text = $"{_request.FirstName} {_request.LastName}".Trim();

                    UserEmail.Text = _request.Email;
                    UserPhone.Text = _request.Phone;
                    UserDescription.Text = _request.UserDescription;

                    // Інфо про тваринку
                    PetInfo.Text = $"{_request.PetType}, {_request.PetName}, {_request.PetAge}, {_request.PetGender}";
                }
            }
        }

        private async void OnApprove(object sender, EventArgs e)
        {
            if (_request != null)
            {
                await _adoptionService.UpdateStatusAsync(_request.Id, AdoptionStatus.Підтверджено);
                await DisplayAlert("Успіх", "Заявку підтверджено", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void OnReject(object sender, EventArgs e)
        {
            if (_request != null)
            {
                await _adoptionService.UpdateStatusAsync(_request.Id, AdoptionStatus.Відхилено);
                await DisplayAlert("Готово", "Заявку відхилено", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void OnMarkAdopted(object sender, EventArgs e)
        {
            if (_request != null)
            {
                await _adoptionService.MarkPetAsAdoptedAsync(_request.PetId);
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
