using eOdvjetnik.Data;
using eOdvjetnik.ViewModel;
using eOdvjetnik.Views;
using Syncfusion.Maui.Scheduler;
using System.Diagnostics;
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

        var CultureString = CultureInfo.CurrentUICulture.Name;

        CultureInfo.CurrentCulture = new CultureInfo(CultureString);
        CultureInfo.CurrentUICulture = new CultureInfo(CultureString);
       // SfSchedulerResources.ResourceManager = new ResourceManager("eOdvjetnik.Resources.SfScheduler."+CultureString, Application.Current.GetType().Assembly);

        //SfSchedulerResources.ResourceManager = new ResourceManager("eOdvjetnik.Resources.SfScheduler", Application.Current.GetType().Assembly);

        //SfSchedulerResources.ResourceManager = new ResourceManager("eOdvjetnik.Resources.SfScheduler", Application.Current.GetType().Assembly);
        
        //CultureInfo.CurrentCulture = new CultureInfo("hr-HR");
        //CultureInfo.CurrentUICulture = new CultureInfo("hr-HR");
        //CultureInfo.CurrentCulture = new CultureInfo("sr-SR");
        //CultureInfo.CurrentUICulture = new CultureInfo("sr-SR");

  
        SfSchedulerResources.ResourceManager = new ResourceManager("eOdvjetnik.Resources.Strings.AppResources", Application.Current.GetType().Assembly);





        Application.Current.UserAppTheme = AppTheme.Light;
        MainPage = new AppShell();

        SharedNaplataViewModel = new NaplataViewModel();
        SharedPostavkeViewModel = new PostavkeViewModel();
        SharedKlijentiViewModel = new KlijentiViewModel();
    }


    


}
