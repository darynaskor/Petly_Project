using Petly.Maui.Models;
using Petly.Maui.Services;

namespace Petly.Maui.Views;

public partial class DonationPage : ContentPage
{
    private readonly IDonationService _donationService;
    private readonly UserContext _userCtx;
    private bool _isAdmin;

    public DonationPage()
    {
        InitializeComponent();

        _donationService = GetService<IDonationService>()!;
        _userCtx = GetService<UserContext>()!;

        // варіанти призначення – можеш змінити
        PurposePicker.ItemsSource = new[]
        {
            "Загальна підтримка притулків",
            "Ліки та лікування",
            "Корм та базові потреби",
            "Ремонт / покращення умов"
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _isAdmin = _userCtx?.IsAdmin ?? false;

        // для звичайного користувача – лише форма
        UserFormScroll.IsVisible = !_isAdmin;
        UserFormScroll.IsEnabled = !_isAdmin;

        // для адміна – лише список внесків
        AdminDonationsLayout.IsVisible = _isAdmin;
        AdminDonationsLayout.IsEnabled = _isAdmin;

        if (_isAdmin)
        {
            // завантажити всі внески
            var all = await _donationService.GetAllAsync();
            DonationsList.ItemsSource = all;
        }
        else
        {
            // підтягнути дані юзера, якщо є
            var acc = await _userCtx.GetCurrentAccountAsync();
            if (acc != null)
            {
                OwnerNameEntry.Text = $"{acc.FirstName} {acc.LastName}".Trim();
                EmailEntry.Text = acc.Email;
            }
        }
    }

    // Надсилання внеску – тільки для звичайних користувачів
    private async void OnDonateClicked(object sender, EventArgs e)
    {
        if (_isAdmin) return; // про всяк випадок

        var card = CardNumberEntry.Text?.Trim() ?? "";
        var owner = OwnerNameEntry.Text?.Trim() ?? "";
        var email = EmailEntry.Text?.Trim() ?? _userCtx.Email ?? "";
        var phone = PhoneEntry.Text?.Trim() ?? "";
        var expiry = ExpiryEntry.Text?.Trim() ?? "";
        var cvv = CvvEntry.Text?.Trim() ?? "";
        var amountText = AmountEntry.Text?.Trim() ?? "";
        var purpose = PurposePicker.SelectedItem?.ToString() ?? "";

        if (!decimal.TryParse(amountText, out var amount) || amount <= 0 ||
            string.IsNullOrWhiteSpace(card) ||
            string.IsNullOrWhiteSpace(owner) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(purpose))
        {
            await DisplayAlert("Помилка",
                "Перевір, будь ласка, дані картки, суму та призначення.", "OK");
            return;
        }

        var donation = new Donation
        {
            OwnerName = owner,
            Email = email,
            Phone = phone,
            Amount = amount,
            Purpose = purpose,
            CreatedAt = DateTime.Now
        };

        await _donationService.AddAsync(donation);

        await DisplayAlert("Дякуємо!", "Ваш внесок збережено.", "OK");

        // очищаємо суму, щоб можна було ввести нову
        AmountEntry.Text = "";
        PurposePicker.SelectedIndex = -1;
        CvvEntry.Text = "";
        ExpiryEntry.Text = "";
    }

    private static T? GetService<T>() where T : class =>
        Application.Current?.Handler?.MauiContext?.Services?.GetService<T>();
}
