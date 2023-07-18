using eOdvjetnik.ViewModel;
using System.Diagnostics;

namespace eOdvjetnik.Views;

public partial class Postavke : ContentPage
{
    public Postavke()
	{
		InitializeComponent();
        BindingContext = new PostavkeViewModel();
        FetchCompanyDevices();
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
    protected override void OnAppearing()
    {
        base.OnAppearing();
        this.Window.MinimumHeight = 620;
        this.Window.MinimumWidth = 860;
    }
    public double MinWidth { get; set; }
    private void Button1_Clicked(object sender, EventArgs e)
    {
        Frame1.IsVisible = true;
        Frame2.IsVisible = false;
        Frame3.IsVisible = false;
        Frame4.IsVisible = false;
        Frame5.IsVisible = false;
        Frame6.IsVisible = false;
        Frame7.IsVisible = false;
    }

    private void Button2_Clicked(object sender, EventArgs e)
    {
        Frame1.IsVisible = false;
        Frame2.IsVisible = true;
        Frame3.IsVisible = false;
        Frame4.IsVisible = false;
        Frame5.IsVisible = false;
        Frame6.IsVisible = false;
        Frame7.IsVisible = false;
    }
    private void Button3_Clicked(object sender, EventArgs e)
    {
        Frame1.IsVisible = false;
        Frame2.IsVisible = false;
        Frame3.IsVisible = true;
        Frame4.IsVisible = false;
        Frame5.IsVisible = false;
        Frame6.IsVisible = false;
        Frame7.IsVisible = false;
    }
    private void Button4_Clicked(object sender, EventArgs e)
    {
        Frame1.IsVisible = false;
        Frame2.IsVisible = false;
        Frame3.IsVisible = false;
        Frame4.IsVisible = true;
        Frame5.IsVisible = false;
        Frame6.IsVisible = false;
        Frame7.IsVisible = false;
    }
    private void Button5_Clicked(object sender, EventArgs e)
    {
        Frame1.IsVisible = false;
        Frame2.IsVisible = false;
        Frame3.IsVisible = false;
        Frame4.IsVisible = false;
        Frame5.IsVisible = true;
        Frame6.IsVisible = false;
        Frame7.IsVisible = false;

    }
    private void Button6_Clicked(object sender, EventArgs e)
    {
        Frame1.IsVisible = false;
        Frame2.IsVisible = false;
        Frame3.IsVisible = false;
        Frame4.IsVisible = false;
        Frame5.IsVisible = false;
        Frame6.IsVisible = true;
        Frame7.IsVisible = false;
    }
    private void Button7_Clicked(object sender, EventArgs e)
    {
        Frame1.IsVisible = false;
        Frame2.IsVisible = false;
        Frame3.IsVisible = false;
        Frame4.IsVisible = false;
        Frame5.IsVisible = false;
        Frame6.IsVisible = false;
        Frame7.IsVisible = true;
    }


}