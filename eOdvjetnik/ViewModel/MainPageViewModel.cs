﻿using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Timer = System.Timers.Timer;


namespace eOdvjetnik.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {

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

        public bool ActivationVisible
        {
            get; set;
        } 
        #endregion


        public MainPageViewModel()
        {
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

        private void LicenceCheck() // Provjera isteka licence
        {
          expiredLicence = true;

          if (licenceStatus == "0" || licenceStatus == null)
          {
              expiredLicence = true;

          }
          else if (licenceStatus == "1")
          {
              expiredLicence = false;

          }
            else if (expire_date > current_date)
          {
              expiredLicence = true;
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
