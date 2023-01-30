namespace eOdvjetnik;

public partial class App : Application
	{
	public App()
	{
		Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTAzMjU5MEAzMjMwMmUzNDJlMzBlM2lHT2xtUlAxZTFrL2RyR3RFb0tORlduRG5jTmljVUJQRGNjTEo1bnRjPQ==");


        InitializeComponent();

		MainPage = new AppShell();
	}
}
