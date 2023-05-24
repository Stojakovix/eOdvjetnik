namespace eOdvjetnik;
using System.Diagnostics;
using Views;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Popup;
using System.Windows.Input;
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

	private void buttonClick()
	{
		Current.GoToAsync("Kalendar");
	}

	private void OnButtonClicked(object sender, EventArgs e )
	{
	 Debug.WriteLine("CLICKED---------------");
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
