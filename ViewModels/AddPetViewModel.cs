using CommunityToolkit.Maui.Alerts; // Це для 'Toast'
using CommunityToolkit.Maui.Core;    // Це для 'ToastDuration'
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Petly.Maui.Models;
using System.Diagnostics;

namespace Petly.Maui.ViewModels
{
    public partial class AddPetViewModel : BaseViewModel
    {
        // Цей атрибут створює об'єкт Pet
        // і автоматично повідомляє UI про зміни
        [ObservableProperty]
        Pet pet;

        public AddPetViewModel()
        {
            Title = "Add Pet";
            // Створюємо нову, порожню тварину для заповнення форми
            Pet = new Pet();
        }

        [RelayCommand]
        private async Task SavePet()
        {
            if (string.IsNullOrWhiteSpace(Pet.pet_name) || string.IsNullOrWhiteSpace(Pet.type))
            {
                // Використовуємо Toast з CommunityToolkit
                await Toast.Make("Please fill in all required fields.",
                    CommunityToolkit.Maui.Core.ToastDuration.Short)
                    .Show();
                return;
            }

            if (IsBusy) return;

            try
            {
                IsBusy = true;

                // ---
                // Тут буде твоя логіка збереження в базу даних
                // ---
                Debug.WriteLine($"Saving pet: {Pet.pet_name}, {Pet.type}");

                // Повідомлення про успіх
                await Toast.Make("Pet saved successfully!", 
                    CommunityToolkit.Maui.Core.ToastDuration.Short)
                    .Show();

                // Повернення на головну сторінку
                // ".." - це команда Shell для "повернутися назад"
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving pet: {ex.Message}");

                // ---- ВИПРАВЛЕНО ТУТ ----
                await Shell.Current.DisplayAlert("Error", "Could not save pet.", "OK");
                // -------------------------
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}