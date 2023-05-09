using System;
using MySql.Data.MySqlClient;
using System.Diagnostics;


namespace eOdvjetnik.Services
{
	public class ExternalSQLConnect
	{
        //Varijable za MySQL preferences
        private const string IP_mysql = "IP Adresa2";
        private const string USER_mysql = "Korisničko ime2";
        private const string PASS_mysql = "Lozinka2";
        private const string databasename_mysql = "databasename";


        public void sqlQuery(string query)
        {
            Debug.WriteLine("Usao u sqlQuerry  *******");
            // MySQL connection settings
            string connString = "server=" + Microsoft.Maui.Storage.Preferences.Get(IP_mysql, "") + ";user=" + Microsoft.Maui.Storage.Preferences.Get(USER_mysql, "") + ";password=" + Microsoft.Maui.Storage.Preferences.Get(PASS_mysql, "") + ";database=" + Microsoft.Maui.Storage.Preferences.Get(databasename_mysql, "");

            // Connect to MySQL database
            using MySqlConnection conn = new MySqlConnection(connString);
            conn.Open();

            // SQL query
            //string query = "SELECT * FROM your_table";

            // Execute query and retrieve data
            using MySqlCommand cmd = new MySqlCommand(query, conn);
            using MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Debug.WriteLine("Usao u while  *******");
                // Access data using column names or indices
                int column1 = reader.GetInt32("ID");
                string column2 = reader.GetString("EventName");

                Debug.WriteLine(column1);
                Debug.WriteLine(column2);

                // ...
            }

            // Close the reader and the connection
            reader.Close();
            conn.Close();
        }

        public ExternalSQLConnect()
		{


		}
	}
}

