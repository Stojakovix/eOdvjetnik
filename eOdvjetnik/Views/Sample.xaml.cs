namespace eOdvjetnik.Views;
using SQLite;

public partial class Sample : ContentPage
{
	public Sample()
	{
		InitializeComponent();

        // Store data
        string data = "data to be stored";
        using (SQLiteConnection conn = new SQLiteConnection(Path.Combine(FileSystem.AppDataDirectory, "mydatabase.db")))
        {
            conn.CreateTable<MyTable>();
            conn.Insert(new MyTable { Data = data });
        }

        // Retrieve data
        using (SQLiteConnection conn = new SQLiteConnection(Path.Combine(FileSystem.AppDataDirectory, "mydatabase.db")))
        {
            MyTable myData = conn.Table<MyTable>().FirstOrDefault();
            string readData = myData?.Data;
        }

    }
    public class MyTable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Data { get; set; }
    }

}

