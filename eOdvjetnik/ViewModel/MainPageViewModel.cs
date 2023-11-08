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
using System.Security.Cryptography;
using System.Text;

namespace eOdvjetnik.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {

        ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();

        private bool ServiceModeEnabled { get; set; }
        public bool ServiceMode
    {
            get { return ServiceModeEnabled; }
            set
            {
            ServiceModeEnabled = value;
                OnPropertyChanged(nameof(ServiceMode));
            }
        }

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
  

        private string pvtActivation_code;
        public string Activation_code
    {
            get { return pvtActivation_code; }
            set
            {
                if (pvtActivation_code != value)
                {
                pvtActivation_code = value;
                    OnPropertyChanged(nameof(Activation_code));
                }
            }
        }

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
        public string hardwareID = TrecaSreca.Get("key");
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
            ServiceMode = false;
            try
            {
                GenerateHWID();
            }

            catch (Exception ex)
            {
                Debug.WriteLine("GenerateHWID() error: " + ex.Message);
            }
            Version = $"{AppResources.Version} {AppInfo.VersionString}";
            ClearPrefrences = new Command(DeletePreferences);
            CheckLicenceStatus = new Command(OnRefreshLicenceClick);
            CurrentDateDT = DateTime.Now.Date;
            RefreshTime();


            ExpireDateString = TrecaSreca.Get("expire_date");
            LicenceStatus = TrecaSreca.Get("licence_active");
            CompanyName = TrecaSreca.Get("naziv_tvrtke");
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
            TrecaSreca.Set("vrsta_platforme", VrstaPlatforme);

