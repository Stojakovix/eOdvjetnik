using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using eOdvjetnik.Model;
using eOdvjetnik.Services;

namespace eOdvjetnik.ViewModel
{
    public class SpisiViewModel : INotifyPropertyChanged
    {
        ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
        string query = "Select * from files";
        private ObservableCollection<FileItem> fileItems;
        public ObservableCollection<FileItem> FileItems
        {
            get { return fileItems; }
            set { this.fileItems = value; }
        }
        public SpisiViewModel()
        {
            
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
                string query = "Select * from files Limit 50";
                Debug.WriteLine(query + "u SpisiViewModelu");
                Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
                if (filesData != null)
                {
                    foreach (Dictionary<string, string> filesRow in filesData)
                    {
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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
