﻿using System;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using SMBLibrary;
using SMBLibrary.Client;
using System.Collections.ObjectModel;
using Syncfusion.DocIO.DLS;
using System.Collections.Generic;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Networking;
using System.Data;
using System.Text;
using System.Security.Cryptography;

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
    //public static class Kodiranje
    //{
    //    public static void Main()
    //    {
    //        string original = "Hello, world!";
    //        byte[] key = GenerateRandomKey();
    //        byte[] iv = GenerateRandomIV();

    //        byte[] encrypted = EncryptString(original, key, iv);
    //        string decrypted = DecryptString(encrypted, key, iv);

    //        Console.WriteLine($"Original: {original}");
    //        Console.WriteLine($"Encrypted: {Convert.ToBase64String(encrypted)}");
    //        Console.WriteLine($"Decrypted: {decrypted}");
    //    }

    //    private static byte[] GenerateRandomKey()
    //    {
    //        using var aes = Aes.Create();
    //        aes.GenerateKey();
    //        return aes.Key;
    //    }

    //    private static byte[] GenerateRandomIV()
    //    {
    //        using var aes = Aes.Create();
    //        aes.GenerateIV();
    //        return aes.IV;
    //    }

    //    private static byte[] EncryptString(string plaintext, byte[] key, byte[] iv)
    //    {
    //        using var aes = Aes.Create();
    //        aes.Key = key;
    //        aes.IV = iv;

    //        var encryptor = aes.CreateEncryptor();
    //        byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
    //        byte[] encryptedBytes = encryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);
    //        return encryptedBytes;
    //    }

    //    private static string DecryptString(byte[] ciphertext, byte[] key, byte[] iv)
    //    {
    //        using var aes = Aes.Create();
    //        aes.Key = key;
    //        aes.IV = iv;

    //        var decryptor = aes.CreateDecryptor();
    //        byte[] decryptedBytes = decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
    //        string plaintext = Encoding.UTF8.GetString(decryptedBytes);
    //        return plaintext;
    //    }
    //}
    public class SMBConnect
    {
        //Varijable za SMB preferences
        private const string IP_nas = "IP Adresa";
        private const string USER_nas = "Korisničko ime";
        private const string PASS_nas = "Lozinka";
        private const string FOLDER_nas = "Folder";
        private const string SUBFOLDER_nas = "SubFolder";


        //public ObservableCollection<DocsItem> Items { get; set; } = new();
        public ObservableCollection<string> ShareFiles { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> ShareFolders { get; set; } = new ObservableCollection<string>();

        public List<QueryDirectoryFileInformation> ListPath(string path)
        {

            try { 
            //Debug.WriteLine("Core.cs -> ListPath -> Usao u ListPath  ****"+ Preferences.Get(IP_nas, "") + "***");
            SMB2Client client = new SMB2Client();
            bool isConnected = client.Connect(System.Net.IPAddress.Parse(TrecaSreca.Get(IP_nas)), SMBTransportType.DirectTCPTransport);
            NTStatus status = client.Login(String.Empty, TrecaSreca.Get(USER_nas), TrecaSreca.Get(PASS_nas));
            //Debug.WriteLine("6666666666666666666");
            //Debug.WriteLine(status);
            //Debug.WriteLine("6666666666666666666");

            ISMBFileStore fileStore = client.TreeConnect(TrecaSreca.Get(FOLDER_nas), out status);
            if (status == NTStatus.STATUS_SUCCESS)
            {
                //Debug.WriteLine("7777777777777777777");
                //Debug.WriteLine(status);
                //Debug.WriteLine("7777777777777777777");
                object directoryHandle;
                FileStatus fileStatus;
                //status = fileStore.CreateFile(out directoryHandle, out fileStatus, String.Empty, AccessMask.GENERIC_READ, SMBLibrary.FileAttributes.Directory, ShareAccess.Read | ShareAccess.Write, CreateDisposition.FILE_OPEN, CreateOptions.FILE_DIRECTORY_FILE, null);
                status = fileStore.CreateFile(out directoryHandle, out fileStatus, TrecaSreca.Get(SUBFOLDER_nas), AccessMask.GENERIC_READ, SMBLibrary.FileAttributes.Directory, ShareAccess.Read | ShareAccess.Write, CreateDisposition.FILE_OPEN, CreateOptions.FILE_DIRECTORY_FILE, null);
                //status = fileStore.CreateFile(out directoryHandle, out fileStatus, "*", AccessMask.SYNCHRONIZE | (AccessMask)DirectoryAccessMask.FILE_LIST_DIRECTORY, 0, ShareAccess.Read | ShareAccess.Write | ShareAccess.Delete, CreateDisposition.FILE_OPEN, CreateOptions.FILE_SYNCHRONOUS_IO_NONALERT | CreateOptions.FILE_DIRECTORY_FILE, null);

                if (status == NTStatus.STATUS_SUCCESS)
                {
                    //Debug.WriteLine("8888888888888888888");
                    //Debug.WriteLine(status);
                    //Debug.WriteLine("8888888888888888888");
                    List<QueryDirectoryFileInformation> fileList;

                    status = fileStore.QueryDirectory(out fileList, directoryHandle, "*", FileInformationClass.FileDirectoryInformation);
                    status = fileStore.CloseFile(directoryHandle);
                    status = fileStore.Disconnect();
                    return fileList;
                }
                else
                {
                    //Debug.WriteLine("9999999999999999999");
                    //Debug.WriteLine(status);
                    //Debug.WriteLine("9999999999999999999");
                    List<QueryDirectoryFileInformation> fileList = new List<QueryDirectoryFileInformation>();
                    return fileList;

                }
            }
            else
            {
                //Debug.WriteLine("10101010010101010101");
                //Debug.WriteLine(status);
                //DisplayAlert("Error", string(status), "OK");
                //Debug.WriteLine("10101010010101010101");
                List<QueryDirectoryFileInformation> fileList = new List<QueryDirectoryFileInformation>();
                return fileList;

            }

            }
            catch (Exception ex)
            {
                // An unexpected error occurred. No browser may be installed on the device.
                return null;
            }

        }
        public List<string> getRootShare()
        {
            try { 
            //INICIRAJ SMB KONEKCIJU DA DOHVATI SVE DOKUMENTE
            //Debug.WriteLine("Core.cs -> getRootShare -> INICIRAJ SMB KONEKCIJU  ****" + Preferences.Get(IP_nas, "") + "***");

            SMB2Client client = new SMB2Client();
            //Debug.WriteLine("Core.cs -> getRootShare -> new SMB2Client()  *******");
            bool isConnected = client.Connect(System.Net.IPAddress.Parse(TrecaSreca.Get(IP_nas)), SMBTransportType.DirectTCPTransport);

            NTStatus status = client.Login(String.Empty, TrecaSreca.Get(USER_nas), TrecaSreca.Get(PASS_nas));
            List<string> shares = client.ListShares(out status);
            if (isConnected)
            {

                //Debug.WriteLine("6666666666666666666");
                //Debug.WriteLine(status);
                //Debug.WriteLine("6666666666666666666");
                if (status == NTStatus.STATUS_SUCCESS)
                {

                    //Debug.WriteLine("7777777777777777777");
                    foreach (string nesto in shares)
                    {
                        ////Debug.WriteLine(nesto);

                    }

                    //Debug.WriteLine("7777777777777777777");
                    client.Logoff();
                }
                client.Disconnect();

            }
            return shares;


            }
            catch (Exception ex)
            {
                // An unexpected error occurred. No browser may be installed on the device.
                return null;
            }

        }


    }

    public class ExternalSQLConnect
    {

        //Varijable za MySQL preferences
        private const string IP_mysql = "IP Adresa2";
        private const string USER_mysql = "Korisničko ime2";
        private const string PASS_mysql = "Lozinka2";
        private const string databasename_mysql = "databasename";


        public string Install(string data)
        {
            string sqlCommands = File.ReadAllText("Resource/Install/odvjetnik_local.sql");

            // Split the SQL commands into individual commands
            IEnumerable<string> commands = sqlCommands.Split(new[] { ";\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string command in commands)
            {
                sqlQuery(command);
            }
            return data;
        }
        public void createDatabase(string[] args)
        {
            // MySQL server connection string
            string connectionString = "Server=" + TrecaSreca.Get(IP_mysql) + ";Port=3306;User=" + TrecaSreca.Get(USER_mysql) + ";Password=" + TrecaSreca.Get(PASS_mysql) + ";";

            try
            {
                // Create a MySqlConnection
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // Create a MySqlCommand to execute SQL commands
                    using (MySqlCommand cmd = connection.CreateCommand())
                    {
                        // Specify the SQL command to create a new database
                        cmd.CommandText = "CREATE DATABASE IF NOT EXISTS `eodvjetnik_install` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;";
                        Debug.WriteLine(cmd.CommandText);
                        cmd.CommandType = CommandType.Text;

                        // Execute the SQL command
                        cmd.ExecuteNonQuery();

                        Console.WriteLine("Database created successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        public Dictionary<string, string>[] sqlQuery(string query)
        {

            try { 
            //Debug.WriteLine("Core.cs -> Dictionary -> Usao u sqlQuerry  *******");
            // MySQL connection settings
            string connString = "server=" + TrecaSreca.Get(IP_mysql) + ";user=" + TrecaSreca.Get(USER_mysql) + ";password=" + TrecaSreca.Get(PASS_mysql) + ";database=" + TrecaSreca.Get(databasename_mysql);

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
            catch (Exception ex)
            {
                // An unexpected error occurred. No browser may be installed on the device.
                return null;
            }

        }

        public void ExecuteSqlFile()
        {
            // MySQL connection settings
            string connString = "server=" + TrecaSreca.Get(IP_mysql) + ";user=" + TrecaSreca.Get(USER_mysql) + ";password=" + TrecaSreca.Get(PASS_mysql) + ";database=" + TrecaSreca.Get(databasename_mysql);

            // Connect to MySQL database
            using MySqlConnection conn = new MySqlConnection(connString);
            conn.Open();

            using MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;

            var assembly = typeof(App).Assembly;
            using (var stream = assembly.GetManifestResourceStream("eOdvjetnik.Resources.Install.odvjetnik_local.sql"))
            {
                Debug.WriteLine("stream name " + stream);
                using (var reader = new StreamReader(stream))
                {
                    try
                    {
                        string sqlBatch = reader.ReadToEnd();
                        cmd.CommandText = sqlBatch;
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error executing SQL: " + ex.Message);
                    }
                }
            }

            // Close the connection
            conn.Close();
        }

    }

}

