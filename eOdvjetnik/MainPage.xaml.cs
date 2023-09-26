using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;
using eOdvjetnik.ViewModel;
using System.Globalization;
using Preferences = Microsoft.Maui.Storage.Preferences;
using Permissions = Microsoft.Maui.ApplicationModel.Permissions;
using PermissionStatus = Microsoft.Maui.ApplicationModel.PermissionStatus;
using DeviceType = Microsoft.Maui.Devices.DeviceType;
using DeviceInfo = Microsoft.Maui.Devices.DeviceInfo;
using Plugin.LocalNotification;

namespace eOdvjetnik;



public partial class MainPage : ContentPage
{

    private const string url = "https://cc.eodvjetnik.hr/token.json?token="; //DD: čemu ovo?
    private HttpClient _Client = new HttpClient();
      
    private void SaveNotification()
    {
        try
        {
            var request = new NotificationRequest
            {
                NotificationId = 1,
                Title = "Kliknut kalendar",
                Description = "U kalendaru je dodan novi događaj",
                BadgeNumber = 1,
                CategoryType = NotificationCategoryType.Status,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(5),
                }
            };
            LocalNotificationCenter.Current.AreNotificationsEnabled();
            LocalNotificationCenter.Current.Show(request);
            Debug.WriteLine(request.Title);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
       
    private void OnLanguageSelected(object sender, EventArgs e)
    {
        // Get the selected language from the dropdown
        var selectedLanguage = ((Picker)sender).SelectedItem.ToString();

        // Set the current culture to the selected language
        Thread.CurrentThread.CurrentCulture = new CultureInfo(selectedLanguage);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(selectedLanguage);

        Preferences.Set("CurrentCulture", selectedLanguage);
        // Refresh the UI to reflect the changes in language
        InitializeComponent();
    }

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

    public MainPage()
    {
        InitializeComponent();
        ReadDeviceInfo();
        GetMicroSeconds();
        AskForWiFiPermission();
        BindingContext = new MainPageViewModel();


    }


    private async void BrowserOpen_Clicked(object sender, EventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://cc.eodvjetnik.hr/users/add");
            await Microsoft.Maui.ApplicationModel.Browser.Default.OpenAsync(uri, Microsoft.Maui.ApplicationModel.BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            // An unexpected error occurred. No browser may be installed on the device.
        }
    }

    public static void ReadDeviceInfo()
    {

        StringBuilder sb = new();
        sb.AppendLine($"Model: {Microsoft.Maui.Devices.DeviceInfo.Current.Model}");
        sb.AppendLine($"Manufacturer: {Microsoft.Maui.Devices.DeviceInfo.Current.Manufacturer}");
        sb.AppendLine($"Name: {Microsoft.Maui.Devices.DeviceInfo.Current.Name}");
        sb.AppendLine($"OS Version: {Microsoft.Maui.Devices.DeviceInfo.Current.VersionString}");
        sb.AppendLine($"Idiom: {Microsoft.Maui.Devices.DeviceInfo.Current.Idiom}");
        sb.AppendLine($"Platform: {Microsoft.Maui.Devices.DeviceInfo.Current.Platform}");

        bool isVirtual = Microsoft.Maui.Devices.DeviceInfo.Current.DeviceType switch
        {
            DeviceType.Physical => false,
            DeviceType.Virtual => true,
            _ => false
        };

        sb.AppendLine($"Virtual device? {isVirtual}");
        Debug.WriteLine(sb.ToString());


    }

    public static string GetMicroSeconds() //DD: čemu ovo?
    {
        double timestamp = Stopwatch.GetTimestamp();
        double microseconds = 1_000_000.0 * timestamp / Stopwatch.Frequency;
        string hashedData = ComputeSha256Hash(microseconds.ToString());

        return hashedData;

    }
    static string ComputeSha256Hash(string rawData) //DD: čemu ovo?
    {
        // Create a SHA256   
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // ComputeHash - returns byte array  
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            // Convert byte array to a string   
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

    }
   
    protected override void OnAppearing()
    {

        // Get the current device information
        var deviceInfo = DeviceInfo.Current;
        Debug.WriteLine(deviceInfo);
        // Get the culture information


        // Get the current culture information
        var cultureInfo = CultureInfo.CurrentCulture;
        Debug.WriteLine(cultureInfo);
        // Get the language code
        var languageCode = cultureInfo.TwoLetterISOLanguageName;
        Debug.WriteLine(languageCode);
        // Get the country/region code
        var countryCode = cultureInfo.ThreeLetterISOLanguageName;
        Debug.WriteLine(countryCode);
        // Get the country/region code
        var countryCode2 = cultureInfo.ThreeLetterWindowsLanguageName;
        Debug.WriteLine(countryCode2);


        base.OnAppearing();
        this.Window.MinimumHeight = 680;
        this.Window.MinimumWidth = 860;
        try
        {
            //zakomentirati nakon setanja na null
            //Microsoft.Maui.Storage.Preferences.Set("key", null);
            //Provjerava da li ima ključ spremnjen u preferences
            if (string.IsNullOrEmpty(Microsoft.Maui.Storage.Preferences.Get("key", null)))
            {

                base.OnAppearing();

                var time = GetMicroSeconds();
                // ----------------- platform ispod --------------
                var device = Microsoft.Maui.Devices.DeviceInfo.Current.Platform;
                Debug.WriteLine("url je----------------main" + url + time);
                

                //Sprema u preferences index neku vrijednost iz varijable
                Microsoft.Maui.Storage.Preferences.Set("key", time);
                Debug.WriteLine("spremio u preferences");
                string preferencesKey = Microsoft.Maui.Storage.Preferences.Get("key", null);
                Debug.WriteLine("Izvađen iz preferences: " + preferencesKey);
                
            }
            else
            {
                Debug.WriteLine("VAŠ KLJUČ JE VEĆ IZGENERIRAN: " + Microsoft.Maui.Storage.Preferences.Get("key", null));



            }
        }
        //Kraj IF preferences

        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message + " in MainPage");
        }

    }

}

