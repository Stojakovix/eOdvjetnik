using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eOdvjetnik.Models
{
    public class Appointment
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool AllDay { get; set; }
        public string EventName { get; set; }
        public string DescriptionNotes { get; set; }
    }
}
