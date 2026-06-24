using delivery_management_systeem.DALS;
using delivery_management_systeem.Models;
using System.Collections.ObjectModel;
using ZXing.QrCode.Internal;

namespace delivery_management_systeem.Pages;

public partial class RetourPage : ContentPage
{
    public ObservableCollection<Product> Producten { get; set; } = new();

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
    ProductRepository productDAL = new ProductRepository();
    public ObservableCollection<Product> RetourProducten { get; set; } = new();

    private async void BarcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        var first = e.Results.FirstOrDefault();

        if (first != null)
        {
            await Dispatcher.DispatchAsync(async () =>
            {
                RetourProducten.Add(productDAL.productRetouren(first.Value));
            });
        }
    }

    private async void SubmitBarcode_Clicked(object sender, EventArgs e)
    {
        string barcode = BarcodeEntry.Text;
        
        if(barcode!= null)
        {
            RetourProducten.Add(productDAL.productRetouren(barcode));
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