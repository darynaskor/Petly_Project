using CommunityToolkit.Mvvm.Input;
using Petly.Maui.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Petly.Maui.ViewModels
{
    // Наслідуємо від BaseViewModel
    public partial class PetsListViewModel : BaseViewModel
    {
        // ObservableCollection - це список, який автоматично
        // повідомляє CollectionView про додавання/видалення елементів
        public ObservableCollection<Pet> PetsCollection { get; } = new();

        public PetsListViewModel()
        {
            Title = "My Pets";
            LoadPetsAsync(); // Завантажуємо дані при створенні
        }

        // Цей метод заповнює список (поки що тестовими даними)
        void LoadPetsAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                
                // Тимчасові дані, щоб ти побачила щось на екрані
                var tempPets = new List<Pet>
                {
                    new Pet {
                        pet_id = 1,
                        pet_name = "Simba",
                        type = "Cat",
                        age = 3,
                        gender = "Male",
                        description = "A playful kitten",
                        health = "Healthy",
                        photourl = "https://example.com/simba.png",
                        shelter_id = 101,
                        status = true
                    },
                    new Pet {
                        pet_id = 2,
                        pet_name = "Polly",
                        type = "Dog",
                        age = 5,
                        gender = "Female",
                        description = "A playful dog",
                        health = "Healthy",
                        photourl = "https://example.com/polly.png",
                        shelter_id = 102,
                        status = false
                    },
                };

                // Очищуємо колекцію і додаємо нові дані
                PetsCollection.Clear();
                foreach (var pet in tempPets)
                {
                    PetsCollection.Add(pet);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading pets: {ex.Message}");
                // Тут можна показати Alert
            }
            finally
            {
                IsBusy = false;
            }
        }

        // --- КОМАНДИ ДЛЯ КОНТЕКСТНОГО МЕНЮ ---
        // Атрибут [RelayCommand] автоматично створює ICommand
        // (напр. EditPetCommand) для прив'язки у XAML

        [RelayCommand]
        private static void EditPet(Pet pet)
        {
            if (pet == null)
                return;

            Debug.WriteLine($"Editing pet: {pet.pet_name}");
            // Логіка переходу на сторінку редагування
            // await Shell.Current.GoToAsync($"{nameof(PetEditPage)}?PetId={pet.Id}");
        }
        [RelayCommand]
        private async Task DeletePet(Pet pet)
        {
            if (pet == null)
                return;

            // ---- ВИПРАВЛЕНО ТУТ ----
            // Використовуємо Shell.Current замість Application.Current.MainPage
            bool answer = await Shell.Current.DisplayAlert(
                "Confirm Delete",
                $"Are you sure you want to delete {pet.pet_name}?",
                "Yes, Delete",
                "Cancel");
            // -------------------------

            if (answer)
            {
                PetsCollection.Remove(pet);
                Debug.WriteLine($"Deleted pet: {pet.pet_name}");
            }
        }

        [RelayCommand]
        private void MarkAdopted(Pet pet)
        {
            if (pet == null)
                return;
            
            pet.status = true;
            Debug.WriteLine($"Marked {pet.pet_name} as adopted.");
            // Тут можна оновити вигляд картки або видалити зі списку
        }
    }
}