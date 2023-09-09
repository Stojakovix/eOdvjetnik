using System.Diagnostics;
using System.Text;
using eOdvjetnik.Data;
using System.Security.Cryptography;
using eOdvjetnik.ViewModel;
using System.Globalization;
using System.Text.Json;
using eOdvjetnik.Models;
using System.Windows.Input;
using System.Threading;
using System;
using System.Resources;
using Microsoft.Maui.Controls;
using Xamarin.Essentials;
using Preferences = Microsoft.Maui.Storage.Preferences;
using Permissions = Microsoft.Maui.ApplicationModel.Permissions;
using PermissionStatus = Microsoft.Maui.ApplicationModel.PermissionStatus;
using DeviceType = Microsoft.Maui.Devices.DeviceType;
using DeviceInfo = Microsoft.Maui.Devices.DeviceInfo;
using CommunityToolkit.Mvvm.Messaging;

namespace eOdvjetnik;



public partial class MainPage : ContentPage
{



    //MySQL varijable
    public string query;


    //int count = 0;
    private const string url = "https://cc.eodvjetnik.hr/token.json?token=";
    private HttpClient _Client = new HttpClient();
    

    public string ActivationCode { get; set; }
    public string LicenceType { get; set; }
    public string ExpireDateString { get; set; }

    //KRAJ NAS


    private void OnRefreshLicenceClick(object sender, EventArgs e)
    {
        LicenceCheck();
    }

    private void LicenceUpdatedMessage()
    {
        WeakReferenceMessenger.Default.Send(new LicenceUpdated("Licence updated!"));
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

    public async void ActivationLoop()
    {
        Debug.WriteLine("MainPageViewModel - > ActivationLoop");
        string string1 = "https://cc.eodvjetnik.hr/eodvjetnikadmin/waiting-lists/request?cpuid=";
        string string2 = Preferences.Get("key", null);
        string activationURL = string.Concat(string1, string2);
        Debug.WriteLine("MainPageViewModel - > ActivationLoop - Spoio URL" + activationURL);
        try
        {
            Debug.WriteLine("MainPageViewModel - > ActivationLoop -> usao u try");

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(activationURL);
                Debug.WriteLine("MainPageViewModel - > ActivationLoop -> Dohvatio response");

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("MainPageViewModel - > ActivationLoop -> uspješno dohvatio");

                    string jsonContent = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("MainPageViewModel - > ActivationLoop -> content " + content);

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var data = System.Text.Json.JsonSerializer.Deserialize<ActivationData[]>(content, options);

                    ActivationCode = data[0].activation_code;
                    Preferences.Set("activation_code", ActivationCode);

                    Debug.WriteLine($"Received data: {data[0].id}, {data[0].created}, {data[0].hwid}, {data[0].IP}, {data[0].activation_code}");
                }
                else
                {
                    // Što ako se ne može povezati

                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Activation error:" + ex.Message);
        }
        LicenceCheck();
    }




    public async void LicenceCheck()
    {
        Debug.WriteLine("MainPageViewModel - > LicenceCheck");
        string string1 = "https://cc.eodvjetnik.hr/eodvjetnikadmin/licences/request?cpuid=";
        string string2 = Preferences.Get("key", null);
        string licenceURL = string.Concat(string1, string2);
        Debug.WriteLine("MainPageViewModel - > LicenceCheck - Spoio URL" + licenceURL);
        try
        {
            Debug.WriteLine("MainPageViewModel - > LicenceCheck -> usao u try");

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(licenceURL);
                Debug.WriteLine("MainPageViewModel - > LicenceCheck -> Dohvatio response");

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("MainPageViewModel - > LicenceCheck -> uspješno dohvatio");

                    string jsonContent = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("MainPageViewModel - > LicenceCheck -> content " + content);


                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var jsonObject = JsonSerializer.Deserialize<JsonElement>(content, options);

                    // int licenceIsActive = jsonObject.GetProperty("Devices")[0].GetProperty("licence_active").GetInt32(); 

                    string licenceIsActive = jsonObject.GetProperty("Devices")[0].GetProperty("licence_active").GetString();
                    int companyID = jsonObject.GetProperty("Devices")[0].GetProperty("company_id").GetInt32();
                    string company_ID = companyID.ToString();
                    int devicetypeID = jsonObject.GetProperty("Devices")[0].GetProperty("device_type_id").GetInt32();
                    string devicetype_ID = devicetypeID.ToString();
                    string licenceType = jsonObject.GetProperty("LicenceTypes")[0].GetProperty("naziv").GetString();
                    string expireDate = jsonObject.GetProperty("Licences")[0].GetProperty("expire_date").GetString();
                    string nazivTvrtke = jsonObject.GetProperty("Companies")[0].GetProperty("naziv").GetString();
                    string OIBTvrtke = jsonObject.GetProperty("Companies")[0].GetProperty("OIB").GetString();
                    string adresaTvrtke = jsonObject.GetProperty("Companies")[0].GetProperty("adresa").GetString();


                    Preferences.Set("expire_date", expireDate);
                    Preferences.Set("licence_type", licenceType);
                    Preferences.Set("licence_active", licenceIsActive);
                    Preferences.Set("naziv_tvrtke", nazivTvrtke);
                    Preferences.Set("OIBTvrtke", OIBTvrtke);
                    Preferences.Set("adresaTvrtke", adresaTvrtke);
                    Preferences.Set("company_id", company_ID);
                    Preferences.Set("device_type_id", devicetype_ID);


                    Debug.WriteLine("MainPageViewModel - > Company info: " + nazivTvrtke + OIBTvrtke + adresaTvrtke);

                    WeakReferenceMessenger.Default.Send(new CheckLicence("CheckLicence!"));

                }
                else
                {
                    // Što ako se ne može povezati
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Activation error:" + ex.Message);
        }

        LicenceUpdatedMessage();
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
        ActivationLoop();



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

    public static string GetMicroSeconds()
    {
        double timestamp = Stopwatch.GetTimestamp();
        double microseconds = 1_000_000.0 * timestamp / Stopwatch.Frequency;
        string hashedData = ComputeSha256Hash(microseconds.ToString());

        return hashedData;

    }
    static string ComputeSha256Hash(string rawData)
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

