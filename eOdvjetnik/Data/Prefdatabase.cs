using eOdvjetnik.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eOdvjetnik.Data
{
    public class Prefdatabase : IDisposable
    {
        private readonly SQLiteConnection _connection;

        public Prefdatabase(string dbPath) 
        {
            _connection = new SQLiteConnection(dbPath);
            _connection.CreateTable<PrefItem>();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public void InsertOrUpdate(string key, string value)
        {
            var item = _connection.Table<PrefItem>().FirstOrDefault(x => x.Key == key);
            if (item == null)
            {
                _connection.Insert(new PrefItem { Key = key, Value = value });
            }
            else
            {
                item.Value = value;
                _connection.Update(item);
            }
        }

        public string GetValue(string key)
        {
            var item = _connection.Table<PrefItem>().FirstOrDefault(x => x.Key == key);
            return item?.Value;
        }
    }
}
