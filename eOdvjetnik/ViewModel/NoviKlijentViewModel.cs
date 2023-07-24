﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using eOdvjetnik.Models;
using eOdvjetnik.Services;


namespace eOdvjetnik.ViewModel
{
    public class NoviKlijentViewModel : INotifyPropertyChanged
    {

        ContactItem contactItem;

        public ContactItem ContactItem
        {
            get { return contactItem; }
            set
            {
                if (contactItem != value)
                {
                    contactItem = value;
                    OnPropertyChanged(nameof(ContactItem));
                }
            }
        }
        ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
        public ICommand AddKlijent { get; set; }

        public NoviKlijentViewModel()
        {
            try
            {
                AddKlijent = new Command(OnButtonCLick);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private ObservableCollection<ContactItem> contacts;
        public ObservableCollection<ContactItem> Contacts
        {
            get { return contacts; }
            set
            {
                if (contacts != value)
                {
                    contacts = value;
                    OnPropertyChanged(nameof(Contacts));
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

                Debug.WriteLine(query + " in novi klijent viewModel");

            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            Debug.WriteLine("prije Bate");


            // Call GenerateFiles() on the shared instance

            Debug.WriteLine("Poslije Bate");
        }



        public async void OnButtonCLick()
        {
            AddKlijentToRemoteServer(contactItem);
            await Shell.Current.GoToAsync("///Klijenti");
            //Debug.WriteLine("Klijent dodan" + klijent.Ime);

        }




        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
