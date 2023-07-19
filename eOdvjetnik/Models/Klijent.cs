using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eOdvjetnik.Models
{
    public class Klijent
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string OIB { get; set; }
        public string Adresa { get; set; }
        public string Ostalo { get; set; }
        public string Boraviste { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Mob { get; set; }
        public string Email { get; set; }
        public string Drzava { get; set; }
        public DateTime? Datum { get; set; }
        public string Pravna { get; set; }
    }
}
