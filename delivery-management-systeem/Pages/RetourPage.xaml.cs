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
}