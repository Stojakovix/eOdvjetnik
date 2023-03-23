using SQLite;
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
            _ = await Database.CreateTableAsync<DocsItem>();
            _ = await Database.CreateTableAsync<DeviceIdItem>();
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

        public async Task<int> SaveLicenceAsync(DeviceIdItem item)
        {

            if (item.ID != 0)
            {
                return await Database.UpdateAsync(item);
            }
            else
            {
                return await Database.InsertAsync(item);
            }
        }
    }
}
