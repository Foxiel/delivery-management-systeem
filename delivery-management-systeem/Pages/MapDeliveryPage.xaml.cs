namespace delivery_management_systeem.Pages;

public partial class MapDeliveryPage : ContentPage
{
    private bool isfinished = false;
    
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

        // Als we terugkomen van de HandtekeningPage en isfinished is true, pas de knop aan
        if (isfinished)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                BezorgingButton.Text = "Afmelden";
                ShowKaart();
            });
        }
    }

    private void KaartButton_Clicked(object sender, EventArgs e)
    {
        ShowKaart();
    }

    private void BezorgingButton_Clicked(object sender, EventArgs e)
    {
        if (isfinished)
        {
            AfmeldProcedure();
        }
        else
        {
            ShowBezorging();
        }
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

        //KaartTabButton.BackgroundColor = Color.FromArgb("#003D12");
        //BezorgingTabButton.BackgroundColor = Colors.Transparent;
    }

    private void ShowBezorging()
    {
        KaartGrid.IsVisible = false;
        BezorgGrid.IsVisible = true;

        //KaartTabButton.BackgroundColor = Colors.Transparent;
        //BezorgingTabButton.BackgroundColor = Color.FromArgb("#003D12");
    }
    //=========================kaartpage==================================

    private async void RetourButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RetourPage());
    }

    //=========================deliverypage================================

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
}