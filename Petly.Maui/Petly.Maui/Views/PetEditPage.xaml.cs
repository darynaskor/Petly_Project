using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Petly.Maui.Models;
using Petly.Maui.Services;

namespace Petly.Maui.Views;

public partial class PetEditPage : ContentPage, IQueryAttributable
{
    private readonly PetService _petService;
    private readonly UserContext _userCtx;

    private ObservableCollection<Pet> _pets = new();
    private Pet? _current;    // редагована тваринка (або null для нової)

    public PetEditPage()
    {
        InitializeComponent();

        _petService = GetService<PetService>()!;
        _userCtx = GetService<UserContext>()!;

        // дозволено лише адмінам
        if (!_userCtx.IsAdmin)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Доступ", "Ця сторінка лише для адміністратора.", "OK");
                await Shell.Current.GoToAsync("//pets");
            });
        }
    }

    // приймаємо параметр id (коли редагування існуючої анкети)
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var raw) &&
            raw is string id &&
            !string.IsNullOrWhiteSpace(id))
        {
            // асинхронно підгружаємо дані
            _ = LoadExistingAsync(id);
        }
        else
        {
            // нова анкета
            _current = null;
            DeleteBtn.IsVisible = false;
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _pets = await _petService.LoadAsync();
    }

    private async Task LoadExistingAsync(string idString)
    {
        // PetService зберігає pet_id як int — idString очікуємо у форматі int
        if (!int.TryParse(idString, out var id))
            return;

        var list = await _petService.LoadAsync();

        _current = list.FirstOrDefault(p => p.pet_id == id);
        if (_current is null)
        {
            DeleteBtn.IsVisible = false;
            return;
        }

        NameEntry.Text = _current.pet_name;
        TypeEntry.Text = _current.type;
        AgeEntry.Text = _current.age.ToString();
        GenderEntry.Text = _current.gender;
        DescEditor.Text = _current.description ?? "";
        PhotoEntry.Text = _current.photourl ?? "";
        PhotoPreview.Source = _current.photourl;

        // одразу оновлюємо смайлик статі
        UpdateGenderEmoji(_current.gender);

        DeleteBtn.IsVisible = true;
    }

    // ЗБЕРЕГТИ
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var name = NameEntry.Text?.Trim() ?? "";
        var type = TypeEntry.Text?.Trim() ?? "";
        var gender = GenderEntry.Text?.Trim() ?? "";
        var desc = DescEditor.Text?.Trim() ?? "";
        var photo = PhotoEntry.Text?.Trim();

        if (!int.TryParse(AgeEntry.Text, out var age))
            age = 0;

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(type))
        {
            await DisplayAlert("Помилка", "Заповніть ім'я та вид тварини.", "OK");
            return;
        }

        // нормалізуємо стать (male / female / невідомо)
        string normalizedGender;
        var g = gender.ToLowerInvariant();
        if (g == "male" || g == "m" || g == "ч" || g.Contains("самець"))
            normalizedGender = "male";
        else if (g == "female" || g == "f" || g == "ж" || g.Contains("самка"))
            normalizedGender = "female";
        else
            normalizedGender = "unknown";

        if (_current is null) // створення нової
        {
            var p = new Pet
            {
                // pet_id задасть репозиторій (інкремент) або sqlite, якщо є
                pet_name = name,
                type = type,
                age = age,
                gender = normalizedGender,
                description = string.IsNullOrWhiteSpace(desc) ? null : desc,
                photourl = string.IsNullOrWhiteSpace(photo) ? null : photo,
                status = "available",
                shelter_id = 1 // TODO: підв'язати вибір притулку
            };

            await _petService.AddAsync(_pets, p);
        }
        else // оновлення існуючої
        {
            _current.pet_name = name;
            _current.type = type;
            _current.age = age;
            _current.gender = normalizedGender;
            _current.description = string.IsNullOrWhiteSpace(desc) ? null : desc;
            _current.photourl = string.IsNullOrWhiteSpace(photo) ? null : photo;

            await _petService.UpdateAsync(_pets, _current);
        }

        await DisplayAlert("Готово", "Анкету збережено.", "OK");
        await Shell.Current.GoToAsync("//pets");
    }

    // ВИДАЛИТИ
    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (_current is null) return;

        var ok = await DisplayAlert("Підтвердження", "Видалити цю анкету?", "Так", "Ні");
        if (!ok) return;

        await _petService.DeleteAsync(_pets, _current.pet_id);
        await Shell.Current.GoToAsync("//pets");
    }

    // СКАСУВАТИ
    private async void OnCancelClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("//pets");

    // Зміна URL фото — оновлюємо превʼю
    private void OnPhotoUrlCompleted(object sender, EventArgs e)
        => PhotoPreview.Source =
            string.IsNullOrWhiteSpace(PhotoEntry.Text) ? null : PhotoEntry.Text;

    // ---------- Стать + смайлик ----------

    private void OnGenderChanged(object sender, TextChangedEventArgs e)
        => UpdateGenderEmoji(e.NewTextValue);

    private void UpdateGenderEmoji(string? gender)
    {
        var g = gender?.Trim().ToLowerInvariant();

        if (g == "male" || g == "m" || g == "ч" || (g?.Contains("самець") ?? false))
            GenderEmoji.Text = "♂️";
        else if (g == "female" || g == "f" || g == "ж" || (g?.Contains("самка") ?? false))
            GenderEmoji.Text = "♀️";
        else
            GenderEmoji.Text = string.Empty;
    }

    // DI helper
    private static T? GetService<T>() where T : class =>
        Application.Current?.Handler?.MauiContext?.Services?.GetService<T>();
}
