using eOdvjetnik.Models;
using SQLite;
using eOdvjetnik.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Internals;
using System.Diagnostics;

namespace eOdvjetnik.Data
{
    public class Prefdatabase
    {
        SQLiteAsyncConnection Database;

        public Prefdatabase() 
        {
            //Database = new SQLiteConnection(dbPath);
            //Database.CreateTable<PrefItem>();
        }

        public async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            _ = await Database.CreateTableAsync<PrefItem>();
            Debug.WriteLine("Baza inicijalizirana --------------------------------------");
            
        }

        public async Task InsertOrUpdate(string key, string value)
        {
            var item = await Database.Table<PrefItem>().FirstOrDefaultAsync(x => x.Key == key);
            if (item == null)
            {
                await Database.InsertAsync(new PrefItem { Key = key, Value = value });
            }
            else
            {
                item.Value = value;
                await Database.UpdateAsync(item);
            }
        }

        public async Task<string> GetValue(string key)
        {
            var item = await Database.Table<PrefItem>().FirstOrDefaultAsync(x => x.Key == key);
            return item?.Value;
        }
    }
}
