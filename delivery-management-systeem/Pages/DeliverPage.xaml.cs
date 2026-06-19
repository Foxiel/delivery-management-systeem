using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace delivery_management_systeem.Pages;

public partial class DeliverPage : ContentPage
{
    public DeliverPage()
    {
        InitializeComponent();
        barcodeReaderView.Options = new ZXing.Net.Maui.BarcodeReaderOptions
        {
            Formats = ZXing.Net.Maui.BarcodeFormats.All,
            AutoRotate = true,
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
                await DisplayAlertAsync("Barcode detected", $"Type: {first.Format}\nValue: {first.Value} \nraw?: {first.Raw}", "OK");
            });
        }
    }
    private void ToggleLamp(object sender, EventArgs e)
    {
        barcodeReaderView.IsTorchOn = !barcodeReaderView.IsTorchOn;
    }
}