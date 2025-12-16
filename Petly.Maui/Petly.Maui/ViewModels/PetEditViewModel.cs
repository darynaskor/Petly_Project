using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Petly.Maui.Models;
using System.Diagnostics;

namespace Petly.Maui.ViewModels
{
    public partial class PetEditViewModel : BaseViewModel
    {
        [ObservableProperty]
        private Pet editablePet = new(); // ✅ ініціалізація

        public PetEditViewModel()
        {
            Title = "Edit Pet";
        }

        [RelayCommand]
        private async Task SaveChangesAsync()
        {
            if (EditablePet == null)
                return;

            Debug.WriteLine($"Saving changes for {EditablePet.pet_name}");

            await Shell.Current.DisplayAlert(
                "Збережено",
                $"Зміни для {EditablePet.pet_name} збережено успішно!",
                "OK");

            await Shell.Current.GoToAsync("..");
        }
    }
}
