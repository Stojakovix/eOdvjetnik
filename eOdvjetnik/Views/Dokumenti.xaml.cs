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












            /*
            //DisplayAlert("Error", "asd","OK");

            SMB2Client client = new SMB2Client();
            bool isConnected = client.Connect(System.Net.IPAddress.Parse(Preferences.Get(IP, "")), SMBTransportType.DirectTCPTransport);
            NTStatus status = client.Login(String.Empty, Preferences.Get(USER, ""), Preferences.Get(PASS, ""));
            Debug.WriteLine("6666666666666666666");
            Debug.WriteLine(status);
            Debug.WriteLine("6666666666666666666");







            ISMBFileStore fileStore = client.TreeConnect(@"\\", out status);
            if (status == NTStatus.STATUS_SUCCESS)
            {
                Debug.WriteLine("7777777777777777777");
                Debug.WriteLine(status);
                Debug.WriteLine("7777777777777777777");
                object directoryHandle;
                FileStatus fileStatus;
                status = fileStore.CreateFile(out directoryHandle, out fileStatus, String.Empty, AccessMask.GENERIC_READ, SMBLibrary.FileAttributes.Directory, ShareAccess.Read | ShareAccess.Write, CreateDisposition.FILE_OPEN, CreateOptions.FILE_DIRECTORY_FILE, null);
                //status = fileStore.CreateFile(out directoryHandle, out fileStatus, "*", AccessMask.SYNCHRONIZE | (AccessMask)DirectoryAccessMask.FILE_LIST_DIRECTORY, 0, ShareAccess.Read | ShareAccess.Write | ShareAccess.Delete, CreateDisposition.FILE_OPEN, CreateOptions.FILE_SYNCHRONOUS_IO_NONALERT | CreateOptions.FILE_DIRECTORY_FILE, null);

                if (status == NTStatus.STATUS_SUCCESS)
                {
                    Debug.WriteLine("8888888888888888888");
                    Debug.WriteLine(status);
                    Debug.WriteLine("8888888888888888888");
                    List<QueryDirectoryFileInformation> fileList;
                    status = fileStore.QueryDirectory(out fileList, directoryHandle, "*", FileInformationClass.FileDirectoryInformation);
                    status = fileStore.CloseFile(directoryHandle);
                    foreach (SMBLibrary.FileDirectoryInformation file in fileList)
                    {
                        Debug.WriteLine($"Filename: {file.FileName}");
                        Debug.WriteLine($"File Attributes: {file.FileAttributes}");
                        Debug.WriteLine($"File Size: {file.AllocationSize / 1024}KB");
                        Debug.WriteLine($"Created Date: {file.CreationTime.ToString("f")}");
                        Debug.WriteLine($"Last Modified Date: {file.LastWriteTime.ToString("f")}");
                        Debug.WriteLine("----------End of Folder/file-----------");
                        //Debug.WriteLine();
                        ShareFiles.Add(file.FileName);


                    }
                    status = fileStore.Disconnect();
                }
                else {
                    Debug.WriteLine("9999999999999999999");
                    Debug.WriteLine(status);
                    Debug.WriteLine("9999999999999999999");
                }
            }
            else
            {
                Debug.WriteLine("10101010010101010101");
                Debug.WriteLine(status);
                //DisplayAlert("Error", string(status), "OK");
                Debug.WriteLine("10101010010101010101");
            }
            */
            /*
            //SMB2
            SMB2Client client = new SMB2Client();
            bool isConnected = client.Connect(System.Net.IPAddress.Parse(Preferences.Get(IP, "")), SMBTransportType.DirectTCPTransport);
            if (isConnected)
            {
                NTStatus status = client.Login(String.Empty, Preferences.Get(USER, ""), Preferences.Get(PASS, ""));
                    ISMBFileStore fileStore = client.TreeConnect(Preferences.Get(FOLDER, ""), out status);
                    if (status == NTStatus.STATUS_SUCCESS)
                    {
                        object directoryHandle;
                        FileStatus fileStatus;
                        status = fileStore.CreateFile(out directoryHandle, out fileStatus, Preferences.Get(SUBFOLDER, ""), AccessMask.GENERIC_READ, SMBLibrary.FileAttributes.Directory, ShareAccess.Read | ShareAccess.Write, CreateDisposition.FILE_OPEN, CreateOptions.FILE_DIRECTORY_FILE, null);
                        List<QueryDirectoryFileInformation> fileList;
                        status = fileStore.QueryDirectory(out fileList, directoryHandle, "*", FileInformationClass.FileDirectoryInformation);
                    Debug.WriteLine("---------------------Before foreach");
                    foreach (SMBLibrary.FileDirectoryInformation file in fileList)
                        {
                            Debug.WriteLine($"Filename: {file.FileName}");
                            Debug.WriteLine($"File Attributes: {file.FileAttributes}");
                            Debug.WriteLine($"File Size: {file.AllocationSize / 1024}KB");
                            Debug.WriteLine($"Created Date: {file.CreationTime.ToString("f")}");
                            Debug.WriteLine($"Last Modified Date: {file.LastWriteTime.ToString("f")}");
                            Debug.WriteLine("----------End of Folder/file-----------");
                            Debug.WriteLine("---------------------Before foreach");
                            ShareFiles.Add(file.FileName);

                        }
                    Debug.WriteLine("-----------------------After foreach");
                    //var listView = new ListView
                    //{
                    //    ItemsSource = ShareFiles
                    //};

                    //listView.ItemTemplate = new DataTemplate(() =>
                    //{
                    //    var imageCell = new ImageCell();
                    //    imageCell.SetBinding(ImageCell.TextProperty, ".");
                    //    imageCell.SetBinding(ImageCell.ImageSourceProperty, new Binding("Resources/icons/folder_1484.png", BindingMode.Default, null, null, null, new FileImageSourceConverter()));
                    //    return imageCell;
                    //});

                    status = fileStore.CloseFile(directoryHandle);
                        foreach (var file1 in fileList)
                        {
                            System.Diagnostics.Debug.WriteLine(file1.Length.ToString());
                            System.Diagnostics.Debug.WriteLine(file1.ToString());
                        System.Diagnostics.Debug.WriteLine("44444444444444");
                    }
                        System.Diagnostics.Debug.WriteLine("44444444444444");
                        System.Diagnostics.Debug.WriteLine(fileList);
                    }
                    System.Diagnostics.Debug.WriteLine("----------------------------------------------------------------");
                    //DisplayAlert("Connection", "Connection established", "ok");
                }
                else
                {
                    DisplayAlert("Connection", "Connection not established", "try again");
                Shell.Current.GoToAsync("..");
                }
                client.Logoff();
                client.Disconnect();
            */
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