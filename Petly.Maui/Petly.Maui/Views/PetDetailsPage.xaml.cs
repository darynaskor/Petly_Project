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
                Name = "Томас",
                Type = "Кіт",
                Age = 2,
                Image = "https://pesyk.kiev.ua/wp-content/uploads/Ryzhie-britanskie-koshki-2.jpg",
                Description = "Цей кіт поєднує в собі аристократичну зовнішність та ніжний характер. Томас надзвичайно лагідний і обожнює довгі сеанси погладжування, під час яких голосно муркоче. Ідеальний компаньйон для спокійної сім'ї."
            },
            new PetInfo
            {
                Name = "Рік",
                Type = "Собака",
                Age = 6,
                Image = "https://www.tierschutzbund.de/fileadmin/_processed_/7/c/csm_schwarzer_Hund_auf_Wiese_c_xkunclova-Shutterstock_01_5566a80d25.jpg",
                Description = "Рік — відданий охоронець і справжній друг. Любить прогулянки, добре ладнає з дітьми."
            },
            new PetInfo
            {
                Name = "Голді",
                Type = "Собака",
                Age = 3,
                Image = "https://www.dogsforgood.org/wp-content/uploads/2020/06/Dogs-For-Good-October-22-2019-308-1024x660.jpg",
                Description = "Голді — розумна і весела, любить активні прогулянки та швидко навчається."
            },
            new PetInfo
            {
                Name = "Мурчик",
                Type = "Кіт",
                Age = 1,
                Image = "https://people.com/thmb/xHPJAus5iELyf5ndsPJ84GeJTwI=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc():focal(694x160:696x162)/cat-study-110223-1-efc838c9067349ab82ac24abc4cc2de5.jpg",
                Description = "Маленький пустун, лагідний і грайливий. Любить м’ячики та мотузочки."
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
            => await Shell.Current.GoToAsync("//pets");   // було //petlist — виправлено під реальний маршрут

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
