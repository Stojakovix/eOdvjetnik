using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;

namespace eOdvjetnik.Views;

public partial class Register : ContentPage
{
	public Register()
	{
		InitializeComponent();
	}
    private const string url = "https://zadar-ict.hr/eodvjetnik/token.php?token=";
    private HttpClient _Client = new HttpClient();
    //private ObservableCollection<Licence> userCollection;

    protected override async void OnAppearing()
    {

        base.OnAppearing();


        double timestamp = Stopwatch.GetTimestamp();
        double microseconds = 1_000_000.0 * timestamp / Stopwatch.Frequency;

         

        Debug.WriteLine("url je--------------------" + url + microseconds);
        var httpResponse = await _Client.GetAsync(url+ microseconds);
        //Items = new List<TodoItem>();

        //Items = JsonSerializer.Deserialize<List<TodoItem>>(content, _serializerOptions);


        
        if (httpResponse.IsSuccessStatusCode)
        {

            string content = await httpResponse.Content.ReadAsStringAsync();
            Debug.WriteLine(content);
            Debug.WriteLine("Uso u if");
            /*

            Response responseData = JsonConvert.DeserializeObject<Response>(await httpResponse.Content.ReadAsStringAsync());




            
            userCollection = new ObservableCollection<Licence>(responseData.Licence);
            User_List.ItemsSource = userCollection;
            Debug.WriteLine(responseData);
            Debug.WriteLine(userCollection);
            Debug.WriteLine(User_List);
            */

        }
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
    }*/
}