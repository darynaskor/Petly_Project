namespace Petly.Maui.Views;

public partial class PetDetailsPage : ContentPage
{
	public PetDetailsPage()
	{
		InitializeComponent();
	}

	private async void OnBackButtonClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
	}
}