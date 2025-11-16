using Petly.Maui.Models;
using Petly.Maui.Services;
using Microsoft.Maui.Controls;

namespace Petly.Maui.Views
{
    [QueryProperty(nameof(RequestId), "id")]
    public partial class AdminVolunteerDetailsPage : ContentPage
    {
        private readonly IVolunteerService _volunteerService;
        private readonly UserContext _userCtx;

        private VolunteerRequest? _request;

        public AdminVolunteerDetailsPage(IVolunteerService volunteerService)
        {
            InitializeComponent();
            _volunteerService = volunteerService;

            // DI для UserContext
            _userCtx = GetService<UserContext>()!;
        }

        private string requestId = string.Empty;

        public string RequestId
        {
            get => requestId;
            set
            {
                requestId = value;
                LoadRequest(value);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // 🔐 Перевірка доступу
            await _userCtx.LoadCurrentUserAsync();

            if (!_userCtx.IsAdmin)
            {
                await DisplayAlert("Доступ", "Ця сторінка доступна тільки адміністратору.", "OK");
                await Shell.Current.GoToAsync("//home");
                return;
            }
        }

        private async void LoadRequest(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return;

            _request = await _volunteerService.GetByIdAsync(id);

            if (_request != null)
            {
                UserInfoLabel.Text = $"{_request.FirstName} {_request.LastName}\n" +
                                     $"Email: {_request.Email}\n" +
                                     $"Телефон: {_request.Phone}";

                VolunteerInfoLabel.Text = $"Притулок: {_request.Shelter}\nТип: {_request.Type}";

                StatusLabel.Text = _request.StatusEnum.ToString();
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnApproveClicked(object sender, EventArgs e)
        {
            if (_request == null) return;

            await _volunteerService.UpdateStatusAsync(_request.Id, VolunteerStatus.Підтверджено);
            StatusLabel.Text = "Підтверджено";

            // 🟢 Додаємо користувачу статус "волонтер"
            await _userCtx.MarkAsVolunteerAsync(_request.Email);

            await DisplayAlert("Успіх", "Заявку підтверджено. Користувач тепер волонтер.", "OK");
            await Shell.Current.GoToAsync("..");
        }

        private async void OnRejectClicked(object sender, EventArgs e)
        {
            if (_request == null) return;

            await _volunteerService.UpdateStatusAsync(_request.Id, VolunteerStatus.Відхилено);
            StatusLabel.Text = "Відхилено";

            await DisplayAlert("Готово", "Заявку відхилено.", "OK");
            await Shell.Current.GoToAsync("..");
        }

        // DI helper
        private static T? GetService<T>() where T : class =>
            Application.Current?.Handler?.MauiContext?.Services?.GetService<T>();
    }
}
