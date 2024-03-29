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
using Org.BouncyCastle.Asn1.BC;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;

namespace eOdvjetnik.ViewModel
{
    public class DocsViewModel : INotifyPropertyChanged
    {

        //Varijable za SMB preferences
        private const string IP_nas = "IP Adresa";
        private const string USER_nas = "Korisničko ime";
        private const string PASS_nas = "Lozinka";
        private const string FOLDER_nas = "Folder";
        private const string SUBFOLDER_nas = "SubFolder";

        public string FileName { get; set; }

        SMBConnect sMBConnect = new SMBConnect();
        public ICommand RefreshButton { get; set; }
        public ICommand ItemClicked { get; set; }
        public ICommand OtvoriClicked { get; set; }
        public ICommand NazadClicked { get; set; }
        public ICommand HomeClicked { get; set; }

        public ObservableCollection<RootShare> rootShares;
        public ObservableCollection<RootShare> RootShares
        {
            get => rootShares;
            //set
            //{
            //    rootShares = value;
            //    OnPropertyChanged(nameof(RootShares));
            //}
        }

        private string textEntry;

        public string TextEntry
        {
            get => textEntry;
            set
            {
                textEntry = value;
                OnPropertyChanged(nameof(TextEntry));
            }
        }
        

        public ObservableCollection<DocsItem> items;


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

            //ItemClicked = new Command(itemClicked);
            RefreshButton = new Command(RefreshButtonClick);
            Items = new ObservableCollection<DocsItem>();
            OtvoriClicked = new Command(OtvoriButtonCLick);
            NazadClicked = new Command(NazadButtonClick);
            HomeClicked = new Command(HomeButtonClick);

