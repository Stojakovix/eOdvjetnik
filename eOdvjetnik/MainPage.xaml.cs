using System.Diagnostics;
using System.Text;
using eOdvjetnik.Data;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;

//using OpenVpn;
//using WireGuardNT_PInvoke;



namespace eOdvjetnik;

public partial class MainPage : ContentPage
{
    //Varijable za NAS preferenceas
    private const string IP = "IP Adresa";
    private const string USER = "Korisničko ime";
    private const string PASS = "Lozinka";
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
    public MainPage()
    {
        InitializeComponent();
        ReadDeviceInfo();
        GetMicroSeconds();

    }
    private void OnSaveClicked(object sender, EventArgs e)
    {
        Preferences.Set(IP, IPEntry.Text);
        Preferences.Set(USER, USEREntry.Text);
        Preferences.Set(PASS, PASSEntry.Text);
        DisplayAlert("Success", "Data saved", "OK");
    }
    private void OnSaveClickedMySQL(object sender, EventArgs e)
    {
        Preferences.Set(IP_mysql, IPEntryMySQL.Text);
        Preferences.Set(USER_mysql, USEREntryMySQL.Text);
        Preferences.Set(PASS_mysql, PASSEntryMySQL.Text);
        Preferences.Set(databasename_mysql, databasenameEntryMySQL.Text);

        DisplayAlert("Success", "Data saved", "OK");
    }
    private void OnLoadClicked(object sender, EventArgs e)
    {

        var ip = Preferences.Get(IP, "");
        IPEntry.Text = ip;
        var user = Preferences.Get(USER, "");
        USEREntry.Text = user;
        var pass = Preferences.Get(PASS, "");
        PASSEntry.Text = pass;

        //Preferences.Set(IP, "");
        //Preferences.Set(USER, "");
        //Preferences.Set(PASS, "");
    }
    private void OnLoadClickedMySQL(object sender, EventArgs e)
    {
        var ipmysql = Preferences.Get(IP_mysql, "");
        IPEntryMySQL.Text = ipmysql;
        var useripmysql = Preferences.Get(USER_mysql, "");
        USEREntryMySQL.Text = useripmysql;
        var passipmysql = Preferences.Get(PASS_mysql, "");
        PASSEntryMySQL.Text = passipmysql;
        var databasenamemysql = Preferences.Get(databasename_mysql, "");
        databasenameEntryMySQL.Text = databasenamemysql;

        //Preferences.Set(IP, "");
        //Preferences.Set(USER, "");
        //Preferences.Set(PASS, "");
    }
    private void OnDeleteClicked(object sender, EventArgs e)
    {
        Preferences.Remove(IP);
        Preferences.Remove(USER);
        Preferences.Remove(PASS);
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
    public void sqlQuery(string query) {
        Debug.WriteLine("Usao u sqlQuerry  *******");
        // MySQL connection settings
        string connString = "server="+ Preferences.Get(IP_mysql, "") + ";user="+ Preferences.Get(USER_mysql, "") + ";password="+ Preferences.Get(PASS_mysql, "") + ";database="+ Preferences.Get(databasename_mysql, "");

        // Connect to MySQL database
        using MySqlConnection conn = new MySqlConnection(connString);
        conn.Open();

        // SQL query
        //string query = "SELECT * FROM your_table";

        // Execute query and retrieve data
        using MySqlCommand cmd = new MySqlCommand(query, conn);
        using MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Debug.WriteLine("Usao u while  *******");
            // Access data using column names or indices
            int column1 = reader.GetInt32("ID");
            string column2 = reader.GetString("EventName");
            
            Debug.WriteLine(column1);
            Debug.WriteLine(column2);

            // ...
        }

        // Close the reader and the connection
        reader.Close();
        conn.Close();
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
        if (string.IsNullOrEmpty(Preferences.Get("key", "default_value")))
        {

            var time = GetMicroSeconds();
            // ----------------- platform ispod --------------
            var device = DeviceInfo.Current.Platform;
            Debug.WriteLine("url je--------------------" + url + time);
            var httpResponse = await _Client.GetAsync(url + time);
            //Items = new List<TodoItem>();

            //Sprema u preferences index neku vrijednost iz varijable
            Preferences.Set("key", time);
            Debug.WriteLine("spremio u preferences");
            string preferencesKey = Preferences.Get("key", "default_value");
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
            Debug.WriteLine("VAŠ KLJUČ JE VEĆ IZGENERIRAN: " + Preferences.Get("key", "default_value"));





        }
        //Kraj IF preferences
         //MySQL Query
         sqlQuery("SELECT * FROM events;");

        // Check if the current connection profile is set to WiFi
        if (Connectivity.NetworkAccess == NetworkAccess.Internet && Connectivity.ConnectionProfiles.Contains(ConnectionProfile.WiFi))
        {
            // Your network code here
        }
        else
        {
            // Handle case where the device is not connected to a WiFi network
        }





    }




}

