namespace eOdvjetnik;

using Syncfusion.Maui.Scheduler;
using System.Globalization;
using Syncfusion.Maui.Scheduler;
using System.Resources;
using Views;


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
        Routing.RegisterRoute(nameof(Register), typeof(Register));

        
    }
}
