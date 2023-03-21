using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Maui.Devices;

namespace eOdvjetnik.Views;

public partial class Register : ContentPage
{
	public Register()
	{
		InitializeComponent();
	}
    private const string url = "https://zadar-ict.hr/eodvjetnik/token.php?token=";
    private HttpClient _Client = new HttpClient();
    private ObservableCollection<Licence> userCollection;

    protected override async void OnAppearing()
    {

        base.OnAppearing();


        double timestamp = Stopwatch.GetTimestamp();
        double microseconds = 1_000_000.0 * timestamp / Stopwatch.Frequency;
        string hashedData= ComputeSha256Hash(microseconds.ToString());
        // ----------------- platform ispod --------------
        var device = DeviceInfo.Current.Platform;
        

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

        
        Debug.WriteLine("url je--------------------" + url + hashedData + device);
        var httpResponse = await _Client.GetAsync(url+ hashedData + device);
        //Items = new List<TodoItem>();

        string content = await httpResponse.Content.ReadAsStringAsync();
        //Items = JsonSerializer.Deserialize<List<TodoItem>>(content, _serializerOptions);


        Debug.WriteLine(content);
        if (httpResponse.IsSuccessStatusCode)
        {
            Debug.WriteLine("Uso u if");
            Response responseData = JsonConvert.DeserializeObject<Response>(await httpResponse.Content.ReadAsStringAsync());





            userCollection = new ObservableCollection<Licence>(responseData.Licence);
            User_List.ItemsSource = userCollection;
            Debug.WriteLine(responseData);
            Debug.WriteLine(userCollection);
            Debug.WriteLine(User_List);


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