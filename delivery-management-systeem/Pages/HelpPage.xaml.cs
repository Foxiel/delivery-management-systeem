namespace delivery_management_systeem.Pages;

public partial class HelpPage : ContentPage
{
    public HelpPage()
    {
        InitializeComponent();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnSendTicketClicked(object sender, EventArgs e)
    {
        string subject = SubjectEntry.Text;
        string description = DescriptionEditor.Text;

        if (string.IsNullOrWhiteSpace(subject) ||
            string.IsNullOrWhiteSpace(description))
        {
            await DisplayAlertAsync("Fout", "Vul een onderwerp en beschrijving in.", "OK");
            return;
        }

        // Hier komt later je database-code
        // await TicketRepository.CreateTicketAsync(subject, description);

        await DisplayAlertAsync("Gelukt", "Je ticket is verzonden.", "OK");

        SubjectEntry.Text = string.Empty;
        DescriptionEditor.Text = string.Empty;
    }
}