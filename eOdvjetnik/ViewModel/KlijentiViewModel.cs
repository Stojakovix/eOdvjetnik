using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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

    public void GenerateFiles()
    {
        Debug.WriteLine(contacts);
        try
        {
            if (contacts != null)
            {
                contacts.Clear();
            }
            string query = "SELECT * FROM `contacts` ORDER by id desc limit 30;";
            Debug.WriteLine(query);
            Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
            Debug.WriteLine(query + " u Search resultu");
            if (filesData != null)
            {
                foreach (Dictionary<string, string> filesRow in filesData)
                {

                    int id;
                    int pravna;
                    DateTime datum_rodenja;

                    int.TryParse(filesRow["id"], out id);
                    int.TryParse(filesRow["pravna"], out pravna);
                    DateTime.TryParse(filesRow["datum_rodenja"], out datum_rodenja);

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
                        Pravna = pravna

                    });


                }
                foreach (ContactItem item in Contacts)
                {
                }

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
    public void GenerateSearchResults()
    {
        Debug.WriteLine(contacts);
        try
        {
            if (contacts != null)
            {
                contacts.Clear();
            }
            string query = "SELECT * FROM `contacts` WHERE ime LIKE '%" + SearchText + "%' OR OIB LIKE '%" + SearchText + "%'";
            Debug.WriteLine(query);
            Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
            Debug.WriteLine(query + " u Search resultu");
            if (filesData != null)
            {
                foreach (Dictionary<string, string> filesRow in filesData)
                {

                    int id;
                    int pravna;
                    DateTime datum_rodenja;
     
                    int.TryParse(filesRow["id"], out id);
                    int.TryParse(filesRow["pravna"], out pravna);
                    DateTime.TryParse(filesRow["datum_rodenja"], out datum_rodenja);

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
                        Pravna = pravna

                    });


                }
                foreach (ContactItem item in Contacts)
                {
                }

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    public class ContactItem
    {

        public int Id { get; set; }

        public string Ime { get; set; }
        public string OIB { get; set; }
        public DateTime Datum_rodenja { get; set; }
        public string Adresa { get; set; }
        public string Boraviste { get; set; }
        public string Telefon { get; set; }
        public string Fax { get; set; }
        public string Mobitel { get; set; }
        public string Email { get; set; }
        public string Ostalo { get; set; }
        public string Drzava { get; set; }
        public int Pravna { get; set; }

    }




    public KlijentiViewModel()
	{
        Contacts = new ObservableCollection<ContactItem>();
        GenerateFiles();
        var timer = Application.Current.Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromMilliseconds(200);
        timer.Tick += (s, e) => Refresh();
        timer.Start();

    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    void Refresh()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
         


        }
        );
    }
}