
namespace delivery_management_systeem.Pages;
// gemaakt door jesse
public partial class ScaningPage : ContentPage
{
	public ScaningPage()
	{
		InitializeComponent();
		barcodeReaderView.Options = new ZXing.Net.Maui.BarcodeReaderOptions
		{
            //Formats = ZXing.Net.Maui.BarcodeFormats.All, //voor ale codes te kunnen scannen, maar dat is te traag
            Formats = ZXing.Net.Maui.BarcodeFormat.Code128, // voor telefoons
            //Formats = ZXing.Net.Maui.BarcodeFormat.QrCode, //voor tobias/laptob debugging
            AutoRotate = false,
			Multiple = true
		};
		
    }

	private void BarcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
	{
		var first = e.Results.FirstOrDefault();
		if (first != null)
		{
			Dispatcher.DispatchAsync(async () =>
			{
                await DisplayAlertAsync("Barcode detected", $"Type: {first.Format}\nValue: {first.Value}\n", "OK");
            });
		}
    }
	private void ToggleLamp(object sender, EventArgs e)
	{
		barcodeReaderView.IsTorchOn = !barcodeReaderView.IsTorchOn;
	}
}