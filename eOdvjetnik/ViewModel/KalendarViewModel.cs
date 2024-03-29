﻿using CommunityToolkit.Mvvm.Messaging;
using eOdvjetnik.Model;
using eOdvjetnik.Models;
using eOdvjetnik.Services;
using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Color = Microsoft.Maui.Graphics.Color;


namespace eOdvjetnik.ViewModel
{
    public class KalendarViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<SchedulerAppointment> appointments;
        private SpisiViewModel spisiViewModel = new SpisiViewModel();

        private ObservableCollection<FileItem> fileItems;
        public ObservableCollection<FileItem> FileItems
        {
            get { return fileItems; }
            set
            {
                if (fileItems != value)
                {
                    fileItems = value;
                    OnPropertyChanged(nameof(FileItems));
                }
            }
        }


        private ObservableCollection<AdminCalendarItem> adminAppointments;
        public ObservableCollection<AdminCalendarItem> AdminAppointments
        {
            get
            {
                return adminAppointments;
            }
            set
            {
                adminAppointments = value;
                this.RaiseOnPropertyChanged(nameof(AdminAppointments));
            }
        }
        public ICommand AdminViewByDate { get; set; }
        public ICommand AdminViewByName { get; set; }

        public ICommand DodajButtonClick { get; set; }


        //public bool SpisiGridIsVisible { get; set; }

        private bool spisiGridIsVisible;
        public bool SpisiGridIsVisible
        {
            get { return spisiGridIsVisible; }
            set
            {
                if (spisiGridIsVisible != value)
                {
                    spisiGridIsVisible = value;
                    OnPropertyChanged(nameof(SpisiGridIsVisible));
                }
            }
        }



        //private bool showBusyIndicator;
        //private bool AdminTrue { get; set; }
        //public ICommand QueryAppointmentsCommand { get; set; }

        //public bool ShowBusyIndicator
        //{
        //    get { return showBusyIndicator; }
        //    set
        //    {
        //        showBusyIndicator = value;
        //        this.RaiseOnPropertyChanged(nameof(ShowBusyIndicator));
        //    }
        //}
        public ObservableCollection<SchedulerAppointment> Appointments
        {
            get
            {
                return appointments;
            }
            set
            {
                appointments = value;
                this.RaiseOnPropertyChanged(nameof(Appointments));
            }
        }

        private ObservableCollection<EmployeeItem> employeeItem;
        public ObservableCollection<EmployeeItem> EmployeeItems
        {
            get { return employeeItem; }
            set
            {
                if (employeeItem != value)
                {
                    employeeItem = value;
                    OnPropertyChanged(nameof(EmployeeItems));
                }
            }
        }
        public string SQLUserID { get; set; }
        //Resources - 1
        public ObservableCollection<SchedulerResource> Resources { get; set; }


        ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
        #region Colors
        private ObservableCollection<ColorItem> _CategoryColor;
        public ObservableCollection<ColorItem> CategoryColor
        {
            get { return _CategoryColor; }
            set { _CategoryColor = value; }
        }




