using delivery_management_systeem.Pages;
using delivery_management_systeem;
using Microsoft.Extensions.DependencyInjection;

namespace delivery_management_systeem;


public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // start altijd met NavigationPage
        MainPage = new NavigationPage(new LoginPage());
    }
}

