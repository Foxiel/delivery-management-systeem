using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace delivery_management_systeem.Pages;

public partial class HandtekeningPage : ContentPage
{
    public HandtekeningPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (SignatureView.Lines == null || SignatureView.Lines.Count == 0)
        {
            await DisplayAlertAsync(
                "Fout",
                "Er is geen handtekening om op te slaan.",
                "OK");
            return;
        }

        //string json = JsonSerializer.Serialize(SignatureView.Lines);

        //string path = Path.Combine(FileSystem.AppDataDirectory, "handtekening.json");

        //await File.WriteAllTextAsync(path, json);

        await DisplayAlertAsync(
            "Opgeslagen",
            $"Handtekening opgeslagen",
            "OK");
        await Navigation.PopAsync();
    }

    private async void OnClearClicked(object sender, EventArgs e)
    {
        SignatureView.Lines?.Clear();

        await DisplayAlertAsync(
            "Handtekening",
            "Handtekening gewist.",
            "OK");
    }

}