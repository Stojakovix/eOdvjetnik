using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Maui.Devices;
using System.ComponentModel.DataAnnotations.Schema;
using eOdvjetnik.Data;

namespace eOdvjetnik.Views;

public partial class Register : ContentPage
{
    DeviceIdDatabase database;
    public Register()
    {
        InitializeComponent();
       // database = deviceIdDatabase;
    }
    private const string url = "https://zadar-ict.hr/eodvjetnik/token.php?token=";
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

        // ----------------- platform ispod --------------
        var device = DeviceInfo.Current.Platform;

        

        


        Debug.WriteLine("url je--------------------" + url + GetMicroSeconds() + device);
        var httpResponse = await _Client.GetAsync(url + GetMicroSeconds() + device);
        //Items = new List<TodoItem>();

        //Items = JsonSerializer.Deserialize<List<TodoItem>>(content, _serializerOptions);


        
        if (httpResponse.IsSuccessStatusCode)
        {

            string content = await httpResponse.Content.ReadAsStringAsync();
            Debug.WriteLine(content);
            Debug.WriteLine("Uso u if");
            

            Response _ = JsonConvert.DeserializeObject<Response>(await httpResponse.Content.ReadAsStringAsync());

        }
    }

    public class TodoItem
    {
        public string Respond { get; set; }
        public string Licence { get; set; }
        public string Active { get; set; }

    }



    
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
    }

    public class Response
    {
        [JsonProperty("Response")]
        public int Page { get; set; }

        [JsonProperty("per_page")]
        public int PerPage { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("data")]
        public ObservableCollection<Licence> Licence { get; set; }
    }
}