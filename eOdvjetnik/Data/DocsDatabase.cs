using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using eOdvjetnik.Models;
using System.Reflection.Metadata;

namespace eOdvjetnik.Data
{
    public class DocsDatabase
    {
        SQLiteAsyncConnection Database;
        public DocsDatabase()
        {
        }
        async Task Init()
        {
            if (Database is not null)
                return;
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await Database.CreateTableAsync<DocsItem>();
        } 

        public async Task<List<DocsItem>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<DocsItem>().ToListAsync();
        }
        //public async Task<List<Item>> GetItemsNotDoneAsync()
        //{
        //    await Init();
        //    return await Database.Table<Item>().Where(t =>t.Done).ToListAsync();
        //}  *za todo, izbacuje gotove zadatke* može se iskoristit za gotove sastanke
        public async Task<DocsItem> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<DocsItem>().Where(i => i.Id== id).FirstOrDefaultAsync();
        }
        public async Task<int> SaveItemAsync(DocsItem item)
        {
            await Init();
            if (item.Id != 0)
            {
                return await Database.UpdateAsync(item);
            }
            else
            {
                return await Database.InsertAsync(item);
            }
         }
        public async Task <int> DeleteItemAsync(DocsItem item)
            {
            await Init();
            return await Database.DeleteAsync(item);
            }
    }
}
