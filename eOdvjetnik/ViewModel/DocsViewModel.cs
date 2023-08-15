﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using eOdvjetnik.Data;
using eOdvjetnik.Models;
using SMBLibrary.Client;
using SMBLibrary;
using eOdvjetnik.Services;
using Microsoft.Maui.Storage;
using System.Windows.Input;
using System.Linq;
using System.Reflection;
using System;
using System.IO;
using Microsoft.Maui.Controls;




namespace eOdvjetnik.ViewModel
{
    public class DocsViewModel : INotifyPropertyChanged
    {
        private Navigacija navigacija;
        SMBConnect sMBConnect = new SMBConnect();

        private ObservableCollection<RootShare> rootShares;
        public ObservableCollection<RootShare> RootShares
        {
            get => rootShares;
            set
            {
                rootShares = value;
                OnPropertyChanged(nameof(RootShares));
            }
        }

        private ObservableCollection<DocsItem> items;
        public ObservableCollection<DocsItem> Items
        {
            get => items;
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private const string IP = "IP Adresa";
        private const string USER = "Korisničko ime";
        private const string PASS = "Lozinka";
        private const string FOLDER = "Folder";
        private const string SUBFOLDER = "SubFolder";

        public DocsViewModel()
        {
            navigacija = new Navigacija();
            Items = new ObservableCollection<DocsItem>();
            RootShares = new ObservableCollection<RootShare>();
            ConnectAndFetchDocumentsAsync();
            

        }

        public void GetDocuments()
        {
            try
            {
                ConnectAndFetchDocumentsAsync();
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.Message);
            }
        }
        public void ConnectAndFetchDocumentsAsync()
        {
            try
            {
                //Ispis svega na putanji
                List<QueryDirectoryFileInformation> fileList = sMBConnect.ListPath(@"Users");

                //Root share list
                List<string> shares = sMBConnect.getRootShare();


                foreach (FileDirectoryInformation file in fileList)
                {
                    Debug.WriteLine($"Filename: {file.FileName}");
                    Debug.WriteLine($"File Attributes: {file.FileAttributes}");
                    Debug.WriteLine($"File -------: {file.NextEntryOffset}");
                    Debug.WriteLine($"File Size: {file.AllocationSize / 1024}KB");
                    Debug.WriteLine($"Created Date: {file.CreationTime.ToString("f")}");
                    Debug.WriteLine($"Last Modified Date: {file.LastWriteTime.ToString("f")}");
                    Debug.WriteLine("----------End of Folder/file-----------");
                    Debug.WriteLine("---------------------Before foreach");

                    if (file.FileName == "." || file.FileName == "..") 
                    {

                    }
                    else { 
                        var icon = "blank.png";

                        if (file.FileAttributes.ToString("f") == "Directory")
                        {
                            icon = "folder_1484.png";
                        }
                        else {

                                icon = Path.GetExtension(file.FileName).TrimStart('.') + ".png";
                            string imagePath = Path.Combine(AppContext.BaseDirectory, icon);
                            //Debug.WriteLine("Checking image path: " + imagePath);

                            bool imageExists = File.Exists(imagePath);

                            if (imageExists)
                            {
                                // Image exists, do something
                                icon = Path.GetExtension(file.FileName).TrimStart('.') + ".png";
                                //Debug.WriteLine("Image exists!");
                            }
                            else
                            {
                                // Image doesn't exist, do something else
                                icon = "blank.png";
                                //Debug.WriteLine("Image does not exist!");
                            }




                        }

                        DocsItem fileData = new DocsItem
                        {
                            Name = file.FileName,
                            Changed = file.CreationTime,
                            Icon = icon,

                        };
                        items.Add(fileData);

                    }
                  

                }

                foreach (var file1 in shares)
                {
                    //Debug.WriteLine(file1.Length.ToString());
                    //Debug.WriteLine(file1.ToString());
                    //ShareFiles.Add(file1.ToString());
                    //Debug.WriteLine("44444444444444");

                    RootShare fileData = new RootShare
                    {
                        Name = file1.ToString(),

                    };
                    rootShares.Add(fileData);
                }

                //Debug.WriteLine("----------------------------------------------------------------");

            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.StackTrace);

            }

        }
        public async Task OpenFile(string fileName)
        {
            try
            {
                var fileUri = new Uri($"smb://{Preferences.Get(IP, "")}/{Preferences.Get(FOLDER, "")}/{fileName}");
                await Launcher.OpenAsync(fileUri);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Unable to open the file: {ex.Message}", "OK");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
