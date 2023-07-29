using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eOdvjetnik.Models
{
    public class SpiDokItem
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public string Naziv { get; set; }
        public string Lokacija { get; set; }
        public string DocumentsCol { get; set; }
        public DateTime Datum { get; set; }
        public string InicijaliDodao { get; set; }
        public string InicijaliDodjeljeno { get; set; }
        public string Referenca { get; set; }
        public string Dokument { get; set; }
        public string Biljeska { get; set; }
        public string LinkArt { get; set; }
        public string FileStatus { get; set; }
        public DateTime DatumIzmjeneDokumenta { get; set; }
        public DateTime DatumKreiranjaDokumenta { get; set; }
        public string Neprocitano { get; set; }
        public int BrojPrivitaka { get; set; }
        public int TipDokumenta { get; set; }
        public string NaziviPrivitaka { get; set; }
        public string EmailAdrese { get; set; }
        public string Kreirao { get; set; }
        public string ZadnjeUredio { get; set; }
    }
}
