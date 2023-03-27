using SQLite;
using eOdvjetnik.Models;
using System;
using System.Diagnostics;


namespace eOdvjetnik.Data
{
    public class DeviceIdDatabase
    {
        SQLiteAsyncConnection Database;
        private SQLiteConnection conn;
        
        string _dbPath;

        public DeviceIdDatabase(string dbPath)
        {
            _dbPath = dbPath;
        }
  
        public void Init()
        {
            Debug.WriteLine("-----------------------inicijaliziro bazu");
            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<DeviceIdItem>();
            
        }
        public List<DeviceIdItem> GetLicences() 
        {
            Init();
            return conn.Table<DeviceIdItem>().ToList();
        }
        public void Add(DeviceIdItem item)
        {
            conn = new SQLiteConnection(_dbPath);
            conn.Insert(item);
        }

    }
}
