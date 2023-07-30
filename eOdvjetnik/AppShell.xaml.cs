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
        Routing.RegisterRoute(nameof(NoviKlijent), typeof(NoviKlijent));
        Routing.RegisterRoute(nameof(Racun), typeof(Racun));
        Routing.RegisterRoute(nameof(Zaposlenici), typeof(Zaposlenici));
        Routing.RegisterRoute(nameof(UrediKlijenta), typeof(UrediKlijenta));
        Routing.RegisterRoute(nameof(NoviZaposlenik), typeof(NoviZaposlenik));
        Routing.RegisterRoute(nameof(SpiDok), typeof(SpiDok));
        Routing.RegisterRoute(nameof(Zaposlenici), typeof(Zaposlenici));

        BindingContext = new AppShellViewModel();

		SfPopup popup = new SfPopup();
		
    }


}
