using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using eOdvjetnik.Model;
using eOdvjetnik.Services;
using MySqlX.XDevAPI;
using Syncfusion.Maui.Scheduler;

namespace eOdvjetnik.ViewModel
{
    public class NoviSpisViewModel : INotifyPropertyChanged
    {
        public ICommand AddFilesToRemoteServer { get; set; }

        public ICommand OnDodajClick { get; set; }
        public ICommand OnNazadClick { get; set; }

        ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
        public string DevicePlatform { get; set; }


        FileItem fileitem;
        public string UserInitials { get; set; }
        public NoviSpisViewModel()
        {
            try
            {
                OnDodajClick = new Command(DodajClickButton);
                OnNazadClick = new Command(NazadClickButton);

                AddFilesToRemoteServer = new Command(() => AddSpisToRemoteServer(fileitem));
                EmployeeItems1 = new List<EmployeeItem>();
                EmployeeItems2 = new List<EmployeeItem>();

                GetEmployees1();
                GetEmployees2();

                ClientId = TrecaSreca.Get("FilesClientID");
                ClientName = TrecaSreca.Get("FilesClientName");
                OpponentId = TrecaSreca.Get("FilesOpponent");
                OpponentName = TrecaSreca.Get("FilesOpponentName");
                DevicePlatform = TrecaSreca.Get("vrsta_platforme");
                UserInitials = TrecaSreca.Get("UserInitials");

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }



        public List<EmployeeItem> EmployeeItems1 { get; set; }
        private EmployeeItem selectedEmployeeItem1;

        public EmployeeItem SelectedEmployeeItem1
        {
            get { return selectedEmployeeItem1; }
            set
            {
                if (selectedEmployeeItem1 != value)
                {
                    selectedEmployeeItem1 = value;
                    OnPropertyChanged(nameof(SelectedEmployeeItem1));
                    InicijaliVoditelj = selectedEmployeeItem1?.Initals;
                }
            }
        }

        public List<EmployeeItem> EmployeeItems2 { get; set; }
        private EmployeeItem selectedEmployeeItem2;

        public EmployeeItem SelectedEmployeeItem2
        {
            get { return selectedEmployeeItem2; }
            set
            {
                if (selectedEmployeeItem2 != value)
                {
                    selectedEmployeeItem2 = value;
                    OnPropertyChanged(nameof(SelectedEmployeeItem2));
                    InicijaliDodijeljeno = selectedEmployeeItem2?.Initals;
                }
            }
        }


        public void GetEmployees1()
        {
            try
            {
                if (EmployeeItems1 != null)
                {
                    EmployeeItems1.Clear();

                }

                string query = "SELECT id, ime, inicijali FROM employees;";


                // Debug.WriteLine(query + "u SpisiViewModelu");
                Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
                if (filesData != null)
                {
                    foreach (Dictionary<string, string> filesRow in filesData)
                    {
                        #region Varijable za listu
                        int id;
                        int active;

                        int.TryParse(filesRow["id"], out id);
                        int.TryParse(filesRow["inicijali"], out active);
                        #endregion

                        var employee = new EmployeeItem()
                        {
                            Id = id,
                            EmployeeName = filesRow["ime"],
                            Initals = filesRow["inicijali"],
                        };


                        EmployeeItems1.Add(employee);
                    }
                    OnPropertyChanged(nameof(EmployeeItems1));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "in viewModel generate files");
            }
        }

        public void GetEmployees2()
        {
            try
            {
                if (EmployeeItems2 != null)
                {
                    EmployeeItems2.Clear();

                }

                string query = "SELECT id, ime, inicijali FROM employees;";


                // Debug.WriteLine(query + "u SpisiViewModelu");
                Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
                if (filesData != null)
                {
                    foreach (Dictionary<string, string> filesRow in filesData)
                    {
                        #region Varijable za listu
                        int id;
                        int active;

                        int.TryParse(filesRow["id"], out id);
                        int.TryParse(filesRow["inicijali"], out active);
                        #endregion

                        var employee = new EmployeeItem()
                        {
                            Id = id,
                            EmployeeName = filesRow["ime"],
                            Initals = filesRow["inicijali"],
                        };


                        EmployeeItems2.Add(employee);
                    }
                    OnPropertyChanged(nameof(EmployeeItems2));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "in viewModel generate files");
            }
        }

        private ObservableCollection<FileItem> fileitems;
        public ObservableCollection<FileItem> fileItems
        {
            get { return fileitems; }
            set
            {
                if (fileitems != value)
                {
                    fileitems = value;
                    OnPropertyChanged(nameof(fileItems));
                }
            }
        }

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

        public string ClientName { get; set; }
        public string OpponentName { get; set; }


        private string opponentId;
        public string OpponentId
        {
            get { return opponentId; }
            set
            {
                if (opponentId != value)
                {
                    opponentId = value;
                    OnPropertyChanged(nameof(OpponentId));
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

        private void GetCurrentDate()
        {
            DateTime currentDate = DateTime.Now;

            Created = currentDate;
            DatumPromjene = currentDate;
            DatumKreiranja = currentDate;
            DatumIzmjene = currentDate;

        }

        private void AddSpisToRemoteServer(FileItem fileItem)
        {

            try
            {
                GetCurrentDate();
                #region Varijable za spremanje
                //int idValue = int.Parse(Id);
                int idValue = int.Parse(Id ?? "0");
                string brojSpisa = BrojSpisa ?? string.Empty;
                string spisiCol = SpisiCol ?? string.Empty;
                int clientId = int.Parse(ClientId ?? "0");
                int opponentId = int.Parse(OpponentId ?? "0");
                string inicijaliVoditelj = InicijaliVoditelj ?? "0";
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
                Debug.WriteLine("DDDDDDDDDD broj spisa " + brojSpisa + ", spisiCol " + spisiCol + ", ClientId " + ClientId);


                string query = $"INSERT INTO files (broj_spisa, spisicol, client_id, opponent_id, inicijali_voditelj_id, inicijali_dodao, filescol, inicijali_dodjeljeno, created, aktivno_pasivno, referenca, datum_promjene_statusa, uzrok, datum_kreiranja_spisa, datum_izmjene_spisa, kreirao, zadnje_uredio, jezik,broj_predmeta ) " +
                       $"VALUES ('{brojSpisa}', '{spisiCol}', '{ClientId}', '{OpponentId}', '{InicijaliVoditelj}' , '{UserInitials}' , '{filesCol}' , '{InicijaliDodijeljeno}' , '{created.ToString("yyyy-MM-dd HH:mm:ss")}' , '{aktivnoPasivno}' , '{referenca}' , '{datumPromjene.ToString("yyyy-MM-dd HH:mm:ss")}' , '{uzrok}' , '{datumKreiranja.ToString("yyyy-MM-dd HH:mm:ss")}' , '{datumIzmjene.ToString("yyyy-MM-dd HH:mm:ss")}' , '{kreirao}' , '{zadnjeUredio}' , '{jezik}' , '{brojPredmeta}' )";
                Debug.WriteLine(query + " in novi spis viewModel");


                externalSQLConnect.sqlQuery(query);
                Debug.WriteLine("Appointment added to remote server in novi spis viewModel");



            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " in noviSpisViewModel AddAppointmentToServer");
            }



        }

        private void DodajClickButton()
        {
            string nullString = "";
            if (BrojSpisa == nullString || BrojSpisa == null)
            {
                Application.Current.MainPage.DisplayAlert("Upozorenje", "Potrebno je unijeti broj spisa.", "OK");
             }
            else if (ClientId == nullString || ClientId == null)
            {
                Application.Current.MainPage.DisplayAlert("Upozorenje", "Potrebno je otvoriti 'Kontakti' i dodati kijenta.", "OK");

            }
            else if (OpponentId == nullString || OpponentId == null)
            {
                Application.Current.MainPage.DisplayAlert("Upozorenje", "Potrebno je otvoriti 'Kontakti' i dodati protustranku.", "OK");

            }
            else
            {
                AddSpisToRemoteServer(FileItem);
                if (DevicePlatform == "MacCatalyst")
                {
                    Shell.Current.GoToAsync("///LoadingPageSpisi");

                }
                else
                {
                    Shell.Current.GoToAsync("///Spisi");

                }
            }
        }
        private void NazadClickButton()
        {
            if (DevicePlatform == "MacCatalyst")
            {
                Shell.Current.GoToAsync("///LoadingPageSpisi");

            }
            else
            {
                Shell.Current.GoToAsync("///Spisi");

            }
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
