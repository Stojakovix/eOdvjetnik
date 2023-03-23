using SQLite;
using eOdvjetnik.Models;
using System;
using System.Diagnostics;

namespace eOdvjetnik.Data
{
    public class DeviceIdDatabase
    {
        SQLiteAsyncConnection Database;
        public const string DatabaseFilename = "Licence.db3";
        public static string dbLocation = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        public static string DatabasePath =>
        Path.Combine(dbLocation, DatabaseFilename);

        public DeviceIdDatabase()
        {

        }
  
        public async Task Init()
        {
            Debug.WriteLine("-----------------------inicijaliziro bazu");
            if (Database is not null)
                return;
            Database = new SQLiteAsyncConnection(Environment.GetFolderPath(Environment.SpecialFolder.Personal), Constants.Flags);
            _ = await Database.CreateTableAsync<DeviceIdItem>();
        }
        public async Task<List<DeviceIdItem>> GetItemsAsync()
        {
            await Init();
            try
            {
                return await Database.Table<DeviceIdItem>().ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to receive data. {ex.Message}");
            }
            return new List<DeviceIdItem>();
        }
        public async Task<int> SaveItemAsync(DeviceIdItem deviceIdItem)
        {
            await Init();
            if(deviceIdItem.ID != 0)
            {
                Debug.WriteLine("Uspješno dodano u bazu");
                return await Database.UpdateAsync(deviceIdItem);
                
            }
            else
            {
                Debug.WriteLine("NeUspješno dodano u bazu");
                return await Database.InsertAsync(deviceIdItem);
            }
        }


    }
}
