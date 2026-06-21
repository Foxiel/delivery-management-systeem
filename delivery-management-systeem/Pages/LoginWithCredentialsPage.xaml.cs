namespace delivery_management_systeem.Pages;
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using delivery_management_systeem.Pages;

public partial class LoginWithCredentialsPage : ContentPage
{
    private bool _isNavigating;

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

        if (_isNavigating)
            return;

        _isNavigating = true;
        try
        {
            // Try normal push navigation first; fall back to replacing MainPage when needed
            if (this.Navigation != null)
            {
                await Navigation.PushAsync(new Ingelogd());
                return;
            }

            if (Application.Current?.MainPage is NavigationPage nav)
            {
                await nav.PushAsync(new Ingelogd());
                return;
            }

            Application.Current.MainPage = new NavigationPage(new Ingelogd());
        }
        finally
        {
            _isNavigating = false;
        }
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
