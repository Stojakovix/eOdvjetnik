using eOdvjetnik.Data;
using eOdvjetnik.Views;
using Syncfusion.Maui.Scheduler;
using System.Globalization;
using System.Reflection;
using System.Resources;
using eOdvjetnik.ViewModel;


namespace eOdvjetnik;

public partial class App : Application
{

    public static NaplataViewModel SharedNaplataViewModel;
    public static KlijentiViewModel SharedKlijentiViewModel;



    public App()
    {
        InitializeComponent();
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjQ5MTQ1M0AzMjMyMmUzMDJlMzBRT2JkTm1HczFuTmdmNTVFcWNWU29xbGt6Z2lhRDFYYk1GZWppS3pjWnlNPQ==");


        Application.Current.UserAppTheme = AppTheme.Light;
        MainPage = new AppShell();

        SharedKlijentiViewModel = new KlijentiViewModel();


    }





}