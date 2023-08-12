using System;
using System.Windows.Input;
using eOdvjetnik.Views;
using System.Diagnostics;
using Syncfusion.Maui.Popup;
using System.ComponentModel;
using eOdvjetnik.Resources.Strings;

namespace eOdvjetnik.ViewModel
{
	public class AppShellViewModel : INotifyPropertyChanged
	{
        public int DaysRemaining {get;set;}
        public string Days_Remaining { get; set; }

        private bool ExpiryisOpen, Expiryvisible;

        public bool ExpiryPopupOpen
        {
            get { return ExpiryisOpen; }
            set
            {
                ExpiryisOpen = value;
                OnPropertyChanged(nameof(ExpiryPopupOpen));
            }
        }

        public bool ExpiryVisible
        {
            get { return Expiryvisible; }
            set
            {
                Expiryvisible = value;
                OnPropertyChanged(nameof(ExpiryVisible));
            }
        }
        #region Navigacija
        public ICommand MainClickCommand { get; set; }
        public ICommand KalendarClickCommand { get; set; }
        public ICommand DokumentiClickCommand { get; set; }
        public ICommand KlijentiClickCommand { get; set; }
        public ICommand NaplataClickCommand { get; set; }
        public ICommand SpisiClickCommand { get; set; }
        public ICommand PostavkeClickCommand { get; set; }
        public ICommand SpiDokClickCommand { get; set; }

        #endregion
        // popup komande
        #region Support
        public ICommand OnSupportClickCommand { get; set; }
        public ICommand PopupCloseCommand { get; set; }

        private bool SupportisOpen, Supportvisible;
        
        public bool SupportPopupOpen
        {
            get { return SupportisOpen; }
            set
            {
                SupportisOpen = value;
                OnPropertyChanged(nameof(SupportPopupOpen));
            }
        }

        public bool SupportVisible
        {
            get { return Supportvisible; }
            set
            {
                Supportvisible = value;
                OnPropertyChanged(nameof(SupportVisible));
            }
        }
         
        public string Version { get; set; }
        #endregion

        #region NAS
        //Varijable za NAS preferenceas
        private const string IP_nas = "IP Adresa";
        private const string USER_nas = "Korisničko ime";
        private const string PASS_nas = "Lozinka";
        private const string FOLDER_nas = "Folder";
        private const string SUBFOLDER_nas = "SubFolder";

        //Save za nas

        public ICommand SaveCommandNAS { get; set; }
        public ICommand LoadCommandNAS { get; set; }
        public ICommand DeleteCommandNAS { get; set; }

        

        //Vrijednost varijabli NAS

        public string IPNas { get; set; }
        public string UserNas { get; set; }
        public string PassNas { get; set; }
        public string Folder { get; set; }
        public string SubFolder { get; set; }

        // Varijable za PopupNAS

        private bool NASisOpen, NASvisible;
        public ICommand ShowNASPopupCommand { get; set; }
        public ICommand CloseNASPopupCommand { get; set; }

        public bool NASPopupOpen
        {
            get { return NASisOpen; }
            set
            {
                NASisOpen = value;
                OnPropertyChanged(nameof(NASPopupOpen));
            }
        }

        public bool NASVisible
        {
            get { return NASvisible; }
            set
            {
                NASvisible = value;
                OnPropertyChanged(nameof(NASVisible));
            }
        }
        #endregion

        #region SQL
        //Varijable za MySQL preferences
        public const string IP_mysql = "IP Adresa2";
        public const string USER_mysql = "Korisničko ime2";
        public const string PASS_mysql = "Lozinka2";
        public const string databasename_mysql = "databasename";
        //MySQL varijable
        public string query;

        //Save za mysql
        public ICommand SQLSaveCommand { get; set; }
        public ICommand SQLLoadCommand { get; set; }
        public ICommand SQLDeleteCommand { get; set; }

        //Dohvaća vrijednost varijabli iz mainPagea SQL
        public string IP { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }

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
 
        public bool disableMenu { get; set; }
        #endregion
        public AppShellViewModel()
		{
            #region Navigacija komande

            MainClickCommand = new Command(OnMainClick);
			KalendarClickCommand = new Command(OnKalendarClick);
			DokumentiClickCommand = new Command(OnDokumentiClick);
            KlijentiClickCommand = new Command(OnKlijentiClick);
            NaplataClickCommand = new Command(OnNaplataClick);
            SpisiClickCommand = new Command(OnSpisiClick);
            PostavkeClickCommand = new Command(OnPostavkeClick);
            SpiDokClickCommand = new Command(OnSpiDokClick);
            #endregion

            #region Support komande
            Version = $"{AppResources.Version} {AppInfo.VersionString}";

            //Popup pozivanje
            PopupCloseCommand = new Command(PopupClose);
            OnSupportClickCommand = new Command(OnSupportClick);
            #endregion

            #region NAS komande
            SaveCommandNAS = new Command(OnSaveClickedNas);
            LoadCommandNAS = new Command(OnLoadClickedNas);
            DeleteCommandNAS = new Command(OnDeleteClickedNas);
            ShowNASPopupCommand = new Command(NASPopup);
            CloseNASPopupCommand = new Command(NASPopupClosed);

            #endregion

            #region SQL komande
            SQLSaveCommand = new Command(OnSaveClickedMySQL);
            SQLLoadCommand = new Command(OnLoadClickedMySQL);
            SQLDeleteCommand = new Command(OnDeleteClickedMySQL);
            SQLShowPopupCommand = new Command(SQLPopup);
            #endregion

            #region NAS Varijable
            IPNas = Preferences.Get(IP_nas, "");
            UserNas = Preferences.Get(USER_nas, "");
            PassNas = Preferences.Get(PASS_nas, "");
            Folder = Preferences.Get(FOLDER_nas, "");
            SubFolder = Preferences.Get(SUBFOLDER_nas, "");

            #endregion

            #region SQL varijable

            IP = Preferences.Get(IP_mysql, "");
            UserName = Preferences.Get(USER_mysql, "");
            Password = Preferences.Get(PASS_mysql, "");
            DatabaseName = Preferences.Get(databasename_mysql, "");

            #endregion
            SfPopup popup = new SfPopup();
           

            CheckExpiry();
            
        }

