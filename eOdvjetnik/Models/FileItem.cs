using System;

namespace eOdvjetnik.Model
{
    public class FileItem
    {
        public int Id { get; set; }
        public string BrojSpisa { get; set; }
        public string Spisicol { get; set; }
        public int? ClientId { get; set; }
        public int? OpponentId { get; set; }
        public int? InicijaliVoditeljId { get; set; }
        public string InicijaliDodao { get; set; }
        public string Filescol { get; set; }
        public string InicijaliDodjeljeno { get; set; }
        public DateTime? Created { get; set; }
        public string AktivnoPasivno { get; set; }
        public string Referenca { get; set; }
        public DateTime? DatumPromjeneStatusa { get; set; }
        public string Uzrok { get; set; }
        public DateTime? DatumKreiranjaSpisa { get; set; }
        public DateTime? DatumIzmjeneSpisa { get; set; }
        public string Kreirao { get; set; }
        public string ZadnjeUredio { get; set; }
        public string Jezik { get; set; }
        public string BrojPredmeta { get; set; }
        public string ClientName { get; set; }
        public string OpponentName { get; set; }
    }
}