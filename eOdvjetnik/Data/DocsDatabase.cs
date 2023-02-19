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
            var result = await Database.CreateTableAsync<Item>();
        } 

        public async Task<List<Item>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<Item>().ToListAsync();
        }
        //public async Task<List<Item>> GetItemsNotDoneAsync()
        //{
        //    await Init();
        //    return await Database.Table<Item>().Where(t =>t.Done).ToListAsync();
        //}  *za todo, izbacuje gotove zadatke* može se iskoristit za gotove sastanke
        public async Task<Item> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<Item>().Where(i => i.Id== id).FirstOrDefaultAsync();
        }
        public async Task<int> SaveItemAsync(Item AddItem)
        {
            await Init();
            if (AddItem.Id != 0)
            {
                return await Database.UpdateAsync(AddItem);
            }
            else
            {
                return await Database.InsertAsync(AddItem);
            }
         }
        public async Task <int> DeleteItemAsync(Item DeleteItem)
            {
            await Init();
            return await Database.DeleteAsync(DeleteItem);
            }
    }
}
