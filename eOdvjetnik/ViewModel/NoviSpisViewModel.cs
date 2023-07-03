using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using eOdvjetnik.Model;
using eOdvjetnik.Services;
using Syncfusion.Maui.Scheduler;

namespace eOdvjetnik.ViewModel
{
    public class NoviSpisViewModel : INotifyPropertyChanged
    {
        public ICommand AddFilesToRemoteServer { get; set; }

        ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();

        FileItem fileitem;
        //private FileItem fileitem;

        #region Varijable za spremanje na server
        private string id;
        public string Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        private string brojSpisa;
        public string BrojSpisa
        {
            get { return brojSpisa; }
            set
            {
                if (brojSpisa != value)
                {
                    brojSpisa = value;
                    OnPropertyChanged(nameof(BrojSpisa));
                }
            }
        }
        private string spisiCol;
        public string SpisiCol
        {
            get { return spisiCol; }
            set
            {
                if (spisiCol != value)
                {
                    spisiCol = value;
                    OnPropertyChanged(nameof(SpisiCol));
                }
            }
        }
        private string clientId;
        public string ClientId
        {
            get { return clientId; }
            set
            {
                if (clientId != value)
                {
                    clientId = value;
                    OnPropertyChanged(nameof(ClientId));
                }
            }
        }
        private string opponentId;
        public string OpponnentId
        {
            get { return opponentId; }
            set
            {
                if (opponentId != value)
                {
                    opponentId = value;
                    OnPropertyChanged(nameof(OpponnentId));
                }
            }
        }
        private string inicijaliVoditelj;
        public string InicijaliVoditelj
        {
            get { return inicijaliVoditelj; }
            set
            {
                if (inicijaliVoditelj != value)
                {
                    inicijaliVoditelj = value;
                    OnPropertyChanged(nameof(InicijaliVoditelj));
                }
            }
        }
        private string inicijaliDodao;
        public string InicijaliDodao
        {
            get { return inicijaliDodao; }
            set
            {
                if (inicijaliDodao != value)
                {
                    inicijaliDodao = value;
                    OnPropertyChanged(nameof(InicijaliDodao));
                }
            }
        }
        private string filesCol;
        public string FilesCol
        {
            get { return filesCol; }
            set
            {
                if (filesCol != value)
                {
                    filesCol = value;
                    OnPropertyChanged(nameof(FilesCol));
                }
            }
        }
        private string inicijaliDodijeljeno;
        public string InicijaliDodijeljeno
        {
            get { return inicijaliDodijeljeno; }
            set
            {
                if (inicijaliDodijeljeno != value)
                {
                    inicijaliDodijeljeno = value;
                    OnPropertyChanged(nameof(InicijaliDodijeljeno));
                }
            }
        }
        private DateTime created;
        public DateTime Created
        {
            get { return created; }
            set
            {
                if (created != value)
                {
                    created = DateTime.Now;
                    OnPropertyChanged(nameof(Created));
                }
            }
        }
        private string aktivnoPasivno;
        public string AktivnoPasivno
        {
            get { return aktivnoPasivno; }
            set
            {
                if (aktivnoPasivno != value)
                {
                    aktivnoPasivno = value;
                    OnPropertyChanged(nameof(AktivnoPasivno));
                }
            }
        }
        private string referenca;
        public string Referenca
        {
            get { return referenca; }
            set
            {
                if (referenca != value)
                {
                    referenca = value;
                    OnPropertyChanged(nameof(Referenca));
                }
            }
        }
        private DateTime datumPromjene;
        public DateTime DatumPromjene
        {
            get { return datumPromjene; }
            set
            {
                if (datumPromjene != value)
                {
                    datumPromjene = DateTime.Now;
                    OnPropertyChanged(nameof(DatumPromjene));
                }
            }
        }

        private string uzrok;
        public string Uzrok
        {
            get { return uzrok; }
            set
            {
                if (uzrok != value)
                {
                    uzrok = value;
                    OnPropertyChanged(nameof(Uzrok));
                }
            }
        }

        private DateTime datumKreiranja;
        public DateTime DatumKreiranja
        {
            get { return datumKreiranja; }
            set
            {
                if (datumKreiranja != value)
                {
                    datumKreiranja = DateTime.Now;
                    OnPropertyChanged(nameof(DatumKreiranja));
                }
            }
        }

        private DateTime datumIzmjene;
        public DateTime DatumIzmjene
        {
            get { return datumIzmjene; }
            set
            {
                if (datumIzmjene != value)
                {
                    datumIzmjene = DateTime.Now;
                    OnPropertyChanged(nameof(DatumIzmjene));
                }
            }
        }
        private string kreirao;
        public string Kreirao
        {
            get { return kreirao; }
            set
            {
                if (kreirao != value)
                {
                    kreirao = value;
                    OnPropertyChanged(nameof(Kreirao));
                }
            }
        }

