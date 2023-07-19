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


namespace eOdvjetnik.Views;


public partial class Dokumenti : ContentPage
{
    DocsDatabase database;

    public ObservableCollection<DocsItem> Items { get; set; } = new();

        public ObservableCollection<string> ShareFiles { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> ShareFolders { get; set; } = new ObservableCollection<string>();

    private const string IP = "IP Adresa";
    private const string USER = "Korisničko ime";
    private const string PASS = "Lozinka";
    private const string FOLDER = "Folder";
    private const string SUBFOLDER = "SubFolder";


    private void OnLabelTapped(object sender, EventArgs e)
    {
        StackLayout stackLayout = (StackLayout)sender;
        Label label = (Label)stackLayout.FindByName("DocumentLabel");
        string labelText = label.Text;
        // Do something with the label text

        // Do something with the item, such as display it in a message box
        DisplayAlert("Item Tapped", labelText, "OK");
    }


    public Dokumenti(DocsDatabase docsdatabase)
    {

        
        InitializeComponent();
        database = docsdatabase;
        BindingContext = this;
        
        //INICIRAJ SMB KONEKCIJU DA DOHVATI SVE DOKUMENTE
        try
        {


            SMB2Client client = new SMB2Client(); // SMB2Client can be used as well
            bool isConnected = client.Connect(System.Net.IPAddress.Parse(Preferences.Get(IP, "")), SMBTransportType.DirectTCPTransport);
            if (isConnected)
            {
                NTStatus status = client.Login(String.Empty, "user", "walter");
                Debug.WriteLine("6666666666666666666");
                Debug.WriteLine(status);
                Debug.WriteLine("6666666666666666666");
                if (status == NTStatus.STATUS_SUCCESS)
                {
                    List<string> shares = client.ListShares(out status);
                    
                    Debug.WriteLine("7777777777777777777");
                    foreach (string nesto in shares) {
                        Debug.WriteLine(nesto);
                    }
                    
                    Debug.WriteLine("7777777777777777777");
                    client.Logoff();
                }
                client.Disconnect();
            }

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
        var items = await database.GetItemsAsync();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }
        });
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
           
}