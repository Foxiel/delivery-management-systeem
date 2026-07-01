//Gemaakt door Tobias

using System;
using Microsoft.Maui.Controls;

namespace delivery_management_systeem.Pages;

public partial class Ingelogd : ContentPage
{
	public Ingelogd()
	{
		InitializeComponent();
	}

	private Grid _menuOverlay;
	private Frame _menuPanel;
	private Button _pauseButton;
	private bool _isPaused;
	private double _menuWidth = 280;

	protected override void OnAppearing()
	{
		base.OnAppearing();

		// attempt to get named elements from XAML in a safe way
		_menuOverlay = this.FindByName<Grid>("MenuOverlay");
		_menuPanel = this.FindByName<Frame>("MenuPanel");
		_pauseButton = this.FindByName<Button>("PauzeButton");

		// initialize off-screen position for the sliding menu
		if (_menuPanel != null)
		{
			_menuPanel.TranslationX = -_menuWidth;
		}
	}

	private async void OnMenuClicked(object sender, EventArgs e)
	{
		// if elements not found, fallback to simple toggle
		if (_menuOverlay == null || _menuPanel == null)
		{
			if (_menuOverlay != null)
				_menuOverlay.IsVisible = !_menuOverlay.IsVisible;
			return;
		}

		if (!_menuOverlay.IsVisible)
		{
			// show overlay and slide menu in
			_menuOverlay.IsVisible = true;
			_menuPanel.TranslationX = -_menuWidth;
			await _menuPanel.TranslateTo(0, 0, 250, Easing.SinOut);
		}
		else
		{
			// slide menu out then hide overlay
			await _menuPanel.TranslateTo(-_menuWidth, 0, 200, Easing.SinIn);
			_menuOverlay.IsVisible = false;
		}
	}

	private async void OnOverlayTapped(object sender, EventArgs e)
	{
		if (_menuPanel != null)
		{
			await _menuPanel.TranslateTo(-_menuWidth, 0, 200, Easing.SinIn);
		}
		if (_menuOverlay != null)
			_menuOverlay.IsVisible = false;
	}

	private async void OnHelpClickedFromMenu(object sender, EventArgs e)
	{
		if (_menuOverlay != null)
			_menuOverlay.IsVisible = false;
		await Navigation.PushAsync(new HelpPage());
	}

	private async void OnPauzeClickedFromMenu(object sender, EventArgs e)
	{
		// toggle pause state
		_isPaused = !_isPaused;
		if (_pauseButton != null)
		{
			_pauseButton.Text = _isPaused ? "Hervatten" : "Pauze";
		}

		if (_menuOverlay != null)
			_menuOverlay.IsVisible = false;

		await DisplayAlert("Pauze", _isPaused ? "Pauze gestart" : "Pauze gestopt", "OK");
	}

	private async void OnSettingsClickedFromMenu(object sender, EventArgs e)
	{
		if (_menuOverlay != null)
			_menuOverlay.IsVisible = false;

		// navigate to a dedicated Settings page
		try
		{
			await Navigation.PushAsync(new SettingsPage());
		}
		catch
		{
			await DisplayAlert("Settings", "Instellingen niet beschikbaar.", "OK");
		}
	}

	private async void OnLogoutClicked(object sender, EventArgs e)
	{
		// WHAT: Navigate to service completion page instead of logout
		// HOW: Push DienstBeëindigen page to navigation stack
		// WHY: User can end their delivery route/shift from here
		if (_menuOverlay != null)
			_menuOverlay.IsVisible = false;

		await Navigation.PushAsync(new DienstBeëindigen());
	}

	private async void OnDoorgaanClicked(object sender, EventArgs e)
	{
		// Placeholder action for doorgaan button. Replace with actual navigation as needed.
		await Navigation.PushAsync(new ScaningPage());
	}
}
