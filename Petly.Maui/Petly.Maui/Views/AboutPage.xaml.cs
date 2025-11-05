namespace Petly.Maui.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private async void OnWebsiteClicked(object sender, EventArgs e)
        {
            await Launcher.OpenAsync("https://petly.example.com"); 
        }

        private async void OnInstagramClicked(object sender, EventArgs e)
        {
            await Launcher.OpenAsync("https://www.instagram.com/");
        }
    }
}
