using System;


namespace eOdvjetnik.Models
{
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
}
