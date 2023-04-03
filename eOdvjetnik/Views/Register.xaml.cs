using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Maui.Devices;
using eOdvjetnik.Data;
using eOdvjetnik.Models;
//using Microsoft.Maui.;

namespace eOdvjetnik.Views;

public partial class Register : ContentPage
{
    DeviceIdDatabase database;
    

    public ObservableCollection<DeviceIdItem> Items { get; set; } = new();

    public Register()//DocsDatabase docsdatabase
    {
        InitializeComponent();
        Debug.WriteLine("inicijal");
        
    }
    /*
    public DeviceIdItem Item
    {
        get => BindingContext as DeviceIdItem;
        set => BindingContext = value;
    }
    */
    private const string url = "https://cc.eodvjetnik.hr/token.json?token=";
    private HttpClient _Client = new HttpClient();
    


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

            Preferences.Set("key", time);
            Debug.WriteLine("spremio u preferences");
            string preferencesKey = Preferences.Get("key", "default_value");
            Debug.WriteLine("Izvađen iz preferences: " + preferencesKey);



            //Items = JsonSerializer.Deserialize<List<TodoItem>>(content, _serializerOptions);
            database.Add(new DeviceIdItem
            {
                HID = time
            });

            /*

                public class DeviceIdItem
        {
            [PrimaryKey, AutoIncrement]
            public int ID { get; set; }
            public string HID{ get; set; }

        }
             */



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

            



        }//Kraj IF preferences
    }

    public class TodoItem
    {
        public string Respond { get; set; }
        public string Licence { get; set; }
        public string Active { get; set; }

    }


    /*
    
    public class Licence
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("avatar")]
        public string AvatarURI { get; set; }

        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }
    }*/

    //public class Response
    //{
    //    [JsonProperty("Response")]
    //    public int Page { get; set; }

    //    [JsonProperty("per_page")]
    //    public int PerPage { get; set; }

    //    [JsonProperty("total")]
    //    public int Total { get; set; }

    //    [JsonProperty("total_pages")]
    //    public int TotalPages { get; set; }

    //    [JsonProperty("data")]
    //    public ObservableCollection<Licence> Licence { get; set; }
    //}
}