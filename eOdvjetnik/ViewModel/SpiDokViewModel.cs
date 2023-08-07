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



namespace eOdvjetnik.ViewModel
{
    public class SpiDokViewModel : INotifyPropertyChanged
    {
        private Navigacija navigacija;

        ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();

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
                navigacija = new Navigacija();
                spiDokItems = new ObservableCollection<SpiDokItem>();
                GenerateFiles();
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
                //spiDokItems.Clear();

                //string query = "SELECT * FROM files ORDER BY id DESC LIMIT 100;";
                string query = "SELECT * FROM `documents` where file_id=11273 ORDER BY `id` DESC";

                // Debug.WriteLine(query + "u SpisiViewModelu");
                Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
                Debug.WriteLine(query);
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
                        });
                        //initialFileItems = new ObservableCollection<FileItem>(fileItems);
                        
                    }
                    OnPropertyChanged(nameof(spiDokItems));
                    Debug.WriteLine(spiDokItems);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "in viewModel generate files");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
