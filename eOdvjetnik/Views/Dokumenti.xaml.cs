using System.Collections.ObjectModel;
using eOdvjetnik.Data;
using eOdvjetnik.Models;
using SMBLibrary;
using SMBLibrary.Client;
using System;
using System.Reflection;
using SMBLibrary.SMB1;
using Microsoft.Maui.Controls;
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
        
        StackLayout stack = (StackLayout)sender;
        string item = Label.TextProperty.ToString();
        // Do something with the item, such as display it in a message box
        DisplayAlert("Item Tapped", item, "OK");
    }


    public Dokumenti(DocsDatabase docsdatabase)
    {

        
        InitializeComponent();
        database = docsdatabase;
        BindingContext = this;
        
        //INICIRAJ SMB KONEKCIJU DA DOHVATI SVE DOKUMENTE
        try
        {
            //SMB
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

                        foreach (SMBLibrary.FileDirectoryInformation file in fileList)
                        {
                            Console.WriteLine($"Filename: {file.FileName}");
                            Console.WriteLine($"File Attributes: {file.FileAttributes}");
                            Console.WriteLine($"File Size: {file.AllocationSize / 1024}KB");
                            Console.WriteLine($"Created Date: {file.CreationTime.ToString("f")}");
                            Console.WriteLine($"Last Modified Date: {file.LastWriteTime.ToString("f")}");
                            Console.WriteLine("----------End of Folder/file-----------");
                            Console.WriteLine();
                            ShareFiles.Add(file.FileName);

                        }

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