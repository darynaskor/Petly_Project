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

namespace Petly.Maui.ViewModels
{
    public partial class PetsListViewModel : ObservableObject
    {
        private readonly PetService _petService;
        private List<PetCard> _allPets = new();
        private bool _isInitialized;

        public ObservableCollection<PetCard> PetsCollection { get; } = new();

        [ObservableProperty]
        private string searchQuery = string.Empty;

        public PetsListViewModel(PetService petService)
        {
            _petService = petService;
        }

        public async Task InitializeAsync()
        {
            if (_isInitialized)
                return;

            await LoadPetsAsync();
            _isInitialized = true;
        }

        private async Task LoadPetsAsync()
        {
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

                ApplyFilter(p => true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[PetsListViewModel] Failed to load pets: {ex.Message}");
            }
        }

        private void ApplyFilter(Func<PetCard, bool> predicate)
        {
            PetsCollection.Clear();
            foreach (var pet in _allPets.Where(predicate))
                PetsCollection.Add(pet);
        }

        // 🔹 Команди фільтрації
        [RelayCommand]
        private void FilterAll() => ApplyFilter(p => true);

        [RelayCommand]
        private void FilterCats() => ApplyFilter(p =>
            p.Type.Contains("Кіт", StringComparison.OrdinalIgnoreCase));

        [RelayCommand]
        private void FilterDogs() => ApplyFilter(p =>
            p.Type.Contains("Собака", StringComparison.OrdinalIgnoreCase));

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
        private async Task MoreInfo(PetCard pet)
        {
            await Shell.Current.GoToAsync("petdetails"); // 🔹 замість nameof(PetDetailsPage)
        }

        // 🔹 Кнопка “Допомога”
        [RelayCommand]
        private async Task HelpPet(PetCard pet)
        {
            await Shell.Current.GoToAsync("donation");
        }

        [RelayCommand]
        private async Task AdoptPet(PetCard pet)
        {
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
