using System.Diagnostics;
using System.Text;
using eOdvjetnik.Data;
using System.Security.Cryptography;
using eOdvjetnik.ViewModel;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Runtime.InteropServices.JavaScript;




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

    public string activation_code { get; set; }
    public string licence_type { get; set; }
    public string expire_date { get; set; }

    //KRAJ NAS

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

                    activation_code = data[0].activation_code;
                    Preferences.Set("activation_code", activation_code);

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
    }


    public class ActivationData
    {
        public int id { get; set; }
        public DateTime created { get; set; }
        public string hwid { get; set; }
        public string IP { get; set; }
        public string activation_code { get; set; }
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
                    string licenceType = jsonObject.GetProperty("LicenceTypes")[0].GetProperty("naziv").GetString();
                    string expireDate = jsonObject.GetProperty("Licences")[0].GetProperty("expire_date").GetString();

                    Preferences.Set("expire_date", expireDate);
                    Preferences.Set("licence_type", licenceType);
                    Preferences.Set("licence_active", licenceIsActive);


                    Debug.WriteLine("Datum isteka licence: " + expireDate);
                    Debug.WriteLine("Vrsta licence: " + expireDate);
                    Debug.WriteLine("Licenca je aktivna? " + licenceIsActive);



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
    }

    public class Device
    {
        public int id { get; set; }
        public int company_id { get; set; }
        public string hwid { get; set; }
        public int licence_active { get; set; }
        public int device_type_id { get; set; }
        public string opis { get; set; }
    }

    public class DevicesHasLicence
    {
        public int devices_id { get; set; }
        public int licences_id { get; set; }
    }

    public class Licence
    {
        public int id { get; set; }
        public int licence_type_id { get; set; }
        public DateTime expire_date { get; set; }
        public DateTime activation_date { get; set; }
        public int company_id { get; set; }
        public object automatic_renewal { get; set; }
    }

    public class Company
    {
        public int id { get; set; }
        public string naziv { get; set; }
        public string OIB { get; set; }
        public string adresa { get; set; }
        public string email { get; set; }
        public string telefon { get; set; }
        public string fax { get; set; }
        public string mobitel { get; set; }
        public int user_id { get; set; }
    }

    public class LicenceType
    {
        public int id { get; set; }
        public string naziv { get; set; }
        public int cijena { get; set; }
    }

    public class RootObject
    {
        public List<Device> Devices { get; set; }
        public List<DevicesHasLicence> DevicesHasLicences { get; set; }
        public List<Licence> Licences { get; set; }
        public List<Company> Companies { get; set; }
        public List<LicenceType> LicenceTypes { get; set; }
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
        string input = "24. 04. 2023. 09:00:00";
        DateTime parsedDateTime = DateTime.ParseExact(input, "dd. MM. yyyy. HH:mm:ss", CultureInfo.InvariantCulture);
        string output = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        Console.WriteLine(output);
        ActivationLoop();
        LicenceCheck();

    }
    //private void OnSaveClicked(object sender, EventArgs e)
    //{
    //    Preferences.Set(IP, IPEntry.Text);
    //    Preferences.Set(USER, USEREntry.Text);
    //    Preferences.Set(PASS, PASSEntry.Text);
    //    Preferences.Set(FOLDER, FOLDEREntry.Text);
    //    Preferences.Set(SUBFOLDER, SUBFOLDEREntry.Text);

    //    DisplayAlert("Success", "Data saved", "OK");
    //    NASPostavkeClicked(sender, e);
    //}
    //private void OnSaveClickedMySQL(object sender, EventArgs e)
    //{
    //    Preferences.Set(IP_mysql, IPEntryMySQL.Text);
    //    Preferences.Set(USER_mysql, USEREntryMySQL.Text);
    //    Preferences.Set(PASS_mysql, PASSEntryMySQL.Text);
    //    Preferences.Set(databasename_mysql, databasenameEntryMySQL.Text);

    //    DisplayAlert("Success", "Data saved", "OK");
    //    MySQLPostavkeClicked(sender, e);

    //}
    //private void OnLoadClicked(object sender, EventArgs e)
    //{

    //    var ip = Microsoft.Maui.Storage.Preferences.Get(IP, "");
    //    IPEntry.Text = ip;
    //    var user = Microsoft.Maui.Storage.Preferences.Get(USER, "");
    //    USEREntry.Text = user;
    //    var pass = Microsoft.Maui.Storage.Preferences.Get(PASS, "");
    //    PASSEntry.Text = pass;
    //    var folder = Microsoft.Maui.Storage.Preferences.Get(FOLDER, "");
    //    FOLDEREntry.Text = folder;
    //    var subfolder = Microsoft.Maui.Storage.Preferences.Get(SUBFOLDER, "");
    //    SUBFOLDEREntry.Text = subfolder;

    //    //Microsoft.Maui.Storage.Preferences.Set(IP, "");
    //    //Microsoft.Maui.Storage.Preferences.Set(USER, "");
    //    //Microsoft.Maui.Storage.Preferences.Set(PASS, "");
    //}
    //private void OnLoadClickedMySQL(object sender, EventArgs e)
    //{
    //    var ipmysql = Microsoft.Maui.Storage.Preferences.Get(IP_mysql, "");
    //    IPEntryMySQL.Text = ipmysql;
    //    var useripmysql = Microsoft.Maui.Storage.Preferences.Get(USER_mysql, "");
    //    USEREntryMySQL.Text = useripmysql;
    //    var passipmysql = Microsoft.Maui.Storage.Preferences.Get(PASS_mysql, "");
    //    PASSEntryMySQL.Text = passipmysql;
    //    var databasenamemysql = Microsoft.Maui.Storage.Preferences.Get(databasename_mysql, "");
    //    databasenameEntryMySQL.Text = databasenamemysql;

    //    //Microsoft.Maui.Storage.Preferences.Set(IP, "");
    //    //Microsoft.Maui.Storage.Preferences.Set(USER, "");
    //    //Microsoft.Maui.Storage.Preferences.Set(PASS, "");
    //}
    private void OnDeleteClicked(object sender, EventArgs e)
    {
        Preferences.Remove(IP);
        Preferences.Remove(USER);
        Preferences.Remove(PASS);
        Preferences.Remove(FOLDER);
        Preferences.Remove(SUBFOLDER);
        DisplayAlert("Success", "Data deleted", "OK");
    }
    private void OnDeleteClickedMySQL(object sender, EventArgs e)
    {
        Preferences.Remove(IP_mysql);
        Preferences.Remove(USER_mysql);
        Preferences.Remove(PASS_mysql);
        Preferences.Remove(databasename_mysql);
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
    //private void naspostavkeclicked(object sender, eventargs e)
    //{

    //    if (nasform.isvisible == true)
    //    {
    //        nasform.isvisible = false;
    //    }
    //    else
    //    {
    //        nasform.isvisible = true;
    //        onloadclicked("", e);

    //    }

    //}

    //private void MySQLPostavkeClicked(object sender, EventArgs e)
    //{
    //    if (MySQLForm.IsVisible == true)
    //    {
    //        MySQLForm.IsVisible = false;
    //    }
    //    else
    //    {
    //        MySQLForm.IsVisible = true;
    //        OnLoadClickedMySQL("", e);
    //    }
    //}


    protected override void OnAppearing()
    {

        base.OnAppearing();
        this.Window.MinimumHeight = 620;
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
                //var httpResponse = await _Client.GetAsync(url + time);
                //Items = new List<TodoItem>();

                //Sprema u preferences index neku vrijednost iz varijable
                Microsoft.Maui.Storage.Preferences.Set("key", time);
                Debug.WriteLine("spremio u preferences");
                string preferencesKey = Microsoft.Maui.Storage.Preferences.Get("key", null);
                Debug.WriteLine("Izvađen iz preferences: " + preferencesKey);
                //Items = JsonSerializer.Deserialize<List<TodoItem>>(content, _serializerOptions);
                /*

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


                */
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

