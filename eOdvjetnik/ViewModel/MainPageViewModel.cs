using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Timer = System.Timers.Timer;
using eOdvjetnik.Services;
using eOdvjetnik.Resources.Strings;
using CommunityToolkit.Mvvm.Messaging;

namespace eOdvjetnik.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        


        //DateTime
        private Navigacija navigacija;

        
        private string LocalCurrentDateTimeString { get; set; }
        public string CurrentDateTimeString
        {
            get { return LocalCurrentDateTimeString; }
            set
            {
                LocalCurrentDateTimeString = value;
                OnPropertyChanged(nameof(CurrentDateTimeString));
            }
        }
        public string Version { get; set; }


        #region Varijable za aktivaciju/licencu
        /// Varijable za aktivaciju i licencu
        public string hardwareID = Preferences.Get("key", null);
        public string Activation_code { get; set; }
        public DateTime ExpireDateDT { get; set; }
        public DateTime CurrentDateDT { get; set; }
        public string ExpireDateString { get; set; }
        public string LicenceStatus { get; set; }
        public bool ExpiredLicence { get; set; }
        public double GracePeriod { get; set; }
        private string LocalLicenceType;

        public string LicenceType
        {
            get { return LocalLicenceType; }
            set
            {
                if (LocalLicenceType != value)
                {
                    LocalLicenceType = value;
                    OnPropertyChanged(nameof(LicenceType));
                }
            }
        }
        private bool IsActivationVisible;
        public bool ActivationVisible
    {
            get { return IsActivationVisible; }
            set
            {
                if (IsActivationVisible != value)
                {
                    IsActivationVisible = value;
                    OnPropertyChanged(nameof(ActivationVisible));
                }
            }
        }
        private string LocalCompanyName;

        public string CompanyName
        {
            get { return LocalCompanyName; }
            set
            {
                if (LocalCompanyName != value)
                {
                    LocalCompanyName = value;
                    OnPropertyChanged(nameof(CompanyName));
                }
            }
        }
        #endregion


        public MainPageViewModel()
        {

            navigacija = new Navigacija();
            Version = $"{AppResources.Version} {AppInfo.VersionString}";
            Activation_code = Preferences.Get("activation_code", "");
                        ExpireDateString = Preferences.Get("expire_date", "");
            LicenceStatus = Preferences.Get("licence_active", "");
            CurrentDateDT = DateTime.Now.Date;
            CompanyName = Preferences.Get("naziv_tvrtke", "");
            WeakReferenceMessenger.Default.Register<CheckLicence>(this, RefreshLicenceData);


            RefreshTime();

            var timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) => RefreshTime();
            timer.Start();
            try
            {
                ParseDate();
                LicenceCheck();
            }

            catch (Exception ex) 
            {
                Debug.WriteLine("Cannot ParseDate()" + ex.Message );

            }

            DevicePlatform CheckPlatform = DeviceInfo.Current.Platform;
            Debug.WriteLine("VRSTA PLATFORME " + CheckPlatform);
            string VrstaPlatforme = CheckPlatform.ToString();
            Preferences.Set("vrsta_platforme", VrstaPlatforme);

            string TypeOfLicence = Preferences.Get("licence_type", "");
            if (LicenceStatus == "0" || TypeOfLicence == null || TypeOfLicence == "")
            {
                LicenceType = "nije aktivirana";
            }
            else
            {
                LicenceType = TypeOfLicence;
            }
        }

        private void RefreshLicenceData(object recipient, CheckLicence message)
        {
            try
            {
                LicenceCheck();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
            ActivationScreen();
            string TypeOfLicence = Preferences.Get("licence_type", "");
            if (LicenceStatus == "0" || TypeOfLicence == null || TypeOfLicence == "")
            {
                LicenceType = "nije aktivirana";
            }
            else
            {
                LicenceType = TypeOfLicence;
            }
        }
     


        public void ParseDate()
        {
            try
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.Parse(ExpireDateString);
                ExpireDateDT = dateTimeOffset.Date;
                TimeSpan difference = ExpireDateDT.Subtract(CurrentDateDT);
                double days = difference.TotalDays;
                string daysR = days.ToString();
                GracePeriod = days + 10;
                Debug.WriteLine("ParseDate() - days until licence expires: " + daysR);
                Debug.WriteLine("ParseDate() - grace period after licence expired: " + GracePeriod);
                Preferences.Set("days_until_expiry", daysR);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Licenca vjerojatno nije aktivna: " + ex.Message);

            }
        }

        void RefreshTime()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                CurrentDateTimeString = DateTime.Now.ToString("f");

            }
            );
        }

        private void LicenceCheck() // Provjera isteka licence
        {
            LicenceStatus = Preferences.Get("licence_active", "");

            ExpiredLicence = true;
            if (LicenceStatus == null)
            {
                ExpiredLicence = true;
            }

            else if (LicenceStatus == "0" && GracePeriod > 0 && GracePeriod < 11)
            {
                ExpiredLicence = true;

            }
            else if (LicenceStatus == "1")
            {
                ExpiredLicence = false;

            }


            ActivationScreen();

        }

        private void ActivationScreen() //Prikaz aktivacije na glavnoj stranici 
        {


            if (ExpiredLicence == true)
            {
                ActivationVisible = true;
                string aktivacija = "LicenceNotActive";
                Preferences.Set("activation_disable", aktivacija);
            }
            else
            {
                ActivationVisible = false;
                string aktivacija = "LicenceActive";
                Preferences.Set("activation_disable", aktivacija);
            }

        }
        // Mora bit kad god je INotifyPropertyChanged na pageu
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
