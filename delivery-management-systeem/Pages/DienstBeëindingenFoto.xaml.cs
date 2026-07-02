

namespace delivery_management_systeem.Pages;

using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Media;

/// <summary>
/// WHAT: Code-behind for DienstBeëindingenFoto page (final service completion screen)
/// HOW: Handles photo capture via MediaPicker and MVVM binding setup
/// WHY: Separates UI concerns from business logic, enabling clean testability
/// </summary>
public partial class DienstBeëindingenFoto : ContentPage
{
    // WHAT: Reference to the ViewModel managing photo and shift state
    // HOW: Instantiated in constructor and set as BindingContext
    // WHY: Enables XAML binding to photo capture commands and vehicle photo property
    private DienstBeëindingenFotoViewModel _viewModel;

    // Menu overlay fields copied from DienstBeëindigen to make menu identical
    private Grid _menuOverlay;
    private Frame _menuPanel;
    private Button _pauseButton;

    private bool _isPaused;
    private const double _menuWidth = 280;

    public DienstBeëindingenFoto()
    {
        InitializeComponent();

        // WHAT: Create and bind ViewModel instance
        // HOW: Instantiate ViewModel and set as binding context
        // WHY: All XAML bindings resolve to ViewModel properties and commands
        _viewModel = new DienstBeëindingenFotoViewModel();
        BindingContext = _viewModel;
    }

    /// <summary>
    /// WHAT: Handle hamburger menu button click
    /// HOW: Navigate or show menu based on application architecture
    /// WHY: Provides consistent access to navigation menu across workflow pages
    /// </summary>
    private async void OnMenuClicked(object sender, EventArgs e)
    {
        if (_menuOverlay == null || _menuPanel == null)
            return;

        if (!_menuOverlay.IsVisible)
        {
            _menuOverlay.IsVisible = true;
            await _menuPanel.TranslateTo(0, 0, 250, Easing.SinOut);
        }
        else
        {
            await CloseMenu();
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _menuOverlay = this.FindByName<Grid>("MenuOverlay");
        _menuPanel = this.FindByName<Frame>("MenuPanel");
        _pauseButton = this.FindByName<Button>("PauzeButton");

        if (_menuOverlay != null)
            _menuOverlay.IsVisible = false;

        if (_menuPanel != null)
            _menuPanel.TranslationX = -_menuWidth;
    }

    private async Task CloseMenu()
    {
        if (_menuPanel != null)
            await _menuPanel.TranslateTo(-_menuWidth, 0, 200, Easing.SinIn);

        if (_menuOverlay != null)
            _menuOverlay.IsVisible = false;
    }

    private async void OnOverlayTapped(object sender, EventArgs e)
    {
        await CloseMenu();
    }

    private async void OnHelpClickedFromMenu(object sender, EventArgs e)
    {
        await CloseMenu();
        await Navigation.PushAsync(new HelpPage());
    }

    private async void OnPauseClickedFromMenu(object sender, EventArgs e)
    {
        _isPaused = !_isPaused;

        if (_pauseButton != null)
            _pauseButton.Text = _isPaused ? "Hervatten" : "Pauze";

        await CloseMenu();

        await DisplayAlertAsync("Pauze",
            _isPaused ? "Pauze gestart" : "Pauze gestopt",
            "OK");
    }

    private async void OnSettingsClickedFromMenu(object sender, EventArgs e)
    {
        await CloseMenu();
        await Navigation.PushAsync(new SettingsPage());
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await CloseMenu();
        await DisplayAlertAsync("Dienst stoppen", $"Je kunt niet meer terug ;)\n je moet nu je dienst beëindigen", "OK");
    }
}

/// <summary>
/// WHAT: ViewModel for DienstBeëindingenFoto page
/// HOW: Manages photo capture, state validation, and service completion
/// WHY: Separates presentation logic from UI, enabling unit testing and reusability
/// </summary>
public class DienstBeëindingenFotoViewModel : INotifyPropertyChanged
{
    private ImageSource _vehiclePhoto;
    private bool _canFinishRoute;
    private bool _isNoPhotoVisible;
    private IAsyncRelayCommand _takePhotoCommand;
    private IAsyncRelayCommand _finishShiftCommand;

    public event PropertyChangedEventHandler PropertyChanged;

    public ImageSource VehiclePhoto
    {
        get => _vehiclePhoto;
        set
        {
            if (_vehiclePhoto != value)
            {
                _vehiclePhoto = value;
                OnPropertyChanged();
                UpdateUIState();
            }
        }
    }

