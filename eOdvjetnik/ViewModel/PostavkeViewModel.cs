using eOdvjetnik.Model;
using eOdvjetnik.Services;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace eOdvjetnik.ViewModel;

public class PostavkeViewModel : INotifyPropertyChanged
{
    public ICommand ClearPrefrences { get; set; }

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
    public string DevicePlatform { get; set; }

    private string _PostavkeUserName;

    public string PostavkeUserName
    {
        get { return _PostavkeUserName; }
        set
        {
            if (_PostavkeUserName != value)
            {
                _PostavkeUserName = value;
                OnPropertyChanged(nameof(PostavkeUserName));
            }
        }
    }

    public string PostavkeUserID { get; set; }

    #region Boje

    public ICommand LoadColors { get; set; }
    public ICommand SaveColors { get; set; }

    private ObservableCollection<ColorItem> LocalColors;
    public ObservableCollection<ColorItem> Colors
    {
        get { return LocalColors; }
        set { LocalColors = value; }
    }

    private bool _AdminColorPopup;

    public bool AdminColorPopup
    {
        get { return _AdminColorPopup; }
        set
        {
            if (_AdminColorPopup != value)
            {
                _AdminColorPopup = value;
                OnPropertyChanged(nameof(AdminColorPopup));
            }
        }
    }
    public void GetColors()
    {
        try
        {
            if (LocalColors != null)
            {
                LocalColors.Clear();
            }
            string query = "SELECT * FROM `event_colors`";
            Debug.WriteLine(query);
            try
            {
                Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
                Debug.WriteLine(query + " u Search resultu");
                if (filesData != null)
                {
                    foreach (Dictionary<string, string> filesRow in filesData)
                    {

                        int id;
                        int.TryParse(filesRow["id"], out id);

                        LocalColors.Add(new ColorItem()
                        {
                            Id = id,
                            NazivBoje = filesRow["naziv_boje"],
                            Boja = filesRow["boja"],
                            VrstaDogadaja = filesRow["vrsta_dogadaja"],

                        });
                        Debug.WriteLine("Dohvatio boje");

                    }

                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);

        }
    }


    public void SetColors()
    {
        string licence_type = TrecaSreca.Get("licence_type");
        int numberOfCharacters = 5;
        string adminCheck = "";
        if (licence_type != null)
        {
             adminCheck = licence_type.Substring(0, Math.Min(licence_type.Length, numberOfCharacters));

        }
        Debug.WriteLine("Spremi boje - 'Admin' provjera: " + adminCheck);
        AdminColorPopup = false;

        if (adminCheck == "Admin" || adminCheck == "Trial")
        {
            AdminColorPopup = false;

            try
            {
                ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
                string disableForeignKeyChecksQuery = "SET FOREIGN_KEY_CHECKS = 0";
                externalSQLConnect.sqlQuery(disableForeignKeyChecksQuery);

                foreach (ColorItem item in Colors)
                {
                    string UpdateQuery = $"UPDATE event_colors SET vrsta_dogadaja = '{item.VrstaDogadaja}' WHERE id = {item.Id}";
                    externalSQLConnect.sqlQuery(UpdateQuery);
                    Debug.WriteLine("Update colors query: " + UpdateQuery);
                }
                GetColors();
                Application.Current.MainPage.DisplayAlert("", "Uspješno ažurirano.", "OK");

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Application.Current.MainPage.DisplayAlert("", "Povezivanje nije uspjelo.", "OK");

            }
        }
        else
        {
            Application.Current.MainPage.DisplayAlert("", "Samo administrator može mijenjati nazive kategorija.", "OK");
            AdminColorPopup = false;
        }
    }


    #endregion


    #region Zaposlenici

    ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();

    private ObservableCollection<EmployeeItem> employeeItem;
    public ObservableCollection<EmployeeItem> EmployeeItems
    {
        get { return employeeItem; }
        set
        {
            if (employeeItem != value)
            {
                employeeItem = value;
                OnPropertyChanged(nameof(EmployeeItems));
            }
        }
    }

    private bool _ZaposleniciVisible;

    public bool ZaposleniciVisible
    {
        get { return _ZaposleniciVisible; }
        set
        {
            if (_ZaposleniciVisible != value)
            {
                _ZaposleniciVisible = value;
                OnPropertyChanged(nameof(ZaposleniciVisible));
            }
        }
    }

    private bool _ZaposleniciOpen;

    public bool ZaposleniciOpen
    {
        get { return _ZaposleniciOpen; }
        set
        {
            if (_ZaposleniciOpen != value)
            {
                _ZaposleniciOpen = value;
                OnPropertyChanged(nameof(ZaposleniciOpen));
            }
        }
    }
    private string _ZaposleniciText;

    public string ZaposleniciText
    {
        get { return _ZaposleniciText; }
        set
        {
            if (_ZaposleniciText != value)
            {
                _ZaposleniciText = value;
                OnPropertyChanged(nameof(ZaposleniciText));
            }
        }
    }
    private string _LicenceType;

    public string LicenceType
    {
        get { return _LicenceType; }
        set
        {
            if (_LicenceType != value)
            {
                _LicenceType = value;
                OnPropertyChanged(nameof(LicenceType));
            }
        }
    }
    private string _ExpiryDate;

    public string ExpiryDate
    {
        get { return _ExpiryDate; }
        set
        {
            if (_ExpiryDate != value)
            {
                _ExpiryDate = value;
                OnPropertyChanged(nameof(ExpiryDate));
            }
        }
    }
    private string _HWID64;

    public string HWID64
    {
        get { return _HWID64; }
        set
        {
            if (_HWID64 != value)
            {
                _HWID64 = value;
                OnPropertyChanged(nameof(HWID64));
            }
        }
    }
    private string _Activation_code;

    public string Activation_code
    {
        get { return _Activation_code; }
        set
        {
            if (_Activation_code != value)
            {
                _Activation_code = value;
                OnPropertyChanged(nameof(Activation_code));
            }
        }
    }
    public string HWID { get; set; }
    public string DateTimeString { get; set; }
    public DateTime ExpiryDateOnly { get; set; }
    public string JsonDevicesData { get; set; }

    private DeviceDataModel _dataModel;
    public DeviceDataModel DataModel
    {
        get { return _dataModel; }
        set
        {
            _dataModel = value;
            OnPropertyChanged(nameof(DataModel));
        }
    }

    private string _NewEmployeeName;

    public string NewEmployeeName
    {
        get { return _NewEmployeeName; }
        set
        {
            if (_NewEmployeeName != value)
            {
                _NewEmployeeName = value;
                OnPropertyChanged(nameof(NewEmployeeName));
            }
        }
    }
    private string _NewEmployeeInitials;

    public string NewEmployeeInitials
    {
        get { return _NewEmployeeInitials; }
        set
        {
            if (_NewEmployeeInitials != value)
            {
                _NewEmployeeInitials = value;
                OnPropertyChanged(nameof(NewEmployeeInitials));
            }
        }
    }
    private bool _NewEmployeeEntryIncomplete;

    public bool NewEmployeeEntryIncomplete
    {
        get { return _NewEmployeeEntryIncomplete; }
        set
        {
            if (_NewEmployeeEntryIncomplete != value)
            {
                _NewEmployeeEntryIncomplete = value;
                OnPropertyChanged(nameof(NewEmployeeEntryIncomplete));
            }
        }
    }

    private bool activityRunning;

    public bool ActivityRunning
    {
        get { return activityRunning; }
        set
        {
            if (activityRunning != value)
            {
                activityRunning = value;
                OnPropertyChanged(nameof(ActivityRunning));
            }
        }
    }

    private bool activityEnabled;

    public bool ActivityEnabled
    {
        get { return activityEnabled; }
        set
        {
            if (activityEnabled != value)
            {
                activityEnabled = value;
                OnPropertyChanged(nameof(ActivityEnabled));
            }
        }
    }

    public ICommand GetAllCompanyDevices { get; set; }
    public ICommand GetAllCompanyEmployees { get; set; }
    public ICommand OpenZaposlenici { get; set; }
    public ICommand CheckHWIDs { get; set; }
    public ICommand UpdateHWIDs { get; set; }
    public ICommand OpenNewEmployee { get; set; }
    public ICommand SaveNewEmployee { get; set; }
    public ICommand BackButtonCommand { get; set; }
    public ICommand CloseNewEmployeeCommand { get; set; }

    public async void OnNoviZaposlenikClick()
    {
        await Shell.Current.GoToAsync("/NoviZaposlenik");
        Debug.WriteLine("novi zaposlenik clicked");
        NewEmployeeName = "";
        NewEmployeeInitials = "";
    }

    private void GetJsonDeviceData()
    {
        try
        {
            JsonDevicesData = TrecaSreca.Get("json_devices");
            Debug.WriteLine("JsonDevicesData = " + JsonDevicesData);

            _dataModel = new DeviceDataModel();

            if (JsonDevicesData != null)
            {
                Debug.WriteLine("JsonDevicesData = " + JsonDevicesData);
                _dataModel = JsonConvert.DeserializeObject<DeviceDataModel>(JsonDevicesData);
                DataModel = _dataModel;


                foreach (Device device in _dataModel.Devices)
                {
                    device.ConvertHwidToHwid64();

                    Debug.WriteLine($"Device ID: {device.id}");
                    Debug.WriteLine($"Device HWID: {device.hwid}");
                    Debug.WriteLine($"Description: {device.opis}");
                    Debug.WriteLine($"Base64 HWID: {device.hwid64}");
                }
                _dataModel.Devices.Insert(0, new Device { opis = "Bez uređaja" });
            }
            else
            {
                Debug.WriteLine("JsonDevicesData = null");
            }


        }
        catch (Exception ex)
        {

            Debug.WriteLine(ex.Message); ;
        }
    }

    public void GetEmployees()
    {
        try
        {
            if (employeeItem != null)
            {
                employeeItem.Clear();

            }

            string query = "SELECT id, ime, inicijali, hwid, active, type FROM employees;";


            // Debug.WriteLine(query + "u SpisiViewModelu");
            Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
            if (filesData != null)
            {
                foreach (Dictionary<string, string> filesRow in filesData)
                {
                    #region Varijable za listu
                    int id;
                    int active;
                    int type;

                    int.TryParse(filesRow["id"], out id);
                    int.TryParse(filesRow["inicijali"], out active);
                    int.TryParse(filesRow["active"], out type);

                    #endregion

                    var employee = new EmployeeItem()
                    {
                        Id = id,
                        EmployeeName = filesRow["ime"],
                        EmployeeHWID = filesRow["hwid"],
                        Initals = filesRow["inicijali"],
                        Active = active,
                        Type = type,
                        Opis = "Bez uređaja",
                    };

                    if (!string.IsNullOrEmpty(employee.EmployeeHWID))
                    {
                        employee.HasLicence = "DA";
                    }
                    else
                    {
                        employee.HasLicence = "NE";

                    }

                    employeeItem.Add(employee);
                }
                OnPropertyChanged(nameof(employeeItem));
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message + "in viewModel generate files");
        }
    }

    ////////////////////////////

    public void AssignEmployees()
    {
        foreach (EmployeeItem employeeItem in EmployeeItems)
        {
            Device companyDeviceItem = DataModel.Devices.FirstOrDefault(device => device.hwid == employeeItem.EmployeeHWID);

            if (companyDeviceItem != null)
            {
                employeeItem.Opis = companyDeviceItem.opis;
            }
        }
    }

    ////////////////////////////
    private async void ZaposleniciClicked()
    {
        string licence_type = TrecaSreca.Get("licence_type");
        int numberOfCharacters = 5;
        string adminCheck = "";
        if(licence_type != null)
        {
            adminCheck = licence_type.Substring(0, Math.Min(licence_type.Length, numberOfCharacters));
        }
        Debug.WriteLine("Zaposlenici button - 'Admin' provjera: " + adminCheck);
        if (adminCheck == "Admin" || adminCheck == "Trial")
        {
            try
            {
                string ip_sql = TrecaSreca.Get(IP_mysql);
                string user_sql = TrecaSreca.Get(USER_mysql);
                string pass_sql = TrecaSreca.Get(PASS_mysql);

                if (String.IsNullOrEmpty(ip_sql) || String.IsNullOrEmpty(user_sql) || String.IsNullOrEmpty(pass_sql))
                {
                    WeakReferenceMessenger.Default.Send(new NoSQLDetected("No SQL settings!"));
                }
                else
                {
                    
                    GetEmployees();
                    await Shell.Current.GoToAsync("///Zaposlenici");
                    Debug.WriteLine("Zaposlenici clicked");
                }

                AssignEmployees();


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("", "Samo administrator može dodavati nove zaposlenike.", "OK");

        }

    }
    private bool employeeDuplicates;

    public bool EmployeeDuplicates
    {
        get { return employeeDuplicates; }
        set
        {
            if (employeeDuplicates != value)
            {
                employeeDuplicates = value;
                OnPropertyChanged(nameof(EmployeeDuplicates));
            }
        }
    }
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
    #endregion

    #region SQL
    //Varijable za MySQL
    //
    public const string IP_mysql = "IP Adresa2";
    public const string USER_mysql = "Korisničko ime2";
    public const string PASS_mysql = "Lozinka2";
    public const string databasename_mysql = "databasename";
    public const string port = "port";
    //MySQL varijable
    public string query;

    //Save za mysql
    public ICommand SQLSaveCommand { get; set; }
    public ICommand SQLLoadCommand { get; set; }
    public ICommand SQLDeleteCommand { get; set; }

    public ICommand SQLDatabaseCommand { get; set; }
    public ICommand ServerClickCommand { get; set; }

    //Dohvaća vrijednost varijabli iz mainPagea SQL
    public string IP { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string DatabaseName { get; set; }
    public string Port { get; set; }

    #endregion

    #region Receipt footer & header saving
    public string ReceiptPDVamount { get; set; }
    public string ReceiptIBAN { get; set; }
    public string ReceiptHeaderText { get; set; }
    public string ReceiptFooterText { get; set; }
    public string ReceiptSignature { get; set; }


    public ICommand SaveReceiptCompanyInfo { get; set; }

    public void ReceiptCompanyInfo()
    {
        try
        {
            TrecaSreca.Set("receiptPDVamount", ReceiptPDVamount);
            TrecaSreca.Set("receiptIBAN", ReceiptIBAN);
            TrecaSreca.Set("receiptHeaderText", ReceiptHeaderText);
            TrecaSreca.Set("receiptFooterText", ReceiptFooterText);
            TrecaSreca.Set("receiptSignature", ReceiptSignature);
            Debug.WriteLine(ReceiptPDVamount + ReceiptIBAN + ReceiptHeaderText + ReceiptFooterText + ReceiptSignature);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);

        }
    }

    #endregion

    #region Feedback

    public string FeedbackText { get; set; }
    public ICommand SendFeedback { get; set; }
    private bool feedbackVisible;

    public bool FeedbackVisible
    {
        get { return feedbackVisible; }
        set
        {
            if (feedbackVisible != value)
            {
                feedbackVisible = value;
                OnPropertyChanged(nameof(FeedbackVisible));
            }
        }
    }
    private bool feedbackErrorVisible;

    public bool FeedbackErrorVisible
    {
        get { return feedbackErrorVisible; }
        set
        {
            if (feedbackErrorVisible != value)
            {
                feedbackErrorVisible = value;
                OnPropertyChanged(nameof(FeedbackErrorVisible));
            }
        }
    }
    public string CompanyID { get; set; }
    public string EmployeeID { get; set; }
    public string DateSentString { get; set; }

    public async void OnFeedbackClicked()
    {
        try
        {
            FeedbackVisible = false;
            FeedbackErrorVisible = false;
            string url = "https://cc.eodvjetnik.hr/eodvjetnikadmin/feedbacks/feedback?cpuid=";
            CompanyID = TrecaSreca.Get("company_id");
            EmployeeID = TrecaSreca.Get("device_type_id");
            DateSentString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string TextWithNoSpaces = ReplaceSpacesAndSectionBreaks(FeedbackText);
            string DateWithNoSpaces = ReplaceSpacesAndSectionBreaks(DateSentString);

            string feedbackURL = string.Concat(url, HWID64, "&company=", CompanyID, "&employee=", EmployeeID, "&date=", DateWithNoSpaces, "&text=", TextWithNoSpaces);

            //Debug.WriteLine(feedbackURL);
            //Debug.WriteLine(HWID64);
            //Debug.WriteLine(CompanyID);
            //Debug.WriteLine(EmployeeID);
            //Debug.WriteLine(DateSentString);
            //Debug.WriteLine(FeedbackText);

            Debug.WriteLine("Feedback -> usao u try");

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(feedbackURL);

                if (response.IsSuccessStatusCode)
                {
                    FeedbackVisible = true;
                    FeedbackNotification();
                }
                else
                {
                    FeedbackErrorVisible = true;
                    FeedbackNotification();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Feedback error:" + ex.Message);
            FeedbackErrorVisible = true;
            FeedbackNotification();
        }


    }
    public void FeedbackNotification()
    {
        if (FeedbackVisible == true)
        {
            Application.Current.MainPage.DisplayAlert("", "Feedback uspješno poslan.", "OK");

        }
        else if (FeedbackErrorVisible == true)
        {
            Application.Current.MainPage.DisplayAlert("", "Povezivanje nije uspjelo.", "OK");

        }
    }

    public static string ReplaceSpacesAndSectionBreaks(string text)
    {
        return text.Replace(Environment.NewLine, "%20").Replace(" ", "%20").Replace("\r\n", "%20").Replace("\n", "%20").Replace("\t", "%20").Replace("\r", "%20").Replace("š", "s").Replace("đ", "dj").Replace("ž", "z").Replace("č", "c").Replace("ć", "c").Replace("ž", "z");
    }

    #endregion

    public PostavkeViewModel()
    {
        try
        {
            ServiceMode = false;
            string serviceModeCheck = TrecaSreca.Get("service_mode");
            if (serviceModeCheck == "True")
            {
                ServiceMode = true;
            }
            DevicePlatform = TrecaSreca.Get("vrsta_platforme");
            LoadColors = new Command(GetColors);
            SaveColors = new Command(SetColors);
            AdminColorPopup = false;
            CloseNewEmployeeCommand = new Command(CloseNewEmployee);
            ClearPrefrences = new Command(DeletePreferences);

            Colors = new ObservableCollection<ColorItem>();
            WeakReferenceMessenger.Default.Register<LicenceUpdated>(this, LicenceUpdatedReceived);


            #region Devic&Licence
            ZaposleniciVisible = false;
            ZaposleniciOpen = false;

            GetAllCompanyDevices = new Command(GetJsonDeviceData);
            GetAllCompanyEmployees = new Command(GetEmployees);
            OpenZaposlenici = new Command(ZaposleniciClicked);
            SaveReceiptCompanyInfo = new Command(ReceiptCompanyInfo);
            CheckHWIDs = new Command(CheckDuplicates);
            UpdateHWIDs = new Command(UpdateEmployeeHWID);
            OpenNewEmployee = new Command(OnNoviZaposlenikClick);
            SaveNewEmployee = new Command(AddNewEmployee);
            BackButtonCommand = new Command(OnBackButtonClick);

            ReceiptPDVamount = TrecaSreca.Get("receiptPDVamount");
            ReceiptIBAN = TrecaSreca.Get("receiptIBAN");
            ReceiptHeaderText = TrecaSreca.Get("receiptHeaderText");
            ReceiptFooterText = TrecaSreca.Get("receiptFooterText");

            HWID = TrecaSreca.Get("key");
            LicenceType = TrecaSreca.Get("licence_type");
            DateTimeString = TrecaSreca.Get("expire_date");
            Activation_code = TrecaSreca.Get("activation_code");
            try
            {
                ParseDate();
                FetchCompanyDevices();
                employeeItem = new ObservableCollection<EmployeeItem>();
                GetJsonDeviceData();
                HWID64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(HWID));
                GetEmployees();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            #endregion
            #region NAS komande
            SaveCommandNAS = new Command(OnSaveClickedNas);
            //LoadCommandNAS = new Command(OnLoadClickedNas);
            //DeleteCommandNAS = new Command(OnDeleteClickedNas);
            #endregion

            #region SQL komande
            SQLSaveCommand = new Command(OnSaveClickedMySQL);
            //SQLLoadCommand = new Command(OnLoadClickedMySQL);
            //SQLDeleteCommand = new Command(OnDeleteClickedMySQL);
            ServerClickCommand = new Command(OnServerClick);
            SQLDatabaseCommand = new Command(OnDatabaseClick);
            #endregion

            #region NAS Varijable
            IPNas = TrecaSreca.Get(IP_nas);
            UserNas = TrecaSreca.Get(USER_nas);
            PassNas = TrecaSreca.Get(PASS_nas);
            Folder = TrecaSreca.Get(FOLDER_nas);
            SubFolder = TrecaSreca.Get(SUBFOLDER_nas);

            #endregion

            #region SQL varijable

            IP = TrecaSreca.Get(IP_mysql);
            UserName = TrecaSreca.Get(USER_mysql);
            Password = TrecaSreca.Get(PASS_mysql);
            DatabaseName = TrecaSreca.Get(databasename_mysql);
            Port = TrecaSreca.Get(port);
            if(string.IsNullOrEmpty(Port))
            {
                Port = "3306";
            }

            activityEnabled = false;
            activityRunning = false;

            #endregion

            #region Feedback
            FeedbackVisible = false;
            FeedbackErrorVisible = false;
            SendFeedback = new Command(OnFeedbackClicked);
            #endregion

            PostavkeUserName = TrecaSreca.Get("UserName");
            PostavkeUserID = TrecaSreca.Get("UserID");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);

        }
    }


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
            TrecaSreca.Set(IP_nas, ip_nas);
            TrecaSreca.Set(PASS_nas, pass_nas);
            TrecaSreca.Set(USER_nas, user_nas);
            TrecaSreca.Set(FOLDER_nas, folder);
            TrecaSreca.Set(SUBFOLDER_nas, subFolder);
            Application.Current.MainPage.DisplayAlert("", "NAS postavke su uspješno spremljene.", "OK");

            //Debug.WriteLine("Nas saved " + Preferences.Default);
            Debug.WriteLine("KLINUTO NA SAVE U NAS POSTAVKAMA");


        }

        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message + "In PostavkeViewModel NAS");
            Application.Current.MainPage.DisplayAlert("", "Došlo je do pogreške prilikom spremanja.", "OK");

        }
    }
    //private void OnLoadClickedNas()
    //{
    //    try
    //    {
    //        var ipmynas = TrecaSreca.Get(IP_nas, IPNas);
    //        var usermynas = TrecaSreca.Get(USER_nas, UserNas);
    //        var passmynas = TrecaSreca.Get(PASS_nas, PassNas);
    //        var folder = TrecaSreca.Get(FOLDER_nas, Folder);
    //        var subfolder = TrecaSreca.Get(SUBFOLDER_nas, SubFolder);

    //        Debug.WriteLine("Load uspješan");
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.WriteLine(ex.Message + "in PostavkeViewModel OnLoadNAS");
    //    }
    //}
    //private void OnDeleteClickedNas()
    //{
    //    try
    //    {
    //        if (String.IsNullOrEmpty(IPNas))
    //        {
    //            ShowAlert("Alert", "Data is already deleted.");
    //        }
    //        else
    //        {
    //            TrecaSreca.DeletePreference(IPNas);
    //            TrecaSreca.DeletePreference(UserNas);
    //            TrecaSreca.DeletePreference(PassNas);
    //            TrecaSreca.DeletePreference(Folder);
    //            TrecaSreca.DeletePreference(SubFolder);

    //            Debug.WriteLine("Succesfully deleted the values");
    //        }
    //        IPNas = "";
    //        UserNas = "";
    //        PassNas = "";
    //        Folder = "";
    //        SubFolder = "";
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.WriteLine(ex.Message);
    //    }
    //}

    #endregion

    #region SQL Funkcije

    private async void OnDatabaseClick()
    {
        try
        {
            ActivityEnabled = true;
            ActivityRunning = true;

            Debug.WriteLine(activityEnabled.ToString());

            string[] arguments = new string[] { "database" };
            await Task.Delay(1500);
            externalSQLConnect.createDatabase(arguments);
            
            externalSQLConnect.ExecuteSqlFile();
            

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            await Application.Current.MainPage.DisplayAlert("", "Greška u izradi baze, Molimo kontaktirajte administratora", "OK");
            return;
        }
        finally
        {
            ActivityRunning = false;
            ActivityEnabled = false;
            await Application.Current.MainPage.DisplayAlert("", "Baza uspješno instalirana.", "OK");
            Debug.WriteLine(activityEnabled.ToString());
            WeakReferenceMessenger.Default.Send(new RestartNaplata("RestartNaplataVM!"));


        }

    }
    private void OnSaveClickedMySQL()
    {
        try
        {
            string ip = IP;
            string password = Password;
            string userName = UserName;
            string databaseName = DatabaseName;
            string portId = Port;


            TrecaSreca.Set(IP_mysql, IP);
            TrecaSreca.Set(USER_mysql, UserName);
            TrecaSreca.Set(PASS_mysql, Password);
            TrecaSreca.Set(databasename_mysql, DatabaseName);
            TrecaSreca.Set(port, Port);

            Debug.WriteLine("Saved");
            Debug.WriteLine(UserName + " " + Password + " " + Port);
            Debug.WriteLine("KLINUTO NA SAVE U SQL POSTAVKAMA");

            if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(portId))
            {
                Application.Current.MainPage.DisplayAlert("", "Došlo je do pogreške prilikom spremanja.", "OK");
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("", "SQL postavke su uspješno spremljene.", "OK");
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Application.Current.MainPage.DisplayAlert("", "Došlo je do pogreške prilikom spremanja.", "OK");
            return;
        }
        finally
        {
            string sqlip = TrecaSreca.Get(IP_mysql);
            string sqlpassword = TrecaSreca.Get(PASS_mysql);
            string sqluserName = TrecaSreca.Get(USER_mysql);
            string sqldatabaseName = TrecaSreca.Get(databasename_mysql);
            string sqlportId = TrecaSreca.Get(port);
            WeakReferenceMessenger.Default.Send(new RestartNaplata("RestartNaplataVM!"));


        }
    }
    //private void OnLoadClickedMySQL()
    //{
    //    try
    //    {
    //        var ipmysql = Preferences.Get(IP, IP_mysql);
    //        var usermysql = Preferences.Get(UserName, USER_mysql);
    //        var passmysql = Preferences.Get(Password, PASS_mysql);
    //        var databaseNamemysql = Preferences.Get(DatabaseName, databasename_mysql);

    //        Debug.WriteLine("Load uspješan");
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.WriteLine(ex.Message + "in MainPageViewModel OnLoadSQL");
    //    }
    //}

    //private void OnDeleteClickedMySQL()
    //{
    //    if (String.IsNullOrEmpty(IP))
    //    {
    //        ShowAlert("Alert", "Data is already deleted.");
    //    }
    //    else
    //    {
    //        Preferences.Remove(IP);
    //        Preferences.Remove(UserName);
    //        Preferences.Remove(Password);
    //        Preferences.Remove(databasename_mysql);

    //    }

    //    IP = "";
    //    UserName = "";
    //    Password = "";
    //    DatabaseName = "";
    //}

    private async void OnServerClick()
    {
        string websiteUrl = "https://dev.mysql.com/downloads/mysql/"; // Replace with the URL you want to open

        try
        {
            await Launcher.OpenAsync(websiteUrl);
        }
        catch (Exception ex)
        {
            // Handle any exceptions that may occur, such as if the URL is invalid
            Console.WriteLine("Error opening website: " + ex.Message);
        }
    }


    #endregion

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

    private async void ShowAlert(string title, string message)
    {
        await Application.Current.MainPage.DisplayAlert(title, message, "OK");
    }

    public void ParseDate()
    {
        try
        {
            DateTimeString = TrecaSreca.Get("expire_date");
            DateTimeOffset dateTimeOffset = DateTimeOffset.Parse(DateTimeString);
            ExpiryDateOnly = dateTimeOffset.Date;
            ExpiryDate = ExpiryDateOnly.ToString("D");
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Date parsing error:" + ex.Message);
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public async void OnBackButtonClick()
    {
        await Shell.Current.GoToAsync("///Postavke");
    }

    public async void FetchCompanyDevices()
    {
        ZaposleniciVisible = false;
        ZaposleniciOpen = false;


        Debug.WriteLine("PostavkeModel - > FetchCompanyDevices");
        string string1 = "https://cc.eodvjetnik.hr/eodvjetnikadmin/devices/getAll?cpuid=";
        string string2 = TrecaSreca.Get("key");
        string activationURL = string.Concat(string1, string2);
        Debug.WriteLine("PostavkeModel - > FetchCompanyDevices - URL: " + activationURL);
        try
        {
            Debug.WriteLine("PostavkeModel - > FetchCompanyDevices -> Try");

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(activationURL);
                Debug.WriteLine("PostavkeModel - > FetchCompanyDevices -> Received a response");

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("PostavkeModel - > FetchCompanyDevices -> Received data");

                    string jsonContent = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("PostavkeModel - > FetchCompanyDevices -> Json string: " + content);
                    string json_devices = content.ToString();
                    TrecaSreca.Set("json_devices", json_devices);

                }
                else
                {
                    // 
                }
            }
            Debug.WriteLine("PostavkeModel - > FetchCompanyDevices -> Uspjesno dovrseno");

        }
        catch (Exception ex)
        {
            Debug.WriteLine("FetchCompanyDevices error: " + ex.Message);
        }
    }


    public void CheckDuplicates()
    {
        try
        {
            ZaposleniciVisible = false;
            ZaposleniciOpen = false;
            EmployeeDuplicates = false;

            if (EmployeeItems == null || EmployeeItems.Count <= 1)
                return;

            var duplicateHWIDGroups = EmployeeItems
                .Where(e => !string.IsNullOrEmpty(e.EmployeeHWID))
                .GroupBy(e => e.EmployeeHWID)
                .Where(g => g.Count() > 1)
                .ToList();

            if (duplicateHWIDGroups.Any())
            {
                //Debug.WriteLine("Više korisnika ima isti HWID:");
                foreach (var group in duplicateHWIDGroups)
                {
                    var employeeNames = group.Select(e => e.EmployeeName).ToList();
                    var employeeHWID = group.Key;

                    //Debug.WriteLine($"Korisnici povezani s istim uređajem: {string.Join(", ", employeeNames)}");
                }
                ZaposleniciVisible = true;
                ZaposleniciOpen = true;
                EmployeeDuplicates = true;

            }
            else
            {
                ZaposleniciVisible = false;
                ZaposleniciOpen = false;
                EmployeeDuplicates = false;

                //Debug.WriteLine("Nema duplih HWID.");
            }
            GetDuplicateEmployeesString();
            Debug.WriteLine(GetDuplicateEmployeesString());
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);

        }
    }

    public string GetDuplicateEmployeesString()
    {
        try
        {
            if (EmployeeItems == null || EmployeeItems.Count <= 1)
                return string.Empty;

            var duplicateHWIDGroups = EmployeeItems
                .Where(e => !string.IsNullOrEmpty(e.EmployeeHWID))
                .GroupBy(e => e.EmployeeHWID)
                .Where(g => g.Count() > 1)
                .ToList();

            if (duplicateHWIDGroups.Any())
            {
                var duplicatesStringBuilder = new StringBuilder();
                //duplicatesStringBuilder.AppendLine("\n");

                foreach (var group in duplicateHWIDGroups)
                {
                    var employeeNames = group.Select(e => e.EmployeeName);
                    var employeeHWID = group.Key;

                    duplicatesStringBuilder.AppendLine($" {string.Join(", ", employeeNames)}\n");
                }
                string zaposleniciPopupString = duplicatesStringBuilder.ToString();
                ZaposleniciText = zaposleniciPopupString;

                return duplicatesStringBuilder.ToString();

            }
            else
            {
                ZaposleniciVisible = false;
                ZaposleniciOpen = false;

                return "Nema duplih HWID.";
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null;

        }
    }

    public void UpdateEmployeeHWID()
    {
        try
        {
            CheckDuplicates();

            if (EmployeeDuplicates == false)
            {
                try
                {
                    ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
                    string disableForeignKeyChecksQuery = "SET FOREIGN_KEY_CHECKS = 0";
                    externalSQLConnect.sqlQuery(disableForeignKeyChecksQuery);

                    foreach (EmployeeItem item in EmployeeItems)
                    {
                        string UpdateQuery = $"UPDATE employees SET hwid = '{item.EmployeeHWID}' WHERE id = {item.Id}";
                        externalSQLConnect.sqlQuery(UpdateQuery);
                        Debug.WriteLine("Update contact query: " + UpdateQuery);
                    }
                    GetEmployees();
                    Application.Current.MainPage.DisplayAlert("", "Uspješno ažurirano.", "OK");
                    AssignEmployees();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Application.Current.MainPage.DisplayAlert("", "Povezivanje nije uspjelo.", "OK");

                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Application.Current.MainPage.DisplayAlert("", "Povezivanje nije uspjelo.", "OK");

        }
    }

    public void EntryIncompleteCheck()
    {
        try
        {
            NewEmployeeInitials = NewEmployeeInitials.Replace(" ", "");
            if (NewEmployeeName == "")
            {
                NewEmployeeEntryIncomplete = true;
                Application.Current.MainPage.DisplayAlert("", "Potrebno je unijeti ime.", "OK");

            }
            else if (NewEmployeeInitials == "")
            {
                NewEmployeeEntryIncomplete = true;
                Application.Current.MainPage.DisplayAlert("", "Potrebno je unijeti inicijale", "OK");


            }
            else if (NewEmployeeInitials != "")
            {
                DuplicateInitialsCheck();
            }


            else
            {
                string newInitialsUpper = NewEmployeeInitials.ToUpper();
                NewEmployeeInitials = newInitialsUpper;
                NewEmployeeEntryIncomplete = false;

            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);

        }

    }
    public async void AddNewEmployee()
    {
        try
        {
            EntryIncompleteCheck();
            Debug.WriteLine(NewEmployeeEntryIncomplete.ToString());

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }


        if (NewEmployeeEntryIncomplete == false)
        {
            try
            {

                ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
                string disableForeignKeyChecksQuery = "SET FOREIGN_KEY_CHECKS = 0";
                externalSQLConnect.sqlQuery(disableForeignKeyChecksQuery);

                string query = $"INSERT INTO employees (ime, inicijali) " +
                               $"VALUES ('{NewEmployeeName}', '{NewEmployeeInitials}')";
                externalSQLConnect.sqlQuery(query);

                if (DevicePlatform == "MacCatalyst")
                {
                    await Shell.Current.GoToAsync("///LoadingPageZaposlenici");


                }
                else
                {
                    await Shell.Current.GoToAsync("//Zaposlenici");

                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        GetEmployees();
        AssignEmployees();


    }
    private void LicenceUpdatedReceived(object recipient, LicenceUpdated message)
    {
        try
        {
            PostavkeUserName = TrecaSreca.Get("UserName");
            PostavkeUserID = TrecaSreca.Get("UserID");
            Debug.WriteLine("PostavkeViewModel - LicenceUpdatedReceived -> Username: " + PostavkeUserName + " UserID: " + PostavkeUserID);
            GetJsonDeviceData();
            Activation_code = TrecaSreca.Get("activation_code");
            LicenceType = TrecaSreca.Get("licence_type");
            HWID = TrecaSreca.Get("key");
            HWID64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(HWID));
            ParseDate();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);

        }
    }

    public void DuplicateInitialsCheck()
    {
        try
        {
            string newInitialsLower = NewEmployeeInitials.ToLower();

            string newInitialsUpper = NewEmployeeInitials.ToUpper();

            foreach (var employeeItem in EmployeeItems)
            {
                if (employeeItem.Initals == newInitialsLower)
                {
                    string duplicateID = employeeItem.EmployeeName;
                    Application.Current.MainPage.DisplayAlert("", "Inicijali su već dodijeljeni korisniku " + duplicateID, "OK");
                    NewEmployeeEntryIncomplete = true;
                }
            }
            foreach (var employeeItem in EmployeeItems)
            {
                if (employeeItem.Initals == newInitialsUpper)
                {
                    string duplicateID = employeeItem.EmployeeName;
                    Application.Current.MainPage.DisplayAlert("", "Inicijali su već dodijeljeni korisniku " + duplicateID, "OK");
                    NewEmployeeEntryIncomplete = true;
                }
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);

        }
    }

    public async void CloseNewEmployee()
    {


        if (DevicePlatform == "MacCatalyst")
        {
            await Shell.Current.GoToAsync("///LoadingPageZaposlenici");

        }
        else
        {
            await Shell.Current.GoToAsync("//Zaposlenici");

        }
    }


}
