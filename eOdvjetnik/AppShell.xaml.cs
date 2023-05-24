﻿namespace eOdvjetnik;
using System.Diagnostics;
using Views;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Popup;
using eOdvjetnik.ViewModel;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
		Routing.RegisterRoute(nameof(Kalendar), typeof(Kalendar));
		Routing.RegisterRoute(nameof(Dokumenti), typeof(Dokumenti));
		Routing.RegisterRoute(nameof(DocsItemPage), typeof(DocsItemPage));
		Routing.RegisterRoute(nameof(AppointmentDialog), typeof(AppointmentDialog));

		BindingContext = new AppShellViewModel();

		SfPopup popup = new SfPopup();
		
    }

	private async void OnButtonClicked(object sender, EventArgs e)
	{
		try
		{
			string route = "";

			if (sender == PočetnaButton)
				route = "///MainPage";
			else if (sender == KalendarButton)
				route = "Kalendar";
			else if (sender == DokumentiButton)
				route = "Dokumenti";

			await Current.GoToAsync(route);
			Debug.WriteLine("CLICKED___");
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex.Message + "in AppShell");
		}


	}

	private void OnSupportButtonClicked(object sender, EventArgs e)
	{
		try
		{
			Debug.WriteLine("stisnut support");
			popup.ShowCloseButton = true;

			popup.Show();
		}

		catch (Exception ex)
		{
			Debug.WriteLine(ex.Message);
		}

	}

}
