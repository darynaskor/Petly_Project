using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Petly.Maui.Views;

namespace Petly.Maui.ViewModels
{
    public partial class PetsListViewModel : ObservableObject
    {
        // 🔹 Усі тварини
        private List<Pet> _allPets = new();

        // 🔹 Видимий список
        public ObservableCollection<Pet> PetsCollection { get; } = new();

        [ObservableProperty]
        private string searchQuery = string.Empty;

        public PetsListViewModel()
        {
            LoadPets();
        }

        // 🔹 Статичні дані (тестові)
        private void LoadPets()
        {
            _allPets = new List<Pet>
            {
                new() { PetName = "Томас", Type = "Кіт", Age = 12, Status = "available",
                        Description = "Ніжний і лагідний кіт з аристократичною зовнішністю.",
                        PhotoUrl = "https://pesyk.kiev.ua/wp-content/uploads/Ryzhie-britanskie-koshki-2.jpg" },

                new() { PetName = "Рік", Type = "Собака", Age = 6, Status = "available",
                        Description = "Вірний пес, який обожнює прогулянки та дітей.",
                        PhotoUrl = "https://www.tierschutzbund.de/fileadmin/_processed_/7/c/csm_schwarzer_Hund_auf_Wiese_c_xkunclova-Shutterstock_01_5566a80d25.jpg" },

                new() { PetName = "Голді", Type = "Собака", Age = 5, Status = "available",
                        Description = "Весела, розумна та слухняна — справжня подруга для сім’ї.",
                        PhotoUrl = "https://image.petmd.com/files/styles/978x550/public/2024-08/dogs-for-first-time-owners.jpg" },

                new() { PetName = "Мурчик", Type = "Кіт", Age = 9, Status = "adopted",
                        Description = "Маленький пустун, лагідний і дуже грайливий.",
                        PhotoUrl = "https://people.com/thmb/xHPJAus5iELyf5ndsPJ84GeJTwI=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc():focal(694x160:696x162)/cat-study-110223-1-efc838c9067349ab82ac24abc4cc2de5.jpg" }
            };

            ApplyFilter(p => true);
        }

        // 🔹 Метод фільтрації
        private void ApplyFilter(Func<Pet, bool> predicate)
        {
            PetsCollection.Clear();
            foreach (var pet in _allPets.Where(predicate))
                PetsCollection.Add(pet);
        }

        // 🔹 Команди фільтрації
        [RelayCommand]
        private void FilterAll() => ApplyFilter(p => true);

        [RelayCommand]
        private void FilterCats() => ApplyFilter(p => p.Type.Contains("Кіт", StringComparison.OrdinalIgnoreCase));

        [RelayCommand]
        private void FilterDogs() => ApplyFilter(p => p.Type.Contains("Собака", StringComparison.OrdinalIgnoreCase));

        [RelayCommand]
        private void FilterAdopted() => ApplyFilter(p => p.Status == "adopted");

        // 🔹 Команда пошуку
        [RelayCommand]
        private void Search()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                FilterAll();
                return;
            }

            var results = _allPets
                .Where(p => p.PetName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();

            PetsCollection.Clear();
            foreach (var pet in results)
                PetsCollection.Add(pet);
        }

        // 🔹 Кнопка “Більше”
       [RelayCommand]
        private async Task MoreInfo(object pet)
        {
            await Shell.Current.GoToAsync("petdetails"); // 🔹 замість nameof(PetDetailsPage)
        }

        // 🔹 Кнопка “Допомога”
        [RelayCommand]
        private async Task HelpPet(object pet)
        {
            await Shell.Current.GoToAsync("donation");
        }

        [RelayCommand]
        private async Task AdoptPet(object pet)
        {
            await Shell.Current.GoToAsync("adoption");
        }

    }

    // 🔹 Клас моделі тварини
    public class Pet
    {
        public string PetName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
    }
}
