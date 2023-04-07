using System.Collections.ObjectModel;
using eOdvjetnik.Data;
using eOdvjetnik.Models;
using SMBLibrary;
using SMBLibrary.Client;
using System;
using System.Reflection;


namespace eOdvjetnik.Views
{
    public partial class Dokumenti : ContentPage
    {
        DocsDatabase database;

        public ObservableCollection<DocsItem> Items { get; set; } = new();

        public ObservableCollection<string> ShareFiles { get; set; } = new ObservableCollection<string>();

        private const string IP = "IP Adresa";
        private const string USER = "Korisničko ime";
        private const string PASS = "Lozinka";

        public Dokumenti(DocsDatabase docsdatabase)
        {
            InitializeComponent();
            database = docsdatabase;
            BindingContext = this;

            //INICIRAJ SMB KONEKCIJU DA DOHVATI SVE DOKUMENTE

            //SMB
            SMB2Client client = new SMB2Client();
            bool isConnected = client.Connect(System.Net.IPAddress.Parse(Preferences.Get(IP, "")), SMBTransportType.DirectTCPTransport);
            if (isConnected)
            {
                NTStatus status = client.Login(String.Empty, Preferences.Get(USER, ""), Preferences.Get(PASS, ""));

                if (status == NTStatus.STATUS_SUCCESS)
                {

                    /*
                    Type type = client.ListShares(out _).GetType();
                    PropertyInfo[] properties = type.GetProperties();

                    foreach (PropertyInfo property in properties)
                    {
                        object value = property.GetValue(client.ListShares(out _));
                        Console.WriteLine($"{property.Name}: {value}");
                    }
                    */
                    List<string> shares = client.ListShares(out _);
                    System.Diagnostics.Debug.WriteLine("----------------------------------------------------------------");

                    foreach (var share in shares)
                    {
                        System.Diagnostics.Debug.WriteLine(share);
                        ShareFiles.Add(share);
                    }
                    System.Diagnostics.Debug.WriteLine("-------------------111111------------------");


                    ISMBFileStore fileStore = client.TreeConnect("Racuni", out status);
                    if (status == NTStatus.STATUS_SUCCESS)
                    {
                        System.Diagnostics.Debug.WriteLine("-------------------222222------------------");

                        object directoryHandle;
                        FileStatus fileStatus;
                        status = fileStore.CreateFile(out directoryHandle, out fileStatus, String.Empty, AccessMask.GENERIC_READ, SMBLibrary.FileAttributes.Directory, ShareAccess.Read | ShareAccess.Write, CreateDisposition.FILE_OPEN, CreateOptions.FILE_DIRECTORY_FILE, null);

                        System.Diagnostics.Debug.WriteLine("-------------------333333------------------");

                        List<QueryDirectoryFileInformation> fileList;
                        status = fileStore.QueryDirectory(out fileList, directoryHandle, "*", FileInformationClass.FileDirectoryInformation);



                        status = fileStore.CloseFile(directoryHandle);
                        foreach (var file1 in fileList)
                        {
                            System.Diagnostics.Debug.WriteLine(file1.Length.ToString());
                            System.Diagnostics.Debug.WriteLine(file1.ToString());

                        }

                        System.Diagnostics.Debug.WriteLine("44444444444444");

                        System.Diagnostics.Debug.WriteLine(fileList);



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
}
