using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Windows.Input;
using System.Xml.Linq;
using eOdvjetnik.Models;
using eOdvjetnik.Services;

using CommunityToolkit.Mvvm.Messaging;
using System.Globalization;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Web;

namespace eOdvjetnik.ViewModel;

public class KlijentiViewModel : INotifyPropertyChanged
{

    private void RočišnikMenuItem_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine("MainPageViewModel - > ActivationLoop");

    }
    ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();

    private ObservableCollection<ContactItem> contacts;
    public ObservableCollection<ContactItem> Contacts
    {
        get { return contacts; }
        set
        {
            contacts = value;
            OnPropertyChanged(nameof(Contacts));
        }
    }

    private string contactDeletedText;
    public string ContactDeletedText
    {
        get { return contactDeletedText; }
        set
        {
            if (contactDeletedText != value)
            {
                contactDeletedText = value;
                OnPropertyChanged(nameof(ContactDeletedText));
            }
        }
    }

    private string contactEditedText;
    public string ContactEditedText
    {
        get { return contactEditedText; }
        set
        {
            if (contactEditedText != value)
            {
                contactEditedText = value;
                OnPropertyChanged(nameof(ContactEditedText));
            }
        }
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

    private bool clientLegalPerson;
    public bool ClientLegalPerson
    {
        get { return clientLegalPerson; }
        set
        {
            if (clientLegalPerson != value)
            {
                clientLegalPerson = value;
                OnPropertyChanged(nameof(ClientLegalPerson));
            }
        }
    }


    public bool LocalContactDeleted { get; set; }
    public bool ContactDeleted
    {
        get { return LocalContactDeleted; }
        set
        {
            if (LocalContactDeleted != value)
            {
                LocalContactDeleted = value;
                OnPropertyChanged(nameof(ContactDeleted));
            }
        }
    }

    public bool LocalContactEdited { get; set; }
    public bool ContactEdited
    {
        get { return LocalContactEdited; }
        set
        {
            if (LocalContactEdited != value)
            {
                LocalContactEdited = value;
                OnPropertyChanged(nameof(ContactEdited));
            }
        }
    }

    public bool LocalNoQueryResult { get; set; }
    public bool NoQueryResult
    {
        get { return LocalNoQueryResult; }
        set
        {
            if (LocalNoQueryResult != value)
            {
                LocalNoQueryResult = value;
                OnPropertyChanged(nameof(NoQueryResult));
            }
        }
    }
    public bool LocalNoSQLreply { get; set; }
    public bool NoSQLreply
    {
        get { return LocalNoSQLreply; }
        set
        {
            if (LocalNoSQLreply != value)
            {
                LocalNoSQLreply = value;
                OnPropertyChanged(nameof(NoSQLreply));
            }
        }
    }

    private string Tekst2 { get; set; }

    public string tekst2
    {
        get { return Tekst2; }
        set
        {
            if (Tekst2 != value)
            {
                Tekst2 = value;
                OnPropertyChanged(nameof(tekst2));
            }
        }
    }


    public void GenerateFiles()
    {
        ContactEdited = false;
        ContactDeleted = false;
        NoSQLreply = false;
        NoQueryResult = false;
        Debug.WriteLine(contacts);
        try
        {
            if (contacts != null)
            {
                contacts.Clear();
            }
            string query = "SELECT * FROM `contacts` ORDER by id desc limit 30;";
            Debug.WriteLine("Generate contacts query: " + query);
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

                        contacts.Add(new ContactItem()
                        {
                            Id = id,
                            Ime = filesRow["ime"],
                            OIB = filesRow["OIB"],
                            Datum_rodenja = filesRow["datum_rodenja"],
                            Adresa = filesRow["adresa"],
                            Boraviste = filesRow["boraviste"],
                            Telefon = filesRow["telefon"],
                            Fax = filesRow["fax"],
                            Mobitel = filesRow["mobitel"],
                            Email = filesRow["email"],
                            Ostalo = filesRow["ostalo"],
                            Drzava = filesRow["drzava"],
                            Pravna = filesRow["pravna"],
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
    public void GenerateSearchResults()
    {
        ContactEdited = false;
        NoQueryResult = false;
        NoSQLreply = false;
        ContactDeleted = false;
        Debug.WriteLine(contacts);
        try
        {
            if (contacts != null)
            {
                NoQueryResult = false;
                contacts.Clear();
            }
            string query = "SELECT * FROM `contacts` WHERE LOWER(ime) LIKE LOWER('%" + SearchText + "%') OR OIB LIKE '%" + SearchText + "%' limit 30";
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
                        DateTime datum_rodenja;

                        int.TryParse(filesRow["id"], out id);
                        DateTime.TryParse(filesRow["datum_rodenja"], out datum_rodenja);

                        contacts.Add(new ContactItem()
                        {
                            Id = id,
                            Ime = filesRow["ime"],
                            OIB = filesRow["OIB"],
                            Datum_rodenja = filesRow["datum_rodenja"],
                            Adresa = filesRow["adresa"],
                            Boraviste = filesRow["boraviste"],
                            Telefon = filesRow["telefon"],
                            Fax = filesRow["fax"],
                            Mobitel = filesRow["mobitel"],
                            Email = filesRow["email"],
                            Ostalo = filesRow["ostalo"],
                            Drzava = filesRow["drzava"],
                            Pravna = filesRow["pravna"],
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

    private string _ClientID;
    public string ClientID
    {
        get { return _ClientID; }
        set
        {
            if (_ClientID != value)
            {
                _ClientID = value;
                OnPropertyChanged(nameof(ClientID));
            }
        }
    }
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
        get { return _ClientPhone; }
        set
        {
            if (_ClientPhone != value)
            {
                _ClientPhone = value;
                OnPropertyChanged(nameof(ClientPhone));
            }
        }
    }
    private string _ClientFax;
    public string ClientFax
    {
        get { return _ClientFax; }
        set
        {
            if (_ClientFax != value)
            {
                _ClientFax = value;
                OnPropertyChanged(nameof(ClientFax));
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


    private string _ClientLegalPersonString;

    public string ClientLegalPersonString
    {
        get { return _ClientLegalPersonString; }
        set
        {
            if (_ClientLegalPersonString != value)
            {
                _ClientLegalPersonString = value;
                OnPropertyChanged(nameof(ClientLegalPersonString));
            }
        }
    }

    #endregion

    public ICommand OnReciptClickCommand { get; set; }
    public ICommand RefreshContacts { get; set; }
    public ICommand EditClientButton { get; set; }
    public int FilesCounter { get; set; }
    public ICommand AddAsClient { get; set; }
    public ICommand AddAsOpponent { get; set; }

    public async Task InitializeData()
    {
        try
        {
            ClientID = await SecureStorage.GetAsync("SelectedID");
            ClientName = await SecureStorage.GetAsync("SelectedName");
            ClientOIB = await SecureStorage.GetAsync("SelectedOIB");
            ClientAddress = await SecureStorage.GetAsync("SelectedAddress");
            ClientResidence = await SecureStorage.GetAsync("SelectedRsidence");
            ClientPhone = await SecureStorage.GetAsync("SelectedPhone");
            ClientFax = await SecureStorage.GetAsync("SelectedFax");
            ClientMobile = await SecureStorage.GetAsync("SelectedMobile");
            ClientEmail = await SecureStorage.GetAsync("SelectedEmail");
            ClientOther = await SecureStorage.GetAsync("SelectedOther");
            ClientCountry = await SecureStorage.GetAsync("SelectedCountry");
            ClientLegalPersonString = await SecureStorage.GetAsync("SelectedLegalPersonString");
            ClientBirthDate = await SecureStorage.GetAsync("SelectedBrithDateString");


            if (ClientLegalPersonString == "True")
            {
                ClientLegalPerson = true;
            }
            else
            {
                ClientLegalPerson = false;
            }

            if (ClientLegalPersonString == "True")
            {
                ClientLegalPerson = true;
            }
            else
            {
                ClientLegalPerson = false;
            }
            tekst2 = await SecureStorage.GetAsync("ClientEditedName");

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message + " in initialize data");

        }
    }
    public KlijentiViewModel()
    {
        WeakReferenceMessenger.Default.Register<RefreshContacts>(this, NewContactAddedReceived);
        WeakReferenceMessenger.Default.Register<ContactDeleted>(this, ContactDeletedReceived);
        WeakReferenceMessenger.Default.Register<ContactEdited>(this, ContactEditedReceived);


        Contacts = new ObservableCollection<ContactItem>();
        ContactDeleted = false;

        
        OnReciptClickCommand = new Command(OpenRecipt);
        RefreshContacts = new Command(GenerateFiles);
        EditClientButton = new Command(EditClient);
        AddAsClient = new Command(AddSelectedClient);
        AddAsOpponent = new Command(AddSelectedAsOpponent);
        GenerateFiles();

        var timer = Application.Current.Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromMilliseconds(200);
        timer.Tick += (s, e) => Refresh();
        timer.Start();
        Debug.WriteLine("Klijenti ViewModel uspješno izvršeni");
        //EmptyContactRows();
        //FilesCounter = 1;

    }


    //public void EmptyContactRows()
    //{
    //    FilesCounter++;
    //    Contacts = new ObservableCollection<ContactItem>();


    //    for (int i = 0; i < 30; i++)
    //    {
    //        Contacts.Add(new ContactItem
    //        {
    //            Id = i + 1,
    //            Ime = " ",
    //            OIB = " ",
    //            Datum_rodenja = " ",
    //            Adresa = " ",
    //            Boraviste = " ",
    //            Telefon = " ",
    //            Fax = " ",
    //            Mobitel = " ",
    //            Email = " ",
    //            Ostalo = " ",
    //            Drzava = " ",
    //            Pravna = " "
    //        });
    //    }
    //    DelayGenerateFiles();
    //}

    //private async void DelayGenerateFiles()
    //{
    //    await Task.Delay(TimeSpan.FromSeconds(1)); // Wait for 2 seconds

    //    GenerateFiles();
    //}

    public async void OnButtonClick()
    {
        await Shell.Current.GoToAsync("/NoviKlijent");
        Debug.WriteLine("novi klijent clicked");
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
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            //if (FilesCounter == 1)
            //{
            //    EmptyContactRows();
            //}

            try
            {
                await InitializeData();
                //ClientID = Preferences.Get("SelectedID", "");
                //ClientName = Preferences.Get("SelectedName", "");
                //ClientOIB = Preferences.Get("SelectedOIB", "");
                //ClientAddress = Preferences.Get("SelectedAddress", "");
                //ClientResidence = Preferences.Get("SelectedRsidence", "");
                //ClientPhone = Preferences.Get("SelectedPhone", "");
                //ClientFax = Preferences.Get("SelectedFax", "");
                //ClientMobile = Preferences.Get("SelectedMobile", "");
                //ClientEmail = Preferences.Get("SelectedEmail", "");
                //ClientOther = Preferences.Get("SelectedOther", "");
                //ClientCountry = Preferences.Get("SelectedCountry", "");
                //ClientLegalPersonString = Preferences.Get("SelectedLegalPersonString", "");
                //ClientBirthDate = Preferences.Get("SelectedBrithDateString", "");
                if (ClientLegalPersonString == "True")
                {
                    ClientLegalPerson = true;
                }
                else
                {
                    ClientLegalPerson = false;
                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " in refresh");

            }
        }
        );
    }



    private async void EditClient()
    {
        await Shell.Current.GoToAsync("/UrediKlijenta");
        Debug.WriteLine("uredi klijenta clicked");


    }

    private void NewContactAddedReceived(object recipient, RefreshContacts message)
    {
        GenerateFiles();
        Debug.WriteLine("Generating files after adding a new contact");
        Application.Current.MainPage.DisplayAlert("", "Uspješno ste dodali novi kontakt.", "OK");


    }
    private async Task getContactText()
    {
        ContactDeletedText = await SecureStorage.GetAsync("ContactDeleted");
    }

    private async void ContactDeletedReceived(object recipient, ContactDeleted message)
    {
        GenerateFiles();
        Debug.WriteLine("Generating files after deleting a contact");
        await getContactText();
        //ContactDeletedText = Preferences.Get("ContactDeleted", "");
        ContactDeleted = true;
        await Application.Current.MainPage.DisplayAlert("", ContactDeletedText, "OK");

    }

    private void ContactEditedReceived(object recipient, ContactEdited message)
    {

        NoQueryResult = false;
        NoSQLreply = false;
        ContactDeleted = false;

        Debug.WriteLine(contacts);
        try
        {
            if (contacts != null)
            {
                NoQueryResult = false;
                contacts.Clear();
            }
            string query = "SELECT * FROM contacts WHERE id = " + ClientID;
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
                        DateTime datum_rodenja;

                        int.TryParse(filesRow["id"], out id);
                        DateTime.TryParse(filesRow["datum_rodenja"], out datum_rodenja);

                        contacts.Add(new ContactItem()
                        {
                            Id = id,
                            Ime = filesRow["ime"],
                            OIB = filesRow["OIB"],
                            Datum_rodenja = filesRow["datum_rodenja"],
                            Adresa = filesRow["adresa"],
                            Boraviste = filesRow["boraviste"],
                            Telefon = filesRow["telefon"],
                            Fax = filesRow["fax"],
                            Mobitel = filesRow["mobitel"],
                            Email = filesRow["email"],
                            Ostalo = filesRow["ostalo"],
                            Drzava = filesRow["drzava"],
                            Pravna = filesRow["pravna"],
                        });
                    }
                    ContactEdited = true;
                }
                else
                {
                    NoQueryResult = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " in Contact Edited Received if");
                NoSQLreply = true;
            }

            string tekst1 = "Uspješno ste izmijenili kontakt: ";
            //string tekst2 = Preferences.Get("ClientEditedName", "");
            string tekstObavijest = string.Concat(tekst1, tekst2);
            ContactEditedText = tekstObavijest;
            Application.Current.MainPage.DisplayAlert("", ContactEditedText, "OK");

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message + " in Contact Edited Received");
        }
    }

    private ContactItem _selectedItem; //Za SyncfusionListView, za obični je kod u Naplata.xaml.cs
    public ContactItem SelectedItem
    {
        get { return _selectedItem; }
        set
        {
            _selectedItem = value;
            // Save to preferences
            
            SaveSelectedItem();

            OnPropertyChanged(nameof(SelectedItem));
        }
    }
    public async Task SaveItems()
    {
        try
        {
            Debug.WriteLine("UŠO U JEBENU FUNKCIJU SAVE ITEMS");
            if (SelectedItem != null)
            {
                await SecureStorage.SetAsync("SelectedName", SelectedItem.Ime);
                await SecureStorage.SetAsync("SelectedOIB", SelectedItem.OIB);
                await SecureStorage.SetAsync("SelectedAddress", SelectedItem.Adresa);
                await SecureStorage.SetAsync("SelectedRsidence", SelectedItem.Boraviste);
                await SecureStorage.SetAsync("SelectedPhone", SelectedItem.Telefon);
                await SecureStorage.SetAsync("SelectedFax", SelectedItem.Fax);
                await SecureStorage.SetAsync("SelectedMobile", SelectedItem.Mobitel);
                await SecureStorage.SetAsync("SelectedEmail", SelectedItem.Email);
                await SecureStorage.SetAsync("SelectedOther", SelectedItem.Ostalo);
                await SecureStorage.SetAsync("SelectedCountry", SelectedItem.Drzava);
                await SecureStorage.SetAsync("SelectedLegalPersonString", SelectedItem.Pravna);
                await SecureStorage.SetAsync("SelectedBrithDateString", SelectedItem.Datum_rodenja);
                string IDstring = SelectedItem.Id.ToString();
                Debug.WriteLine(IDstring);
                await SecureStorage.SetAsync("SelectedID", IDstring);
            }
            else if (SelectedItem == null)
            {
                Debug.WriteLine("Selected item is null");
            }
            else
            {
                Debug.WriteLine("Selected item is null");
            }
            Debug.WriteLine("Posle svega");
        }
        catch (Exception ex)
        {

            Debug.WriteLine(ex.Message + " in save Items");
        }
    }
    public async void SaveSelectedItem()
    {
        
        try
        {
            await SaveItems();
            Debug.WriteLine(ClientName + " " + ClientID + " In save selected item");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message + " in save items");

        }
    }

    public async Task getClientName()
    {
        await SecureStorage.SetAsync("FilesClientID", ClientID);
        await SecureStorage.SetAsync("FilesClientName", ClientName);
    }

    public async void AddSelectedClient()
    {
        try
        {
            await InitializeData();
            await getClientName();
            if (ClientID != null && ClientName != null)
            {
                

                string tekst1 = "Kontakt '";
                string tekst2 = ClientName;
                string tekst3 = "' spremljen je kao klijent.";
                string tekstObavijest = string.Concat(tekst1, tekst2, tekst3);

                await Application.Current.MainPage.DisplayAlert("", tekstObavijest, "OK");
            }
            else
            {
                Debug.WriteLine("ClientID or ClientName is null.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message + " in AddSelectedClientAsync");
        }
    }

    public async void AddSelectedAsOpponent()
    {
        try
        {
            await InitializeData();
            //Preferences.Set("FilesOpponent", ClientID);
            //Preferences.Set("FilesOpponentName", ClientName);
            await SecureStorage.SetAsync("FilesOpponent", ClientID);
            await SecureStorage.SetAsync("FilesOpponentName", ClientName);

            string tekst1 = "Kontakt '";
            string tekst2 = ClientName;
            string tekst3 = "' spremljen je kao protustranka.";
            string tekstObavijest = string.Concat(tekst1, tekst2, tekst3);

            await Application.Current.MainPage.DisplayAlert(" ", tekstObavijest, "OK");

        }
        catch (Exception ex)
        { 
            Debug.WriteLine(ex.Message);
        }
    }
}