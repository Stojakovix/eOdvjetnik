using System;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Text.Json;


namespace eOdvjetnik.Services
{


    public class ConnectivityTest
    {
        public ConnectivityTest() =>
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

        ~ConnectivityTest() =>
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.ConstrainedInternet)
                Console.WriteLine("Internet access is available but is limited.");

            else if (e.NetworkAccess != NetworkAccess.Internet)
                Console.WriteLine("Internet access has been lost.");

            // Log each active connection
            Console.Write("Connections active: ");

            foreach (var item in e.ConnectionProfiles)
            {
                switch (item)
                {
                    case ConnectionProfile.Bluetooth:
                        Console.Write("Bluetooth");
                        break;
                    case ConnectionProfile.Cellular:
                        Console.Write("Cell");
                        break;
                    case ConnectionProfile.Ethernet:
                        Console.Write("Ethernet");
                        break;
                    case ConnectionProfile.WiFi:
                        Console.Write("WiFi");
                        break;
                    default:
                        break;
                }
            }

            Console.WriteLine();
        }
    }


    public class ExternalSQLConnect
	{
        //Varijable za MySQL preferences
        private const string IP_mysql = "IP Adresa2";
        private const string USER_mysql = "Korisničko ime2";
        private const string PASS_mysql = "Lozinka2";
        private const string databasename_mysql = "databasename";


        //public int SqlScalar(string query)
        //{
        //    int result = 0;
        //    string connString = $"server={IP_mysql};user={USER_mysql};password={PASS_mysql};database={databasename_mysql}";
        //    using (MySqlConnection conn = new MySqlConnection(connString))
        //    {
        //        conn.Open();
        //        MySqlCommand cmd = new MySqlCommand(query, conn);
        //        result = (int)cmd.ExecuteScalar();
        //    }
        //    return result;
        //}
        public Dictionary<string, string>[] sqlQuery(string query)
        {

            Debug.WriteLine("Usao u sqlQuerry  *******");
            // MySQL connection settings
            string connString = "server=" + Microsoft.Maui.Storage.Preferences.Get(IP_mysql, "") + ";user=" + Microsoft.Maui.Storage.Preferences.Get(USER_mysql, "") + ";password=" + Microsoft.Maui.Storage.Preferences.Get(PASS_mysql, "") + ";database=" + Microsoft.Maui.Storage.Preferences.Get(databasename_mysql, "");

            // Connect to MySQL database
            using MySqlConnection conn = new MySqlConnection(connString);
            conn.Open();

            // Execute query and retrieve data
            using MySqlCommand cmd = new MySqlCommand(query, conn);
            using MySqlDataReader reader = cmd.ExecuteReader();
            List<Dictionary<string, string>> results = new List<Dictionary<string, string>>();
            
            
                while (reader.Read())
                {
                    Dictionary<string, string> row = new Dictionary<string, string>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string attributeName = reader.GetName(i);
                        string attributeValue = reader[i].ToString();
                        row.Add(attributeName, attributeValue);
                    }

                    results.Add(row);
                }


                // Close the reader and the connection
                reader.Close();
                conn.Close();

                return results.ToArray();
            
            


        }


    }
}