    public bool CanFinishRoute
    {
        get => _canFinishRoute;
        set
        {
            if (_canFinishRoute != value)
            {
                _canFinishRoute = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsNoPhotoVisible
    {
        get => _isNoPhotoVisible;
        set
        {
            if (_isNoPhotoVisible != value)
            {
                _isNoPhotoVisible = value;
                OnPropertyChanged();
            }
        }
    }

    public IAsyncRelayCommand TakePhotoCommand
    {
        get => _takePhotoCommand ??= new AsyncRelayCommand(OnTakePhoto);
    }

    public IAsyncRelayCommand FinishShiftCommand
    {
        get => _finishShiftCommand ??= new AsyncRelayCommand(OnFinishShift);
    }

    public DienstBeëindingenFotoViewModel()
    {
        // WHAT: Initialize UI state
        // HOW: Set IsNoPhotoVisible to true when no photo exists
        // WHY: Displays placeholder until photo is taken
        IsNoPhotoVisible = true;
        CanFinishRoute = false;
    }

    /// <summary>
    /// WHAT: Capture vehicle photo using device camera
    /// HOW: Use MediaPicker.Default.CapturePhotoAsync() to open camera
    /// WHY: Enables user to photograph vehicle for proof of parking location
    /// </summary>
    private async Task OnTakePhoto()
    {
        try
        {
            // WHAT: Check media permissions
            // HOW: Request camera permissions from device
            // WHY: Required for camera access on modern mobile platforms
            PermissionStatus cameraPermission = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (cameraPermission != PermissionStatus.Granted)
            {
                cameraPermission = await Permissions.RequestAsync<Permissions.Camera>();
            }

            if (cameraPermission != PermissionStatus.Granted)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Toestemming",
                    "Cameratoegang is vereist om foto's te maken.",
                    "OK");
                return;
            }

            // WHAT: Capture photo from camera
            // HOW: Try using the native camera; if not available, fall back to picking from gallery
            // WHY: Ensures functionality on devices/emulators without a camera implementation
            FileResult photo = null;
            try
            {
                photo = await MediaPicker.Default.CapturePhotoAsync(new MediaPickerOptions { Title = "Foto voertuig" });
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Fout", $"Kan geen foto maken: {ex.Message}", "OK");
                // fallback: allow user to pick an existing photo if capture not supported
                try
                {
                    photo = await MediaPicker.Default.PickPhotoAsync();
                }
                catch (Exception pickEx)
                {
                    await Application.Current.MainPage.DisplayAlert("Fout", $"Kan geen foto maken of selecteren: {pickEx.Message}", "OK");
                    return;
                }
            }

            if (photo != null)
            {
                // WHAT: Convert captured file to ImageSource
                // HOW: Read file stream and create ImageSource for Image control
                // WHY: Enables displaying captured photo in UI
                // Save to AppData so the photo persists on device between sessions
                string targetFile = Path.Combine(FileSystem.AppDataDirectory, "vehicle_photo.jpg");


                using (Stream sourceStream = await photo.OpenReadAsync())
                {
                    using (FileStream targetStream = File.Create(targetFile))
                    {
                        await sourceStream.CopyToAsync(targetStream);
                    }
                }

                // WHAT: Update UI with captured photo; load from file stream to avoid locking issues
                VehiclePhoto = ImageSource.FromStream(() => File.OpenRead(targetFile));
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Fout",
                $"Fout bij foto maken: {ex.Message}",
                "OK");
        }
    }

    /// <summary>
    /// WHAT: Complete service/shift
    /// HOW: Save photo, mark service as completed, navigate to home
    /// WHY: Final step in delivery route workflow
    /// </summary>
    private async Task OnFinishShift()
    {
        // WHAT: Validate photo exists
        // HOW: Check if VehiclePhoto is not null
        // WHY: Prevents completing shift without required evidence
        if (VehiclePhoto == null)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Validatie",
                "Maak eerst een foto van het voertuig.",
                "OK");
            return;
        }

        try
        {
            var services = Application.Current?.MainPage?.Handler?.MauiContext?.Services;
            var loginPage = IPlatformApplication.Current?.Services.GetService<delivery_management_systeem.Pages.LoginPage>();
            
            // WHAT: Save photo and service completion
            // HOW: Call service/repository to persist shift completion
            // WHY: Records end of shift with photographic evidence
            // TODO: Implement actual service completion logic
            // Example: await _shiftService.CompleteShiftAsync(VehiclePhoto);

            await Application.Current.MainPage.DisplayAlert(
                "Succes",
                "Dienst succesvol beëindigd.",
                "OK");

            // WHAT: Return to home screen
            // HOW: Navigate back to beginning of app (or dedicated home page)
            // WHY: Completes workflow and shows delivery completed state
            if (loginPage != null)
            {
                Application.Current.MainPage = new NavigationPage(loginPage);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlertAsync("Navigatie", "Kan niet terugkeren naar het startscherm.", "OK");
            }
            // Alternative: await Navigation.PopToRootAsync();
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Fout",
                $"Fout bij beëindigen dienst: {ex.Message}",
                "OK");
        }
    }

    /// <summary>
    /// WHAT: Update UI state based on photo
    /// HOW: Show/hide placeholder label based on photo existence
    /// WHY: Provides visual feedback about photo capture status
    /// </summary>
    private void UpdateUIState()
    {
        // WHAT: Hide placeholder when photo exists
        // HOW: Set IsNoPhotoVisible to inverse of VehiclePhoto existence
        // WHY: Provides clean UI showing either placeholder or actual photo
        IsNoPhotoVisible = VehiclePhoto == null;

        // WHAT: Enable finish button only with valid photo
        // HOW: Set CanFinishRoute based on VehiclePhoto presence
        // WHY: Enforces workflow requirement for photo before completion
        CanFinishRoute = VehiclePhoto != null;
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

/// <summary>
/// WHAT: Async relay command for MVVM pattern
/// HOW: Generic command implementation wrapping async task
/// WHY: Bridges XAML command bindings to async ViewModel methods
/// </summary>
public class AsyncRelayCommand : IAsyncRelayCommand
{
    private readonly Func<Task> _execute;
    private readonly Func<bool> _canExecute;
    private bool _isExecuting;

    public event EventHandler CanExecuteChanged;

    public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter) => !_isExecuting && (_canExecute?.Invoke() ?? true);

    public async void Execute(object parameter) => await ExecuteAsync(parameter);

    public async Task ExecuteAsync(object parameter)
    {
        if (!CanExecute(parameter))
            return;

        try
        {
            _isExecuting = true;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            await _execute();
        }
        finally
        {
            _isExecuting = false;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

/// <summary>
/// WHAT: Interface for async relay command
/// HOW: Extends ICommand to support async operations
/// WHY: Enables proper async command implementation in MVVM
/// </summary>
public interface IAsyncRelayCommand : ICommand
{
    Task ExecuteAsync(object parameter);
}
