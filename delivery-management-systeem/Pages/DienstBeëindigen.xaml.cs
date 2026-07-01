using delivery_management_systeem.ViewModels;

namespace delivery_management_systeem.Pages;

public partial class DienstBeëindigen : ContentPage
{
    private Grid _menuOverlay;
    private Frame _menuPanel;
    private Button _pauseButton;

    private bool _isPaused;
    private const double _menuWidth = 280;

    private readonly DienstBeëindigenViewModel _viewmodel;

    public DienstBeëindigen()
    {
        InitializeComponent();

        _viewmodel = new DienstBeëindigenViewModel();
        BindingContext = _viewmodel;
    }

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

    // ---------------- MENU ----------------

    private async void OnMenuClicked(object sender, EventArgs e)
    {
        if (_menuOverlay == null || _menuPanel == null)
            return;

        if (!_menuOverlay.IsVisible)
        {
            _menuOverlay.IsVisible = true;
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

    // ---------------- MENU ITEMS ----------------

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

        await DisplayAlert("Pauze",
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

        // CORRECT logout
        Application.Current.MainPage =
            new NavigationPage(new LoginPage());
    }
}
