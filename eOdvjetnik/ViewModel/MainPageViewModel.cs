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


        #region Varijable za aktivaciju/licencu
        /// Varijable za aktivaciju i licencu
        public string hardwareID = Preferences.Get("key", null);
        public string Activation_code { get; set; }
        public string licence_type { get; set; }
        public DateTime expire_date { get; set; }
        public DateTime current_date { get; set; }
        public string expireDate { get; set; }
        public string licenceStatus { get; set; }
        public bool expiredLicence { get; set; }
        public double gracePeriod { get; set; }
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
        #endregion


        public MainPageViewModel()
        {

            navigacija = new Navigacija();
            Version = $"{AppResources.Version} {AppInfo.VersionString}";
            Activation_code = Preferences.Get("activation_code", "");
            licence_type = Preferences.Get("licence_type", "");
            expireDate = Preferences.Get("expire_date", "");
            licenceStatus = Preferences.Get("licence_active", "");
            current_date = DateTime.Now.Date;

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
        }
     


        public void ParseDate()
        {
            try
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.Parse(expireDate);
                expire_date = dateTimeOffset.Date;
                TimeSpan difference = expire_date.Subtract(current_date);
                double days = difference.TotalDays;
                string daysR = days.ToString();
                gracePeriod = days + 10;
                Debug.WriteLine("ParseDate() - days until licence expires: " + daysR);
                Debug.WriteLine("ParseDate() - grace period after licence expired: " + gracePeriod);
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
                datetime = DateTime.Now.ToString("f");

            }
            );
        }

        private void LicenceCheck() // Provjera isteka licence
        {
            licenceStatus = Preferences.Get("licence_active", "");

            expiredLicence = true;
            if (licenceStatus == null)
            {
                expiredLicence = true;
            }

            else if (licenceStatus == "0" && gracePeriod > 0 && gracePeriod < 11)
            {
                expiredLicence = true;

            }
            else if (licenceStatus == "1")
            {
                expiredLicence = false;

            }


            ActivationScreen();

        }

        private void ActivationScreen() //Prikaz aktivacije na glavnoj stranici 
        {


            if (expiredLicence == true)
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
