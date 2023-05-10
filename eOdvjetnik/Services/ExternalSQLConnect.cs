using System;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Text.Json;


namespace eOdvjetnik.Services
{
	public class ExternalSQLConnect
	{
        //Varijable za MySQL preferences
        private const string IP_mysql = "IP Adresa2";
        private const string USER_mysql = "Korisničko ime2";
        private const string PASS_mysql = "Lozinka2";
        private const string databasename_mysql = "databasename";


        public string[][] sqlQuery(string query)
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
            List<string[]> results = new List<string[]>();

            while (reader.Read())
            {
                string[] row = new string[reader.FieldCount];

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[i] = reader[i].ToString();
                }

                results.Add(row);
            }



            // Close the reader and the connection
            reader.Close();
            conn.Close();

            return results.ToArray();

            
        }

        public ExternalSQLConnect()
		{


		}
	}
}