        public void DisableMenu()
        {
             string ProvjeraAktivacije = Preferences.Get("activation_disable", "");
            Debug.WriteLine(ProvjeraAktivacije);
            if (ProvjeraAktivacije == "Licence") //promijeniti "Licence" u "LicenceNotActive" da se aplikacija onemogući
            {
                disableMenu = true;
            }
            else
            {
                disableMenu = false;
            }
            Debug.WriteLine(disableMenu);
        }

        #region Navigacija Funkcije
        private async void OnMainClick()
		{
            DisableMenu();
            if (disableMenu == false)
            {
                await Shell.Current.GoToAsync("///MainPage");
                Debug.WriteLine("KLIKNO");
            }
			
		}

        private async void OnDokumentiClick()
        {
            DisableMenu();
            if (disableMenu == false)
            {
                await Shell.Current.GoToAsync("///Dokumenti");
                Debug.WriteLine("KLIKNO");
            }
        }

        private async void OnKalendarClick()
        {
            try
            {
                DisableMenu();
                if (disableMenu == false)
                {
                    await Shell.Current.GoToAsync("///Kalendar");
                    Debug.WriteLine("KLIKNO");
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }

        private async void OnKlijentiClick()
        {
            DisableMenu();
            if (disableMenu == false)
            {
                await Shell.Current.GoToAsync("///Klijenti");
                Debug.WriteLine("KLIKNO Klijente");
            }
        }

        private async void OnNaplataClick()
        {
            DisableMenu();
            if (disableMenu == false)
            {
                await Shell.Current.GoToAsync("///Naplata");
                Debug.WriteLine("KLIKNO Naplatu");
            }
        }
        private async void OnSpisiClick()
        {
            DisableMenu();
            if (disableMenu == false)
            {
                await Shell.Current.GoToAsync("///Spisi");
                Debug.WriteLine("KLIKNO Spisi");
            }
        }
        private async void OnPostavkeClick()
        {
            DisableMenu();
            if (disableMenu == false)
            {
                await Shell.Current.GoToAsync("///Postavke");
                Debug.WriteLine("KLIKNO Postavke");
            }
        }
        private async void OnSpiDokClick()
        {
            DisableMenu();
            if (disableMenu == false)
            {
                await Shell.Current.GoToAsync("///SpiDok");
                Debug.WriteLine("KLIKNO SpiDok");
            }
        }
        #endregion

        #region Support funkcije
        private void OnSupportClick()
        {
            DisableMenu();
            if (disableMenu == false)
            {
                SupportPopupOpen = true;
                Supportvisible = true;
            }
           
        }

        private void PopupClose()
        {
            SupportPopupOpen = false;
            Supportvisible = false;
        }
        #endregion

        #region NAS Funkcije
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
                NASPopupClosed();
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
        private void NASPopup()
        {
            try
            {
                NASPopupOpen = true;
                NASVisible = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
            
        }

        private void NASPopupClosed()
        {
            try
            {
                NASPopupOpen = false;
                NASVisible = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
        }
        #endregion

        #region SQL Funkcije
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

                
                Debug.WriteLine("Saved");
                Debug.WriteLine(UserName + " " + Password);
                SQLPopupClose();

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

        #endregion

        #region Istek
        private void CheckExpiry()
        {
            try
            {
                DaysRemaining = 0;
                string days_remaining = Preferences.Get("days_until_expiry", "");
                DaysRemaining = Convert.ToInt32(days_remaining);
                Debug.WriteLine(DaysRemaining);
                if (DaysRemaining > 1 && DaysRemaining < 10)
                {
                    Days_Remaining = string.Concat("Vaša licenca ističe za ", days_remaining, " dana");
                    ExpiryPopupOpen = true;
                    Expiryvisible = true;
                }
                else if (DaysRemaining == 1)
                {
                    Days_Remaining = "Vaša licenca ističe sutra!";
                    ExpiryPopupOpen = true;
                    Expiryvisible = true;
                }
                else if (DaysRemaining == 0 || DaysRemaining < 0)
                {
                    Days_Remaining = "Vaša licenca je istekla!";
                    ExpiryPopupOpen = true;
                    Expiryvisible = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


        }

        private void PopupExpiryClose()
        {
            ExpiryPopupOpen = false;
            Expiryvisible = false;
        }
        #endregion
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

