using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using eOdvjetnik.Model;
using eOdvjetnik.Services;

namespace eOdvjetnik.ViewModel
{
    public class SpisiViewModel : INotifyPropertyChanged
    {
        ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
        public ICommand OnDodajClick { get; set; }

        

        private ObservableCollection<FileItem> fileItems;
        public ObservableCollection<FileItem> FileItems
        {
            get { return fileItems; }
            set { fileItems = value; }
        }


        #region Search


        //public ICommand PerformSearch => new Command<string>((query) =>
        //{
        //    try
        //    {
        //        // Perform search based on query
        //        // Update SearchResults property accordingly
        //        SearchResults = new ObservableCollection<FileItem>(FileItems.Where(item =>
        //        {
        //            foreach (var property in item.GetType().GetProperties())
        //            {
        //                var value = property.GetValue(item)?.ToString();
        //                if (!string.IsNullOrEmpty(value) && value.Contains(query))
        //                {
        //                    return true;
        //                }
        //            }
        //            return false;
        //        }));
        //    }
        //    catch(Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message + "in Perform search");
        //        throw;
        //    }
        //});
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


        public void GenerateSearchResults()
        {
            try
            {
                string query = "SELECT * FROM files WHERE BrojSpisa %like% @searchText";
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
                            BrojPredmeta = filesRow["broj_predmeta"]
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
            
            OnDodajClick = new Command(onDodajCLick);
            try
            {
                fileItems = new ObservableCollection<FileItem>();
                this.GenerateFiles();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void GenerateFiles()
        {
            try
            {
                string query = "SELECT * FROM files ORDER BY id DESC LIMIT 100;";

               // Debug.WriteLine(query + "u SpisiViewModelu");
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
                            BrojPredmeta = filesRow["broj_predmeta"]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #region Komande

        private async void onDodajCLick()
        {
            await Shell.Current.GoToAsync("/NoviSpis");
            Debug.WriteLine("novi spis clicked");
        }
        #endregion
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
