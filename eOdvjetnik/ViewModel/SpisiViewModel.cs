using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using eOdvjetnik.Model;
using eOdvjetnik.Services;
using Syncfusion.Maui.Data;

namespace eOdvjetnik.ViewModel
{
    public class SpisiViewModel : INotifyPropertyChanged
    {


        ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();

        public ICommand OnDodajClick { get; set; }
        public ICommand OnItemClick { get; set; }

        private ObservableCollection<FileItem> initialFileItems;

        private ObservableCollection<FileItem> fileItems;
        public ObservableCollection<FileItem> FileItems
        {
            get { return fileItems; }
            set
            {
                if (fileItems != value)
                {
                    fileItems = value;
                    OnPropertyChanged(nameof(FileItems));
                }
            }
        }

        private int broj_spisa { get; set; }
        public int brojSpisa { get; set; }

        #region Search

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                }
            }
        }
        private ICommand searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (searchCommand == null)
                {
                    searchCommand = new Command(GenerateSearchResults);
                }
                return searchCommand;
            }
        }
        public ICommand OnResetClick { get; set; }

        public void ResetListView()
        {
            fileItems.Clear();
            foreach (var item in initialFileItems)
            {
                fileItems.Add(item);
            }
        }


        public void GenerateSearchResults()
        {
            try
            {
                fileItems.Clear();

                // parametri za pretragu broj spisa, klijent id, opponent id, referenca, uzrok, broj predmeta
                string search_term = SearchText;
                string escaped_search_term = search_term.Replace("/", "\\/");
                //string query = "SELECT * FROM `files` WHERE `broj_spisa` LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci  or `referenca` LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci or `uzrok` LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci  or `broj_predmeta` LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci ORDER BY `broj_predmeta` DESC";
                //string query = "SELECT files.*, client.ime AS client_name, opponent.ime AS opponent_name FROM files LEFT JOIN contacts AS client ON files.client_id = client.id LEFT JOIN contacts AS opponent ON files.opponent_id = opponent.id WHERE files.broj_spisa LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR files.referenca LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR files.uzrok LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR files.broj_predmeta LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci ORDER BY files.broj_predmeta DESC";
                string query = "SELECT files.*, client.ime AS client_name, opponent.ime AS opponent_name FROM files LEFT JOIN contacts AS client ON files.client_id = client.id LEFT JOIN contacts AS opponent ON files.opponent_id = opponent.id WHERE files.broj_spisa LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR files.referenca LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR files.uzrok LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR files.broj_predmeta LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR client.ime LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR opponent.ime LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR files.client_id IN (SELECT id FROM contacts WHERE ime LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci) OR files.opponent_id IN (SELECT id FROM contacts WHERE ime LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci) ORDER BY files.id DESC LIMIT 20";
                // Debug.WriteLine(query);
                Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
                Debug.WriteLine(query + " u Search resultu");
                if (filesData != null)
                {
                    foreach (Dictionary<string, string> filesRow in filesData)
                    {

                        #region Varijable za listu
                        int id;
                        int clientId;
                        int opponentId;
                        int inicijaliVoditeljId;
                        DateTime created;
                        DateTime datumPromjeneStatusa;
                        DateTime datumKreiranjaSpisa;
                        DateTime datumIzmjeneSpisa;

                        int.TryParse(filesRow["id"], out id);
                        int.TryParse(filesRow["client_id"], out clientId);
                        int.TryParse(filesRow["opponent_id"], out opponentId);
                        int.TryParse(filesRow["inicijali_voditelj_id"], out inicijaliVoditeljId);
                        DateTime.TryParse(filesRow["created"], out created);
                        DateTime.TryParse(filesRow["datum_promjene_statusa"], out datumPromjeneStatusa);
                        DateTime.TryParse(filesRow["datum_kreiranja_spisa"], out datumKreiranjaSpisa);
                        DateTime.TryParse(filesRow["datum_izmjene_spisa"], out datumIzmjeneSpisa);
                        #endregion
                        fileItems.Add(new FileItem()
                        {
                            Id = id,
                            BrojSpisa = filesRow["broj_spisa"],
                            Spisicol = filesRow["spisicol"],
                            ClientId = clientId,
                            OpponentId = opponentId,
                            InicijaliVoditeljId = inicijaliVoditeljId,
                            InicijaliDodao = filesRow["inicijali_dodao"],
                            Filescol = filesRow["filescol"],
                            InicijaliDodjeljeno = filesRow["inicijali_dodjeljeno"],
                            Created = created,
                            AktivnoPasivno = filesRow["aktivno_pasivno"],
                            Referenca = filesRow["referenca"],
                            DatumPromjeneStatusa = datumPromjeneStatusa,
                            Uzrok = filesRow["uzrok"],
                            DatumKreiranjaSpisa = datumKreiranjaSpisa,
                            DatumIzmjeneSpisa = datumIzmjeneSpisa,
                            Kreirao = filesRow["kreirao"],
                            ZadnjeUredio = filesRow["zadnje_uredio"],
                            Jezik = filesRow["jezik"],
                            BrojPredmeta = filesRow["broj_predmeta"],
                            ClientName = filesRow["client_name"],
                            OpponentName = filesRow["opponent_name"]

                        });


                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        #endregion


        public SpisiViewModel()
        {
            
            OnResetClick = new Command(ResetListView);
            OnDodajClick = new Command(onDodajCLick);
            OnItemClick = new Command(ItemClicked);
            // ItemSelected = new Command(OnItemSelected);
            try
            {
                fileItems = new ObservableCollection<FileItem>();
                //LoadEmptyRows();
                GenerateFiles();
                Debug.WriteLine("inicijalizirano u spisiViewModel");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            var timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += (s, e) => CheckCount();
            timer.Start();
            
        }

        //private void OnItemSelected(FileItem selectedItem)
        //{

        //}
        //public void LoadEmptyRows()
        //{

        //    FileItems = new ObservableCollection<FileItem>();

        //    for (int i = 0; i < 30; i++)
        //    {
        //        FileItems.Add(new FileItem
        //        {
        //            Id = i + 1,
        //            BrojSpisa = " ",
        //            Spisicol = " ",
        //            ClientId = null,
        //            OpponentId = null,
        //            InicijaliVoditeljId = null,
        //            InicijaliDodao = " ",
        //            Filescol = " ",
        //            InicijaliDodjeljeno = " ",
        //            Created = null,
        //            AktivnoPasivno = " ",
        //            Referenca = " ",
        //            DatumPromjeneStatusa = null,
        //            Uzrok = " ",
        //            DatumKreiranjaSpisa = null,
        //            DatumIzmjeneSpisa = null,
        //            Kreirao = " ",
        //            ZadnjeUredio = " ",
        //            Jezik = " ",
        //            BrojPredmeta = " ",
        //            ClientName = " ",
        //            OpponentName = " "
        //        });
        //    }

        //}

        void CheckCount()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    broj_spisa = brojSpisa;

                    string query = "SELECT COUNT(id) AS id FROM files;";

                    Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
                    if (filesData != null)
                    {
                        foreach (Dictionary<string, string> filesRow in filesData)
                        {
                            int id;
                            int.TryParse(filesRow["id"], out id);
                            brojSpisa = id;
                            //Debug.WriteLine(broj_spisa);
                            //Debug.WriteLine(brojSpisa);

                            if (brojSpisa > broj_spisa)
                            {
                                GenerateFiles();
                            }
                        }
                    }


                }

                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            );
        }

        public void ItemClicked()
        {
            try
            {
                Shell.Current.GoToAsync("///SpiDok");
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message + " in spisiViewModel");
            }
        }

        public void GenerateFiles()
        {
            try
            {
                fileItems.Clear();

                //string query = "SELECT * FROM files ORDER BY id DESC LIMIT 100;";
                string query = "SELECT files.*, client.ime AS client_name, opponent.ime AS opponent_name FROM files LEFT JOIN contacts AS client ON files.client_id = client.id LEFT JOIN contacts AS opponent ON files.opponent_id = opponent.id ORDER BY files.id DESC LIMIT 20";


                Debug.WriteLine(query + "u SpisiViewModelu");
                Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
                if (filesData != null)
                {
                    foreach (Dictionary<string, string> filesRow in filesData)
                    {
                        #region Varijable za listu
                        int id;
                        int clientId;
                        int opponentId;
                        int inicijaliVoditeljId;
                        DateTime created;
                        DateTime datumPromjeneStatusa;
                        DateTime datumKreiranjaSpisa;
                        DateTime datumIzmjeneSpisa;

                        int.TryParse(filesRow["id"], out id);
                        int.TryParse(filesRow["client_id"], out clientId);
                        int.TryParse(filesRow["opponent_id"], out opponentId);
                        int.TryParse(filesRow["inicijali_voditelj_id"], out inicijaliVoditeljId);
                        DateTime.TryParse(filesRow["created"], out created);
                        DateTime.TryParse(filesRow["datum_promjene_statusa"], out datumPromjeneStatusa);
                        DateTime.TryParse(filesRow["datum_kreiranja_spisa"], out datumKreiranjaSpisa);
                        DateTime.TryParse(filesRow["datum_izmjene_spisa"], out datumIzmjeneSpisa);
                        #endregion
                        fileItems.Add(new FileItem()
                        {
                            Id = id,
                            BrojSpisa = filesRow["broj_spisa"],
                            Spisicol = filesRow["spisicol"],
                            ClientId = clientId,
                            OpponentId = opponentId,
                            InicijaliVoditeljId = inicijaliVoditeljId,
                            InicijaliDodao = filesRow["inicijali_dodao"],
                            Filescol = filesRow["filescol"],
                            InicijaliDodjeljeno = filesRow["inicijali_dodjeljeno"],
                            Created = created,
                            AktivnoPasivno = filesRow["aktivno_pasivno"],
                            Referenca = filesRow["referenca"],
                            DatumPromjeneStatusa = datumPromjeneStatusa,
                            Uzrok = filesRow["uzrok"],
                            DatumKreiranjaSpisa = datumKreiranjaSpisa,
                            DatumIzmjeneSpisa = datumIzmjeneSpisa,
                            Kreirao = filesRow["kreirao"],
                            ZadnjeUredio = filesRow["zadnje_uredio"],
                            Jezik = filesRow["jezik"],
                            BrojPredmeta = filesRow["broj_predmeta"],
                            ClientName = filesRow["client_name"],
                            OpponentName = filesRow["opponent_name"]
                        });
                        initialFileItems = new ObservableCollection<FileItem>(fileItems);
                        foreach(FileItem item in fileItems)
                        {
                            Debug.WriteLine(item.BrojSpisa);
                        }

                    }
                    OnPropertyChanged(nameof(fileItems));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "in viewModel generate files");
            }
        }
        public void RefreshDataFromServer()
        {
            // Call this method to refresh the data from the server after adding a new entry
            GenerateFiles();
        }


        #region Komande

        private async void onDodajCLick()
        {
            await Shell.Current.GoToAsync("/NoviSpis");
            Debug.WriteLine("novi spis clicked");
        }
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
