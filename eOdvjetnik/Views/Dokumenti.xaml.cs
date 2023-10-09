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
using eOdvjetnik.Services;

namespace eOdvjetnik.Views;


public partial class Dokumenti : ContentPage
{
    //DocsDatabase database;
    //public DocsViewModel viewModel;
    public DocsViewModel viewModel = new DocsViewModel();
    public ObservableCollection<DocsItem> Items { get; set; } = new();


    public const string IP = "IP Adresa";
    public const string USER = "Korisničko ime";
    public const string PASS = "Lozinka";
    public const string FOLDER = "Folder";
    public const string SUBFOLDER = "SubFolder";


    //private void OnLabelTapped(object sender, EventArgs e)
    //{
    //    StackLayout stackLayout = (StackLayout)sender;
    //    Label label = (Label)stackLayout.FindByName("DocumentLabel");
    //    string labelText = label.Text;
    //    // Do something with the label text

    //    // Do something with the item, such as display it in a message box
    //    DisplayAlert("Item Tapped", labelText, "OK");
    //}

    private async void OnItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        var selectedFileItem = (DocsItem)e.DataItem;
        string fileName = selectedFileItem.Name;
        Debug.WriteLine(fileName);
        //if("filenamezavršava sa doc/folder il kako god")
        //{
        //    izvrši funkciju(connectandfetchasync) u viewmodelu
        //
        //} else
        //{
        // await viewModel.OpenFile(fileName);
        //}

        await viewModel.OpenFile(fileName);
    }
    protected override void OnDisappearing()
    {

    }


    public Dokumenti(DocsDatabase docsdatabase)
    {

        //INICIRAJ SMB KONEKCIJU DA DOHVATI SVE DOKUMENTE
        try
        {

            InitializeComponent();
            //database = docsdatabase;
            BindingContext = viewModel;
            string folder = TrecaSreca.Get(FOLDER);
            string subfolder = TrecaSreca.Get(SUBFOLDER);
            textEntry2.Text = "\\" + folder + "\\" + subfolder;


        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }

    //SMB


    //protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    //{
    //    base.OnNavigatedTo(args);

    //}

    // async void OnItemAdded(object sender, EventArgs e)
    // {
    //     await Shell.Current.GoToAsync(nameof(DocsItemPage), true, new Dictionary<string, object>
    //
    //     {
    //         ["Item"] = new DocsItem()
    //     });
    // }

    //private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    //{
    //    if (e.CurrentSelection.FirstOrDefault() is not DocsItem item)
    //        return;
    //    await Shell.Current.GoToAsync(nameof(DocsItemPage), true, new Dictionary<string, object>
    //    {
    //        ["Item"] = item
    //
    //    });
    //}

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