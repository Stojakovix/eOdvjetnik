using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using eOdvjetnik.Model;
using eOdvjetnik.Models;
using eOdvjetnik.Services;
using CommunityToolkit.Mvvm.Messaging;



namespace eOdvjetnik.ViewModel
{
    public class NoviKlijentViewModel : INotifyPropertyChanged
    {
        Klijent klijent;
        ContactItem contactItem;
        ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();

        public ICommand DodajNovogKlijenta { get; set; }
        public ICommand UpdateClientData { get; set; }
        public ICommand DeleteClientData { get; set; }

        public NoviKlijentViewModel()
        {

            try
            {
                ClientID = Preferences.Get("SelectedID", "");
                ClientName = Preferences.Get("SelectedName", "");
                ClientOIB = Preferences.Get("SelectedOIB", "");
                ClientAddress = Preferences.Get("SelectedAddress", "");
                ClientResidence = Preferences.Get("SelectedRsidence", "");
                ClientPhone = Preferences.Get("SelectedPhone", "");
                ClientFax = Preferences.Get("SelectedFax", "");
                ClientMobile = Preferences.Get("SelectedMobile", "");
                ClientEmail = Preferences.Get("SelectedEmail", "");
                ClientOther = Preferences.Get("SelectedOther", "");
                ClientCountry = Preferences.Get("SelectedCountry", "");
                ClientLegalPerson = Preferences.Get("SelectedLegalPerson", "");

                string brithDateString = Preferences.Get("SelectedBrithDateString", "");
                ClientBirthDate = DateTime.ParseExact(brithDateString, "dd-MM-yyyy", null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
            DodajNovogKlijenta = new Command(OnButtonCLick);
            UpdateClientData = new Command(OnUpdateCLick);
            DeleteClientData = new Command(OnDeleteCLick);


        }

        private ObservableCollection<Klijent> klijenti;
        public ObservableCollection<Klijent> Klijenti
        {
            get { return klijenti; }
            set
            {
                if (klijenti != value)
                {
                    klijenti = value;
                    OnPropertyChanged(nameof(Klijenti));
                }
            }
        }
        #region Varijable za klijente
        private string id;
        public string Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private string oib;
        public string Oib
        {
            get { return oib; }
            set
            {
                if (oib != value)
                {
                    oib = value;
                    OnPropertyChanged(nameof(Oib));
                }
            }
        }

        private DateTime datumRodjenja;
        public DateTime DatumRodjenja
        {
            get { return datumRodjenja; }
            set
            {
                if (datumRodjenja != value)
                {
                    datumRodjenja = value;
                    OnPropertyChanged(nameof(DatumRodjenja));
                }
            }
        }

        private string adresa;
        public string Adresa
        {
            get { return adresa; }
            set
            {
                if (adresa != value)
                {
                    adresa = value;
                    OnPropertyChanged(nameof(Adresa));
                }
            }
        }

        private string ostalo;
        public string Ostalo
        {
            get { return ostalo; }
            set
            {
                if (ostalo != value)
                {
                    ostalo = value;
                    OnPropertyChanged(nameof(Ostalo));
                }
            }
        }

        private string boraviste;
        public string Boraviste
        {
            get { return boraviste; }
            set
            {
                if (boraviste != value)
                {
                    boraviste = value;
                    OnPropertyChanged(nameof(Boraviste));
                }
            }
        }

        private string telefon;
        public string Telefon
        {
            get { return telefon; }
            set
            {
                if (telefon != value)
                {
                    telefon = value;
                    OnPropertyChanged(nameof(Telefon));
                }
            }
        }

        private string fax;
        public string Fax
        {
            get { return fax; }
            set
            {
                if (fax != value)
                {
                    fax = value;
                    OnPropertyChanged(nameof(Fax));
                }
            }
        }

        private string mobitel;
        public string Mobitel
        {
            get { return mobitel; }
            set
            {
                if (mobitel != value)
                {
                    mobitel = value;
                    OnPropertyChanged(nameof(Mobitel));
                }
            }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        private string drzava;
        public string Drzava
        {
            get { return drzava; }
            set
            {
                if (drzava != value)
                {
                    drzava = value;
                    OnPropertyChanged(nameof(Drzava));
                }
            }
        }

        private string pravna;
        public string Pravna
        {
            get { return pravna; }
            set
            {
                if (pravna != value)
                {
                    pravna = value;
                    OnPropertyChanged(nameof(Pravna));
                }
            }
        }


        #endregion

        private void AddKlijentToRemoteServer(ContactItem contact)
        {
            try
            {


                #region VarijableZaServer
                string name = Name ?? string.Empty;
                string oib = Oib ?? string.Empty;
                DateTime datum_rodjenja = DatumRodjenja;
                string adresa = Adresa ?? string.Empty;
                string ostalo = Ostalo ?? string.Empty;
                string boraviste = Boraviste ?? string.Empty;
                string telefon = Telefon ?? string.Empty;
                string fax = Fax ?? string.Empty;
                string mobitel = Mobitel ?? string.Empty;
                string email = Email ?? string.Empty;
                string drzava = Drzava ?? string.Empty;
                int pravna = int.Parse(Pravna ?? "0");

                #endregion
                ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
                string disableForeignKeyChecksQuery = "SET FOREIGN_KEY_CHECKS = 0";
                externalSQLConnect.sqlQuery(disableForeignKeyChecksQuery);


                string query = $"INSERT INTO Contacts (ime, OIB, datum_rodenja, adresa, ostalo, boraviste, telefon, fax, mobitel, email, drzava, pravna) " +
                $"VALUES ('{name}', '{oib}', '{datum_rodjenja.ToString("yyyy-MM-dd")}', '{adresa}' , '{ostalo}' , '{boraviste}' , '{telefon}' , '{fax}' , '{mobitel}' , '{email}' , '{drzava}' , '{pravna}')";
                externalSQLConnect.sqlQuery(query);

            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }



        }



        public async void OnButtonCLick()
        {
            AddKlijentToRemoteServer(contactItem);
            await Shell.Current.GoToAsync("///Klijenti");
            NewContactAdded();
            //Debug.WriteLine("Klijent dodan" + klijent.Ime);

        }

        private void DeleteContactOnRemoteServer(ContactItem contact)
        {
            try
            {
                if (ClientID != null)
                {
                    ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
                    Debug.WriteLine("Client ID " + ClientID);
                    string contactDeleted = "Obrisali ste kontakt: " + ClientName;
                    Preferences.Set("ContactDeleted", contactDeleted);


                    string DeleteQuery = "DELETE FROM Contacts WHERE ID = " + ClientID;
                    externalSQLConnect.sqlQuery(DeleteQuery);
                    
                }
               else
                {
                    Debug.WriteLine("Client ID is null " + ClientID);

                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }



        }

        public async void OnDeleteCLick()
        {
            DeleteContactOnRemoteServer(contactItem);
            await Shell.Current.GoToAsync("///Klijenti");
            Debug.WriteLine("Klijent obrisan: " + ClientName);
            ContactDeletedMessage();
        }

        private void UpdateContactOnRemoteServer(ContactItem contact)
        {
            try
            {


                #region VarijableZaServer
                string name = ClientName ?? string.Empty;
                string oib = ClientOIB ?? string.Empty;
                DateTime datum_rodenja = ClientBirthDate;
                string adresa = ClientAddress ?? string.Empty;
                string ostalo = ClientOther ?? string.Empty;
                string boraviste = ClientResidence ?? string.Empty;
                string telefon = ClientPhone ?? string.Empty;
                string fax = ClientFax ?? string.Empty;
                string mobitel = ClientMobile ?? string.Empty;
                string email = ClientEmail ?? string.Empty;
                string drzava = ClientCountry ?? string.Empty;
                int pravna = int.Parse(ClientLegalPerson ?? "0");

                #endregion
                ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
                string disableForeignKeyChecksQuery = "SET FOREIGN_KEY_CHECKS = 0";
                externalSQLConnect.sqlQuery(disableForeignKeyChecksQuery);


                string UpdateQuery = "ROBI ĆE NAPRAVIT"; 

                externalSQLConnect.sqlQuery(UpdateQuery);

                Debug.WriteLine("Update contact query: " + UpdateQuery);

                externalSQLConnect.sqlQuery(UpdateQuery);

            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }



        }

        public async void OnUpdateCLick()
        {
            UpdateContactOnRemoteServer(contactItem);
            await Shell.Current.GoToAsync("///Klijenti");
            // dodati query da prikaže baš taj kontakt
            Debug.WriteLine("Klijent je ažuriran: " + ClientName);

        }
        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

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

        private DateTime _ClientBirthDate;
        public DateTime ClientBirthDate
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



        private void NewContactAdded()
        {
            WeakReferenceMessenger.Default.Send(new RefreshContacts("Refresh contacts!"));
        }

        private void ContactDeletedMessage()
        {
            WeakReferenceMessenger.Default.Send(new RefreshContacts("Contact deleted!"));
        }
    }
}
