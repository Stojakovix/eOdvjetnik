using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Timer = System.Timers.Timer;


namespace eOdvjetnik.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        //Varijable za NAS preferenceas
        private const string IP_nas = "IP Adresa";
        private const string USER_nas = "Korisničko ime";
        private const string PASS_nas = "Lozinka";
        private const string FOLDER_nas = "Folder";
        private const string SUBFOLDER_nas = "SubFolder";

        //Varijable za MySQL preferences
        public const string IP_mysql = "IP Adresa2";
        public const string USER_mysql = "Korisničko ime2";
        public const string PASS_mysql = "Lozinka2";
        public const string databasename_mysql = "databasename";
        //MySQL varijable
        public string query;

        //DateTime
        private Timer timer;
        private string currenttime { get; set; }
        public string datetime
        {
            get { return currenttime; }
            set
            {
                currenttime = value;
                OnPropertyChanged(nameof(datetime));
            }
        }
        public string Version { get; set; }

        //Save za mysql
        public ICommand SaveCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        //Save za nas

        public ICommand SaveCommandNAS { get; set; }
        public ICommand LoadCommandNAS { get; set; }
        public ICommand DeleteCommandNAS { get; set; }
        //Dohvaća vrijednost varijabli iz mainPagea SQL
        public string IP { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }

        //Vrijednost varijabli NAS

        public string IPNas { get; set; }
        public string UserNas { get; set; }
        public string PassNas { get; set; }
        public string Folder { get; set; }
        public string SubFolder { get; set; }


        // Varijable za PopupNAS

        private bool isOpen, visible;
        public ICommand PopupAcceptCommand { get; set; }
        public ICommand ShowPopupCommand { get; set; }

        public ICommand ClosePopupCommand { get; set; }

        // Varijable za SQL

        private bool sqlOpen, sqlvisible;
        public ICommand SQLPopupAcceptCommand { get; set; }
        public ICommand SQLShowPopupCommand { get; set; }

        public ICommand SQLClosePopupCommand { get; set; }



        public bool SQLPopupOpen
        {
            get { return sqlOpen; }
            set
            {
                sqlOpen = value;
                OnPropertyChanged(nameof(SQLPopupOpen));
            }
        }

        public bool SQLVisible
        {
            get { return sqlvisible; }
            set
            {
                sqlvisible = value;
                OnPropertyChanged(nameof(SQLVisible));
            }
        }

        public bool PopupOpen
        {
            get { return isOpen; }
            set
            {
                isOpen = value;
                OnPropertyChanged(nameof(PopupOpen));
            }
        }

        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                OnPropertyChanged(nameof(Visible));
            }
        }

        /// Varijable za aktivaciju i licencu
        public string hardwareID = Preferences.Get("key", null);
        public string Activation_code { get; set; }
        public string licence_type { get; set; }
        public DateTime expire_date { get; set; }
        public DateTime current_date { get; set; }

        public string expireDate { get; set; }
        public string licenceStatus { get; set; }
        public bool expiredLicence { get; set; }

        public bool ActivationVisible
        {
            get; set;
        }


        public MainPageViewModel()
        {
            SaveCommand = new Command(OnSaveClickedMySQL);
            LoadCommand = new Command(OnLoadClickedMySQL);
            DeleteCommand = new Command(OnDeleteClickedMySQL);

            SaveCommandNAS = new Command(OnSaveClickedNas);
            LoadCommandNAS = new Command(OnLoadClickedNas);
            DeleteCommandNAS = new Command(OnDeleteClickedNas);

            //PopupAcceptCommand = new Command(PopupAccept);
            ShowPopupCommand = new Command(Popup);
            ClosePopupCommand = new Command(PopupClose);

            SQLShowPopupCommand = new Command(SQLPopup);

            IPNas = Preferences.Get(IP_nas, "");
            UserNas = Preferences.Get(USER_nas, "");
            PassNas = Preferences.Get(PASS_nas, "");
            Folder = Preferences.Get(FOLDER_nas, "");
            SubFolder = Preferences.Get(SUBFOLDER_nas, "");

            IP = Preferences.Get(IP_mysql, "");
            UserName = Preferences.Get(USER_mysql, "");
            Password = Preferences.Get(PASS_mysql, "");
            DatabaseName = Preferences.Get(databasename_mysql, "");

            Version = $"Verzija {AppInfo.VersionString}";
            Activation_code = Preferences.Get("activation_code", "");
            licence_type = Preferences.Get("licence_type", "");
            expireDate = Preferences.Get("expire_date", "");
            licenceStatus = Preferences.Get("licence_active", "");
            current_date = DateTime.Now.Date;
            ParseDate();
            LicenceCheck();
            RefreshTime();
            var timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) => RefreshTime();
            timer.Start();
        }

        public void ParseDate()
        {
            try
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.Parse(expireDate);
                expire_date = dateTimeOffset.Date;
                TimeSpan difference = current_date.Subtract(expire_date);
                double days = difference.TotalDays;
                string daysR = days.ToString();
                Debug.WriteLine(daysR);
                Preferences.Set("days_until_expiry", daysR);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Date parsing error:" + ex.Message);

            }
        }

        void RefreshTime()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                datetime = DateTime.Now.ToString("f");

            }
            );
        }

        // POČETAK KOMANDI ZA SQL
        private void OnSaveClickedMySQL()
        {
            try
            {
                string ip = IP;
                string password = Password;
                string userName = UserName;
                string databaseName = DatabaseName;

                Preferences.Set(IP_mysql, IP);
                Preferences.Set(USER_mysql, UserName);
                Preferences.Set(PASS_mysql, Password);
                Preferences.Set(databasename_mysql, DatabaseName);

                SQLPopupClose();
                Debug.WriteLine("Saved");
                Debug.WriteLine(UserName + " " + Password);


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void OnLoadClickedMySQL()
        {
            try
            {
                var ipmysql = Preferences.Get(IP, IP_mysql);
                var usermysql = Preferences.Get(UserName, USER_mysql);
                var passmysql = Preferences.Get(Password, PASS_mysql);
                var databaseNamemysql = Preferences.Get(DatabaseName, databasename_mysql);

                Debug.WriteLine("Load uspješan");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "in MainPageViewModel OnLoadSQL");
            }
        }

        private void OnDeleteClickedMySQL()
        {
            if (String.IsNullOrEmpty(IP))
            {
                ShowAlert("Alert", "Data is already deleted.");
            }
            else
            {
                Preferences.Remove(IP);
                Preferences.Remove(UserName);
                Preferences.Remove(Password);
                Preferences.Remove(databasename_mysql);

            }

            IP = "";
            UserName = "";
            Password = "";
            DatabaseName = "";
        }

        //KRAJ KOMANDI ZA SQL

        //POČETAK KOMANDI ZA NAS

        private void OnSaveClickedNas()
        {
            try
            {
                // Preference value
                string ip_nas = IPNas;
                string pass_nas = PassNas;
                string user_nas = UserNas;
                string folder = Folder;
                string subFolder = SubFolder;
                Preferences.Set(IP_nas, ip_nas);
                Preferences.Set(PASS_nas, pass_nas);
                Preferences.Set(USER_nas, user_nas);
                Preferences.Set(FOLDER_nas, folder);
                Preferences.Set(SUBFOLDER_nas, subFolder);

                Debug.WriteLine("Nas saved " + Preferences.Default);
                PopupClose();
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "In MainPageViewModel NAS");
            }
        }

        private void OnLoadClickedNas()
        {
            try
            {
                var ipmynas = Preferences.Get(IP_nas, IPNas);
                var usermynas = Preferences.Get(USER_nas, UserNas);
                var passmynas = Preferences.Get(PASS_nas, PassNas);
                var folder = Preferences.Get(FOLDER_nas, Folder);
                var subfolder = Preferences.Get(SUBFOLDER_nas, SubFolder);

                Debug.WriteLine("Load uspješan");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "in MainPageViewModel OnLoadNAS");
            }
        }

        private void OnDeleteClickedNas()
        {
            try
            {
                if (String.IsNullOrEmpty(IPNas))
                {
                    ShowAlert("Alert", "Data is already deleted.");
                }
                else
                {
                    Preferences.Remove(IPNas);
                    Preferences.Remove(UserNas);
                    Preferences.Remove(PassNas);
                    Preferences.Remove(Folder);
                    Preferences.Remove(SubFolder);

                    Debug.WriteLine("Succesfully deleted the values");
                }
                IPNas = "";
                UserNas = "";
                PassNas = "";
                Folder = "";
                SubFolder = "";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        //KRAJ NAS KOMANDI

        //Komande za otvaranje

        private void Popup()
        {
            try
            {
                PopupOpen = true;
                Visible = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void PopupClose()
        {
            try
            {
                PopupOpen = false;
                Visible = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// NAPRAVI SQL POPUP NE RADI
        /// 
        /// </summary>
        private void SQLPopup()
        {

            try
            {
                SQLPopupOpen = true;
                SQLVisible = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private void SQLPopupClose()
        {
            try
            {
                SQLPopupOpen = false;
                SQLVisible = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        //private void PopupAccept()
        //{
        //    try
        //    {
        //        var ipmysql = Preferences.Get(IPNas, "");
        //        var usermysql = Preferences.Get(UserNas, "");
        //        var passmysql = Preferences.Get(PassNas, "");
        //        if ()
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //    }
        //}

        // Provjera licence //

        private void LicenceCheck() // Provjera isteka licence
        {

            if (expire_date > current_date)
            {
                expiredLicence = true;

            }
            else if (licenceStatus == "0" || licenceStatus == null)
            {
                expiredLicence = true;
            }
            else { expiredLicence = false; }
            ActivationScreen();
        }

        private void ActivationScreen() //Prikaz aktivacije na glavnoj stranici 
        {

            if (expiredLicence == true)
            {
                ActivationVisible = true;
            }
            else
            {
                ActivationVisible = false;
            }
        }

        private async void ShowAlert(string title, string message)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }
        // Mora bit kad god je INotifyPropertyChanged na pageu
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