        private string zadnjeuUredio;
        public string ZadnjeUredio
        {
            get { return zadnjeuUredio; }
            set
            {
                if (zadnjeuUredio != value)
                {
                    zadnjeuUredio = value;
                    OnPropertyChanged(nameof(ZadnjeUredio));
                }
            }
        }

        private string jezik;
        public string Jezik
        {
            get { return jezik; }
            set
            {
                if (jezik != value)
                {
                    jezik = value;
                    OnPropertyChanged(nameof(Jezik));
                }
            }
        }
        private string brojPredmeta;
        public string BrojPredmeta
        {
            get { return brojPredmeta; }
            set
            {
                if (brojPredmeta != value)
                {
                    brojPredmeta = value;
                    OnPropertyChanged(nameof(BrojPredmeta));
                }
            }
        }

        #endregion


        public FileItem FileItem
        {
            get { return fileitem; }
            set
            {
                if (fileitem != value)
                {
                    fileitem = value;
                    OnPropertyChanged(nameof(FileItem));
                }
            }
        }

        //Napravi još bindinge sa svakim entry fieldom za svako polje da dodaje, vjerojatno neće bit dobar i trebat će se nešto konvertat, date time ili slično
        public NoviSpisViewModel()
        {
            try
            {
                AddFilesToRemoteServer = new Command(() => AddSpisToRemoteServer(fileitem));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private void AddSpisToRemoteServer(FileItem fileItem)
        {
            try
            {
                #region Varijable za spremanje
                //int idValue = int.Parse(Id);
                int idValue = int.Parse(Id ?? "0");
                string brojSpisa = BrojSpisa ?? string.Empty;
                string spisiCol = SpisiCol ?? string.Empty;
                int clientId = int.Parse(ClientId ?? "0");
                int opponentId = int.Parse(OpponnentId ?? "0");
                int inicijaliVoditelj = int.Parse(InicijaliVoditelj ?? "0");
                string inicijaliDodao = InicijaliDodao ?? string.Empty;
                string filesCol = FilesCol ?? string.Empty;
                string inicijaliDodijeljeno = InicijaliDodijeljeno ?? string.Empty;
                DateTime created = Created;
                string aktivnoPasivno = AktivnoPasivno ?? string.Empty;
                string referenca = Referenca ?? string.Empty;
                DateTime datumPromjene = DatumPromjene;
                string uzrok = Uzrok ?? string.Empty;
                DateTime datumKreiranja = DatumKreiranja;
                DateTime datumIzmjene = DatumIzmjene;
                string kreirao = Kreirao ?? string.Empty;
                string zadnjeUredio = ZadnjeUredio ?? string.Empty;
                string jezik = Jezik ?? string.Empty;
                string brojPredmeta = BrojPredmeta ?? string.Empty;


                #endregion


                ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
                string disableForeignKeyChecksQuery = "SET FOREIGN_KEY_CHECKS = 0";
                externalSQLConnect.sqlQuery(disableForeignKeyChecksQuery);

                string query = $"INSERT INTO Files (broj_spisa, spisicol, client_id, opponent_id, inicijali_voditelj_id, inicijali_dodao, filescol, inicijali_dodjeljeno, created, aktivno_pasivno, referenca, datum_promjene_statusa, uzrok, datum_kreiranja_spisa, datum_izmjene_spisa, kreirao, zadnje_uredio, jezik,broj_predmeta ) " +
                        $"VALUES ('{brojSpisa}', '{spisiCol}', '{clientId}', '{opponentId}', '{inicijaliVoditelj}' , '{inicijaliDodao}' , '{filesCol}' , '{inicijaliDodijeljeno}' , '{created.ToString("yyyy-MM-dd HH:mm:ss")}' , '{aktivnoPasivno}' , '{referenca}' , '{datumPromjene.ToString("yyyy-MM-dd HH:mm:ss")}' , '{uzrok}' , '{datumKreiranja.ToString("yyyy-MM-dd HH:mm:ss")}' , '{datumIzmjene.ToString("yyyy-MM-dd HH:mm:ss")}' , '{kreirao}' , '{zadnjeUredio}' , '{jezik}' , '{brojPredmeta}' )";
                Debug.WriteLine(query + " in novi spis viewModel");
                externalSQLConnect.sqlQuery(query);
                Debug.WriteLine("Appointment added to remote server in novi spis viewModel");
                

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " in noviSpisViewModel AddAppointmentToServer");
            }
        }

        private void DodajButtonClicked()
        {
            AddSpisToRemoteServer(fileitem);

        }

        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


    }


}
