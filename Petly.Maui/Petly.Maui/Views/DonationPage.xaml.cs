using Petly.Maui.Models;
using Petly.Maui.Services;

namespace Petly.Maui.Views;

public partial class DonationPage : ContentPage
{
    private readonly IDonationService _donationService;
    private readonly UserContext? _userCtx;
    private readonly bool _isAdmin;

    public DonationPage()
    {
        InitializeComponent();

        // пробуємо взяти сервіси з DI
        _donationService = GetService<IDonationService>() ?? new DonationService();
        _userCtx = GetService<UserContext>();

        _isAdmin = _userCtx?.IsAdmin ?? false;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            // якщо адмін – показуємо список, якщо ні – форму
            UserFormLayout.IsVisible = !_isAdmin;
            AdminListLayout.IsVisible = _isAdmin;

            if (_isAdmin)
            {
                var list = await _donationService.GetAllAsync();
                DonationsList.ItemsSource = list;
            }
        }
        catch (Exception ex)
        {
            // щоб не падав додаток, а показував помилку
            await DisplayAlert("Помилка", ex.Message, "OK");
        }
    }

    private async void OnDonateClicked(object sender, EventArgs e)
    {
        try
        {
            var owner = OwnerNameEntry.Text?.Trim() ?? "";
            var email = EmailEntry.Text?.Trim() ?? "";
            var phone = PhoneEntry.Text?.Trim() ?? "";
            var card = CardNumberEntry.Text?.Trim() ?? "";
            var purpose = PurposePicker.SelectedItem?.ToString() ?? "Будь-які потреби";
            var amountText = AmountEntry.Text?.Trim() ?? "0";

            if (string.IsNullOrWhiteSpace(owner) || string.IsNullOrWhiteSpace(email))
            {
                await DisplayAlert("Помилка", "Вкажіть ПІБ та email.", "OK");
                return;
            }

            if (!decimal.TryParse(amountText, out var amount) || amount <= 0)
            {
                await DisplayAlert("Помилка", "Вкажіть коректну суму.", "OK");
                return;
            }

            string last4 = card.Length >= 4 ? card[^4..] : card;

            var donation = new Donation
            {
                OwnerName = owner,
                Email = email,
                Phone = phone,
                Purpose = purpose,
                Amount = amount,
                CardLast4 = last4,
                UserEmail = _userCtx?.Email
            };

            await _donationService.AddAsync(donation);

            await DisplayAlert("Дякуємо!", "Ваш внесок збережено.", "OK");

            // очистити форму
            CardNumberEntry.Text = "";
            CardExpiryEntry.Text = "";
            OwnerNameEntry.Text = "";
            CvvEntry.Text = "";
            EmailEntry.Text = "";
            PhoneEntry.Text = "";
            AmountEntry.Text = "";
            PurposePicker.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка", ex.Message, "OK");
        }
    }

    private static T? GetService<T>() where T : class =>
        Application.Current?.Handler?.MauiContext?.Services?.GetService<T>();
}
