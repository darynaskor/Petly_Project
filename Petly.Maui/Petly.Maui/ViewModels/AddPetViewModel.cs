using CommunityToolkit.Maui.Alerts;       // Для Toast
using CommunityToolkit.Maui.Core;         // Для ToastDuration
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Petly.Maui.Models;
using Petly.Maui.Services;                // ✅ Додано для JsonRepository
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Petly.Maui.ViewModels
{
    public partial class AddPetViewModel : BaseViewModel
    {
        // ✅ Ініціалізуємо об’єкт одразу
        [ObservableProperty]
        Pet pet = new Pet();

        public AddPetViewModel()
        {
            Title = "Add Pet";
        }

        [RelayCommand]
        private async Task SavePet()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                // ✅ --- ВАЛІДАЦІЯ ---
                var context = new ValidationContext(Pet);
                var results = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(Pet, context, results, true);

                if (!isValid)
                {
                    string errors = string.Join("\n", results.Select(r => $"• {r.ErrorMessage}"));
                    await Shell.Current.DisplayAlert("Помилка валідації", errors, "OK");
                    return;
                }

                // ✅ --- ЗБЕРЕЖЕННЯ у JSON ---
                var repo = new JsonRepository<Pet>("pets.json");
                var pets = await repo.LoadAsync();

                // Додаємо нову тваринку
                pets.Add(Pet);

                // Зберігаємо оновлений список
                await repo.SaveAsync(pets);

                // ✅ Повідомлення про успіх
                await Toast.Make("Тваринку успішно збережено!",
                    CommunityToolkit.Maui.Core.ToastDuration.Short)
                    .Show();

                // Повернення на попередню сторінку
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving pet: {ex.Message}");
                await Shell.Current.DisplayAlert("Помилка", "Не вдалося зберегти тваринку.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
