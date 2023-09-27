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
        private Navigacija navigacija;

        ContactItem contactItem;
        ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();

        public ICommand DodajNovogKlijenta { get; set; }
        public ICommand UpdateClientData { get; set; }
        public ICommand DeleteClientData { get; set; }
        public ICommand ConfirmDelete { get; set; }
        public ICommand CancelDelete { get; set; }
        public ICommand BackButtonCommand { get; set; }

        

        public NoviKlijentViewModel()
        {

            try
            {

                ClientID = TrecaSreca.Get("SelectedID");
                ClientName = TrecaSreca.Get("SelectedName");
                ClientOIB = TrecaSreca.Get("SelectedOIB");
                ClientAddress = TrecaSreca.Get("SelectedAddress");
                ClientResidence = TrecaSreca.Get("SelectedRsidence");
                ClientPhone = TrecaSreca.Get("SelectedPhone");
                ClientFax = TrecaSreca.Get("SelectedFax");
                ClientMobile = TrecaSreca.Get("SelectedMobile");
                ClientEmail = TrecaSreca.Get("SelectedEmail");
                ClientOther = TrecaSreca.Get("SelectedOther");
                ClientCountry = TrecaSreca.Get("SelectedCountry");
                ClientLegalPersonString = TrecaSreca.Get("SelectedLegalPersonString");
                ClientBirthDate = TrecaSreca.Get("SelectedBrithDateString");

                if (ClientLegalPersonString == "True")
                {
                    ClientLegalPerson = true;
                }
                else
                {
                    ClientLegalPerson = false;

                }
                DeleteClientPopupEnabled = false;
                DeleteClientPopupVisible = false;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
            DodajNovogKlijenta = new Command(OnButtonCLick);
            UpdateClientData = new Command(OnUpdateCLick);
            DeleteClientData = new Command(DeletePopup);
            ConfirmDelete = new Command(OnDeleteCLick);
            CancelDelete = new Command(OnCancelCLick);
            BackButtonCommand = new Command(OnBackButtonClick);
            ClientHasNoName = false;

        }


        #region Navigacija
        public ICommand PocetnaClick => navigacija.PocetnaClick;
        public ICommand KalendarClick => navigacija.KalendarClick;
        public ICommand SpisiClick => navigacija.SpisiClick;
        public ICommand TarifaClick => navigacija.TarifaClick;
        public ICommand DokumentiClick => navigacija.DokumentiClick;
        public ICommand KontaktiClick => navigacija.KontaktiClick;
        public ICommand KorisnickaClick => navigacija.KorisnickaPodrskaClick;
        public ICommand PostavkeClick => navigacija.PostavkeClick;
        private bool clientHasNoName;
        #endregion
        public bool ClientHasNoName
        {
            get { return clientHasNoName; }
            set
            {
                if (clientHasNoName != value)
                {
                    clientHasNoName = value;
                    OnPropertyChanged(nameof(ClientHasNoName));
                }
            }
        }
        private bool _DeleteClientPopupVisible;

        public bool DeleteClientPopupVisible
        {
            get { return _DeleteClientPopupVisible; }
            set
            {
                if (_DeleteClientPopupVisible != value)
                {
                    _DeleteClientPopupVisible = value;
                    OnPropertyChanged(nameof(DeleteClientPopupVisible));
                }
            }
        }
        private bool _DeleteClientPopupEnabled;

        public bool DeleteClientPopupEnabled
        {
            get { return _DeleteClientPopupEnabled; }
            set
            {
                if (_DeleteClientPopupEnabled != value)
                {
                    _DeleteClientPopupEnabled = value;
                    OnPropertyChanged(nameof(DeleteClientPopupEnabled));
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

        private string datumRodjenja;
        public string DatumRodjenja
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

        private bool pravna;
        public bool Pravna
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
        public string PravnaString { get; set; }

        #endregion

        private void AddKlijentToRemoteServer(ContactItem contact)
        {
            
                try
                {


                    #region VarijableZaServer
                    string name = Name ?? string.Empty;
                    string oib = Oib ?? string.Empty;
                    string datum_rodjenja = DatumRodjenja ?? string.Empty;
                    string adresa = Adresa ?? string.Empty;
                    string ostalo = Ostalo ?? string.Empty;
                    string boraviste = Boraviste ?? string.Empty;
                    string telefon = Telefon ?? string.Empty;
                    string fax = Fax ?? string.Empty;
                    string mobitel = Mobitel ?? string.Empty;
                    string email = Email ?? string.Empty;
                    string drzava = Drzava ?? string.Empty;

                    if (Pravna == true)
                    {
                        PravnaString = "1";
                    }
                    else
                    {
                        PravnaString = "0";
                    }

                    #endregion
                    ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
                    string disableForeignKeyChecksQuery = "SET FOREIGN_KEY_CHECKS = 0";
                    externalSQLConnect.sqlQuery(disableForeignKeyChecksQuery);


                    string query = $"INSERT INTO contacts (ime, OIB, datum_rodenja, adresa, ostalo, boraviste, telefon, fax, mobitel, email, drzava, pravna) " +
                    $"VALUES ('{name}', '{oib}', '{datum_rodjenja}', '{adresa}' , '{ostalo}' , '{boraviste}' , '{telefon}' , '{fax}' , '{mobitel}' , '{email}' , '{drzava}' , '{PravnaString}')";
                    externalSQLConnect.sqlQuery(query);
                }

                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
           
           



        }



        public async void OnButtonCLick()
        {
            ClientHasNoName = false;

            if (Name != null && Name != "")
            {
                AddKlijentToRemoteServer(contactItem);
                await Shell.Current.GoToAsync("///Klijenti");
                NewContactAdded();
                //Debug.WriteLine("Klijent dodan" + klijent.Ime);
            }
            else
            {
                ClientHasNoName = true;
                await Application.Current.MainPage.DisplayAlert("Upozorenje", "Potrebno je unijeti ime (naziv) klijenta.", "OK");

            }

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
                    TrecaSreca.Set("ContactDeleted", contactDeleted);


                    string DeleteQuery = "DELETE FROM contacts WHERE ID = " + ClientID;
                    externalSQLConnect.sqlQuery(DeleteQuery);

                    Debug.WriteLine(contactDeleted);
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

        public void DeletePopup()
        {
            DeleteClientPopupEnabled = true;
            DeleteClientPopupVisible = true;
                    }
        public void OnCancelCLick()
        {
            DeleteClientPopupEnabled = false;
            DeleteClientPopupVisible = false;
        }
        
        public async void OnDeleteCLick()
        {
            DeleteClientPopupEnabled = false;
            DeleteClientPopupVisible = false;
            DeleteContactOnRemoteServer(contactItem);
            await Shell.Current.GoToAsync("///Klijenti");
            ContactDeletedMessage();

        }

        private void UpdateContactOnRemoteServer(ContactItem contact)
        {
            
            try
            {

                ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
                string disableForeignKeyChecksQuery = "SET FOREIGN_KEY_CHECKS = 0";
                externalSQLConnect.sqlQuery(disableForeignKeyChecksQuery);

                if (ClientLegalPerson == true)
                {
                    PravnaString = "1";
                }
                else
                {
                    PravnaString = "0";
                }

                string UpdateQuery = $"UPDATE contacts SET ime = '{ClientName}', datum_rodenja = '{ClientBirthDate}', oib = '{ClientOIB}', adresa = '{ClientAddress}', ostalo = '{ClientOther}', boraviste = '{ClientResidence}', telefon = '{ClientPhone}', fax = '{ClientFax}', mobitel = '{ClientMobile}', email = '{ClientEmail}', drzava = '{ClientCountry}', pravna = '{PravnaString}' WHERE id = " + ClientID;

                externalSQLConnect.sqlQuery(UpdateQuery);

                Debug.WriteLine("Update contact query: " + UpdateQuery);

                externalSQLConnect.sqlQuery(UpdateQuery);


                TrecaSreca.Set("SelectedName", ClientName);
                TrecaSreca.Set("SelectedOIB", ClientOIB);
                TrecaSreca.Set("SelectedAddress", ClientAddress);
                TrecaSreca.Set("SelectedRsidence", ClientResidence);
                TrecaSreca.Set("SelectedPhone", ClientPhone);
                TrecaSreca.Set("SelectedFax", ClientFax);
                TrecaSreca.Set("SelectedMobile", ClientMobile);
                TrecaSreca.Set("SelectedEmail", ClientEmail);
                TrecaSreca.Set("SelectedOther", ClientOther);
                TrecaSreca.Set("SelectedCountry", ClientCountry);
                TrecaSreca.Set("SelectedBrithDateString", ClientBirthDate);

             
                if (ClientLegalPerson == true)
                 {
                    TrecaSreca.Set("SelectedLegalPersonString", "True");

                }




            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
          

        }
        public async void OnBackButtonClick()
        {
            await Shell.Current.GoToAsync("///Klijenti");
        }
        public async void OnUpdateCLick()
        {
            ClientHasNoName = false;
            if (ClientName != null && ClientName != "")
            {
                UpdateContactOnRemoteServer(contactItem);
                await Shell.Current.GoToAsync("///Klijenti");
                TrecaSreca.Set("ClientEditedName", ClientName);

                ContactEditedMessage();
                Debug.WriteLine("Klijent je ažuriran: " + ClientName);
            }
            else
            {
                ClientHasNoName = true;
                await Application.Current.MainPage.DisplayAlert("Upozorenje", "Potrebno je unijeti ime (naziv) klijenta.", "OK");

            }


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
        private bool _ClientLegalPerson;

        public bool ClientLegalPerson
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

        public string ClientLegalPersonString { get; set; }

        #endregion



        private void NewContactAdded()
        {
            WeakReferenceMessenger.Default.Send(new RefreshContacts("Refresh contacts!"));
        }

        private void ContactDeletedMessage()
        {
            WeakReferenceMessenger.Default.Send(new ContactDeleted("Contact deleted!"));
        }
        private void ContactEditedMessage()
        {
            WeakReferenceMessenger.Default.Send(new ContactEdited("Contact edited!"));
        }
    }
}
