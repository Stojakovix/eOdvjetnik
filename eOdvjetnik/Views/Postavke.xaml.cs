using eOdvjetnik.Services;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;

namespace eOdvjetnik.Views;

public partial class Postavke : ContentPage
{
    public Postavke()
	{
		InitializeComponent();
        this.BindingContext = App.SharedPostavkeViewModel;
        WeakReferenceMessenger.Default.Register<NoNasDetected>(this, EnterNAS);
        WeakReferenceMessenger.Default.Register<NoSQLDetected>(this, EnterSQL);


    }

    private async void OnLogoImageClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Odaberite sliku logotipa",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                CompanyLogo.Source = ImageSource.FromStream(() => stream);
                string filePath = result.FullPath;
                TrecaSreca.Set("LogoImagePath", filePath);
            }
            else
            {
                // User canceled the file picking, do nothing.
            }
        }
        catch (Exception ex)
        {
       
            Console.WriteLine($"Error picking image: {ex.Message}");
        }
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        this.Window.MinimumHeight = 620;
        this.Window.MinimumWidth = 860;
    }

    public double MinWidth { get; set; }
    public void EnterNAS(object recipient, NoNasDetected message)
    {

        Frame1.IsVisible = false;
        Frame2.IsVisible = false;
        Frame3.IsVisible = false;
        Frame4.IsVisible = true;
        Frame5.IsVisible = false;
        Frame7.IsVisible = false;
        Application.Current.MainPage.DisplayAlert("", "Za pristup dokumentima unesite NAS postavke", "OK");

    }

    public void EnterSQL(object recipient, NoSQLDetected message)
    {
        Frame1.IsVisible = false;
        Frame2.IsVisible = false;
        Frame3.IsVisible = false;
        Frame4.IsVisible = false;
        Frame5.IsVisible = true;
        Frame7.IsVisible = false;
        Application.Current.MainPage.DisplayAlert("", "Za pristup podatcima unesite SQL postavke", "OK");
    }



    private void Button1_Clicked(object sender, EventArgs e)
    {
        Frame1.IsVisible = true;
        Frame2.IsVisible = false;
        Frame3.IsVisible = false;
        Frame4.IsVisible = false;
        Frame5.IsVisible = false;
        Frame7.IsVisible = false;
    }

    private void Button2_Clicked(object sender, EventArgs e)
    {
        Frame1.IsVisible = false;
        Frame2.IsVisible = true;
        Frame3.IsVisible = false;
        Frame4.IsVisible = false;
        Frame5.IsVisible = false;
        Frame7.IsVisible = false;
    }
    private void Button3_Clicked(object sender, EventArgs e)
    {
        string ip_sql = TrecaSreca.Get("IP Adresa2");
        string user_sql = TrecaSreca.Get("Korisničko ime2");
        string pass_sql = TrecaSreca.Get("Lozinka2");

        if (String.IsNullOrEmpty(ip_sql) || String.IsNullOrEmpty(user_sql) || String.IsNullOrEmpty(pass_sql))
        {
            WeakReferenceMessenger.Default.Send(new NoSQLDetected("No SQL settings!"));
        }
        else
        {
            Frame1.IsVisible = false;
            Frame2.IsVisible = false;
            Frame3.IsVisible = true;
            Frame4.IsVisible = false;
            Frame5.IsVisible = false;
            Frame7.IsVisible = false;
        }

    }
    private void Button4_Clicked(object sender, EventArgs e)
    {
        Frame1.IsVisible = false;
        Frame2.IsVisible = false;
        Frame3.IsVisible = false;
        Frame4.IsVisible = true;
        Frame5.IsVisible = false;
        Frame7.IsVisible = false;
    }
    private void Button5_Clicked(object sender, EventArgs e)
    {
        Frame1.IsVisible = false;
        Frame2.IsVisible = false;
        Frame3.IsVisible = false;
        Frame4.IsVisible = false;
        Frame5.IsVisible = true;
        Frame7.IsVisible = false;

    }
    private void Button6_Clicked(object sender, EventArgs e)
    {
        Frame1.IsVisible = false;
        Frame2.IsVisible = false;
        Frame3.IsVisible = false;
        Frame4.IsVisible = false;
        Frame5.IsVisible = false;
        Frame7.IsVisible = false;
    }
    private void Button7_Clicked(object sender, EventArgs e)
    {
        Frame1.IsVisible = false;
        Frame2.IsVisible = false;
        Frame3.IsVisible = false;
        Frame4.IsVisible = false;
        Frame5.IsVisible = false;
        Frame7.IsVisible = true;
    }

 
}