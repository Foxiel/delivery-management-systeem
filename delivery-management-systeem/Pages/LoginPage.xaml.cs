namespace delivery_management_systeem.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnNfcScanClicked(object sender, EventArgs e)
    {
        await DisplayAlertAsync("NFC", "NFC scan gestart", "OK");
    }

    private async void OnLoginWithCredentialsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginWithCredentialsPage());
    }

    private async void OnHelpClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HelpPage());
    }
    
    private async void OnScanClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ScaningPage());
    }
    
    private async void OnDeliverClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MapDeliveryPage());
    }
}