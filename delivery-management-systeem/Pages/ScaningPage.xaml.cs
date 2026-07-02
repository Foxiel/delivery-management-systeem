using delivery_management_systeem.Repositories;
using delivery_management_systeem.Models;
using System.Collections.ObjectModel;

namespace delivery_management_systeem.Pages;

public partial class ScaningPage : ContentPage
{
    public ObservableCollection<Bezorging> Bezorgingen { get; set; } = new();

    public ScaningPage()
    {
        InitializeComponent();

        BindingContext = this;

        barcodeReaderView.Options = new ZXing.Net.Maui.BarcodeReaderOptions
        {
            //Formats = ZXing.Net.Maui.BarcodeFormats.All, //voor ale codes te kunnen scannen, maar dat is te traag
            //Formats = ZXing.Net.Maui.BarcodeFormat.Code128, // voor telefoons
            Formats = ZXing.Net.Maui.BarcodeFormat.QrCode, //voor tobias/laptob debugging
            AutoRotate = false,
            Multiple = true
        };


        BezorgingRepositorie bezorgingDAL = new BezorgingRepositorie();

        List<Bezorging> bezorgingen = bezorgingDAL.GetBezorgingInfoBestelling();

        foreach (Bezorging bezorging in bezorgingen)
        {
            Bezorgingen.Add(bezorging);
        }
    }

    private async void BarcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        var first = e.Results.FirstOrDefault();

        if (first != null)
        {
            await Dispatcher.DispatchAsync(async () =>
            {
                foreach (Bezorging bezorging in Bezorgingen)
                {
                    if (bezorging.Code == first.Value && bezorging.IsGescand == false)
                    {
                        bezorging.MistNaControle = false;
                        bezorging.IsGescand = true;

                        packetjesCollectionView.ItemsSource = null;
                        packetjesCollectionView.ItemsSource = Bezorgingen;


                        return;
                    }
                }

                await DisplayAlertAsync(
                    "packetje niet gevonden",
                    $"packetje: {first.Value} is niet gevonden in de bestelling.",
                    "OK");
            });
        }
    }

    private async void SubmitBarcode_Clicked(object sender, EventArgs e)
    {
        string barcode = BarcodeEntry.Text;

        foreach (Bezorging bezorging in Bezorgingen)
        {
            if (bezorging.Code == barcode && bezorging.IsGescand == false)
            {
                bezorging.MistNaControle = false;
                bezorging.IsGescand = true;

                packetjesCollectionView.ItemsSource = null;
                packetjesCollectionView.ItemsSource = Bezorgingen;

                return;
            }
        }

        await DisplayAlertAsync(
            "packetje niet gevonden",
            $"EAN: {barcode} is niet gevonden in de bestelling.",
            "OK");
    }

    private void ToggleLamp(object sender, EventArgs e)
    {
        barcodeReaderView.IsTorchOn = !barcodeReaderView.IsTorchOn;
    }
    private async void Help_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HelpPage());
    }
    private async void StartButton_Clicked(object sender, EventArgs e)
    {
        bool allesGescand = true;

        foreach (Bezorging bezorging in Bezorgingen)
        {
            if (!bezorging.IsGescand)
            {
                bezorging.MistNaControle = true;
                allesGescand = false;
            }
        }

        packetjesCollectionView.ItemsSource = null;
        packetjesCollectionView.ItemsSource = Bezorgingen;

        if (allesGescand)
        {
            await Navigation.PushAsync(new MapDeliveryPage
                ());
        }
    }
    //======================MENU========================
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