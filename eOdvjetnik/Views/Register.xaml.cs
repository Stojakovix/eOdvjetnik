using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace eOdvjetnik.Views;

public partial class Register : ContentPage
{
	public Register()
	{
		InitializeComponent();
	}
    private const string url = "https://zadar-ict.hr/eodvjetnik/token.php?token=";
    private HttpClient _Client = new HttpClient();
    private ObservableCollection<User> userCollection;

    protected override async void OnAppearing()
    {

        base.OnAppearing();
        Debug.WriteLine("url je--------------------" + url);
        var httpResponse = await _Client.GetAsync(url);
      

        if (httpResponse.IsSuccessStatusCode)
        {
            Response responseData = JsonConvert.DeserializeObject<Response>(await httpResponse.Content.ReadAsStringAsync());
            userCollection = new ObservableCollection<User>(responseData.Users);
            User_List.ItemsSource = userCollection;
        }
    }

    public class User
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
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("per_page")]
        public int PerPage { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("data")]
        public ObservableCollection<User> Users { get; set; }
    }
}