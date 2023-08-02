using eOdvjetnik.Data;
using eOdvjetnik.ViewModel;
using eOdvjetnik.Views;
using Syncfusion.Maui.Scheduler;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace eOdvjetnik;

public partial class App : Application
	{

    public static NaplataViewModel SharedNaplataViewModel;
    public static PostavkeViewModel SharedPostavkeViewModel;
    public static KlijentiViewModel SharedKlijentiViewModel;



    public App(){
        InitializeComponent();
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjQ5MTQ1M0AzMjMyMmUzMDJlMzBRT2JkTm1HczFuTmdmNTVFcWNWU29xbGt6Z2lhRDFYYk1GZWppS3pjWnlNPQ==");

        //SfSchedulerResources.ResourceManager = new ResourceManager("eOdvjetnik.Resources.SfScheduler", Application.Current.GetType().Assembly);
        SfSchedulerResources.ResourceManager = new ResourceManager("eOdvjetnik.Resources.SfScheduler.sr-SR", Application.Current.GetType().Assembly);
        //CultureInfo.CurrentCulture = new CultureInfo("hr-HR");
        //CultureInfo.CurrentUICulture = new CultureInfo("hr-HR");
        CultureInfo.CurrentCulture = new CultureInfo("sr-SR");
        CultureInfo.CurrentUICulture = new CultureInfo("sr-SR");

        Application.Current.UserAppTheme = AppTheme.Light;
        MainPage = new AppShell();

        SharedNaplataViewModel = new NaplataViewModel();
        SharedPostavkeViewModel = new PostavkeViewModel();
        SharedKlijentiViewModel = new KlijentiViewModel();
    }


    


}
