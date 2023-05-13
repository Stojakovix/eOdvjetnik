using System.Diagnostics;
using System.Text;
using eOdvjetnik.Data;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using System.Globalization;



//using OpenVpn;
//using WireGuardNT_PInvoke;



namespace eOdvjetnik;

public partial class MainPage : ContentPage
{
    //Varijable za NAS preferenceas
    private const string IP = "IP Adresa";
    private const string USER = "Korisničko ime";
    private const string PASS = "Lozinka";
    private const string FOLDER = "Folder";
    private const string SUBFOLDER = "SubFolder";

    //Varijable za MySQL preferences
    private const string IP_mysql = "IP Adresa2";
    private const string USER_mysql = "Korisničko ime2";
    private const string PASS_mysql = "Lozinka2";
    private const string databasename_mysql = "databasename";
    //MySQL varijable
    public string query;


    //int count = 0;
    private const string url = "https://cc.eodvjetnik.hr/token.json?token=";
    private HttpClient _Client = new HttpClient();
    DeviceIdDatabase database;

    //KRAJ NAS



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

        string input = "24. 04. 2023. 09:00:00";
        DateTime parsedDateTime = DateTime.ParseExact(input, "dd. MM. yyyy. HH:mm:ss", CultureInfo.InvariantCulture);
        string output = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        Console.WriteLine(output);



    }
    private void OnSaveClicked(object sender, EventArgs e)
    {
        Microsoft.Maui.Storage.Preferences.Set(IP, IPEntry.Text);
        Microsoft.Maui.Storage.Preferences.Set(USER, USEREntry.Text);
        Microsoft.Maui.Storage.Preferences.Set(PASS, PASSEntry.Text);
        Microsoft.Maui.Storage.Preferences.Set(FOLDER, FOLDEREntry.Text);
        Microsoft.Maui.Storage.Preferences.Set(SUBFOLDER, SUBFOLDEREntry.Text);

        DisplayAlert("Success", "Data saved", "OK");
    }
    private void OnSaveClickedMySQL(object sender, EventArgs e)
    {
        Microsoft.Maui.Storage.Preferences.Set(IP_mysql, IPEntryMySQL.Text);
        Microsoft.Maui.Storage.Preferences.Set(USER_mysql, USEREntryMySQL.Text);
        Microsoft.Maui.Storage.Preferences.Set(PASS_mysql, PASSEntryMySQL.Text);
        Microsoft.Maui.Storage.Preferences.Set(databasename_mysql, databasenameEntryMySQL.Text);

        DisplayAlert("Success", "Data saved", "OK");
    }
    private void OnLoadClicked(object sender, EventArgs e)
    {

        var ip = Microsoft.Maui.Storage.Preferences.Get(IP, "");
        IPEntry.Text = ip;
        var user = Microsoft.Maui.Storage.Preferences.Get(USER, "");
        USEREntry.Text = user;
        var pass = Microsoft.Maui.Storage.Preferences.Get(PASS, "");
        PASSEntry.Text = pass;
        var folder = Microsoft.Maui.Storage.Preferences.Get(FOLDER, "");
        FOLDEREntry.Text = folder;
        var subfolder = Microsoft.Maui.Storage.Preferences.Get(SUBFOLDER, "");
        SUBFOLDEREntry.Text = subfolder;

        //Microsoft.Maui.Storage.Preferences.Set(IP, "");
        //Microsoft.Maui.Storage.Preferences.Set(USER, "");
        //Microsoft.Maui.Storage.Preferences.Set(PASS, "");
    }
    private void OnLoadClickedMySQL(object sender, EventArgs e)
    {
        var ipmysql = Microsoft.Maui.Storage.Preferences.Get(IP_mysql, "");
        IPEntryMySQL.Text = ipmysql;
        var useripmysql = Microsoft.Maui.Storage.Preferences.Get(USER_mysql, "");
        USEREntryMySQL.Text = useripmysql;
        var passipmysql = Microsoft.Maui.Storage.Preferences.Get(PASS_mysql, "");
        PASSEntryMySQL.Text = passipmysql;
        var databasenamemysql = Microsoft.Maui.Storage.Preferences.Get(databasename_mysql, "");
        databasenameEntryMySQL.Text = databasenamemysql;

        //Microsoft.Maui.Storage.Preferences.Set(IP, "");
        //Microsoft.Maui.Storage.Preferences.Set(USER, "");
        //Microsoft.Maui.Storage.Preferences.Set(PASS, "");
    }
    private void OnDeleteClicked(object sender, EventArgs e)
    {
        Microsoft.Maui.Storage.Preferences.Remove(IP);
        Microsoft.Maui.Storage.Preferences.Remove(USER);
        Microsoft.Maui.Storage.Preferences.Remove(PASS);
        Microsoft.Maui.Storage.Preferences.Remove(FOLDER);
        Microsoft.Maui.Storage.Preferences.Remove(SUBFOLDER);
        DisplayAlert("Success", "Data deleted", "OK");
    }
    private void OnDeleteClickedMySQL(object sender, EventArgs e)
    {
        Microsoft.Maui.Storage.Preferences.Remove(IP_mysql);
        Microsoft.Maui.Storage.Preferences.Remove(USER_mysql);
        Microsoft.Maui.Storage.Preferences.Remove(PASS_mysql);
        Microsoft.Maui.Storage.Preferences.Remove(databasename_mysql);
        DisplayAlert("Success", "Data deleted", "OK");
    }
    
    private async void OnCounterClicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new kalendar());
        await Shell.Current.GoToAsync("///Dokumenti");

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
            Microsoft.Maui.Devices.DeviceType.Physical => false,
            Microsoft.Maui.Devices.DeviceType.Virtual => true,
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
    private void NASPostavkeClicked(object sender, EventArgs e)
    {
        
        if (NASForm.IsVisible == true)
        {
            NASForm.IsVisible = false;
        }
        else {
            NASForm.IsVisible = true;
            OnLoadClicked("", e);
           
        }

    }

    private void MySQLPostavkeClicked(object sender, EventArgs e) {
        if (MySQLForm.IsVisible == true)
        {
            MySQLForm.IsVisible = false;
        }
        else
        {
            MySQLForm.IsVisible = true;
            OnLoadClickedMySQL("", e);
        }
    }


    protected override async void OnAppearing()
    {

        base.OnAppearing();

        //Provjerava da li ima ključ spremnjen u preferences
        if (string.IsNullOrEmpty(Microsoft.Maui.Storage.Preferences.Get("key", "default_value")))
        {

            var time = GetMicroSeconds();
            // ----------------- platform ispod --------------
            var device = Microsoft.Maui.Devices.DeviceInfo.Current.Platform;
            Debug.WriteLine("url je--------------------" + url + time);
            var httpResponse = await _Client.GetAsync(url + time);
            //Items = new List<TodoItem>();

            //Sprema u preferences index neku vrijednost iz varijable
            Microsoft.Maui.Storage.Preferences.Set("key", time);
            Debug.WriteLine("spremio u preferences");
            string preferencesKey = Microsoft.Maui.Storage.Preferences.Get("key", "default_value");
            Debug.WriteLine("Izvađen iz preferences: " + preferencesKey);



            //Items = JsonSerializer.Deserialize<List<TodoItem>>(content, _serializerOptions);
            



            if (httpResponse.IsSuccessStatusCode)
            {

                string content = await httpResponse.Content.ReadAsStringAsync();
                Debug.WriteLine(content);
                Debug.WriteLine("Uso u if");


                // Response _ = JsonConvert.DeserializeObject<Response>(await httpResponse.Content.ReadAsStringAsync());
                // await database.SaveLicenseAsync(Models.License);

                //await database.SaveItemAsync(Item);

            }
            else
            {
                Debug.WriteLine("Nije uspio response");
            }//Kraj if httpResponse

        }
        else
        {
            Debug.WriteLine("VAŠ KLJUČ JE VEĆ IZGENERIRAN: " + Microsoft.Maui.Storage.Preferences.Get("key", "default_value"));





        }
        //Kraj IF preferences






    }




}

