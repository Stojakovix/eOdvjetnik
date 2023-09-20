using eOdvjetnik.ViewModel;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace eOdvjetnik.Views;

public partial class Postavke : ContentPage
{
    public Postavke()
	{
		InitializeComponent();
        this.BindingContext = App.SharedPostavkeViewModel;

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
                Preferences.Set("LogoImagePath", filePath);
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
        Frame1.IsVisible = false;
        Frame2.IsVisible = false;
        Frame3.IsVisible = true;
        Frame4.IsVisible = false;
        Frame5.IsVisible = false;
        Frame7.IsVisible = false;
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