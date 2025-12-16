using CommunityToolkit.Mvvm.ComponentModel;

namespace Petly.Maui.ViewModels
{
    // ObservableObject - це частина CommunityToolkit.Mvvm
    // Він дозволяє UI автоматично оновлюватися, коли дані змінюються.
    public partial class BaseViewModel : ObservableObject
    {
        // Ми використовуємо [ObservableProperty] замість повного PropertyChanged
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))] // Оновлює IsNotBusy, коли IsBusy змінюється
        bool isBusy; // bool за замовчуванням 'false', тому тут все добре

        [ObservableProperty]
        string title = string.Empty; // <--- ВИПРАВЛЕНО ТУТ

        public bool IsNotBusy => !IsBusy;
    }
}