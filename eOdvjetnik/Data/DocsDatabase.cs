﻿using SQLite;
using eOdvjetnik.Models;

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
            if(Database is not null)
                return;

                Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
                var result = await Database.CreateTableAsync<DocsItem>();
        }

        public async Task<List<DocsItem>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<DocsItem>().ToListAsync();
        }
        public async Task<List<DocsItem>> GetItemsNotDone()
        {
            await Init();
            return await Database.Table<DocsItem>().Where(t => t.Done).ToListAsync();
        }
        public async Task<DocsItem> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<DocsItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }
        public async Task<int> SaveItemAsync(DocsItem item)
        {
            await Init();
            if(item.ID != 0)
            {
                return await Database.UpdateAsync(item);
            }
            else
            {
                return await Database.InsertAsync(item);
            }
        }
        public async Task<int> DeleteItemAsync(DocsItem item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}