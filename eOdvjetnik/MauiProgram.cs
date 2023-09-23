using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using eOdvjetnik.Data;
using eOdvjetnik.Views;
using eOdvjetnik.Services;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Hosting;
//using Microsoft.UI.Xaml.Documents;
//using Microsoft.UI;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using eOdvjetnik.ViewModel;
using Plugin.LocalNotification;
//using Microsoft.UI;

namespace eOdvjetnik;
public static class MauiProgram
{


    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        
        builder.ConfigureSyncfusionCore();
        builder
            .UseMauiApp<App>()
            .UseLocalNotification()
            .ConfigureFonts(fonts =>
            {
                //fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                //fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("SF-Pro-Text-Light.otf", "SF-Pro-Text-Light");
                fonts.AddFont("SFUIDisplay-Ultralight.otf", "SFUIDisplay-Ultralight");
                fonts.AddFont("SFUIDisplay-Light.otf", "SFUIDisplay-Light");
                fonts.AddFont("SF-Pro-Display-Black.otf", "SF-Pro-Display-Black");
                fonts.AddFont("SF-Pro-Text-Medium.otf", "SF-Pro-Text-Medium");
                fonts.AddFont("SF-Pro-Text-Bold.otf", "SF-Pro-Text-Bold");
                fonts.AddFont("SF-Pro-Display-Bold.otf", "SF-Pro-Display-Bold");
                fonts.AddFont("SF-Pro-Display-Semibold.otf", "SF-Pro-Display-Semibold");





            });

        builder
            .UseMauiApp<App>();

        builder.Services.AddTransient<Dokumenti>();
        builder.Services.AddTransient<DocsItemPage>();
        builder.Services.AddTransient<DocsViewModel>();

        builder.Services.AddSingleton<Kalendar>();
        builder.Services.AddTransient<KalendarViewModel>();
        builder.Services.AddTransient<AppointmentDialog>();
        builder.Services.AddTransient<SpiDokViewModel>();
        builder.Services.AddTransient<KlijentiViewModel>();


        builder.Services.AddSingleton<DocsDatabase>();
        builder.Services.AddTransient<NoviSpis>();

        builder.Services.AddTransient<Spisi>();

        builder.Services.AddTransient<NoviKlijent>();
        //builder.Services.AddSingleton<SpiDok>();

        builder.Services.AddSingleton<KeyValueService>();


        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Licence.db3");

        builder.Services.AddSingleton(s => ActivatorUtilities.CreateInstance<DeviceIdDatabase>(s, dbPath));
        

        return builder.Build();


        Application.Current.UserAppTheme = AppTheme.Light;










    }
}
