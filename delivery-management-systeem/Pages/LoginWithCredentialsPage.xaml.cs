namespace delivery_management_systeem.Pages;

using delivery_management_systeem.Pages;


public partial class LoginWithCredentialsPage : ContentPage
{
    public LoginWithCredentialsPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Fout", "Vul gebruikersnaam en wachtwoord in.", "OK");
            return;
        }

        await DisplayAlert("Login", "Inloggen gelukt.", "OK");

        // Later vervangen door je echte bezorger hoofdpagina
        // await Navigation.PushAsync(new DeliveryHomePage());
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    private async void OnHelpClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HelpPage());
    }
}