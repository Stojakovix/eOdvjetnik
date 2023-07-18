using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eOdvjetnik.Models
{
    public class ActivationData
    {
        public int id { get; set; }
        public DateTime created { get; set; }
        public string hwid { get; set; }
        public string IP { get; set; }
        public string activation_code { get; set; }
    }
}