            string TypeOfLicence = TrecaSreca.Get("licence_type");
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
                    TrecaSreca.Set("days_until_expiry", daysR);
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
            string hwid = TrecaSreca.Get("key");
            string activationCode = TrecaSreca.Get("activation_code");
            Debug.WriteLine("Brisanje preferenci " + hwid + " " + activationCode);
            try
            {
                TrecaSreca.Clear();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async void ActivationCodeCheck()
        {
            //var database = new Prefdatabase();

            Debug.WriteLine("MainPageViewModel - > ActivationCodeCheck");
            string string1 = "https://cc.eodvjetnik.hr/eodvjetnikadmin/waiting-lists/request?cpuid=";
            string string2 = TrecaSreca.Get("key");
            string activationURL = string.Concat(string1, string2);
            Debug.WriteLine("MainPageViewModel - > ActivationCodeCheck - URL za waiting list: " + activationURL);
            TrecaSreca.Get("activation_code");

            if (ActivationCode == null || ActivationCode == "")
            {


                try
                {
                    Debug.WriteLine("MainPageViewModel - > ActivationCodeCheck -> usao u try");

                    using (var client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.GetAsync(activationURL);

                        if (response.IsSuccessStatusCode)
                        {
                            Debug.WriteLine("MainPageViewModel - > ActivationCodeCheck -> Dohvatio response");

                            string jsonContent = await response.Content.ReadAsStringAsync();
                            response.EnsureSuccessStatusCode();
                            var content = await response.Content.ReadAsStringAsync();
                            Debug.WriteLine("MainPageViewModel - > ActivationCodeCheck -> JSON: " + content);

                            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                            var data = System.Text.Json.JsonSerializer.Deserialize<ActivationData[]>(content, options);

                            ActivationCode = data[0].activation_code;
                            TrecaSreca.Set("activation_code", ActivationCode);

                            Debug.WriteLine($"MainPageViewModel - > ActivationCodeCheck -> received data: {data[0].id}, {data[0].created}, {data[0].hwid}, {data[0].IP}, {data[0].activation_code}");
                            Debug.WriteLine("MainPageViewModel - > ActivationCodeCheck -> Uspjesno dovrsen!");

                        }
                        else
                        {
                            Debug.WriteLine("MainPageViewModel - > ActivationCodeCheck -> Povezivanje neuspješno");
                            await Application.Current.MainPage.DisplayAlert("Upozorenje", "Povezivanje s poslužiteljem nije uspjelo.", "OK");
                        }
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("MainPageViewModel - ActivationCodeCheck() error:" + ex.Message);
                }
            }
            LicenceCheck();
        }


        public async void LicenceCheck()
        {

            Debug.WriteLine("MainPageViewModel - > LicenceCheck");
            string string1 = "https://cc.eodvjetnik.hr/eodvjetnikadmin/licences/request?cpuid=";
            string string2 = TrecaSreca.Get("key");
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
                        bool serviceMode = jsonObject.GetProperty("Companies")[0].GetProperty("service_mode").GetBoolean();



                        TrecaSreca.Set("expire_date", expireDate);
                        TrecaSreca.Set("licence_type", licenceType);
                        TrecaSreca.Set("licence_active", licenceIsActive);
                        TrecaSreca.Set("naziv_tvrtke", nazivTvrtke);
                        TrecaSreca.Set("OIBTvrtke", OIBTvrtke);
                        TrecaSreca.Set("adresaTvrtke", adresaTvrtke);
                        TrecaSreca.Set("company_id", company_ID);
                        TrecaSreca.Set("device_type_id", devicetype_ID);
                        TrecaSreca.Set("service_mode", serviceMode.ToString());

                        Debug.WriteLine("MainPageViewModel - > Company info: " + nazivTvrtke + " " + OIBTvrtke + " " + adresaTvrtke);
                        Debug.WriteLine("MainPageViewModel - > LicenceCheck -> Uspjesno dovrsen!");

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
                Debug.WriteLine("MainPageViewModel - > LicenceCheck -> Catch! " + ex.Message);


                //LicenceType = Preferences.Get("licence_type", "");
                //if (LicenceType == "" || LicenceType == null)
                //{
                //    await Application.Current.MainPage.DisplayAlert("", "Licenca nije aktivna.", "OK");
                //}

            }
            string TypeOfLicence = TrecaSreca.Get("licence_type");
            if (LicenceStatus == "0" || TypeOfLicence == null || TypeOfLicence == "")
            {
                LicenceType = "nije aktivirana";
            }
            else
            {
                LicenceType = TypeOfLicence;
                CompanyName = TrecaSreca.Get("naziv_tvrtke");
            }
            LicenceUpdatedMessage(); //Javlja postavkama da je licenca ažurirana
            LicenceExpiryCheck();

            string serviceModeCheck = TrecaSreca.Get("service_mode");

            if (serviceModeCheck == "True")
            {
                ServiceMode = true;
                Debug.WriteLine("SERVICE MODE ENABLED!");
            }
        }

        public void CheckNasSQLSettings()  //provjera jesu li NAS i SQL postavke unesne

        {
            string nas = TrecaSreca.Get("IP Adresa");
            string sql = TrecaSreca.Get("IP Adresa2");
            Debug.WriteLine("provjera jesu li dodani nas " + nas + " i sql postavke " + sql + "koja je licenca " + LicenceType);
            if (nas == "" || nas == null || sql == "" || sql == null)
            {
                Application.Current.MainPage.DisplayAlert("", "Unesite NAS i SQL postavke.", "OK");
            }
           
        }



    private void LicenceExpiryCheck() // Provjera isteka licence nakon što izvrti LicenceCheck()
        {
            LicenceStatus = TrecaSreca.Get("licence_active");
            string vrstaLicence = TrecaSreca.Get("licence_type");

            if(vrstaLicence != "" || vrstaLicence != null || vrstaLicence != "nije aktivirana")
            {
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
            }
          

            try //provjera ponovne instalacije Trial licence -> postoji li u SQL-u file ID stariji o 45 dana
            {
                string licence_type = TrecaSreca.Get("licence_type");
                int numberOfCharacters = 5;
                string trialCheck = "";
                if (licence_type != null)
                {
                    trialCheck = licence_type.Substring(0, Math.Min(licence_type.Length, numberOfCharacters));
                }
                Debug.WriteLine("MainPageViewModel - LicenceExpiryCheck - 'Trial' provjera: " + trialCheck);
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
                            DateTime dateTime = DateTime.Now;

                            TimeSpan difference = dateTime.Subtract(TrialFileCreated);
                            int daysDifference = difference.Days;
                            if (daysDifference > 45)
                            {
                                Debug.WriteLine("8888888888888888888888888888888888888888888888");
                                Debug.WriteLine("8888888888888888888888888888888888888888888888");
                                Debug.WriteLine("8888888888888888888888888888888888888888888888");
                                Debug.WriteLine("8888888888888888888888888888888888888888888888");
                                Debug.WriteLine("8888888888888888888888888888888888888888888888");
                                Debug.WriteLine(daysDifference);
                                Debug.WriteLine("8888888888888888888888888888888888888888888888");
                                //ExpiredLicence = true;

                            }
                        }


                    }

                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                Debug.WriteLine("MainPageViewModel - LicenceExpiryCheck - Uspjesno dovrseno");


            }
            catch (Exception ex)
            {
                Debug.WriteLine("MainPageViewModel - LicenceExpiryCheck - Catch!");

                Debug.WriteLine(ex.Message);
            }

