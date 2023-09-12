using eOdvjetnik.Model;
using eOdvjetnik.Services;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using System.Xml.Linq;
using Utilities;
using CommunityToolkit.Mvvm.Messaging;


namespace eOdvjetnik.ViewModel;

public class PostavkeViewModel : INotifyPropertyChanged
{

    private Navigacija navigacija;
    public string PostavkeUserName { get; set; }
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
        string licence_type = Preferences.Get("licence_type", "");
        int numberOfCharacters = 5;
        string adminCheck = licence_type.Substring(0, Math.Min(licence_type.Length, numberOfCharacters));
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
    public string HWID { get; set; }
    public string HWID64 { get; set; }
    public string LicenceType { get; set; }
    public string DateTimeString { get; set; }
    public DateTime ExpiryDateOnly { get; set; }
    public string ExpiryDate { get; set; }
    public string Activation_code { get; set; }
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

    public ICommand GetAllCompanyDevices { get; set; }
    public ICommand GetAllCompanyEmployees { get; set; }
    public ICommand OpenZaposlenici { get; set; }
    public ICommand CheckHWIDs { get; set; }
    public ICommand UpdateHWIDs { get; set; }
    public ICommand OpenNewEmployee { get; set; }
    public ICommand SaveNewEmployee { get; set; }
    public ICommand BackButtonCommand { get; set; }

    public ICommand PocetnaClick => navigacija.PocetnaClick;
    public ICommand KalendarClick => navigacija.KalendarClick;
    public ICommand SpisiClick => navigacija.SpisiClick;
    public ICommand TarifaClick => navigacija.TarifaClick;
    public ICommand DokumentiClick => navigacija.DokumentiClick;
    public ICommand KontaktiClick => navigacija.KontaktiClick;
    public ICommand KorisnickaClick => navigacija.KorisnickaPodrskaClick;
    public ICommand PostavkeClick => navigacija.PostavkeClick;

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
            JsonDevicesData = Preferences.Get("json_devices", null);
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
                    Debug.WriteLine($"Company ID: {device.hwid}");
                    Debug.WriteLine($"Description: {device.opis}");
                    Debug.WriteLine($"Base64 Company ID: {device.hwid64}");
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

    private async void ZaposleniciClicked()
    {
        string licence_type = Preferences.Get("licence_type", "");
        int numberOfCharacters = 5;
        string adminCheck = licence_type.Substring(0, Math.Min(licence_type.Length, numberOfCharacters));
        Debug.WriteLine("Zaposlenici button - 'Admin' provjera: " + adminCheck);
        if (adminCheck == "Admin")
        {
            try
            {
                await Shell.Current.GoToAsync("///Zaposlenici");
                Debug.WriteLine("Zaposlenici clicked");

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
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
        Preferences.Set("receiptPDVamount", ReceiptPDVamount);
        Preferences.Set("receiptIBAN", ReceiptIBAN);
        Preferences.Set("receiptHeaderText", ReceiptHeaderText);
        Preferences.Set("receiptFooterText", ReceiptFooterText);
        Preferences.Set("receiptSignature", ReceiptSignature);
        Debug.WriteLine(ReceiptPDVamount + ReceiptIBAN + ReceiptHeaderText + ReceiptFooterText + ReceiptSignature);
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
        FeedbackVisible = false;
        FeedbackErrorVisible = false;
        string url = "https://cc.eodvjetnik.hr/eodvjetnikadmin/feedbacks/feedback?cpuid=";
        CompanyID = Preferences.Get("company_id", "");
        EmployeeID = Preferences.Get("device_type_id", "");
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
        try
        {
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

        navigacija = new Navigacija();
        LoadColors = new Command(GetColors);
        SaveColors = new Command(SetColors);
        AdminColorPopup = false;

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

        ReceiptPDVamount = Preferences.Get("receiptPDVamount", "");
        ReceiptIBAN = Preferences.Get("receiptIBAN", "");
        ReceiptHeaderText = Preferences.Get("receiptHeaderText", "");
        ReceiptFooterText = Preferences.Get("receiptFooterText", "");

        HWID = Preferences.Get("key", null);
        LicenceType = Preferences.Get("licence_type", "");
        DateTimeString = Preferences.Get("expire_date", "");
        Activation_code = Preferences.Get("activation_code", "");
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
        LoadCommandNAS = new Command(OnLoadClickedNas);
        DeleteCommandNAS = new Command(OnDeleteClickedNas);
        #endregion

        #region SQL komande
        SQLSaveCommand = new Command(OnSaveClickedMySQL);
        SQLLoadCommand = new Command(OnLoadClickedMySQL);
        SQLDeleteCommand = new Command(OnDeleteClickedMySQL);
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

        #region Feedback
        FeedbackVisible = false;
        FeedbackErrorVisible = false;
        SendFeedback = new Command(OnFeedbackClicked);
        #endregion

        PostavkeUserName =  Preferences.Get("UserName", "");
        PostavkeUserID =  Preferences.Get("UserID", "");
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
            Preferences.Set(IP_nas, ip_nas);
            Preferences.Set(PASS_nas, pass_nas);
            Preferences.Set(USER_nas, user_nas);
            Preferences.Set(FOLDER_nas, folder);
            Preferences.Set(SUBFOLDER_nas, subFolder);

            Debug.WriteLine("Nas saved " + Preferences.Default);
            Debug.WriteLine("KLINUTO NA SAVE U NAS POSTAVKAMA");

        }

        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message + "In PostavkeViewModel NAS");
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
            Debug.WriteLine(ex.Message + "in PostavkeViewModel OnLoadNAS");
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
            Debug.WriteLine("KLINUTO NA SAVE U NAS POSTAVKAMA");

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


    #endregion



    private async void ShowAlert(string title, string message)
    {
        await Application.Current.MainPage.DisplayAlert(title, message, "OK");
    }

    public void ParseDate()
    {
        try
        {
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
        string string2 = Preferences.Get("key", null);
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
                    Preferences.Set("json_devices", json_devices);

                }
                else
                {
                    // 
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("FetchCompanyDevices error: " + ex.Message);
        }
    }


    public void CheckDuplicates()
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

    public string GetDuplicateEmployeesString()
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
        else
        {
            NewEmployeeEntryIncomplete = false;

        }


    }
    public async void AddNewEmployee()
    {
        try
        {
            EntryIncompleteCheck();
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

                await Shell.Current.GoToAsync("//Zaposlenici");
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        GetEmployees();

    }
    private void LicenceUpdatedReceived(object recipient, LicenceUpdated message)
    {
        Debug.WriteLine("Refresh postavki nakon ažurianja licence");
        PostavkeUserName = Preferences.Get("UserName", "");
        PostavkeUserID = Preferences.Get("UserID", "");
    }



}
