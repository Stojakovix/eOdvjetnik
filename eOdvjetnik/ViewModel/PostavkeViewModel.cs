using Syncfusion.Maui.Popup;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace eOdvjetnik.ViewModel;

public class PostavkeViewModel : INotifyPropertyChanged
{
    public string HWID { get; set; }
    public string HWID64 { get; set; }

    public string LicenceType  { get; set; }
    public string dateTimeString { get; set; }
    public DateTime expiryDate { get; set; }
    public string ExpiryDate { get; set; }
    public string Activation_code { get; set; }

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

    public PostavkeViewModel()
	{
       HWID = Preferences.Get("key", null);
       LicenceType = Preferences.Get("licence_type", "");
       dateTimeString = Preferences.Get("expire_date", "");
       Activation_code = Preferences.Get("activation_code", "");
       HWID64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(HWID));
       ParseDate();
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

    }

    public void ParseDate()
    {
        try
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.Parse(dateTimeString);
            expiryDate = dateTimeOffset.Date;
            ExpiryDate = expiryDate.ToString("D");
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
}