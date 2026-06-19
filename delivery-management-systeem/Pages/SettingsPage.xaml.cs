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
}
