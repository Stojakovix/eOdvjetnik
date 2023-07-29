using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using eOdvjetnik.Data;
using eOdvjetnik.Models;
using SMBLibrary.Client;
using SMBLibrary;
using eOdvjetnik.Services;
using Microsoft.Maui.Storage;

namespace eOdvjetnik.ViewModel
{
    public class DocsViewModel : INotifyPropertyChanged
    {
        DocsDatabase database;
        SMBConnect sMBConnect;

        private ObservableCollection<DocsItem> items;
        public ObservableCollection<DocsItem> Items
        {
            get => items;
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private const string IP = "IP Adresa";
        private const string USER = "Korisničko ime";
        private const string PASS = "Lozinka";
        private const string FOLDER = "Folder";
        private const string SUBFOLDER = "SubFolder";

        public DocsViewModel()
        {

            Items = new ObservableCollection<DocsItem>();
            GetDocuments();
            SMBConnect sMBConnect = new SMBConnect();

        }
        public async void GetDocuments()
        {
            try
            {
                await ConnectAndFetchDocumentsAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public async Task ConnectAndFetchDocumentsAsync()
        {
            try
            {
                //Ispis svega na putanji
                List<QueryDirectoryFileInformation> fileList = sMBConnect.ListPath("Users");

                //Root share list
                List<string> shares = sMBConnect.getRootShare();


                foreach (SMBLibrary.FileDirectoryInformation file in fileList)
                {
                    Debug.WriteLine($"Filename: {file.FileName}");
                    Debug.WriteLine($"File Attributes: {file.FileAttributes}");
                    Debug.WriteLine($"File Size: {file.AllocationSize / 1024}KB");
                    Debug.WriteLine($"Created Date: {file.CreationTime.ToString("f")}");
                    Debug.WriteLine($"Last Modified Date: {file.LastWriteTime.ToString("f")}");
                    Debug.WriteLine("----------End of Folder/file-----------");
                    Debug.WriteLine("---------------------Before foreach");
                    DocsItem fileData = new DocsItem
                    {
                        Name = file.FileName,
                        Changed = file.ChangeTime,
                    };
                    Items.Add(fileData);
                    Debug.Write(Items.ToString() + "----------+++++++++++++++++++++++++++++++++++------------");

                }

                foreach (var file1 in shares)
                {
                    System.Diagnostics.Debug.WriteLine(file1.Length.ToString());
                    System.Diagnostics.Debug.WriteLine(file1.ToString());
                    //ShareFiles.Add(file1.ToString());
                    System.Diagnostics.Debug.WriteLine("44444444444444");
                }

                System.Diagnostics.Debug.WriteLine("----------------------------------------------------------------");

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }

        }
        public async Task OpenFile(string fileName)
        {
            try
            {
                var fileUri = new Uri($"smb://{Preferences.Get(IP, "")}/{Preferences.Get(FOLDER, "")}/{fileName}");
                await Launcher.OpenAsync(fileUri);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Unable to open the file: {ex.Message}", "OK");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
