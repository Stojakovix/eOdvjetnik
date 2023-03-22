using SQLite;
using eOdvjetnik.Models;
using System;

namespace eOdvjetnik.Data
{
    public class DeviceIdDatabase
    {
        SQLiteAsyncConnection Database;
        public const string DatabaseFilename = "DocsDatabase.db3";
        public static string dbLocation = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        public static string DatabasePath =>
        Path.Combine(dbLocation, DatabaseFilename);

        public DeviceIdDatabase()
        {

        }
        
        public async Task Init()
        {
            
            if (Database is not null)
                return;
            Database = new SQLiteAsyncConnection("C:\\Users\\robi\\Source\\Repos\\eOdvjetnik\\eOdvjetnik", Constants.Flags);
            _ = await Database.CreateTableAsync<DeviceIdItem>();
        }
        public async Task<List<DeviceIdItem>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<DeviceIdItem>().ToListAsync();
        }
        public async Task<int> SaveItemAsync(DeviceIdItem deviceIdItem)
        {
            await Init();
            if(deviceIdItem.ID != 0)
            {
                return await Database.UpdateAsync(deviceIdItem);
                
            }
            else
            {
                return await Database.InsertAsync(deviceIdItem);
            }
        }


    }
}
