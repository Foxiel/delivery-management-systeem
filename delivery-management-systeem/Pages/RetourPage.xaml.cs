using delivery_management_systeem.Repositories;
using delivery_management_systeem.Models;
using System.Collections.ObjectModel;
using ZXing.QrCode.Internal;

namespace delivery_management_systeem.Pages;

public partial class RetourPage : ContentPage
{
    public ObservableCollection<Bezorging> bezorgingen { get; set; } = new();

    public RetourPage()
    {
        InitializeComponent();

        BindingContext = this;

        barcodeReaderView.Options = new ZXing.Net.Maui.BarcodeReaderOptions
        {
            Formats = ZXing.Net.Maui.BarcodeFormats.All, //voor ale codes te kunnen scannen, maar dat is te traag
            //Formats = ZXing.Net.Maui.BarcodeFormat.Code128, // voor telefoons
            //Formats = ZXing.Net.Maui.BarcodeFormat.QrCode, //voor tobias/laptob debugging
            AutoRotate = false,
            Multiple = true
        };


    }
    BezorgingRepositorie bezorgDAL = new BezorgingRepositorie();
    public ObservableCollection<Bezorging> Retourbezorgingen { get; set; } = new();

    private async void BarcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        var first = e.Results.FirstOrDefault();

        if (first != null)
        {
            await Dispatcher.DispatchAsync(async () =>
            {
                Retourbezorgingen.Add(bezorgDAL.RetourBezorging(first.Value));
            });
        }
    }

    private async void SubmitBarcode_Clicked(object sender, EventArgs e)
    {
        string barcode = BarcodeEntry.Text;

        if (barcode != null)
        {
            Retourbezorgingen.Add(bezorgDAL.RetourBezorging(barcode));
        }
    }

    private void ToggleLamp(object sender, EventArgs e)
    {
        barcodeReaderView.IsTorchOn = !barcodeReaderView.IsTorchOn;
    }
    private async void Help_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HelpPage());
    }
    private async void HandtekeningButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HandtekeningPage());
    }
    //==============================MENU==================================
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
        await Navigation.PushAsync(new DienstBeëindigen());
    }
}