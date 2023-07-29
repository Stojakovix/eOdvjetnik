using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eOdvjetnik.Models
{
    public class RootShare
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
