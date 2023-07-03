using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using eOdvjetnik.Models;
using eOdvjetnik.Services;

namespace eOdvjetnik.ViewModel
{
    public class NaplataViewModel : INotifyPropertyChanged
    {

        ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();

        private ObservableCollection<TariffItem> tariffItems;
        public ObservableCollection<TariffItem> TariffItems
        {
            get { return tariffItems; }
            set { tariffItems = value; }
        }
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
            Debug.WriteLine(tariffItems);
            try
            {

                if (tariffItems != null)
                {
                    tariffItems.Clear();
                }
                //string query = "SELECT * FROM `tariffs` WHERE `name` LIKE '%" + SearchText+ "%' or `oznaka` LIKE '%" + SearchText + "%'";
                string query = "SELECT t1.*, CONCAT(t2.name, ' - ', t1.name) AS concatenated_name FROM tariffs t1 LEFT JOIN tariffs t2 ON t1.parent_id = t2.Id WHERE t1.name LIKE '%" + SearchText + "%' OR t1.oznaka LIKE '%" + SearchText + "%'";
                Debug.WriteLine(query);
                Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
                Debug.WriteLine(query + " u Search resultu");
                if (filesData != null)
                {
                    foreach (Dictionary<string, string> filesRow in filesData)
                    {

                        int id;
                        int parent_id;
                        int.TryParse(filesRow["id"], out id);
                        int.TryParse(filesRow["parent_id"], out parent_id);


                        tariffItems.Add(new TariffItem()
                        {
                            Id = id,
                            parent_id = parent_id,
                            name = filesRow["name"],
                            oznaka = filesRow["oznaka"],
                            bodovi = filesRow["bodovi"],
                            concatenated_name = filesRow["concatenated_name"],
                        });
                        

                        //Debug.WriteLine(filesRow["concatenated_name"]);
                    }
                    foreach (TariffItem item in TariffItems)
                    {
                        //Debug.WriteLine(item.name);
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public string NazivTvrtke { get; set; }

        public NaplataViewModel()
        {
            NazivTvrtke = Preferences.Get("naziv_tvrtke", "");

            try
            {
                tariffItems = new ObservableCollection<TariffItem>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}