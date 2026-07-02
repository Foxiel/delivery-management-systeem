namespace delivery_management_systeem.Pages;

public partial class HelpPage : ContentPage
{
    public HelpPage()
    {
        InitializeComponent();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnSendTicketClicked(object sender, EventArgs e)
    {
        string subject = SubjectEntry.Text;
        string description = DescriptionEditor.Text;

        if (string.IsNullOrWhiteSpace(subject) ||
            string.IsNullOrWhiteSpace(description))
        {
            await DisplayAlertAsync("Fout", "Vul een onderwerp en beschrijving in.", "OK");
            return;
        }

        // Hier komt later je database-code
        // await TicketRepository.CreateTicketAsync(subject, description);

        await DisplayAlertAsync("Gelukt", "Je ticket is verzonden.", "OK");

        SubjectEntry.Text = string.Empty;
        DescriptionEditor.Text = string.Empty;
    }
    //=============================MENU=========================
    private Grid _menuOverlay;
    private Frame _menuPanel;
    private Button _pauseButton;

    private bool _isPaused;
    private const double _menuWidth = 280;

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _menuOverlay = this.FindByName<Grid>("MenuOverlay");
        _menuPanel = this.FindByName<Frame>("MenuPanel");
        _pauseButton = this.FindByName<Button>("PauzeButton");

        if (_menuOverlay != null)
            _menuOverlay.IsVisible = false;

        if (_menuPanel != null)
            _menuPanel.TranslationX = -_menuWidth;
    }

    private async void OnMenuClicked(object sender, EventArgs e)
    {
        if (_menuOverlay == null || _menuPanel == null)
            return;

        if (!_menuOverlay.IsVisible)
        {
            _menuOverlay.IsVisible = true;
            _menuPanel.TranslationX = -_menuWidth;
            await _menuPanel.TranslateTo(0, 0, 250, Easing.SinOut);
        }
        else
        {
            await CloseMenu();
        }
    }

    private async Task CloseMenu()
    {
        if (_menuPanel != null)
            await _menuPanel.TranslateTo(-_menuWidth, 0, 200, Easing.SinIn);

        if (_menuOverlay != null)
            _menuOverlay.IsVisible = false;
    }

    private async void OnOverlayTapped(object sender, EventArgs e)
    {
        await CloseMenu();
    }

    private async void OnHelpClickedFromMenu(object sender, EventArgs e)
    {
        await CloseMenu();
        await Navigation.PushAsync(new HelpPage());
    }

    private async void OnPauseClickedFromMenu(object sender, EventArgs e)
    {
        _isPaused = !_isPaused;

        if (_pauseButton != null)
            _pauseButton.Text = _isPaused ? "Hervatten" : "Pauze";

        await CloseMenu();

        await DisplayAlertAsync(
            "Pauze",
            _isPaused ? "Pauze gestart" : "Pauze gestopt",
            "OK");
    }

    private async void OnSettingsClickedFromMenu(object sender, EventArgs e)
    {
        await CloseMenu();
        await Navigation.PushAsync(new SettingsPage());
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await CloseMenu();

        Application.Current.MainPage =
            new NavigationPage(new DienstBeëindigen());
    }
}