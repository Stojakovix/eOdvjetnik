﻿using System.Diagnostics;
using System.Windows.Input;

namespace eOdvjetnik.ViewModel
{
    public class MainPageViewModel
    {
        //Varijable za NAS preferenceas
        private const string IP_nas = "IP Adresa";
        private const string USER_nas = "Korisničko ime";
        private const string PASS_nas = "Lozinka";
        private const string FOLDER_nas = "Folder";
        private const string SUBFOLDER_nas = "SubFolder";

        //Varijable za MySQL preferences
        public const string IP_mysql = "IP Adresa2";
        public const string USER_mysql = "Korisničko ime2";
        public const string PASS_mysql = "Lozinka2";
        public const string databasename_mysql = "databasename";
        //MySQL varijable
        public string query;

        //Save za mysql
        public ICommand SaveCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        //Save za nas

        public ICommand SaveCommandNAS { get; set; }
        public ICommand LoadCommandNAS { get; set; }
        public ICommand DeleteCommandNAS { get; set; }
        //Dohvaća vrijednost varijabli iz mainPagea SQL
        public string IP { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }

        //Vrijednost varijabli NAS

        public string IPNas { get; set; }
        public string UserNas { get; set; }
        public string PassNas { get; set; }
        public string Folder { get; set; } 
        public string SubFolder { get; set; }

        public MainPageViewModel()
        {
            SaveCommand = new Command(OnSaveClickedMySQL);
            LoadCommand = new Command(OnLoadClickedMySQL);
            DeleteCommand = new Command(OnDeleteClickedMySQL);

            SaveCommandNAS = new Command(OnSaveClickedNas);
            LoadCommandNAS = new Command(OnLoadClickedNas);
            DeleteCommandNAS = new Command(OnDeleteClickedNas);
        }


        // POČETAK KOMANDI ZA SQL
        private void OnSaveClickedMySQL()
        {
            try
            {
                string ip = IP;
                string password = Password;
                string userName = UserName;
                string databaseName = DatabaseName;

                Preferences.Set(IP_mysql, IP);
                Preferences.Set(USER_mysql, UserName);
                Preferences.Set(PASS_mysql, Password);
                Preferences.Set(databasename_mysql, DatabaseName);


                Debug.WriteLine("Saved");
                Debug.WriteLine(UserName + " " + Password);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void OnLoadClickedMySQL()
        {
            try
            {
                var ipmysql = Preferences.Get(IP, "");
                var usermysql = Preferences.Get(UserName, "");
                var passmysql = Preferences.Get(Password, "");
                var databaseNamemysql = Preferences.Get(databasename_mysql, "");

                Debug.WriteLine("Load uspješan");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "in MainPageViewModel OnLoadSQL");
            }
        }

        private void OnDeleteClickedMySQL()
        {
            Preferences.Remove(IP);
            Preferences.Remove(UserName);
            Preferences.Remove(Password);
            Preferences.Remove(databasename_mysql);
        }

        //KRAJ KOMANDI ZA SQL

        //POČETAK KOMANDI ZA NAS

        private void OnSaveClickedNas()
        {
            try
            {
                string ip_nas = IPNas;
                string pass_nas = PassNas;
                string user_nas = UserNas;
                string folder = Folder;
                string subFolder = SubFolder;

                Preferences.Set(IP_nas, ip_nas);
                Preferences.Set(PASS_nas, pass_nas);
                Preferences.Set(USER_nas, user_nas);
                Preferences.Set(FOLDER_nas, folder);
                Preferences.Set(SUBFOLDER_nas, subFolder);

                Debug.WriteLine("Nas saved");
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "In MainPageViewModel NAS");
            }
        }
        
        private void OnLoadClickedNas() 
        {
            try
            {
                var ipmysql = Preferences.Get(IPNas, "");
                var usermysql = Preferences.Get(UserNas, "");
                var passmysql = Preferences.Get(PassNas, "");
                var folder = Preferences.Get(Folder, "");
                var subfolder = Preferences.Get(SubFolder, "");

                Debug.WriteLine("Load uspješan");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "in MainPageViewModel OnLoadNAS");
            }
        }

        private void OnDeleteClickedNas()
        {
            Preferences.Remove(IPNas);
            Preferences.Remove(UserNas);
            Preferences.Remove(PassNas);
            Preferences.Remove(Folder);
            Preferences.Remove(SubFolder);

            Debug.WriteLine("Succesfully deleted the values");
        }

        //KRAJ NAS KOMANDI
    }
}
