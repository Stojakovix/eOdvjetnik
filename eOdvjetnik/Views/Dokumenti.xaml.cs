using System.Collections.ObjectModel;
using eOdvjetnik.Data;
using eOdvjetnik.Models;
using SMBLibrary;
using SMBLibrary.Client;

namespace eOdvjetnik.Views;

public partial class Dokumenti : ContentPage
{


    DocsDatabase database;
    public ObservableCollection<DocsItem> Items { get; set; } = new();
    public Dokumenti(DocsDatabase docsdatabase)
    {

        
        InitializeComponent();
        database = docsdatabase;
        BindingContext = this;
        /*
         * 
         INICIRAJ MYSSQL KONEKCIJU DA DOHVATI SVE DOKUMENTE

        //SMB
        SMB2Client client = new SMB2Client();
        bool isConnected = client.Connect(System.Net.IPAddress.Parse("192.168.1.103"), SMBTransportType.DirectTCPTransport);
        if (isConnected)
        {
            NTStatus status = client.Login(String.Empty, "robi", "walter");
            if (status == NTStatus.STATUS_SUCCESS)
            {
                List<string> shares = client.ListShares(out _);
                System.Diagnostics.Debug.WriteLine("----------------------------------------------------------------");
                foreach (var share in shares)
                {
                    System.Diagnostics.Debug.WriteLine(share);
                }
                System.Diagnostics.Debug.WriteLine("----------------------------------------------------------------");
                DisplayAlert("Connection", "Connection established", "ok");
            }
            else
            {
                DisplayAlert("Connection", "Connection not established", "try again");

            }
            client.Logoff();
            client.Disconnect();
        }
        */
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