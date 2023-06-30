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
                string query = "SELECT * FROM `tariffs` WHERE `name` LIKE '%" + SearchText+ "%' or `oznaka` LIKE '%" + SearchText + "%'";
                Debug.WriteLine(query);
                Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
                Debug.WriteLine(query + " u Search resultu");
                if (filesData != null)
                {
                    foreach (Dictionary<string, string> filesRow in filesData)
                    {

                        int id;
                        int Parent_id;
                        int.TryParse(filesRow["id"], out id);
                        int.TryParse(filesRow["parent_id"], out Parent_id);


                        tariffItems.Add(new TariffItem()
                        {
                            Id = id,
                            parent_id = Parent_id,
                            name = filesRow["name"],
                            oznaka = filesRow["oznaka"],
                            bodovi = filesRow["bodovi"],

                        });

                        Debug.WriteLine(filesRow["broj_spisa"]);
                    }
                    foreach (TariffItem item in TariffItems)
                    {
                        Debug.WriteLine(item.name);
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public NaplataViewModel()
        {
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