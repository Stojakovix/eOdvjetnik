using System;


namespace eOdvjetnik.Models
{
    public class TariffItem
    {
        public int Id { get; set; }
        public int parent_id { get; set; }
        public string name { get; set; }
        public string oznaka { get; set; }
        public string bodovi { get; set; }
        public string concatenated_name { get; set; }
        public string parent_name { get; set; }
       

    }
}