        public void GetColors()
        {
            try
            {
                if (_CategoryColor != null)
                {
                    _CategoryColor.Clear();
                }
                string query = "SELECT * FROM `event_colors`";
                Debug.WriteLine(query);
                try
                {
                    Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
                    Debug.WriteLine(query + " u Search resultu");
                    if (filesData != null)
                    {
                        foreach (Dictionary<string, string> filesRow in filesData)
                        {

                            int id;
                            int.TryParse(filesRow["id"], out id);


                            _CategoryColor.Add(new ColorItem()
                            {
                                Id = id,
                                NazivBoje = filesRow["naziv_boje"],
                                Boja = filesRow["boja"],
                                VrstaDogadaja = filesRow["vrsta_dogadaja"],

                            });

                        }

                    }
                    Debug.WriteLine("Dohvatio boje");

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + " u get collorsu u prvom tryu");

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "u get collorsu u drugom tryu");

            }
        }

        #endregion

        public void AdminLicenceCheck()
        {
            EmployeeItems.Clear();
            Appointments.Clear();
            //AdminAppointments.Clear();
            Resources.Clear();
            string licence_type = TrecaSreca.Get("licence_type");
            int numberOfCharacters = 5;
            string adminCheck = licence_type.Substring(0, Math.Min(licence_type.Length, numberOfCharacters));
            Debug.WriteLine("Kalendar ResourceView - 'Admin' provjera: " + adminCheck);
            if (adminCheck == "Admin" || adminCheck == "Trial")
            {
                GetEmployees();
                Debug.WriteLine("izvršen get employees");
            }
            else
            {
                GetUserEvents();
                Debug.WriteLine("izvršen get user events");

            }

        }

        public bool DefaultPicker { get; set; }
        public bool MacBookPicker { get; set; }
        public string DevicePlatform { get; set; }





        public KalendarViewModel()
        {
            //    AdminViewByDate = new Command(GetAdminCalendarEventsByDate);
            //    AdminViewByName = new Command(GetAdminCalendarEventsByName);
            //    AdminAppointments = new ObservableCollection<AdminCalendarItem>();
            WeakReferenceMessenger.Default.Register<AppointmentSpisId>(this, AppointmentSpisIdReceived);
            SQLUserID = TrecaSreca.Get("UserID");
            Debug.WriteLine(" user id je " + SQLUserID);
            try
            {

                Appointments = new ObservableCollection<SchedulerAppointment>(); // Initialize the Appointments collection
                CategoryColor = new ObservableCollection<ColorItem>();
                employeeItem = new ObservableCollection<EmployeeItem>();
                Resources = new ObservableCollection<SchedulerResource>();
                FileItems = new ObservableCollection<FileItem>();
                DodajButtonClick = new Command(DodajSpis_clicked);

                

                GetColors();
                AdminLicenceCheck();
                //GenerateFiles();
                //GetAdminCalendarEventsByDate();
                //this.QueryAppointmentsCommand = new Command<Object(LoadMoreAppointments, CanLoadMoreAppointments);
                Debug.WriteLine("---------------------inicijalizirano kalendarViewModel constructor");

                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "in kalendarViewModel constructor");
            }
            DevicePlatform = TrecaSreca.Get("vrsta_platforme");
            if (DevicePlatform == "MacCatalyst") ///MacBookPicker kontrola 
            {
                MacBookPicker = true;
                DefaultPicker = false; 
            }
            else
            {
                MacBookPicker = false;
                DefaultPicker = true;
            }
        }

        //private bool CanLoadMoreAppointments(object obj)
        //{
        //    return true;
        //}
        //private async void LoadMoreAppointments(object obj)
        //{
        //    this.ShowBusyIndicator = true;
        //    await Task.Delay(1500);
        //    if ()
        //}
        public void GetEmployees()
        {
            try
            {
                if (employeeItem != null)
                {
                    employeeItem.Clear();
                    Debug.WriteLine("Cleared employee item collection");

                }


                string query = "SELECT id, ime, inicijali, hwid, active, type FROM employees;";


                // Debug.WriteLine(query + "u SpisiViewModelu");
                Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
                if (filesData != null)
                {
                    foreach (Dictionary<string, string> filesRow in filesData)
                    {
                        #region Varijable za listu
                        int id;
                        int active;
                        int type;

                        int.TryParse(filesRow["id"], out id);
                        int.TryParse(filesRow["inicijali"], out active);
                        int.TryParse(filesRow["active"], out type);

                        #endregion

                        var employee = new EmployeeItem()
                        {
                            Id = id,
                            EmployeeName = filesRow["ime"],
                            EmployeeHWID = filesRow["hwid"],
                            Initals = filesRow["inicijali"],
                            Active = active,
                            Type = type,
                        };

                        employeeItem.Add(employee);
                    }
                    foreach (EmployeeItem employee in employeeItem)
                    {
                        Resources.Add(new SchedulerResource()
                        {
                            Name = employee.EmployeeName,
                            Foreground = Colors.Black,
                            Background = Colors.Transparent,
                            Id = employee.Id
                        });


                        // Debug.Write(employee.EmployeeName + " id " + employee.Id + ", ");
                    }

                    //foreach (SchedulerResource resource in Resources)
                    //{
                    //    Debug.WriteLine(resource.Name + ", " + resource.Id);

                    //}


                    OnPropertyChanged(nameof(employeeItem));
                    GetAllEvents();

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "in viewModel generate files");
            }
        }

        private void DodajSpis_clicked()
        {
            // GenerateFiles();
            if (SpisiGridIsVisible == false)
            {
                SpisiGridIsVisible = true;
            }
            else
            {
                SpisiGridIsVisible = false;
            }
        }

        private void Zatvori_clicked()
        {
            SpisiGridIsVisible = false;
        }


        //public void GenerateFiles()
        //{
        //    try
        //    {
        //        fileItems.Clear();

        //        //string query = "SELECT * FROM files ORDER BY id DESC LIMIT 100;";
        //        string query = "SELECT files.*, client.ime AS client_name, opponent.ime AS opponent_name FROM files LEFT JOIN contacts AS client ON files.client_id = client.id LEFT JOIN contacts AS opponent ON files.opponent_id = opponent.id ORDER BY files.id DESC LIMIT 20";


        //        Debug.WriteLine(query + "u SpisiViewModelu");
        //        Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
        //        if (filesData != null)
        //        {
        //            foreach (Dictionary<string, string> filesRow in filesData)
        //            {
        //                #region Varijable za listu
        //                int id;
        //                int clientId;
        //                int opponentId;
        //                int inicijaliVoditeljId;
        //                DateTime created;
        //                DateTime datumPromjeneStatusa;
        //                DateTime datumKreiranjaSpisa;
        //                DateTime datumIzmjeneSpisa;

        //                int.TryParse(filesRow["id"], out id);
        //                int.TryParse(filesRow["client_id"], out clientId);
        //                int.TryParse(filesRow["opponent_id"], out opponentId);
        //                int.TryParse(filesRow["inicijali_voditelj_id"], out inicijaliVoditeljId);
        //                DateTime.TryParse(filesRow["created"], out created);
        //                DateTime.TryParse(filesRow["datum_promjene_statusa"], out datumPromjeneStatusa);
        //                DateTime.TryParse(filesRow["datum_kreiranja_spisa"], out datumKreiranjaSpisa);
        //                DateTime.TryParse(filesRow["datum_izmjene_spisa"], out datumIzmjeneSpisa);
        //                #endregion
        //                fileItems.Add(new FileItem()
        //                {
        //                    Id = id,
        //                    BrojSpisa = filesRow["broj_spisa"],
        //                    Spisicol = filesRow["spisicol"],
        //                    ClientId = clientId,
        //                    OpponentId = opponentId,
        //                    InicijaliVoditeljId = inicijaliVoditeljId,
        //                    InicijaliDodao = filesRow["inicijali_dodao"],
        //                    Filescol = filesRow["filescol"],
        //                    InicijaliDodjeljeno = filesRow["inicijali_dodjeljeno"],
        //                    Created = created,
        //                    AktivnoPasivno = filesRow["aktivno_pasivno"],
        //                    Referenca = filesRow["referenca"],
        //                    DatumPromjeneStatusa = datumPromjeneStatusa,
        //                    Uzrok = filesRow["uzrok"],
        //                    DatumKreiranjaSpisa = datumKreiranjaSpisa,
        //                    DatumIzmjeneSpisa = datumIzmjeneSpisa,
        //                    Kreirao = filesRow["kreirao"],
        //                    ZadnjeUredio = filesRow["zadnje_uredio"],
        //                    Jezik = filesRow["jezik"],
        //                    BrojPredmeta = filesRow["broj_predmeta"],
        //                    ClientName = filesRow["client_name"],
        //                    OpponentName = filesRow["opponent_name"]
        //                });
        //                foreach (FileItem item in fileItems)
        //                {
        //                    //Debug.WriteLine(item.BrojSpisa);

        //                }
        //                // Debug.WriteLine(FileItems.Count);

        //            }
        //            OnPropertyChanged(nameof(fileItems));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message + "in viewModel generate files");
        //    }
        //}


          #region Search

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                }
            }
        }
        private ICommand searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (searchCommand == null)
                {
                    searchCommand = new Command(GenerateSearchResults);
                }
                return searchCommand;
            }
        }
        //public ICommand OnResetClick { get; set; }

        //public void ResetListView()
        //{
        //    fileItems.Clear();
        //    foreach (var item in initialFileItems)
        //    {
        //        fileItems.Add(item);
        //    }
        //}


        public void GenerateSearchResults()
        {
            try
            {
                fileItems.Clear();

                // parametri za pretragu broj spisa, klijent id, opponent id, referenca, uzrok, broj predmeta
                string search_term = SearchText;
                string escaped_search_term = search_term.Replace("/", "\\/");
                //string query = "SELECT * FROM `files` WHERE `broj_spisa` LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci  or `referenca` LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci or `uzrok` LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci  or `broj_predmeta` LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci ORDER BY `broj_predmeta` DESC";
                //string query = "SELECT files.*, client.ime AS client_name, opponent.ime AS opponent_name FROM files LEFT JOIN contacts AS client ON files.client_id = client.id LEFT JOIN contacts AS opponent ON files.opponent_id = opponent.id WHERE files.broj_spisa LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR files.referenca LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR files.uzrok LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR files.broj_predmeta LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci ORDER BY files.broj_predmeta DESC";
                string query = "SELECT files.*, client.ime AS client_name, opponent.ime AS opponent_name FROM files LEFT JOIN contacts AS client ON files.client_id = client.id LEFT JOIN contacts AS opponent ON files.opponent_id = opponent.id WHERE files.broj_spisa LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR files.referenca LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR files.uzrok LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR files.broj_predmeta LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR client.ime LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR opponent.ime LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR files.client_id IN (SELECT id FROM contacts WHERE ime LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci) OR files.opponent_id IN (SELECT id FROM contacts WHERE ime LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci) ORDER BY files.id DESC LIMIT 20";
                
                // Debug.WriteLine(query);
                Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
                Debug.WriteLine(query + " u Search resultu");
                if (filesData != null)
                {
                    foreach (Dictionary<string, string> filesRow in filesData)
                    {

                        #region Varijable za listu
                        int id;


                        int.TryParse(filesRow["id"], out id);

                        #endregion
                        fileItems.Add(new FileItem()
                        {
                            Id = id,
                            BrojSpisa = filesRow["broj_spisa"],
                            ClientName = filesRow["client_name"],
                        });


                    }
                    //Debug.Assert(fileItems.Count() == 0);

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "u generate search resultu");
            }
        }
        #endregion






        public void GetAllEvents()
        {
            try
            {
                Appointments.Clear();
                var hardware_id = TrecaSreca.Get("key");
                ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
                List<int> ExternalEventIDs = new List<int>();
                List<int> InternalEventIDs = new List<int>();
                string query = "Select * from events;";

                Debug.WriteLine(query);

                //Rezultat query-ja u apointmentData
                Dictionary<string, string>[] appointmentData = externalSQLConnect.sqlQuery(query);
                if (appointmentData != null)
                {
                    //Debug.WriteLine("Dohvatio evente ----------------------------------**");

                    //Externa lista bez internih
                    List<int> ExtDifference = ExternalEventIDs.Except(InternalEventIDs).ToList();

                    foreach (Dictionary<string, string> appointmentRow in appointmentData)
                    {
                        // Add new appointment

                        string colorName = appointmentRow["color"];
                        if (colorName == null || colorName == "")
                        {
                            colorName = "LightGray";
                        }
                        //Debug.WriteLine("-------------------------------------------------------------- " + colorName + " 0000000000000000");
                        Color backgroundColor = (Color)TypeDescriptor.GetConverter(typeof(Color)).ConvertFromString(colorName);
                        int reminderDuration = int.Parse(appointmentRow["ReminderDuration"]);
                        TimeSpan ReminderTime = TimeSpan.FromMinutes(reminderDuration);



                        try
                        {
                            Appointments.Add(new SchedulerAppointment()
                            {
                                Id = int.Parse(appointmentRow["internal_event_id"]),
                                StartTime = DateTime.Parse(appointmentRow["TimeFrom"]),
                                EndTime = DateTime.Parse(appointmentRow["TimeTo"]),
                                Subject = appointmentRow["EventName"],
                                ResourceIds = new ObservableCollection<object>
                            {
                                (object)int.Parse(appointmentRow["resource_id"]) // Boxing the integer into an object
                            },
                                IsAllDay = bool.Parse(appointmentRow["AllDay"]),
                                Notes = appointmentRow["DescriptionNotes"],
                                Background = backgroundColor,
                                Reminders = new ObservableCollection<SchedulerReminder>
                                {
                                    new SchedulerReminder {TimeBeforeStart = ReminderTime}
                                }

                            });
                            //foreach (SchedulerAppointment appointment in Appointments)
                            //{
                            //    Debug.WriteLine(appointment.Id + " - " + appointment.Subject);
                            //    foreach (object resourceId in appointment.ResourceIds)
                            //    {
                            //        Debug.WriteLine("Resource ID: " + resourceId.ToString());
                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {

                            Debug.WriteLine(ex.Message + " u appointment.try addu ");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine("Catch error in kalendarViewModel GetAllEvents(): " + ex.Message);
            }
        }

        public void GetUserEvents()
        {
            try
            {
                Appointments.Clear();
                var hardware_id = TrecaSreca.Get("key");
                ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
                List<int> ExternalEventIDs = new List<int>();
                List<int> InternalEventIDs = new List<int>();
                string query = "SELECT * FROM `events` WHERE resource_id = " + SQLUserID + ";";
                Debug.WriteLine(query);




                //Rezultat query-ja u apointmentData
                Dictionary<string, string>[] appointmentData = externalSQLConnect.sqlQuery(query);
                if (appointmentData != null)
                {
                    //Debug.WriteLine("Dohvatio evente ----------------------------------**");

                    //Externa lista bez internih
                    List<int> ExtDifference = ExternalEventIDs.Except(InternalEventIDs).ToList();



                    foreach (Dictionary<string, string> appointmentRow in appointmentData)
                    {
                        // Add new appointment

                        string colorName = appointmentRow["color"];
                        if (colorName == null || colorName == "")
                        {
                            colorName = "LightGray";
                        }
                        //Debug.WriteLine("-------------------------------------------------------------- " + colorName + " 0000000000000000");
                        Color backgroundColor = (Color)TypeDescriptor.GetConverter(typeof(Color)).ConvertFromString(colorName);

                        int reminderDuration = int.Parse(appointmentRow["ReminderDuration"]);
                        TimeSpan ReminderTime = TimeSpan.FromMinutes(reminderDuration);

                        Debug.WriteLine("vrijeme podsjetnika" + ReminderTime.ToString());

                        Appointments.Add(new SchedulerAppointment()
                        {
                            Id = int.Parse(appointmentRow["internal_event_id"]),
                            StartTime = DateTime.Parse(appointmentRow["TimeFrom"]),
                            EndTime = DateTime.Parse(appointmentRow["TimeTo"]),
                            Subject = appointmentRow["EventName"],
                            ResourceIds = new ObservableCollection<object>
                            {
                                (object)int.Parse(appointmentRow["resource_id"]) // Boxing the integer into an object
                            },

                            IsAllDay = bool.Parse(appointmentRow["AllDay"]),
                            Notes = appointmentRow["DescriptionNotes"],
                            Background = backgroundColor,
                            Reminders = new ObservableCollection<SchedulerReminder>
                            {
                                new SchedulerReminder {TimeBeforeStart = ReminderTime}
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message + "in kalendarViewModel GetUserEvents");
            }
        }


        public void AppointmentSpisIdReceived(object received, AppointmentSpisId message)
        {
            int idSpisa = int.Parse(TrecaSreca.Get("IDSpisa"));
            SpisiGridIsVisible = true;
            try
            {
                fileItems.Clear();

                // parametri za pretragu broj spisa, klijent id, opponent id, referenca, uzrok, broj predmeta
                string search_term = idSpisa.ToString();
                string escaped_search_term = search_term.Replace("/", "\\/");
                //string query = "SELECT * FROM `files` WHERE `id` = '" + escaped_search_term + "'";
                string query = "SELECT files.*, client.ime AS client_name FROM files LEFT JOIN contacts AS client ON files.client_id = client.id WHERE files.id = '" + escaped_search_term + "'";
                //string query = "SELECT files.*, client.ime AS client_name, opponent.ime AS opponent_name FROM files LEFT JOIN contacts AS client ON files.client_id = client.id LEFT JOIN contacts AS opponent ON files.opponent_id = opponent.id WHERE files.broj_spisa LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR files.referenca LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR files.uzrok LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR files.broj_predmeta LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci ORDER BY files.broj_predmeta DESC";
                //string query = "SELECT files.*, client.ime AS client_name, opponent.ime AS opponent_name FROM files LEFT JOIN contacts AS client ON files.client_id = client.id LEFT JOIN contacts AS opponent ON files.opponent_id = opponent.id WHERE files.broj_spisa LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR files.referenca LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR files.uzrok LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR files.broj_predmeta LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR client.ime LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR opponent.ime LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci OR files.client_id IN (SELECT id FROM contacts WHERE ime LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci) OR files.opponent_id IN (SELECT id FROM contacts WHERE ime LIKE '%" + escaped_search_term + "%' COLLATE utf8mb4_general_ci) ORDER BY files.id DESC LIMIT 20";
                // Debug.WriteLine(query);
                Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
                Debug.WriteLine(query + " u Search resultu");
                if (filesData != null)
                {
                    foreach (Dictionary<string, string> filesRow in filesData)
                    {

                        #region Varijable za listu
                        int id;


                        int.TryParse(filesRow["id"], out id);

                        #endregion
                        fileItems.Add(new FileItem()
                        {
                            Id = id,
                            BrojSpisa = filesRow["broj_spisa"],
                            ClientName = filesRow["client_name"],

                        });


                    }
                    //Debug.WriteLine("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB" + fileItems.Count());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "u AppointmentSpisIdReceived");
            }
            
        }

        /// <summary>
        /// Property changed event handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Invoke method when property changed.
        /// </summary>
        /// <param name="propertyName">property name</param>
        private void RaiseOnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #region Stari kod za adminkalendar
        //public void GetAdminCalendarEventsByDate()
        //{
        //    if (adminAppointments != null)
        //    {
        //        adminAppointments.Clear();
        //    }
        //    string query = "SELECT events.*, employees.ime, event_colors.vrsta_dogadaja FROM events INNER JOIN employees ON events.user_id = employees.id INNER JOIN event_colors ON events.color = event_colors.naziv_boje ORDER BY STR_TO_DATE(events.TimeFrom, '%Y-%m-%d %H:%i:%s') DESC;";

        //    try
        //    {

        //        Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
        //        Debug.WriteLine(query + " u Admin Kalendaru");
        //        if (filesData != null)
        //        {
        //            foreach (Dictionary<string, string> filesRow in filesData)
        //            {

        //                //int id;
        //                DateTime startTime;
        //                DateTime endTime;

        //                //int.TryParse(filesRow["ID"], out id);
        //                DateTime.TryParse(filesRow["TimeFrom"], out startTime);
        //                DateTime.TryParse(filesRow["TimeTo"], out endTime);

        //                string colorName = filesRow["color"];
        //                if (colorName == null || colorName == "")
        //                {
        //                    colorName = "LightGray";
        //                }
        //                Color backgroundColor = (Color)TypeDescriptor.GetConverter(typeof(Color)).ConvertFromString(colorName);

        //                //adminAppointments.Add(new AdminCalendarItem()
        //                //{

        //                //    StartTime = startTime,
        //                //    StartDateString = startTime.ToString("d.' 'MMM yyyy"),
        //                //    StartTimeString = startTime.ToString("HH:mm"),
        //                //    EndTime = endTime,
        //                //    EndTimeString = endTime.ToString("f"),
        //                //    EventName = filesRow["EventName"],
        //                //    DescriptionNotes = filesRow["DescriptionNotes"],
        //                //    UserName = filesRow["ime"],
        //                //    Type = backgroundColor,
        //                //    EventType = filesRow["vrsta_dogadaja"],

        //                //});

        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message + "in viewModel generate files");
        //    }
        //}

        //public void GetAdminCalendarEventsByName()
        //{
        //    if (adminAppointments != null)
        //    {
        //        adminAppointments.Clear();
        //    }
        //    string query = "SELECT events.*, employees.ime, event_colors.vrsta_dogadaja FROM events INNER JOIN employees ON events.user_id = employees.id INNER JOIN event_colors ON events.color = event_colors.naziv_boje ORDER BY employees.ime ASC, STR_TO_DATE(events.TimeFrom, '%Y-%m-%d %H:%i:%s') ASC;";

        //    try
        //    {

        //        Dictionary<string, string>[] filesData = externalSQLConnect.sqlQuery(query);
        //        Debug.WriteLine(query + " u Admin Kalendaru");
        //        if (filesData != null)
        //        {
        //            foreach (Dictionary<string, string> filesRow in filesData)
        //            {

        //                //int id;
        //                DateTime startTime;
        //                DateTime endTime;

        //                //int.TryParse(filesRow["ID"], out id);
        //                DateTime.TryParse(filesRow["TimeFrom"], out startTime);
        //                DateTime.TryParse(filesRow["TimeTo"], out endTime);

        //                string colorName = filesRow["color"];
        //                if (colorName == null || colorName == "")
        //                {
        //                    colorName = "LightGray";
        //                }
        //                Color backgroundColor = (Color)TypeDescriptor.GetConverter(typeof(Color)).ConvertFromString(colorName);

        //                adminAppointments.Add(new AdminCalendarItem()
        //                {

        //                    StartTime = startTime,
        //                    StartDateString = startTime.ToString("d' 'MMM yyyy"),
        //                    StartTimeString = startTime.ToString("HH:mm"),
        //                    EndTime = endTime,
        //                    EndTimeString = endTime.ToString("f"),
        //                    EventName = filesRow["EventName"],
        //                    DescriptionNotes = filesRow["DescriptionNotes"],
        //                    UserName = filesRow["ime"],
        //                    Type = backgroundColor,
        //                    EventType = filesRow["vrsta_dogadaja"],

        //                });

        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message + "in viewModel generate files");
        //    }
        //} 
        #endregion
    }
}
