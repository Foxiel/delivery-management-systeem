using System.Threading;
using Plugin.Maui.Biometric;

namespace delivery_management_systeem.Pages;

public partial class LoginPage : ContentPage
{
    private readonly IBiometric _biometricService;
    
    public LoginPage(IBiometric biometricService)
    {
        _biometricService = biometricService;
        InitializeComponent();
    }

    private async void OnLoginWithCredentialsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginWithCredentialsPage());
    }

    private async void OnHelpClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HelpPage());
    }
    
    private async void OnBiometricLoginClicked(object sender, EventArgs e)
    {
        try
        {
            if (!_biometricService.IsPlatformSupported)
            {
                await DisplayAlert("Biometrie", "Biometrische authenticatie wordt niet ondersteund op dit platform.", "OK");
                return;
            }

            var request = new AuthenticationRequest
            {
                Title = "Inloggen",
                NegativeText = "Scan je vingerafdruk of gezicht om in te loggen"
            };
        
            var result = await _biometricService.AuthenticateAsync(request, CancellationToken.None);

            if (result.Status == BiometricResponseStatus.Success)
            {
                await Navigation.PushAsync(new ScaningPage());
            }
            else
            {
                await DisplayAlert("Authenticatie Mislukt", $"Status: {result.Status}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Fout", $"Er is iets misgegaan: {ex.Message}", "OK");
        }
    }
}