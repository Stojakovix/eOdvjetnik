using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Timer = System.Timers.Timer;
using eOdvjetnik.Services;
using eOdvjetnik.Resources.Strings;
using CommunityToolkit.Mvvm.Messaging;
using Plugin.LocalNotification;
using System.Text.Json;
using eOdvjetnik.Models;


namespace eOdvjetnik.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {

        ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();

        #region Stavke vidljive na MainPageu

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
        public ICommand ClearPrefrences { get; set; }
        public ICommand CheckLicenceStatus { get; set; } //jer premještamo gumb iz clicked cs u binding VM - dodati u constructor!


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
        public string Activation_code { get; set; }

        //Footer:
        public string Version { get; set; }
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
        #endregion

        #region Varijable za aktivaciju/licencu
        public string hardwareID = Preferences.Get("key", null);
        private string LocalUserName;
        public string UserName
        {
            get { return LocalUserName; }
            set
            {
                if (LocalUserName != value)
                {
                    LocalUserName = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }
        public string UserID { get; set; }
        public string UserInitials { get; set; }

        public DateTime TrialFileCreated;

        public DateTime ExpireDateDT { get; set; }
        public DateTime CurrentDateDT { get; set; }
        private string _expireDateString;
        public string ExpireDateString
        {
            get { return _expireDateString; }
            set
            {
                if (_expireDateString != value)
                {
                    _expireDateString = value;
                    OnPropertyChanged(nameof(ExpireDateString));
                }
            }
        }
        public string LicenceStatus { get; set; }
        public bool ExpiredLicence { get; set; }
        public double GracePeriod { get; set; }
        public string ActivationCode { get; set; }

        #endregion

        public MainPageViewModel()
        {
            Version = $"{AppResources.Version} {AppInfo.VersionString}";
            ClearPrefrences = new Command(DeletePreferences);
            CheckLicenceStatus = new Command(OnRefreshLicenceClick);
            CurrentDateDT = DateTime.Now.Date;
            RefreshTime();
            ExpireDateString = Preferences.Get("expire_date", "");
            LicenceStatus = Preferences.Get("licence_active", "");
            CompanyName = Preferences.Get("naziv_tvrtke", "");

            var timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) => RefreshTime();
            timer.Start();
            try
            {
                ActivationCodeCheck();
                ParseDate();
            }
            
            catch (Exception ex)
            {
                Debug.WriteLine("ParseDate() or ActivationCodeCheck() error" + ex.Message);
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
            UserNameAndID();
        }

        void RefreshTime()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                CurrentDateTimeString = DateTime.Now.ToString("f");

            }
            );
        }

        public void ParseDate()
        {
            try
            {
                if (DateTimeOffset.TryParse(ExpireDateString, out DateTimeOffset dateTimeOffset))
                {
                    ExpireDateDT = dateTimeOffset.Date;
                    TimeSpan difference = ExpireDateDT.Subtract(CurrentDateDT);
                    double days = difference.TotalDays;
                    string daysR = days.ToString();
                    GracePeriod = days + 10;
                    Debug.WriteLine("ParseDate() - days until licence expires: " + daysR);
                    Debug.WriteLine("ParseDate() - grace period after licence expired: " + GracePeriod);
                    Preferences.Set("days_until_expiry", daysR);
                }
                else
                {
                    Debug.WriteLine("Invalid date format: " + ExpireDateString);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred: " + ex.Message);
            }
        }


        public void DeletePreferences()
        {
            string hwid = Preferences.Get("key", null);
            string activationCode = Preferences.Get("activation_code", "");
            Debug.WriteLine("Brisanje preferenci " + hwid + " " + activationCode);
            try
            {
                Preferences.Default.Clear();
                Preferences.Clear();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async void ActivationCodeCheck()
        {
            //var database = new Prefdatabase();

            Debug.WriteLine("MainPageViewModel - > ActivationLoop");
            string string1 = "https://cc.eodvjetnik.hr/eodvjetnikadmin/waiting-lists/request?cpuid=";
            string string2 = Preferences.Get("key", null);
            string activationURL = string.Concat(string1, string2);
            Debug.WriteLine("MainPageViewModel - > ActivationLoop - URL za waiting list: " + activationURL);
            Preferences.Get("activation_code", ActivationCode);

            if (ActivationCode == null || ActivationCode == "")
            {


                try
                {
                    Debug.WriteLine("MainPageViewModel - > ActivationLoop -> usao u try");

                    using (var client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.GetAsync(activationURL);

                        if (response.IsSuccessStatusCode)
                        {
                            Debug.WriteLine("MainPageViewModel - > ActivationLoop -> Dohvatio response");

                            string jsonContent = await response.Content.ReadAsStringAsync();
                            response.EnsureSuccessStatusCode();
                            var content = await response.Content.ReadAsStringAsync();
                            Debug.WriteLine("MainPageViewModel - > ActivationLoop -> JSON: " + content);

                            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                            var data = System.Text.Json.JsonSerializer.Deserialize<ActivationData[]>(content, options);

                            ActivationCode = data[0].activation_code;
                            Preferences.Set("activation_code", ActivationCode);

                            Debug.WriteLine($"Received data: {data[0].id}, {data[0].created}, {data[0].hwid}, {data[0].IP}, {data[0].activation_code}");
                        }
                        else
                        {
                            Debug.WriteLine("MainPageViewModel - > ActivationLoop -> Povezivanje neuspješno");
                            await Application.Current.MainPage.DisplayAlert("Upozorenje", "Povezivanje s poslužiteljem nije uspjelo.", "OK");
                        }
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Activation error:" + ex.Message);
                }
            }
            LicenceCheck();
        }


        public async void LicenceCheck()
        {

            Debug.WriteLine("MainPageViewModel - > LicenceCheck");
            string string1 = "https://cc.eodvjetnik.hr/eodvjetnikadmin/licences/request?cpuid=";
            string string2 = Preferences.Get("key", null);
            string licenceURL = string.Concat(string1, string2);
            Debug.WriteLine("MainPageViewModel - > LicenceCheck - URL za dohvaćanje licence: " + licenceURL);
            try
            {
                Debug.WriteLine("MainPageViewModel - > LicenceCheck -> usao u try");

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(licenceURL);

                    if (response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("MainPageViewModel - > LicenceCheck -> uspješno dohvatio licence");

                        string jsonContent = await response.Content.ReadAsStringAsync();
                        response.EnsureSuccessStatusCode();
                        var content = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine("MainPageViewModel - > LicenceCheck -> JSON s licencama: " + content);

                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var jsonObject = JsonSerializer.Deserialize<JsonElement>(content, options);

                        string licenceIsActive = jsonObject.GetProperty("Devices")[0].GetProperty("licence_active").GetString();
                        int companyID = jsonObject.GetProperty("Devices")[0].GetProperty("company_id").GetInt32();
                        string company_ID = companyID.ToString();
                        int devicetypeID = jsonObject.GetProperty("Devices")[0].GetProperty("device_type_id").GetInt32();
                        string devicetype_ID = devicetypeID.ToString();
                        string licenceType = jsonObject.GetProperty("LicenceTypes")[0].GetProperty("naziv").GetString();
                        string expireDate = jsonObject.GetProperty("Licences")[0].GetProperty("expire_date").GetString();
                        string nazivTvrtke = jsonObject.GetProperty("Companies")[0].GetProperty("naziv").GetString();
                        string OIBTvrtke = jsonObject.GetProperty("Companies")[0].GetProperty("OIB").GetString();
                        string adresaTvrtke = jsonObject.GetProperty("Companies")[0].GetProperty("adresa").GetString();

                        Preferences.Set("expire_date", expireDate);
                        Preferences.Set("licence_type", licenceType);
                        Preferences.Set("licence_active", licenceIsActive);
                        Preferences.Set("naziv_tvrtke", nazivTvrtke);
                        Preferences.Set("OIBTvrtke", OIBTvrtke);
                        Preferences.Set("adresaTvrtke", adresaTvrtke);
                        Preferences.Set("company_id", company_ID);
                        Preferences.Set("device_type_id", devicetype_ID);

                        Debug.WriteLine("MainPageViewModel - > Company info: " + nazivTvrtke + " " + OIBTvrtke + " " + adresaTvrtke);

                    }
                    else
                    {
                        // Što ako se ne može povezati - riješiti preko nekog bool-a 
                        //await Application.Current.MainPage.DisplayAlert("Upozorenje", "Povezivanje s poslužiteljem nije uspjelo.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Activation error:" + ex.Message);


                //LicenceType = Preferences.Get("licence_type", "");
                //if (LicenceType == "" || LicenceType == null)
                //{
                //    await Application.Current.MainPage.DisplayAlert("", "Licenca nije aktivna.", "OK");
                //}

            }
          
            LicenceUpdatedMessage(); //Javlja postavkama da je licenca ažurirana
            LicenceExpiryCheck();
        }

        public void CheckNasSQLSettings()  //provjera jesu li NAS i SQL postavke unesne

        {
            string nas = Preferences.Get("IP Adresa", "");
            string sql = Preferences.Get("IP Adresa2", "");
            Debug.WriteLine("provjera jesu li dodani nas " + nas + " i sql postavke " + sql + "koja je licenca " + LicenceType);
            if (nas == "" || nas == null || sql == "" || sql == null)
            {
                Application.Current.MainPage.DisplayAlert("", "Unesite NAS i SQL postavke.", "OK");
            }
            else
            {
                UserNameAndID();
            }
        }



    private void LicenceExpiryCheck() // Provjera isteka licence nakon što izvrti LicenceCheck()
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

            try //provjera ponovne instalacije Trial licence -> postoji li u SQL-u file ID stariji o 45 dana
            {
                string licence_type = Preferences.Get("licence_type", "");
                int numberOfCharacters = 5;
                string trialCheck = licence_type.Substring(0, Math.Min(licence_type.Length, numberOfCharacters));
                Debug.WriteLine("Kalendar ResourceView - 'Trial' provjera: " + trialCheck);
                if (trialCheck == "Trial") 
                {

                    try
                    {
                        string query = "SELECT created FROM files WHERE id = (SELECT MIN(id) FROM files);";
                        Debug.WriteLine("");
                        externalSQLConnect.sqlQuery(query);

                        Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
                        Debug.WriteLine(query + " u Search resultu");
                        if (filesData != null)
                        {
                            foreach (Dictionary<string, string> filesRow in filesData)
                            {

                                DateTime oldestFileCreatedDate;
                                DateTime.TryParse(filesRow["created"], out oldestFileCreatedDate);
                                TrialFileCreated = oldestFileCreatedDate;
                            };
                            Debug.WriteLine("Trial file check " + TrialFileCreated);

                        }

                        DateTime dateTime = DateTime.Now;

                        TimeSpan difference = dateTime.Subtract(TrialFileCreated);
                        int daysDifference = difference.Days;
                        if (daysDifference > 45)
                        {
                            ExpiredLicence = true;
                        
                        }
                    }

                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
               

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

          ActivationScreen();
          Activation_code = Preferences.Get("activation_code", "");

        }

     private void ActivationScreen() //Prikaz aktivacijskog koda i gumba Osvježi licencu
        {


            if (ExpiredLicence == true)
            {
                ActivationVisible = true;
                string aktivacija = "LicenceNotActive";
                Preferences.Set("activation_disable", aktivacija);
            }
            else if (ExpiredLicence == false)
            {
                ActivationVisible = false;
                string aktivacija = "LicenceActive";
                Preferences.Set("activation_disable", aktivacija);
                CheckNasSQLSettings();
            }

        }
        
      
        public void UserNameAndID() //dohvaća username, ID i inicijale za kasnije spremanje novog spisa/kontakta - možda treba dodati kod u SPISE i KLIJENTE
        {
            // dodati da samo izvršava if (username == null || username == "")

            string query = "SELECT * FROM employees WHERE hwid = '" + hardwareID + "';";
            Debug.WriteLine(query);
            try
            {
                Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
                if (filesData != null && filesData.Length > 0)
                {
                    foreach (Dictionary<string, string> filesRow in filesData)
                    {
                        int id;
                        int.TryParse(filesRow["id"], out id);
                        UserName = filesRow["ime"];
                        UserID = id.ToString();
                        UserInitials = filesRow["inicijali"];
                    }
                    Preferences.Set("UserName", UserName);
                    Preferences.Set("UserID", UserID);
                    Preferences.Set("UserInitials", UserInitials);

                }
                else
                {
                    UserName = " ";
                    UserID = " ";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void LicenceUpdatedMessage() //Ažurira licencu u PostavkeViewModelu
        {
            WeakReferenceMessenger.Default.Send(new LicenceUpdated("Licence updated!"));
        }

        private void OnRefreshLicenceClick() 
        {
            LicenceCheck(); 

            try
            {
                var request = new NotificationRequest
                {
                    NotificationId = 1,
                    Title = "Kliknut kalendar",
                    Description = "U kalendaru je dodan novi događaj",
                    BadgeNumber = 1,
                    CategoryType = NotificationCategoryType.Status,
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddSeconds(5),
                    }
                };
                LocalNotificationCenter.Current.AreNotificationsEnabled();
                LocalNotificationCenter.Current.Show(request);
                Debug.WriteLine(request.Title);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


