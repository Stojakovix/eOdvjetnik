using System.Collections.ObjectModel;
using eOdvjetnik.Data;
using eOdvjetnik.Models;
using SMBLibrary;
using SMBLibrary.Client;
using System;
using System.Reflection;
using SMBLibrary.SMB1;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using eOdvjetnik.ViewModel;
namespace eOdvjetnik.Views;


public partial class Dokumenti : ContentPage
{
    //DocsDatabase database;

    public ObservableCollection<DocsItem> Items { get; set; } = new();


   public const string IP = "IP Adresa";
   public const string USER = "Korisničko ime";
   public const string PASS = "Lozinka";
   public const string FOLDER = "Folder";
   public const string SUBFOLDER = "SubFolder";


    private void OnLabelTapped(object sender, EventArgs e)
    {
        StackLayout stackLayout = (StackLayout)sender;
        Label label = (Label)stackLayout.FindByName("DocumentLabel");
        string labelText = label.Text;
        // Do something with the label text

        // Do something with the item, such as display it in a message box
        DisplayAlert("Item Tapped", labelText, "OK");
    }

    //\Users\user\Spisi
    public Dokumenti(DocsDatabase docsdatabase)
    {
        
        //INICIRAJ SMB KONEKCIJU DA DOHVATI SVE DOKUMENTE
        try
        {

            InitializeComponent();
            //database = docsdatabase;
            BindingContext = new DocsViewModel();
          
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }

    //SMB


    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

    }

    async void OnItemAdded(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(DocsItemPage), true, new Dictionary<string, object>

        {
            ["Item"] = new DocsItem()
        });
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not DocsItem item)
            return;
        await Shell.Current.GoToAsync(nameof(DocsItemPage), true, new Dictionary<string, object>
        {
            ["Item"] = item

        });
    }

    //private async void Button_Clicked_home(object sender, EventArgs e)
    //{
    //    //await Shell.Current.GoToAsync(new(nameof(Dokumenti)));
    //    await Shell.Current.GoToAsync("//Dokumenti");
    //}

    private void Button_Clicked_nazad(object sender, EventArgs e)
    {

    }

    private void Button_Clicked_otvori(object sender, EventArgs e)
    {

    }
}