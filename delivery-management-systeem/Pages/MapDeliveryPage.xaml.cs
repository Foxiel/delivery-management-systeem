namespace delivery_management_systeem.Pages;

public partial class MapDeliveryPage : ContentPage
{
    private bool isfinished = false;

    // ========================= MENU =========================
    private Grid _menuOverlay;
    private Frame _menuPanel;
    private Button _pauseButton;

    private bool _isPaused;
    private const double _menuWidth = 280;

    public MapDeliveryPage()
    {
        InitializeComponent();

        deliverBarcodeReaderView.Options = new ZXing.Net.Maui.BarcodeReaderOptions
        {
            Formats = ZXing.Net.Maui.BarcodeFormats.All,
            AutoRotate = true,
            Multiple = true
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (isfinished)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                BezorgingButton.Text = "Afmelden";
                ShowKaart();
            });
        }

        _menuOverlay = this.FindByName<Grid>("MenuOverlay");
        _menuPanel = this.FindByName<Frame>("MenuPanel");
        _pauseButton = this.FindByName<Button>("PauzeButton");

        if (_menuOverlay != null)
            _menuOverlay.IsVisible = false;

        if (_menuPanel != null)
            _menuPanel.TranslationX = -_menuWidth;
    }

    private void KaartButton_Clicked(object sender, EventArgs e)
    {
        ShowKaart();
    }

    private void BezorgingButton_Clicked(object sender, EventArgs e)
    {
        if (isfinished)
            AfmeldProcedure();
        else
            ShowBezorging();
    }

    private void SwipeLeft_Swiped(object sender, SwipedEventArgs e)
    {
        ShowBezorging();
    }

    private void SwipeRight_Swiped(object sender, SwipedEventArgs e)
    {
        ShowKaart();
    }

    private void ShowKaart()
    {
        KaartGrid.IsVisible = true;
        BezorgGrid.IsVisible = false;
    }

    private void ShowBezorging()
    {
        KaartGrid.IsVisible = false;
        BezorgGrid.IsVisible = true;
    }

    private async void RetourButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RetourPage());
    }

    private void BarcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        var first = e.Results.FirstOrDefault();

        if (first != null)
        {
            Dispatcher.DispatchAsync(async () =>
            {
                await DisplayAlertAsync(
                    "Barcode detected",
                    $"Type: {first.Format}\nValue: {first.Value}\nraw?: {first.Raw}",
                    "OK");
            });
        }
    }

    private void ToggleLamp(object sender, EventArgs e)
    {
        deliverBarcodeReaderView.IsTorchOn = !deliverBarcodeReaderView.IsTorchOn;
    }

    private async void Afronden_Clicked(object sender, EventArgs e)
    {
        isfinished = true;
        await Navigation.PushAsync(new HandtekeningPage());
    }

    private async void Help_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HelpPage());
    }

    private async void AfmeldProcedure()
    {
        await Navigation.PushAsync(new DienstBeëindigen());
    }

    // ========================= MENU FUNCTIES =========================

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

        await CloseMenu();
        await Navigation.PushAsync(new DienstBeëindigen());
    }
}