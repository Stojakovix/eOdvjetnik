using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eOdvjetnik.Model;
using eOdvjetnik.Models;
using eOdvjetnik.Services;

namespace eOdvjetnik.ViewModel
{
    public class NoviKlijentViewModel : INotifyPropertyChanged
    {
        Klijent klijent;
        ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();

        public NoviKlijentViewModel()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
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

        private void AddKlijentToRemoteServer(Klijent klijent)
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

                string query = $"INSERT INTO Contacts (id , ime, OIB, datum_rodenja, adresa, ostalo, boraviste, telefon, fax, mobitel, email, drzava, pravna) " +
                $"VALUES ('{name}', '{oib}', '{datum_rodjenja.ToString("yyyy-MM-dd HH:mm:ss")}', '{adresa}' , '{ostalo}' , '{boraviste}' , '{telefon}' , '{fax}' , '{mobitel}' , '{email}' , '{drzava}' , '{pravna}')";
                Debug.WriteLine(query + " in novi spis viewModel");
            }

            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public Klijent Klijent
        {
            get { return klijent; }
            set
            {
                if (klijent != value)
                {
                    klijent = value;
                    OnPropertyChanged(nameof(Klijent));
                }
            }
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
