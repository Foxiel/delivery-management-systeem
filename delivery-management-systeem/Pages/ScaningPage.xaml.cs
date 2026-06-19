using delivery_management_systeem.DALS;
using delivery_management_systeem.Models;
using System.Collections.ObjectModel;

namespace delivery_management_systeem.Pages;

public partial class ScaningPage : ContentPage
{
    public ObservableCollection<Product> Producten { get; set; } = new();

    public ScaningPage()
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

        // Bijvoorbeeld bestelling 1001 laden
        ProductDAL productDAL = new ProductDAL();

        List<Product> producten = productDAL.GetProductInfoBestelling(0);

        foreach (Product product in producten)
        {
            Producten.Add(product);
        }
    }

    private async void BarcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        var first = e.Results.FirstOrDefault();

        if (first != null)
        {
            await Dispatcher.DispatchAsync(async () =>
            {
                foreach (Product product in Producten)
                {
                    if (product.EAN == first.Value && product.IsGescand == false)
                    {
                        product.IsGescand = true;

                        ProductenCollectionView.ItemsSource = null;
                        ProductenCollectionView.ItemsSource = Producten;

                        await DisplayAlertAsync(
                            "Product gevonden",
                            $"Product: {product.Naam}\nEAN: {product.EAN}",
                            "OK");

                        return;
                    }
                }

                await DisplayAlertAsync(
                    "Product niet gevonden",
                    $"EAN: {first.Value} is niet gevonden in de bestelling.",
                    "OK");
            });
        }
    }

    private async void SubmitBarcode_Clicked(object sender, EventArgs e)
    {
        string barcode = BarcodeEntry.Text;

        foreach (Product product in Producten)
        {
            if (product.EAN == barcode && product.IsGescand == false)
            {
                product.IsGescand = true;

                ProductenCollectionView.ItemsSource = null;
                ProductenCollectionView.ItemsSource = Producten;

                await DisplayAlertAsync(
                    "Product gevonden",
                    $"Product: {product.Naam}\nEAN: {product.EAN}",
                    "OK");

                return;
            }
        }

        await DisplayAlertAsync(
            "Product niet gevonden",
            $"EAN: {barcode} is niet gevonden in de bestelling.",
            "OK");
    }

    private void ToggleLamp(object sender, EventArgs e)
    {
        barcodeReaderView.IsTorchOn = !barcodeReaderView.IsTorchOn;
    }
}