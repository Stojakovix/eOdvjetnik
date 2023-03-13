using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using eOdvjetnik.Data;
using eOdvjetnik.Views;
//using Microsoft.UI.Xaml.Documents;
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
			.ConfigureFonts(fonts =>
			{
                //fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                //fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("SF - Pro - Display - Bold.otf", "SF-Pro-Display-Bold");

            });


		builder.Services.AddSingleton<Dokumenti>();
		builder.Services.AddTransient<DocsItemPage>();

		builder.Services.AddSingleton<DocsDatabase>();



		return builder.Build();
	}
}
