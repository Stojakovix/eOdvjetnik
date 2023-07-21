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


    public App(){
        InitializeComponent();
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjQ5MTQ1M0AzMjMyMmUzMDJlMzBRT2JkTm1HczFuTmdmNTVFcWNWU29xbGt6Z2lhRDFYYk1GZWppS3pjWnlNPQ==");


        Application.Current.UserAppTheme = AppTheme.Light;
        MainPage = new AppShell();

        SharedNaplataViewModel = new NaplataViewModel();

    }





}
