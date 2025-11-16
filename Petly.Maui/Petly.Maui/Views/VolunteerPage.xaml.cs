using Petly.Maui.Models;
using Petly.Maui.Services;

namespace Petly.Maui.Views;

public partial class VolunteerPage : ContentPage
{
    private readonly IVolunteerService _volunteerService;
    private readonly UserContext _userCtx;
    private bool _isAdmin;

    public VolunteerPage()
    {
        InitializeComponent();

        _volunteerService = GetService<IVolunteerService>()!;
        _userCtx = GetService<UserContext>()!;

        // заповнюю варіанти у пікерах (можеш змінити під свої реальні дані)
        ShelterPicker.ItemsSource = new[]
        {
            "Домівка",
            "ЛКП \"Лев\"",
            "Милосердя"
        };

        TypePicker.ItemsSource = new[]
        {
            "Прибирання / догляд",
            "Вигул тварин",
            "Фото / соцмережі",
            "Збір коштів / івенти"
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _isAdmin = _userCtx?.IsAdmin ?? false;

        // Адмін бачить список, юзер – форму
        UserFormScroll.IsVisible = !_isAdmin;
        UserFormScroll.IsEnabled = !_isAdmin;
        AdminListLayout.IsVisible = _isAdmin;
        AdminListLayout.IsEnabled = _isAdmin;

        if (_isAdmin)
        {
            // список усіх заявок
            var all = await _volunteerService.GetAllAsync();
            RequestsList.ItemsSource = all;
        }
        else
        {
            // звичайному юзеру підставимо його дані, якщо він залогінений
            var acc = await _userCtx.GetCurrentAccountAsync();
            if (acc != null)
            {
                FirstNameEntry.Text = acc.FirstName;
                LastNameEntry.Text = acc.LastName;
                EmailEntry.Text = acc.Email;
            }
        }
    }

    // Надіслати заявку (для звичайного користувача)
    private async void OnSendClicked(object sender, EventArgs e)
    {
        if (_isAdmin) return; // про всяк випадок

        var first = FirstNameEntry.Text?.Trim() ?? "";
        var last = LastNameEntry.Text?.Trim() ?? "";
        var email = EmailEntry.Text?.Trim() ?? _userCtx.Email ?? "";
        var phone = PhoneEntry.Text?.Trim() ?? "";
        var shelter = ShelterPicker.SelectedItem?.ToString() ?? "";
        var type = TypePicker.SelectedItem?.ToString() ?? "";

        if (string.IsNullOrWhiteSpace(first) ||
            string.IsNullOrWhiteSpace(last) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(shelter) ||
            string.IsNullOrWhiteSpace(type))
        {
            await DisplayAlert("Помилка",
                "Будь ласка, заповни всі обов’язкові поля.", "OK");
            return;
        }

        var req = new VolunteerRequest
        {
            FirstName = first,
            LastName = last,
            Email = email,
            Phone = phone,
            Shelter = shelter,
            Type = type
        };

        await _volunteerService.AddRequestAsync(req);

        await DisplayAlert("Дякуємо", "Заявку на волонтерство надіслано!", "OK");

        // очистити форму
        FirstNameEntry.Text = "";
        LastNameEntry.Text = "";
        // email залишаю – зручно
        PhoneEntry.Text = "";
        ShelterPicker.SelectedIndex = -1;
        TypePicker.SelectedIndex = -1;
    }

    // Перехід на деталі заявки (для адміна)
    private async void OnRequestSelected(object sender, SelectionChangedEventArgs e)
    {
        if (!_isAdmin) return;

        if (e.CurrentSelection.FirstOrDefault() is VolunteerRequest req)
        {
            await Shell.Current.GoToAsync($"admin/volunteer/details?id={req.Id}");
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    private static T? GetService<T>() where T : class =>
        Application.Current?.Handler?.MauiContext?.Services?.GetService<T>();
}
