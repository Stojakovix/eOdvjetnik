using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Windows.Input;
using System.Xml.Linq;
using eOdvjetnik.Models;
using eOdvjetnik.Services;
using Vanara.PInvoke;

namespace eOdvjetnik.ViewModel;

public class KlijentiViewModel : INotifyPropertyChanged
{

    ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();

    private ObservableCollection<ContactItem> contacts;
    public ObservableCollection<ContactItem> Contacts
    {
        get { return contacts; }
        set { contacts = value; }
    }
    private string _searchText;
    public string SearchText
    {
        get { return _searchText; }
        set
        {
            if (_searchText != value)
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }
    }
    private ICommand onNoviKlijentClick;
    public ICommand OnNoviKlijentClick
    {
        get
        {
            if (onNoviKlijentClick == null)
            {
                onNoviKlijentClick = new Command(OnButtonClick);
            }
            return onNoviKlijentClick;
        }
    }


    private ICommand searchCommand;
    public ICommand SearchCommand
    {
        get
        {
            if (searchCommand == null)
            {
                searchCommand = new Command(GenerateSearchResults);
            }
            return searchCommand;
        }
    }
    public bool _NoQueryResult { get; set; }
    public bool NoQueryResult
    {
        get { return _NoQueryResult; }
        set
        {
            if (_NoQueryResult != value)
            {
                _NoQueryResult = value;
                OnPropertyChanged(nameof(NoQueryResult));
            }
        }
    }
    public bool _NoSQLreply { get; set; }
    public bool NoSQLreply
    {
        get { return _NoSQLreply; }
        set
        {
            if (_NoSQLreply != value)
            {
                _NoSQLreply = value;
                OnPropertyChanged(nameof(NoSQLreply));
            }
        }
    }
    public void GenerateFiles()
    {
        NoQueryResult = false;
        Debug.WriteLine(contacts);
        try
        {
            if (contacts != null)
            {
                contacts.Clear();
            }
            string query = "SELECT * FROM `contacts` ORDER by id desc limit 30;";
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
                        int pravnaInt;
                        DateTime datum_rodenja;

                        int.TryParse(filesRow["id"], out id);
                        int.TryParse(filesRow["pravna"], out pravnaInt);
                        DateTime.TryParse(filesRow["datum_rodenja"], out datum_rodenja);

                        string pravnaString = pravnaInt == 1 ? "Da" : "Ne";

                        contacts.Add(new ContactItem()
                        {
                            Id = id,
                            Ime = filesRow["ime"],
                            OIB = filesRow["OIB"],
                            Datum_rodenja = datum_rodenja,
                            Adresa = filesRow["adresa"],
                            Boraviste = filesRow["boraviste"],
                            Telefon = filesRow["telefon"],
                            Fax = filesRow["fax"],
                            Mobitel = filesRow["mobitel"],
                            Email = filesRow["email"],
                            Ostalo = filesRow["ostalo"],
                            Drzava = filesRow["drzava"],
                            PravnaInt = pravnaInt,
                            PravnaString = pravnaString

                        });

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
    public void GenerateSearchResults()
    {
        NoQueryResult = false;
        NoSQLreply = false;
        Debug.WriteLine(contacts);
        try
        {
            if (contacts != null)
            {
                NoQueryResult = false;
                contacts.Clear();
            }
            string query = "SELECT * FROM `contacts` WHERE LOWER(ime) LIKE LOWER('%" + SearchText + "%') OR OIB LIKE '%" + SearchText + "%'";
            Debug.WriteLine(query);
            try
            {
                Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
                Debug.WriteLine(query + " u Search resultu");
                if (filesData != null && filesData.Length > 0)
                {
                    foreach (Dictionary<string, string> filesRow in filesData)
                    {

                        int id;
                        int pravnaInt;
                        DateTime datum_rodenja;

                        int.TryParse(filesRow["id"], out id);
                        int.TryParse(filesRow["pravna"], out pravnaInt);
                        DateTime.TryParse(filesRow["datum_rodenja"], out datum_rodenja);

                        string pravnaString = pravnaInt == 1 ? "Da" : "Ne";

                        contacts.Add(new ContactItem()
                        {
                            Id = id,
                            Ime = filesRow["ime"],
                            OIB = filesRow["OIB"],
                            Datum_rodenja = datum_rodenja,
                            Adresa = filesRow["adresa"],
                            Boraviste = filesRow["boraviste"],
                            Telefon = filesRow["telefon"],
                            Fax = filesRow["fax"],
                            Mobitel = filesRow["mobitel"],
                            Email = filesRow["email"],
                            Ostalo = filesRow["ostalo"],
                            Drzava = filesRow["drzava"],
                            PravnaInt = pravnaInt,
                            PravnaString = pravnaString
                        });
                    }
                }
                else
                {
                    NoQueryResult = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                NoSQLreply = true;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
    #region Selected Client

    private string _ClientName;
    public string ClientName
    {
        get { return _ClientName; }
        set
        {
            if (_ClientName != value)
            {
                _ClientName = value;
                OnPropertyChanged(nameof(ClientName));
            }
        }
    }

    private string _ClientOIB;
    public string ClientOIB
    {
        get { return _ClientOIB; }
        set
        {
            if (_ClientOIB != value)
            {
                _ClientOIB = value;
                OnPropertyChanged(nameof(ClientOIB));
            }
        }
    }

    private string _ClientAddress;
    public string ClientAddress
    {
        get { return _ClientAddress; }
        set
        {
            if (_ClientAddress != value)
            {
                _ClientAddress = value;
                OnPropertyChanged(nameof(ClientAddress));
            }
        }
    }

    private string _ClientBirthDate;
    public string ClientBirthDate
    {
        get { return _ClientBirthDate; }
        set
        {
            if (_ClientBirthDate != value)
            {
                _ClientBirthDate = value;
                OnPropertyChanged(nameof(ClientBirthDate));
            }
        }
    }
    private string _ClientResidence;
    public string ClientResidence
    {
        get { return _ClientResidence; }
        set
        {
            if (_ClientResidence != value)
            {
                _ClientResidence = value;
                OnPropertyChanged(nameof(ClientResidence));
            }
        }
    }
    private string _ClientPhone;
    public string ClientPhone
    {
        get { return _ClientResidence; }
        set
        {
            if (_ClientPhone != value)
            {
                _ClientPhone = value;
                OnPropertyChanged(nameof(ClientPhone));
            }
        }
    }
    private string _ClinetFax;
    public string ClinetFax
    {
        get { return _ClinetFax; }
        set
        {
            if (_ClinetFax != value)
            {
                _ClinetFax = value;
                OnPropertyChanged(nameof(ClinetFax));
            }
        }
    }
    private string _ClientMobile;
    public string ClientMobile
    {
        get { return _ClientMobile; }
        set
        {
            if (_ClientMobile != value)
            {
                _ClientMobile = value;
                OnPropertyChanged(nameof(ClientMobile));
            }
        }
    }
    private string _ClientEmail;

    public string ClientEmail
    {
        get { return _ClientEmail; }
        set
        {
            if (_ClientEmail != value)
            {
                _ClientEmail = value;
                OnPropertyChanged(nameof(ClientEmail));
            }
        }
    }
    private string _ClientOther;

    public string ClientOther
    {
        get { return _ClientOther; }
        set
        {
            if (_ClientOther != value)
            {
                _ClientOther = value;
                OnPropertyChanged(nameof(ClientOther));
            }
        }
    }
    private string _ClientCountry;

    public string ClientCountry
    {
        get { return _ClientCountry; }
        set
        {
            if (_ClientCountry != value)
            {
                _ClientCountry = value;
                OnPropertyChanged(nameof(ClientCountry));
            }
        }
    }
    private string _ClientLegalPerson;

    public string ClientLegalPerson
    {
        get { return _ClientLegalPerson; }
        set
        {
            if (_ClientLegalPerson != value)
            {
                _ClientLegalPerson = value;
                OnPropertyChanged(nameof(ClientLegalPerson));
            }
        }
    }

    #endregion

    public ICommand OnReciptClickCommand { get; set; }
    public ICommand RefreshContacts { get; set; }


    public KlijentiViewModel()
    {
        Contacts = new ObservableCollection<ContactItem>();
        GenerateFiles();
        ClientName = Preferences.Get("SelectedName", "");
        ClientOIB = Preferences.Get("SelectedOIB", "");
        ClientAddress = Preferences.Get("SelectedAddress", "");
        ClientBirthDate = Preferences.Get("SelectedBirthDate", "");
        ClientResidence = Preferences.Get("SelectedRsidence", "");
        ClientPhone = Preferences.Get("SelectedPhone", "");
        ClinetFax = Preferences.Get("SelectedFax", "");
        ClientMobile = Preferences.Get("SelectedMobile", "");
        ClientEmail = Preferences.Get("SelectedEmail", "");
        ClientOther = Preferences.Get("SelectedOther", "");
        ClientCountry = Preferences.Get("SelectedCountry", "");
        try
        {
            ClientLegalPerson = Preferences.Get("SelectedLegalPerson", "");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            
        }
        OnReciptClickCommand = new Command(OpenRecipt);
        RefreshContacts = new Command(GenerateFiles);

        var timer = Application.Current.Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromMilliseconds(200);
        timer.Tick += (s, e) => Refresh();
        timer.Start();

    }

    public async void OnButtonClick()
    {
        await Shell.Current.GoToAsync("/NoviKlijent");
        Debug.WriteLine("novi spis clicked");
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private async void OpenRecipt()
    {
        try
        {
            await Shell.Current.GoToAsync("///Naplata");
            Debug.WriteLine("Racun clicked");

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

    }
    void Refresh()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            ClientName = Preferences.Get("SelectedName", "");
            ClientOIB = Preferences.Get("SelectedOIB", "");
            ClientAddress = Preferences.Get("SelectedAddress", "");
            ClientBirthDate = Preferences.Get("SelectedBirthDate", "");
            ClientResidence = Preferences.Get("SelectedRsidence", "");
            ClientPhone = Preferences.Get("SelectedPhone", "");
            ClinetFax = Preferences.Get("SelectedFax", "");
            ClientMobile = Preferences.Get("SelectedMobile", "");
            ClientEmail = Preferences.Get("SelectedEmail", "");
            ClientOther = Preferences.Get("SelectedOther", "");
            ClientCountry = Preferences.Get("SelectedCountry", "");
            try
            {
                ClientLegalPerson = Preferences.Get("SelectedLegalPerson", "");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
        }
        );
    }
}