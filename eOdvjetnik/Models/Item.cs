using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace eOdvjetnik.Models
{
    public class DocsItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Name { get; set; }
        public int Notes { get; set; }
    }
}
