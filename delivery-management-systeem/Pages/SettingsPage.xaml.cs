//Gemaakt door Tobias
using Microsoft.Maui.Controls;

namespace delivery_management_systeem.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        // initialize theme switch state from current app theme
        try
        {
            var userTheme = Application.Current?.UserAppTheme ?? AppTheme.Unspecified;
            var requested = Application.Current?.RequestedTheme ?? AppTheme.Unspecified;
            bool isDark = (userTheme == AppTheme.Dark) || (userTheme == AppTheme.Unspecified && requested == AppTheme.Dark);
            ThemeSwitch.IsToggled = isDark;
        }
        catch
        {
            // ignore; ThemeSwitch may not be available at construction time in some scenarios
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void OnThemeToggled(object sender, ToggledEventArgs e)
    {
        // switch between Light and Dark theme
        try
        {
            Application.Current.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;
        }
        catch
        {
            // ignore errors
        }
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