          ActivationScreen();
          Activation_code = TrecaSreca.Get("activation_code");

        }

     private void ActivationScreen() //Prikaz aktivacijskog koda i gumba Osvježi licencu
        {


            if (ExpiredLicence == true)
            {
                ActivationVisible = true;
                string aktivacija = "LicenceNotActive";
                TrecaSreca.Set("activation_disable", aktivacija);
            }
            else if (ExpiredLicence == false)
            {
                ActivationVisible = false;
                string aktivacija = "LicenceActive";
                TrecaSreca.Set("activation_disable", aktivacija);
                CheckNasSQLSettings();
            }

        }
        
      
        public void UserNameAndID() //dohvaća username, ID i inicijale za kasnije spremanje novog spisa/kontakta - možda treba dodati kod u SPISE i KLIJENTE
        {
            // dodati da samo izvršava if (username == null || username == "")
            // ako je userName i userId prazan odn izvrši se if, ne može se dodat event u kalendaru
            // odnosno ako je filesData prazan, ne dodaje event
            string query = "SELECT * FROM employees WHERE hwid = '" + hardwareID + "';";
            Debug.WriteLine(query);
            try
            {
                Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
                //Debug.WriteLine(filesData.Length + " dužina filesData-a");
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
                    
                    TrecaSreca.Set("UserName", UserName);
                    TrecaSreca.Set("UserID", UserID);
                    Debug.WriteLine(UserID);
                    TrecaSreca.Set("UserInitials", UserInitials);
                    Debug.WriteLine("izvršio if");
                    LicenceUpdatedMessage();
                }
                else
                {
                    UserName = " ";
                    UserID = " ";
                    Debug.WriteLine("Isčito iz elsea");
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

        #region HWID 

        public static string GetMicroSeconds() //DD: čemu ovo?
        {
            double timestamp = Stopwatch.GetTimestamp();
            double microseconds = 1_000_000.0 * timestamp / Stopwatch.Frequency;
            string hashedData = ComputeSha256Hash(microseconds.ToString());

            return hashedData;

        }
        static string ComputeSha256Hash(string rawData) //DD: čemu ovo?
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }

        }

        public void GenerateHWID()
        {
            string url = "https://cc.eodvjetnik.hr/token.json?token="; 

            try
            {
                //zakomentirati nakon setanja na null
                //Microsoft.Maui.Storage.Preferences.Set("key", null);
                //Provjerava da li ima ključ spremnjen u preferences
                if (string.IsNullOrEmpty(TrecaSreca.Get("key")))
                {


                    var time = GetMicroSeconds();
                    // ----------------- platform ispod --------------
                    var device = Microsoft.Maui.Devices.DeviceInfo.Current.Platform;
                    Debug.WriteLine("url je----------------main" + url + time);


                    //Sprema u preferences index neku vrijednost iz varijable
                    TrecaSreca.Set("key", time);
                    Debug.WriteLine("spremio HWID");
                    string preferencesKey = TrecaSreca.Get("key");
                    Debug.WriteLine("HWID učitan iz preferences: " + preferencesKey);

                }
                else
                {
                    Debug.WriteLine("VAŠ KLJUČ JE VEĆ IZGENERIRAN: " + TrecaSreca.Get("key"));



                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " in MainPage");
            }
        }



        #endregion
    }
}


