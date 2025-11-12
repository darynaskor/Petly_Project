using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using Petly.Maui.Models;
using Petly.Maui.Services;

namespace Petly.Maui.Views;

public partial class MapPage : ContentPage
{
    private readonly ShelterService _shelterService = null!;
    private readonly UserContext _userCtx = null!;

    private ObservableCollection<Shelter> _shelters = new();

    public ObservableCollection<Shelter> Shelters => _shelters;

    public string SelectedShelterId { get; private set; } = string.Empty;
    public ICommand SelectShelterCommand { get; }

    public bool IsAdmin { get; private set; }

    public MapPage()
    {
        InitializeComponent();

        _shelterService = GetService<ShelterService>()!;
        _userCtx = GetService<UserContext>()!;

        IsAdmin = _userCtx?.IsAdmin ?? false;
        AdminForm.IsVisible = IsAdmin;
        ShelterList.ItemsSource = _shelters;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Підтягуємо список із сервісу (де вже є 3 дефолтних елементи)
        Shelters.Clear();
        foreach (var s in await _shelterService.LoadAsync())
            Shelters.Add(s);

        RedrawPins();
    }

    // Додати притулок (адмін)
    private async void OnAddShelter(object sender, EventArgs e)
    {
        var s = new Shelter
        {
            shelter_name = NameEntry.Text?.Trim() ?? "",
            address = AddressEntry.Text?.Trim() ?? "",
            phone = PhoneEntry.Text?.Trim() ?? ""
            // Px/Py НЕ чіпаємо – сервіс проставить RandomNearCenter()
        };

        if (string.IsNullOrWhiteSpace(s.shelter_name))
        {
            await DisplayAlert("Помилка", "Вкажіть назву притулку.", "OK");
            return;
        }

        await _shelterService.AddAsync(_shelters, s);
        NameEntry.Text = AddressEntry.Text = PhoneEntry.Text = "";
        RedrawPins();
    }



    // Видалити (адмін)
    private async void OnDeleteShelter(object sender, EventArgs e)
    {
        if (sender is Button b && b.CommandParameter is string id)
        {
            var ok = await DisplayAlert("Підтвердьте", "Видалити цей притулок?", "Так", "Ні");
            if (!ok) return;

            await _shelterService.DeleteAsync(Shelters, id);

            if (SelectedShelterId == id) SelectedShelterId = string.Empty;
            RedrawPins();
        }
    }

    private void RedrawPins()
    {
        MapCanvas.Children.Clear();

        foreach (var s in _shelters) // ВАЖЛИВО: саме _shelters
        {
            var isSelected = s.Id == SelectedShelterId;

            var dot = new Microsoft.Maui.Controls.Shapes.Ellipse
            {
                WidthRequest = 22,
                HeightRequest = 22,
                Fill = isSelected ? new SolidColorBrush(Color.FromArgb("#D3453F"))
                                  : new SolidColorBrush(Colors.Black),
                Stroke = Colors.White,
                StrokeThickness = 2
            };

            // позиція у відсотках (0..1)
            AbsoluteLayout.SetLayoutFlags(dot, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(dot, new Rect(s.Px, s.Py, 22, 22));

            MapCanvas.Children.Add(dot);
        }
    }


    private static T? GetService<T>() where T : class
        => Application.Current?.Handler?.MauiContext?.Services?.GetService<T>();
}
