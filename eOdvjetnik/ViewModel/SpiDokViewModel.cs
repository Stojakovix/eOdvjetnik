using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eOdvjetnik.Data;
using eOdvjetnik.Models;
using eOdvjetnik.Services;
using System.Windows.Input;
using System.Reflection;



namespace eOdvjetnik.ViewModel
{
    public class SpiDokViewModel : INotifyPropertyChanged
    {
        private const string IP_nas = "IP Adresa";
        private const string USER_nas = "Korisničko ime";
        private const string PASS_nas = "Lozinka";
        private const string FOLDER_nas = "Folder";
        private const string SUBFOLDER_nas = "SubFolder";


        ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
        public int ListItemId { get; set; }
        public string StringIcon { get; set; }

        private ObservableCollection<SpiDokItem> spiDokItems;
        public ObservableCollection<SpiDokItem> SpiDokItems
        {
            get { return spiDokItems; }
            set
            {
                if (spiDokItems != value)
                {
                    spiDokItems = value;
                    OnPropertyChanged(nameof(SpiDokItems));
                }
            }
        }

        public SpiDokViewModel()
        {
            try
            {
                
                Debug.WriteLine("inicijalizirano u spidokViewModelu");
                spiDokItems = new ObservableCollection<SpiDokItem>();
                GenerateFiles();


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task InitializeDataAsync()
        {
            try
            {
                string listItemIdString = await SecureStorage.GetAsync("listItemId");
                Debug.WriteLine(listItemIdString + " u InitializeData ");
                if (int.TryParse(listItemIdString, out int parsedItemId))
                {
                    ListItemId = parsedItemId;
                }
                else
                {
                    // Handle the case where parsing fails - Tu neki popup ili da vrati na spise, smislit nešt
                    Debug.WriteLine("Failed to parse listItemId.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


        public async void GenerateFiles()
        {
            try
            {
                await InitializeDataAsync();
                spiDokItems.Clear();
                
                Debug.WriteLine(ListItemId + " u generateFiles");
                //string query = "SELECT * FROM files ORDER BY id DESC LIMIT 100;";
                string query = $"SELECT * FROM `documents` where file_id='{ListItemId}' ORDER BY `id` DESC";

                // Debug.WriteLine(query + "u SpisiViewModelu");
                Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
                Debug.WriteLine(query);

                //List png
                List<string> resourceNames = new List<string>();
                Assembly assembly = typeof(DocsViewModel).Assembly;
                string resourceNamePrefix = "eOdvjetnik.Resources"; // Replace with your app's actual namespace and "Resources." prefix
                string[] allResourceNames = assembly.GetManifestResourceNames();
                resourceNames.AddRange(allResourceNames.Where(name => name.StartsWith(resourceNamePrefix)));
                //Kraj List png


                if (filesData != null)
                {
                    foreach (Dictionary<string, string> filesRow in filesData)
                    {
                        #region Varijable za listu
                        int id;
                        int fileId;
                        int brojPrivitaka;
                        int tipDokumenta;
                        DateTime datum;
                        DateTime datumKreiranjaDokumenta;
                        DateTime datumIzmjeneDokumenta;

                        int.TryParse(filesRow["id"], out id);
                        int.TryParse(filesRow["file_id"], out fileId);
                        int.TryParse(filesRow["broj_privitaka"], out brojPrivitaka);
                        int.TryParse(filesRow["tip_dokumenta"], out tipDokumenta);
                        DateTime.TryParse(filesRow["datum"], out datum);
                        DateTime.TryParse(filesRow["datum_kreiranja_dokumenta"], out datumKreiranjaDokumenta);
                        DateTime.TryParse(filesRow["datum_izmjene_dokumenta"], out datumIzmjeneDokumenta);
                        #endregion

                        //ikona
                        var icon = "blank.png";
                        icon = Path.GetExtension(filesRow["dokument"]).TrimStart('.') + ".png";
                        bool imageExists = resourceNames.Contains("eOdvjetnik.Resources.Images." + icon);
                        if (imageExists){
                            icon = Path.GetExtension(filesRow["dokument"]).TrimStart('.') + ".png";
                        }else{
                            icon = "blank.png";
                        }
                        //ikona kraj                        

                        spiDokItems.Add(new SpiDokItem()
                        {
                            Id = id,
                            FileId = fileId,
                            Naziv = filesRow["naziv"],
                            Lokacija = filesRow["lokacija"],
                            DocumentsCol = filesRow["documentscol"],
                            Datum = datum,
                            InicijaliDodao = filesRow["inicijali_dodao"],
                            InicijaliDodjeljeno = filesRow["inicijali_dodjeljeno"],
                            Referenca = filesRow["referenca"],
                            Dokument = filesRow["dokument"],
                            Biljeska = filesRow["biljeska"],
                            LinkArt = filesRow["LinkArt"],
                            FileStatus = filesRow["file_status"],
                            DatumIzmjeneDokumenta = datumKreiranjaDokumenta,
                            DatumKreiranjaDokumenta = datumIzmjeneDokumenta,
                            Neprocitano = filesRow["neprocitano"],
                            BrojPrivitaka = brojPrivitaka,
                            TipDokumenta = tipDokumenta,
                            NaziviPrivitaka = filesRow["nazivi_privitaka"],
                            EmailAdrese = filesRow["email_adrese"],
                            Kreirao = filesRow["kreirao"],
                            ZadnjeUredio = filesRow["zadnje_uredio"],
                            Icon = icon,
                        });



                        OnPropertyChanged(nameof(spiDokItems));

                        //Debug.WriteLine(spiDokItems); 

                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "in spidok viewModel generate files");
            }
        }

        public async Task OpenFile(string fileName)
        {
            try
            {
                //var fileUri = new Uri($"smb://{Preferences.Get(IP, "")}/{Preferences.Get(FOLDER, "")}/{fileName}");
                //await Launcher.OpenAsync(fileUri);
                string ip = await SecureStorage.GetAsync(IP_nas);
                string folder = await SecureStorage.GetAsync(FOLDER_nas);
                string subfolder = await SecureStorage.GetAsync(SUBFOLDER_nas);
                //string ip = await SecureStorage.GetAsync(IP_nas);
                Debug.WriteLine("Samo string -> " + @"\\192.168.1.211\Users\user\test.doc");
                string filePath = @"\\" + ip + "\\" + folder + subfolder + "\\" + fileName;
                Debug.WriteLine("Izgenerirani string -> " + @"\\" + ip + "\\" + folder + subfolder + "\\" + fileName);
                await Launcher.OpenAsync(filePath);

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Unable to open the file: {ex.Message}", "OK");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
