using delivery_management_systeem.Pages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using delivery_management_systeem.Models;

namespace delivery_management_systeem.ViewModels;

public class DienstBeëindigenViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    // ---------------- PACKAGES ----------------

    public ObservableCollection<PackageItem> RetourPakketten { get; set; } = new();
    public ObservableCollection<PackageItem> OverigePakketten { get; set; } = new();

    private bool _areAllPackagesVerified;
    public bool AreAllPackagesVerified
    {
        get => _areAllPackagesVerified;
        set
        {
            _areAllPackagesVerified = value;
            OnPropertyChanged();
        }
    }

    private string _statusMessage = "Alle pakketten die zijn gescand of meegenomen.";
    public string StatusMessage
    {
        get => _statusMessage;
        set
        {
            _statusMessage = value;
            OnPropertyChanged();
        }
    }

    // ---------------- COMMAND ----------------

    public IAsyncRelayCommand ConfirmPackagesCommand { get; }

    public DienstBeëindigenViewModel()
    {
        ConfirmPackagesCommand = new AsyncRelayCommand(OnConfirmPackages);
        LoadData();
    }

    // ---------------- INIT DATA ----------------

    private void LoadData()
    {
        RetourPakketten.Add(new PackageItem { PakketNummer = "PKT-001" });
        RetourPakketten.Add(new PackageItem { PakketNummer = "PKT-002" });

        OverigePakketten.Add(new PackageItem { PakketNummer = "PKT-003" });
        OverigePakketten.Add(new PackageItem { PakketNummer = "PKT-004" });

        AreAllPackagesVerified = true;
    }

    // ---------------- ACTION ----------------

    private async Task OnConfirmPackages()
    {
        if (!AreAllPackagesVerified)
        {
            await App.Current.MainPage.DisplayAlert(
                "Validatie",
                "Verifieer eerst alle pakketten.",
                "OK");
            return;
        }

        await App.Current.MainPage.Navigation.PushAsync(
            new DienstBeëindingenFoto());
    }

    // ---------------- PROPERTY CHANGED ----------------

    protected void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}