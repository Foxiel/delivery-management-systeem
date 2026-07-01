using Microsoft.Maui.Controls;

namespace delivery_management_systeem.Components;

public partial class HamburgerMenu : ContentView
{
    public HamburgerMenu()
    {
        InitializeComponent();
    }

    public void ShowMenu()
    {
        MenuOverlay.IsVisible = true;
    }

    public void HideMenu()
    {
        MenuOverlay.IsVisible = false;
    }

    private void OnOverlayTapped(object sender, TappedEventArgs e)
    {
        HideMenu();
    }

    private void OnHelpClicked(object sender, EventArgs e)
    {
        HideMenu();
        Shell.Current.GoToAsync("help");
    }

    private void OnPauzeClicked(object sender, EventArgs e)
    {
        HideMenu();
        Shell.Current.GoToAsync("pause");
    }

    private void OnSettingsClicked(object sender, EventArgs e)
    {
        HideMenu();
        Shell.Current.GoToAsync("settings");
    }

    private void OnLogoutClicked(object sender, EventArgs e)
    {
        HideMenu();
        Shell.Current.GoToAsync("login");
    }
}
