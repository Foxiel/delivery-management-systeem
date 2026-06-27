namespace delivery_management_systeem.Pages;

public partial class MapDeliveryPage : ContentPage
{
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

    private void KaartButton_Clicked(object sender, EventArgs e)
    {
        ShowKaart();
    }

    private void BezorgingButton_Clicked(object sender, EventArgs e)
    {
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
        await Navigation.PushAsync(new HandtekeningPage());
    }
    
    private async void Help_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HelpPage());
    }
}