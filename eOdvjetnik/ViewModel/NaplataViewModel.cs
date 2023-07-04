using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Xml.Linq;
using eOdvjetnik.Models;
using eOdvjetnik.Services;
using Vanara.PInvoke;

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

        private string odabrani_TBR;
        public string odabraniTBR
        {
            get { return odabrani_TBR; }
            set
            {
                if (odabrani_TBR != value)
                {
                    odabrani_TBR = value;
                    OnPropertyChanged(nameof(odabraniTBR));
                }
            }
        }

        private string odabrani_Naziv;
        public string odabraniNaziv
        {
            get { return odabrani_Naziv; }
            set
            {
                if (odabrani_Naziv != value)
                {
                    odabrani_Naziv = value;
                    OnPropertyChanged(nameof(odabraniNaziv));
                }
            }
        }


        private string odabrani_Bodovi;
        public string odabraniBodovi
        {
            get { return odabrani_Bodovi; }
            set
            {
                if (odabrani_Bodovi != value)
                {
                    odabrani_Bodovi = value;
                    OnPropertyChanged(nameof(odabraniBodovi));
                }
            }
        }

        // Za popup 

        private ObservableCollection<ReceiptItem> _receiptItems;
        public ObservableCollection<ReceiptItem> ReceiptItems
        {
            get { return _receiptItems; }
            set
            {
                _receiptItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReceiptItems)));
            }
        }

        public ICommand AddItemCommand { get; }
        public ICommand EditItemCommand { get; }
        public ICommand RemoveItemCommand { get; }


        public NaplataViewModel()
        {
            ReceiptItems = new ObservableCollection<ReceiptItem>();

            AddItemCommand = new Command(AddItem);
            EditItemCommand = new Command<ReceiptItem>(EditItem);
            RemoveItemCommand = new Command<ReceiptItem>(RemoveItem);

            try
            {
                tariffItems = new ObservableCollection<TariffItem>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            NazivTvrtke = Preferences.Get("naziv_tvrtke", "");
            odabrani_TBR = Preferences.Get("SelectedOznaka", "");
            odabrani_Bodovi = Preferences.Get("SelectedBodovi", "");
            odabrani_Naziv = Preferences.Get("SelectedConcatenatedName", "");

            var timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += (s, e) => Refresh();
            timer.Start();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void Refresh()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                NazivTvrtke = Preferences.Get("naziv_tvrtke", "");
                odabraniTBR = Preferences.Get("SelectedOznaka", "");
                odabraniBodovi = Preferences.Get("SelectedBodovi", "");
                odabraniNaziv = Preferences.Get("SelectedConcatenatedName", "");
            }
            );
        }

        //Popup računa

        private void AddItem()
        {
            ReceiptItem newItem = new ReceiptItem
            {
                Tbr = odabraniTBR,
                Name = odabraniNaziv,
                Points = odabraniBodovi
        };
        ReceiptItems.Add(newItem);
        }


        private void EditItem(ReceiptItem item)
        {
            //eh
        }

        private void RemoveItem(ReceiptItem item)
        {
            ReceiptItems.Remove(item);
        }

        public class ReceiptItem
        {
            public string Tbr { get; set; }
            public string Name { get; set; }
            public string Points { get; set; }
        }


    }


}