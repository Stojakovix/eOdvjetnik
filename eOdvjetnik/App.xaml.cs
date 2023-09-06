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
    public static readonly BindableProperty BackgroundProperty;
    public static readonly BindableProperty TextStyleProperty;
    public static readonly BindableProperty TodayBackgroundProperty;
    public static readonly BindableProperty TrailingMonthBackgroundProperty;
    public static readonly BindableProperty TrailingMonthTextStyleProperty;
    public SchedulerTextStyle TrailingMonthTextStyle { get; set; }
    public SchedulerWeekNumberStyle WeekNumberStyle { get; set; }
    public SfScheduler Scheduler { get; }

    public App(){
        InitializeComponent();
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjQ5MTQ1M0AzMjMyMmUzMDJlMzBRT2JkTm1HczFuTmdmNTVFcWNWU29xbGt6Z2lhRDFYYk1GZWppS3pjWnlNPQ==");
       


        var schedulerTextStyle = new SchedulerTextStyle()
        {
            TextColor = Colors.Red,
            FontSize = 14,
            FontFamily = "SF-Pro-Display-Black"
        };
        var schedulerWeekNumberStyle = new SchedulerWeekNumberStyle()
        {
            Background = Brush.LightGreen,
            TextStyle = schedulerTextStyle,
     
         };
        this.Scheduler = new SfScheduler(); // Initialize the Scheduler property
        this.Scheduler.WeekNumberStyle = schedulerWeekNumberStyle;


        string currentCulture = Preferences.Get("CurrentCulture", null);
        if (currentCulture == null)
        {
            // CurrentCulture is not set, save it in preferences
            //string cultureName = CultureInfo.CurrentCulture.Name;
            Preferences.Set("CurrentCulture", "hr-HR");
        }


        //var CultureString = CultureInfo.CurrentUICulture.Name;
        var CultureString = Preferences.Get("CurrentCulture", null);

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
