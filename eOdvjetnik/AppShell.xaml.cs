namespace eOdvjetnik;
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
		Routing.RegisterRoute(nameof(Klijenti), typeof(Klijenti)); 
		Routing.RegisterRoute(nameof(Naplata), typeof(Naplata));
		Routing.RegisterRoute(nameof(Spisi), typeof(Spisi));
		Routing.RegisterRoute(nameof(NoviSpis), typeof(NoviSpis));
        Routing.RegisterRoute(nameof(Postavke), typeof(Postavke));


        BindingContext = new AppShellViewModel();

		SfPopup popup = new SfPopup();
		
    }

	private void onClosed(object sender, EventArgs e)
	{
		try
		{
			Debug.WriteLine("closed SQL");
			popup.IsOpen = false;
			popup.IsVisible = false;

		 }
		catch (Exception ex)
		{
			Debug.WriteLine(ex.Message + " in AppShellxaml.cs");
			
		}
	}


	//private void OnSupportButtonClicked(object sender, EventArgs e)
	//{
	//	try
	//	{
	//		Debug.WriteLine("stisnut support");
	//		popup.ShowCloseButton = true;

	//		popup.Show();
	//	}

	//	catch (Exception ex)
	//	{
	//		Debug.WriteLine(ex.Message);
	//	}

	//}

}