            //RootShares = new ObservableCollection<RootShare>();
            ConnectAndFetchDocumentsAsync();


        }
        public void RefreshButtonClick()
        {

            //RootShares.Clear();
            ConnectAndFetchDocumentsAsync();
            //Items.Clear();
            //rootShares.Clear();


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
        public void OtvoriButtonCLick()
        {
            try
            {
                

                string subfolder_nas = textEntry;
                string[] parts = subfolder_nas.Split('\\');
                string lastPart = parts[parts.Length - 1];

                Debug.WriteLine("Subfolder iz text entrya  " + lastPart + " " + TextEntry);

                TrecaSreca.Set(SUBFOLDER_nas, lastPart);

                ConnectAndFetchDocumentsAsync();
                

            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void NazadButtonClick()
        {
            try
            {
                TrecaSreca.Remove(SUBFOLDER_nas);
                string subfolder_nas = textEntry;

                int lastBackslashIndex = subfolder_nas.LastIndexOf('\\');

                if (lastBackslashIndex >= 0)
                {
                    string trimmedString = subfolder_nas.Substring(0, lastBackslashIndex + 1);
                    TrecaSreca.Set(SUBFOLDER_nas, trimmedString);
                    TextEntry = trimmedString;
                    Debug.WriteLine(TextEntry + " " + trimmedString + " u NazadButton clickedu");
                    OtvoriButtonCLick();

                    // trimmedString will contain "example\\path\\to"
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void HomeButtonClick()
        {
            try
            {
                TrecaSreca.Remove(SUBFOLDER_nas);
                string folder_nas = "\\" + TrecaSreca.Get(FOLDER_nas) + "\\";
                Debug.WriteLine(folder_nas);

                string subfolder_nas = TrecaSreca.Get(SUBFOLDER_nas);
                string empty = " ";
                TrecaSreca.Set(SUBFOLDER_nas, empty);

                
                TextEntry = folder_nas;
                OtvoriButtonCLick();
            }
            catch (Exception ex)
            {
                
            }
        }

        public void ConnectAndFetchDocumentsAsync()
        {
            try
            {


                items.Clear();
                if (Items == null) { } else { Items.Clear(); }
                if (rootShares == null) { } else { rootShares.Clear(); }
                if (RootShares == null) { } else { RootShares.Clear(); }
                if (RootShares == null) { } else { RootShares.Clear(); }
                
                string subfolder_nas = TrecaSreca.Get(SUBFOLDER_nas);
                
                Debug.WriteLine(subfolder_nas);
                //Ispis svega na putanji
                List<QueryDirectoryFileInformation> fileList = sMBConnect.ListPath(subfolder_nas);

                //Root share list
                List<string> shares = sMBConnect.getRootShare();

                //List png
                List<string> resourceNames = new List<string>();

                Assembly assembly = typeof(DocsViewModel).Assembly;
                
                string resourceNamePrefix = "eOdvjetnik.Resources"; // Replace with your app's actual namespace and "Resources." prefix
                string[] allResourceNames = assembly.GetManifestResourceNames();
                resourceNames.AddRange(allResourceNames.Where(name => name.StartsWith(resourceNamePrefix)));
                
                //foreach (string resourceName in resourceNames)
                //{
                //    Debug.WriteLine("---------------------**********resourceNames***********---------------------------");
                //    Debug.WriteLine(resourceName);
                //}
                //Kraj List png

                foreach (FileDirectoryInformation file in fileList)
                {
                    //Debug.WriteLine("---------------------**********" + file.FileName + "***********---------------------------");
                    //Debug.WriteLine($"Filename: {file.FileName}");
                    //Debug.WriteLine($"File Attributes: {file.FileAttributes}");
                    //Debug.WriteLine($"File -------: {file.NextEntryOffset}");
                    //Debug.WriteLine($"File Size: {file.AllocationSize / 1024}KB");
                    //Debug.WriteLine($"Created Date: {file.CreationTime.ToString("f")}");
                    //Debug.WriteLine($"Last Modified Date: {file.LastWriteTime.ToString("f")}");
                    //Debug.WriteLine("----------End of Folder/file-----------");
                    //Debug.WriteLine("---------------------Before foreach");



                    var icon = "blank.png";
                    //bool imageExists = resourceNames.Contains("eOdvjetnik.Resources.Images." + icon);

                    if (file.FileName != null)
                    {
                        if (file.FileAttributes.ToString("f") == "Directory" || file.FileAttributes.ToString("f") == "ReadOnly, Directory")
                        {
                            icon = "folder_1484.png";
                        }

                        else
                        {

                            icon = Path.GetExtension(file.FileName).TrimStart('.') + ".png";
                            Debug.WriteLine(file.FileName+" Ikona -> " + icon);

                            bool imageExists = resourceNames.Contains("eOdvjetnik.Resources.Images." + icon);

                            if (imageExists)
                            {
                                icon = Path.GetExtension(file.FileName).TrimStart('.') + ".png";
                                //Debug.WriteLine("+++Slika postoji " + icon);

                            }
                            else
                            {
                                icon = "blank.png";
                                //Debug.WriteLine("---Slika ne postoji " + icon);
                            }

                        }

                    }
                    else
                    {
                        icon = "blank.png";
                        Debug.WriteLine("izvršen zadnji else " + icon);
                    }
                    DocsItem fileData = new DocsItem
                    {
                        Name = file.FileName,
                        Changed = file.LastWriteTime,
                        Icon = icon,
                        Created = file.CreationTime,
                        Size = $"{file.AllocationSize / 1024} KB",
                       // ext = file.FileInformationClass.ToString

                    };
                    items.Add(fileData);
                }
                Debug.WriteLine(items.Count());
                //items.Clear();
                //Items = new ObservableCollection<DocsItem>();
                Debug.WriteLine("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
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
                    //rootShares.Add(fileData);
                    fileList.Clear();
                    fileData = null;

                }

                //Debug.WriteLine("----------------------------------------------------------------");

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
            //items.Clear();
            //Items.Clear();
        }
        public async Task OpenFile(string fileName)
        {
            try
            {
                //var fileUri = new Uri($"smb://{Preferences.Get(IP, "")}I/{Preferences.Get(FOLDER, "")}/{fileName}");
                //await Launcher.OpenAsync(fileUri);

                Debug.WriteLine(DeviceInfo.Platform);
                Debug.WriteLine(DeviceInfo.Platform);
                Debug.WriteLine(DeviceInfo.Platform);
                Debug.WriteLine(DeviceInfo.Platform);
                Debug.WriteLine(DeviceInfo.Platform);
                var devicePlatform = DeviceInfo.Platform.ToString();
                if (devicePlatform == "MacCatalyst")
                {
                    // Code to execute on macOS
                    Console.WriteLine("Running on macOS");

                    //string filePath = @"/Volumes/" + Preferences.Get(FOLDER_nas, "") + "/" + Preferences.Get(SUBFOLDER_nas, "") + "/" + fileName;
                    string ip = TrecaSreca.Get(IP);
                    string folder = TrecaSreca.Get(FOLDER_nas);
                    string subfolder = TrecaSreca.Get(SUBFOLDER_nas);
                    string filePath = @"smb://" + ip + "/" + folder + "/" + subfolder + "/" + fileName;

                    Debug.WriteLine(filePath);
                    await Launcher.OpenAsync(filePath);
                }
                else
                {
                    // Code to execute on other platforms
                    Console.WriteLine("Running on a platform other than macOS");
                    string ip = TrecaSreca.Get(IP);
                    string folder = TrecaSreca.Get(FOLDER_nas);
                    string subfolder = TrecaSreca.Get(SUBFOLDER_nas);
                    string filePath = @"smb://" + ip + "/" + folder + "/" + subfolder + "/" + fileName;
                    Debug.WriteLine(filePath);
                    await Launcher.OpenAsync(filePath);
                }

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Unable to open the file: {ex.Message}", "OK");
            }
        }

        //public async void itemClicked()
        //{
        //    await OpenFile(fileName);
        //}




        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));



        }
    }
}
