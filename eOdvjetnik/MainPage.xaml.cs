using SMBLibrary.Client;
using SMBLibrary;
using eOdvjetnik.Views;
using System.Diagnostics;
using Microsoft.Maui.Devices;
using System.Text;
using eOdvjetnik.Data;

namespace eOdvjetnik;

public partial class MainPage : ContentPage
{
    //int count = 0;

    public MainPage()
	{
		InitializeComponent();
        ReadDeviceInfo();

        
    }

	private async void OnCounterClicked(object sender, EventArgs e)
	{
        //await Navigation.PushAsync(new kalendar());
        await Shell.Current.GoToAsync("///Dokumenti");

    }
    public static void ReadDeviceInfo()
    {
        
        StringBuilder sb = new();

        sb.AppendLine($"Model: {DeviceInfo.Current.Model}");
        sb.AppendLine($"Manufacturer: {DeviceInfo.Current.Manufacturer}");
        sb.AppendLine($"Name: {DeviceInfo.Current.Name}");
        sb.AppendLine($"OS Version: {DeviceInfo.Current.VersionString}");
        sb.AppendLine($"Idiom: {DeviceInfo.Current.Idiom}");
        sb.AppendLine($"Platform: {DeviceInfo.Current.Platform}");

        bool isVirtual = DeviceInfo.Current.DeviceType switch
        {
            Microsoft.Maui.Devices.DeviceType.Physical => false,
            Microsoft.Maui.Devices.DeviceType.Virtual => true,
            _ => false
        };

        sb.AppendLine($"Virtual device? {isVirtual}");

        Debug.WriteLine(sb.ToString());
        

    }
}

