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


        BezorgingRepositorie productDAL = new BezorgingRepositorie();

        List<Bezorging> bezorgingen = productDAL.GetBezorgingInfoBestelling();

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

                        await DisplayAlertAsync(
                            "packetje gevonden",
                            $"packetje: {bezorging.Code}",
                            "OK");

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


                await DisplayAlertAsync(
                    "packetje gevonden",
                    $"packetje: {bezorging.Code}",
                    "OK");

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

                await DisplayAlertAsync(
                    "Product niet gescand",
                    $"Product: {bezorging.Code}\nEAN: {bezorging.Code} is nog niet gescand.",
                    "OK");
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
}