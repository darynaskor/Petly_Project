using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using AnimalShelter.BLL.Services;
using Petly.Maui.Views;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; 

namespace Petly.Maui.ViewModels
{
    public partial class PetsListViewModel : ObservableObject
    {
        private readonly PetService _petService;
        private readonly ILogger<PetsListViewModel> _logger; 

        private List<PetCard> _allPets = new();
        private bool _isInitialized;

        public ObservableCollection<PetCard> PetsCollection { get; } = new();

        [ObservableProperty]
        private string searchQuery = string.Empty;

        public PetsListViewModel(PetService petService, ILogger<PetsListViewModel> logger)
        {
            _petService = petService;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            if (_isInitialized)
            {
                _logger.LogDebug("Список тварин вже ініціалізований. Пропуск.");
                return;
            }

            await LoadPetsAsync();
            _isInitialized = true;
        }

        private async Task LoadPetsAsync()
        {
            _logger.LogInformation("Початок завантаження списку тварин із сервера...");

            try
            {
                var pets = await _petService.GetAllPetsAsync();

                _allPets = pets.Select(p => new PetCard
                {
                    Id = p.pet_id,
                    PetName = p.pet_name ?? string.Empty,
                    Type = p.type ?? string.Empty,
                    Age = p.age,
                    Status = p.status ?? string.Empty,
                    Description = p.description ?? string.Empty,
                    PhotoUrl = p.photourl ?? string.Empty
                }).ToList();

                _logger.LogInformation("Успішно завантажено {Count} тварин.", _allPets.Count);

                ApplyFilter(p => true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Критична помилка при завантаженні списку тварин.");
                await Shell.Current.DisplayAlert("Помилка", "Не вдалося завантажити дані", "OK");
            }
        }

        private void ApplyFilter(Func<PetCard, bool> predicate)
        {
            PetsCollection.Clear();
            var filtered = _allPets.Where(predicate).ToList();

            foreach (var pet in filtered)
                PetsCollection.Add(pet);

            _logger.LogDebug("Застосовано фільтр. Відображається {Count} записів.", filtered.Count);
        }

        // 🔹 Команди фільтрації
        [RelayCommand]
        private void FilterAll()
        {
            _logger.LogInformation("Користувач обрав фільтр: Всі");
            ApplyFilter(p => true);
        }

        [RelayCommand]
        private void FilterCats()
        {
            _logger.LogInformation("Користувач обрав фільтр: Коти");
            ApplyFilter(p => p.Type.Contains("Кіт", StringComparison.OrdinalIgnoreCase));
        }

        [RelayCommand]
        private void FilterDogs()
        {
            _logger.LogInformation("Користувач обрав фільтр: Собаки");
            ApplyFilter(p => p.Type.Contains("Собака", StringComparison.OrdinalIgnoreCase));
        }

        [RelayCommand]
        private void FilterAdopted()
        {
            _logger.LogInformation("Користувач обрав фільтр: Вже в родині");
            ApplyFilter(p => p.Status == "adopted");
        }

        // 🔹 Команда пошуку
        [RelayCommand]
        private void Search()
        {
            _logger.LogInformation("Виконується пошук за запитом: '{Query}'", SearchQuery);

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

            _logger.LogInformation("Знайдено {Count} тварин за запитом '{Query}'.", results.Count, SearchQuery);
        }

        [RelayCommand]
        private async Task MoreInfo(PetCard pet)
        {
            if (pet == null) return;
            _logger.LogInformation("Перехід до деталей тварини: {Name} (ID: {Id})", pet.PetName, pet.Id);

            await Shell.Current.GoToAsync("petdetails");
        }

        [RelayCommand]
        private async Task HelpPet(PetCard pet)
        {
            _logger.LogInformation("Натиснуто 'Допомогти' для тварини: {Name}", pet?.PetName);
            await Shell.Current.GoToAsync("donation");
        }

        [RelayCommand]
        private async Task AdoptPet(PetCard pet)
        {
            _logger.LogInformation("Натиснуто 'Адопція' для тварини: {Name}", pet?.PetName);
            await Shell.Current.GoToAsync("adoption");
        }
    }

    public class PetCard
    {
        public int Id { get; set; }
        public string PetName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
    }
}