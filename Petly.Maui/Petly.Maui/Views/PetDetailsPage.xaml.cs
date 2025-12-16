using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace Petly.Maui.Views
{
    public partial class PetDetailsPage : ContentPage
    {
        private readonly List<PetInfo> _pets = new()
        {
            new PetInfo
            {
                Name = "Боня",
                Type = "Собака",
                Age = 2,
                Image = "https://woodgreen.org.uk/nitropack_static/xoJzNJaUrazkmGClTAEjQlzsnLULSpuy/assets/images/optimized/rev-4d83912/woodgreen.org.uk/wp-content/uploads/2025/09/Tommy.jpg",
                Description = "Активний, завжди готовий до пригод і довгих прогулянок"
            },
            new PetInfo
            {
                Name = "Лакі",
                Type = "Кіт",
                Age = 9,
                Image = "https://woodgreen.org.uk/nitropack_static/xoJzNJaUrazkmGClTAEjQlzsnLULSpuy/assets/images/optimized/rev-4d83912/woodgreen.org.uk/wp-content/uploads/2021/12/tabby_cat_in_the_garden-1400x0-c-default.jpeg",
                Description = "Самостійний, любить сам собі обирати час для ласки"
            },
            new PetInfo
            {
                Name = "Рекс",
                Type = "Кіт",
                Age = 5,
                Image = "https://woodgreen.org.uk/wp-content/uploads/2025/01/pud2-1980x1485.jpg",
                Description = "Дуже лагідний, постійно шукає тепла і уваги"
            },
            new PetInfo
            {
                Name = "Сніжок",
                Type = "Собака",
                Age = 9,
                Image = "https://woodgreen.org.uk/nitropack_static/xoJzNJaUrazkmGClTAEjQlzsnLULSpuy/assets/images/optimized/rev-4d83912/woodgreen.org.uk/wp-content/uploads/2024/12/117402-Jack-1400x0-c-default.jpg",
                Description = "Урівноважений, слухняний, легко навчається"
            },
            new PetInfo
            {
                Name = "Тайсон",
                Type = "Собака",
                Age = 10,
                Image = "https://woodgreen.org.uk/wp-content/uploads/2021/04/00822-MK-DH2-Website-Hero-Dog-Images_1920x1440_Trinity-v2-e1646724809257-768x443.jpg",
                Description = "Веселий, грайливий, обожнює бути в центрі уваги"
            },
            new PetInfo
            {
                Name = "Мурчик",
                Type = "Кіт",
                Age = 6,
                Image = "https://woodgreen.org.uk/wp-content/uploads/2025/11/LS.jpg",
                Description = "Спокійний, уважний, любить дивитися на все збоку."
            },
            new PetInfo
            {
                Name = "Барсик",
                Type = "Кіт",
                Age = 10,
                Image = "https://woodgreen.org.uk/wp-content/uploads/2025/11/Nosferatu.jpg",
                Description = "Спочатку осторонь, але швидко прив’язується і стає ласкавим."
            },
            new PetInfo
            {
                Name = "Рижик",
                Type = "Собака",
                Age = 6,
                Image = "https://woodgreen.org.uk/nitropack_static/xoJzNJaUrazkmGClTAEjQlzsnLULSpuy/assets/images/optimized/rev-6282543/woodgreen.org.uk/wp-content/uploads/2025/05/Bulut-2.jpeg",
                Description = "Спокійний, любить тишу, рідко гавкає"
            }
        };

        private int _currentIndex = 0;

        public PetDetailsPage()
        {
            InitializeComponent();
            ShowPet(_pets[_currentIndex]);
        }

        private void ShowPet(PetInfo pet)
        {
            PetImage.Source = pet.Image;
            PetInfoLabel.Text = $"{pet.Type}, {pet.Name}, {pet.Age} років";
            PetDescriptionLabel.Text = pet.Description;
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            // якщо є сторінка назад — повертаємось, інакше на список
            if (Navigation?.NavigationStack?.Count > 1)
                await Shell.Current.GoToAsync("..");
            else
                await Shell.Current.GoToAsync("//pets");
        }

        private async void OnHelpClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("donation");

        private async void OnAdoptClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("adoption");

        private void OnPrevClicked(object sender, EventArgs e)
        {
            _currentIndex = (_currentIndex - 1 + _pets.Count) % _pets.Count;
            ShowPet(_pets[_currentIndex]);
        }

        private void OnNextClicked(object sender, EventArgs e)
        {
            _currentIndex = (_currentIndex + 1) % _pets.Count;
            ShowPet(_pets[_currentIndex]);
        }

        private async void DonationButton_Clicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("donation");

        private async void OnPetsClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("//pets");   

        private async void OnAboutClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("about");

        private class PetInfo
        {
            public string Name { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
            public int Age { get; set; }
            public string Image { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
        }
    }
}
