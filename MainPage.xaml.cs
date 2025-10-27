using Microsoft.Maui.Controls;
using Petly.Maui.Views;
namespace Petly.Maui
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
//адміни
        // Обробник кнопки "Редагувати дані"
        private void EditProfileButton_Clicked(object sender, EventArgs e)
        {
            // Тут можна відкрити нову сторінку для редагування даних користувача
            DisplayAlert("Редагування", "Функціонал редагування профілю ще не реалізовано", "OK");
        }

        // Обробник кнопки "Історія внесків"
        private void ContributionHistoryButton_Clicked(object sender, EventArgs e)
        {
            // Відкриваємо сторінку історії внесків
            DisplayAlert("Історія внесків", "Показати історію внесків користувача", "OK");
        }

        // Обробник кнопки "Історія волонтерства"
        private void VolunteerHistoryButton_Clicked(object sender, EventArgs e)
        {
            // Відкриваємо сторінку історії волонтерства
            DisplayAlert("Історія волонтерства", "Показати історію волонтерства користувача", "OK");
        }
        // Обробник кнопки "Переглянути тварин" для юзерів
        private async void PetsButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("petlist");
        }
    }
}
