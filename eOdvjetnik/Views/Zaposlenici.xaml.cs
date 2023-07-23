using eOdvjetnik.ViewModel;
using System.Diagnostics;

namespace eOdvjetnik.Views;
public partial class Zaposlenici : ContentPage
{
	public Zaposlenici()
	{
        InitializeComponent();
        BindingContext = App.SharedPostavkeViewModel;
    }

    public async void FetchCompanyDevices()
    {
        Debug.WriteLine("PostavkeModel - > FetchCompanyDevices");
        string string1 = "https://cc.eodvjetnik.hr/eodvjetnikadmin/devices/getAll?cpuid=";
        string string2 = Preferences.Get("key", null);
        string activationURL = string.Concat(string1, string2);
        Debug.WriteLine("PostavkeModel - > FetchCompanyDevices - URL: " + activationURL);
        try
        {
            Debug.WriteLine("PostavkeModel - > FetchCompanyDevices -> Try");

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(activationURL);
                Debug.WriteLine("PostavkeModel - > FetchCompanyDevices -> Received a response");

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("PostavkeModel - > FetchCompanyDevices -> Received data");

                    string jsonContent = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("PostavkeModel - > FetchCompanyDevices -> Json string: " + content);
                    string json_devices = content.ToString();
                    Preferences.Set("json_devices", json_devices);

                }
                else
                {
                    // 
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("FetchCompanyDevices error: " + ex.Message);
        }
    }


}