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
//using Xamarin.Essentials;

namespace eOdvjetnik;
public static class MauiProgram
{


    public static MauiApp CreateMauiApp()
    {


        async void AskForWiFiPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    // Permission denied. Handle accordingly.
                }
            }
            else
            {
                // Permission already granted. Proceed with using WiFi.
            }
        }

        AskForWiFiPermission();




        var builder = MauiApp.CreateBuilder();

        builder.ConfigureSyncfusionCore();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                //fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                //fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("SF - Pro - Display - Bold.otf", "SF-Pro-Display-Bold");

            });



        builder.Services.AddSingleton<Dokumenti>();
        builder.Services.AddTransient<DocsItemPage>();

        builder.Services.AddTransient<Kalendar>();
        builder.Services.AddTransient<AppointmentDialog>();
        


        builder.Services.AddSingleton<DocsDatabase>();
        

        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Licence.db3");

        builder.Services.AddSingleton(s => ActivatorUtilities.CreateInstance<DeviceIdDatabase>(s, dbPath));


        return builder.Build();













    }
}
