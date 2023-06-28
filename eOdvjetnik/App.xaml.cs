using eOdvjetnik.Data;
using eOdvjetnik.Views;
using Syncfusion.Maui.Scheduler;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace eOdvjetnik;

public partial class App : Application
	{
    static SchedulerDatabase database;


	public App(){
        InitializeComponent();
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjQ5MTQ1M0AzMjMyMmUzMDJlMzBRT2JkTm1HczFuTmdmNTVFcWNWU29xbGt6Z2lhRDFYYk1GZWppS3pjWnlNPQ==");
       //CultureInfo.CurrentUICulture = new CultureInfo("hr-HR");
       //SfSchedulerResources.ResourceManager = new ResourceManager("eOdvjetnik.SfScheduler", Application.Current.GetType().Assembly);


        MainPage = new AppShell();

	}


    public static SchedulerDatabase Database
    {
        get
        {
            if (database == null){
                database = new SchedulerDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "SchedulerDatabase.db3"));
            }
            return database;
        }
    }


}
