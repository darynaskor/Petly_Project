using Petly.Maui.Models;
using Petly.Maui.Services;
using CommunityToolkit.Mvvm.Messaging;

namespace Petly.Maui.Views;

public partial class AdoptionPage : ContentPage
{
    private readonly IAdoptionService _adoptionService;
    private AdoptionRequest? _userRequest;

    public AdoptionPage(IAdoptionService adoptionService)
    {
        InitializeComponent();
        _adoptionService = adoptionService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            // TODO: замініть на реальний userId з вашої Auth системи
            string currentUserId = "current_user_id";
            _userRequest = await _adoptionService.GetUserRequestAsync(currentUserId);

            if (_userRequest != null)
            {
                // Ховаємо форму, показуємо статус
                FormStack.IsVisible = false;
                StatusLabel.IsVisible = true;
                StatusLabel.Text = $"Статус вашої заявки: {_userRequest.StatusEnum}";

                if (_userRequest.StatusEnum == AdoptionStatus.Підтверджено)
                    StatusLabel.Text += "\nВаша заявка підтверджена!";
                else if (_userRequest.StatusEnum == AdoptionStatus.Відхилено)
                    StatusLabel.Text += "\nВашу заявку відхилено.";
            }
            else
            {
                // Якщо заявки немає, показуємо форму
                FormStack.IsVisible = true;
                StatusLabel.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка", $"Не вдалося завантажити дані: {ex.Message}", "OK");
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//home");
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        try
        {
            var request = new AdoptionRequest
            {
                UserId = "current_user_id", // TODO: реальний userId
                FirstName = FirstNameEntry.Text ?? "",
                LastName = LastNameEntry.Text ?? "",
                Email = EmailEntry.Text ?? "",
                Phone = PhoneEntry.Text ?? "",
                UserDescription = DescriptionEntry.Text ?? "",
                PetName = PetNameEntry.Text ?? "",
                PetType = PetTypeEntry.Text ?? "",
                PetAge = PetAgeEntry.Text ?? "",
                PetGender = PetGenderPicker.SelectedItem?.ToString() ?? ""
            };

            if (string.IsNullOrWhiteSpace(request.FirstName) ||
                string.IsNullOrWhiteSpace(request.LastName) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Phone) ||
                string.IsNullOrWhiteSpace(request.PetName) ||
                string.IsNullOrWhiteSpace(request.PetType) ||
                string.IsNullOrWhiteSpace(request.PetAge) ||
                string.IsNullOrWhiteSpace(request.PetGender))
            {
                await DisplayAlert("Помилка", "Заповніть всі обов'язкові поля", "OK");
                return;
            }

            await _adoptionService.AddRequestAsync(request);
            WeakReferenceMessenger.Default.Send(new AdoptionRequestAddedMessage(request));

            await DisplayAlert("Успіх", "Заявку надіслано!", "OK");
            await Shell.Current.GoToAsync("//home");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Помилка", ex.Message, "OK");
        }
    }
}

// Повідомлення для WeakReferenceMessenger
public class AdoptionRequestAddedMessage
{
    public AdoptionRequest Request { get; }
    public AdoptionRequestAddedMessage(AdoptionRequest request)
    {
        Request = request;
    }
}
