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
                string query = "Select * from files";
                Debug.WriteLine(query + "u SpisiViewModelu");
                Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
                if (filesData != null)
                {
                    foreach (Dictionary<string, string> filesRow in filesData)
                    {
                        fileItems.Add(new FileItem()
                        {
                            Id = int.Parse(filesRow["id"]),
                            BrojSpisa = filesRow["broj_spisa"],
                            Spisicol = filesRow["spisicol"],
                            ClientId = int.Parse(filesRow["client_id"]),
                            OpponentId = int.Parse(filesRow["opponent_id"]),
                            InicijaliVoditeljId = int.Parse(filesRow["inicijali_voditelj_id"]),
                            InicijaliDodao = filesRow["inicijali_dodao"],
                            Filescol = filesRow["filescol"],
                            InicijaliDodjeljeno = filesRow["inicijali_dodjeljeno"],
                            Created = DateTime.Parse(filesRow["created"]),
                            AktivnoPasivno = filesRow["aktivno_pasivno"],
                            Referenca = filesRow["referenca"],
                            DatumPromjeneStatusa = DateTime.Parse(filesRow["datum_promjene_statusa"]),
                            Uzrok = filesRow["uzrok"],
                            DatumKreiranjaSpisa = DateTime.Parse(filesRow["datum_kreiranja_spisa"]),
                            DatumIzmjeneSpisa = DateTime.Parse(filesRow["datum_izmjene_spisa"]),
                            Kreirao = filesRow["kreirao"],
                            ZadnjeUredio = filesRow["zadnje uredio"],
                            Jezik = filesRow["jezik"],
                            BrojPredmeta = filesRow["broj_predmeta"]


                        });
                    }
                }
            }
            catch
            {

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
