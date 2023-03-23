using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace eOdvjetnik.Models
{
    public class DeviceIdItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string HID{ get; set; }
        
    }
}
