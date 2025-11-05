using CommunityToolkit.Mvvm.ComponentModel;
using Petly.Maui.Models;

namespace Petly.Maui.ViewModels
{
    public partial class PetDetailsViewModel : BaseViewModel
    {
        [ObservableProperty]
        private Pet selectedPet = new(); // ✅ додано ініціалізацію

        public PetDetailsViewModel()
        {
            Title = "Pet Details";
        }

        public void LoadPet(Pet pet)
        {
            SelectedPet = pet;
        }
    }
}
