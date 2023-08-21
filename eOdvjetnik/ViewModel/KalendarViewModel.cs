using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using eOdvjetnik.Models;
using System.Diagnostics;
using eOdvjetnik.Services;
using System.Windows.Input;
using Newtonsoft.Json;
using eOdvjetnik.Model;
using System.Drawing;
using Color = Microsoft.Maui.Graphics.Color;
using Syncfusion.DocIO.DLS;
using System.Xml.Linq;

namespace eOdvjetnik.ViewModel
{
    public class KalendarViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<SchedulerAppointment> appointments;

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
                    ColorsToJSON();

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
        }

        public void ColorsToJSON()
        {
            string colorsJSON = JsonConvert.SerializeObject(CategoryColor);
            Preferences.Set("colorItems", colorsJSON);
        }
        #endregion

        private void AdminLicenceCheck()
        {
            string licence_type = Preferences.Get("licence_type", "");
            int numberOfCharacters = 5;
            string adminCheck = licence_type.Substring(0, Math.Min(licence_type.Length, numberOfCharacters));
            Debug.WriteLine("Kalendar ResourceView - 'Admin' provjera: " + adminCheck);
            if (adminCheck == "Admin")
            {
                GetEmployees();
            }
          
        }

        public KalendarViewModel()
        {

            //Resources - 2 
            //Resources = new ObservableCollection<SchedulerResource>()
            //{
            //    new SchedulerResource() { Name = "Sophia", Foreground = Colors.Blue, Background = Colors.Green, Id = "1000" },
            //    new SchedulerResource() { Name = "Zoey Addison",  Foreground = Colors.Blue, Background = Colors.Green, Id = "1001" },
            //    new SchedulerResource() { Name = "James William",  Foreground = Colors.Blue, Background = Colors.Green, Id = "1002" },
            //};
            try
            {
                Appointments = new ObservableCollection<SchedulerAppointment>(); // Initialize the Appointments collection
                CategoryColor = new ObservableCollection<ColorItem>();
                employeeItem = new ObservableCollection<EmployeeItem>();
                Resources = new ObservableCollection<SchedulerResource>();

                AdminLicenceCheck();
                GetColors();
             
                var hardware_id = Preferences.Get("key", "default_value");
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



                        Appointments.Add(new SchedulerAppointment()
                        {
                            Id = int.Parse(appointmentRow["internal_event_id"]),
                            StartTime = DateTime.Parse(appointmentRow["TimeFrom"]),
                            EndTime = DateTime.Parse(appointmentRow["TimeTo"]),
                            Subject = appointmentRow["EventName"],
                            ResourceIds = new ObservableCollection<object>
                            {
                                (object)int.Parse(appointmentRow["user_id"]) // Boxing the integer into an object
                            },
                            IsAllDay = bool.Parse(appointmentRow["AllDay"]),
                            Notes = appointmentRow["DescriptionNotes"],
                            Background = backgroundColor,
                        });
                    }
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message + "in kalendarViewModel init");
            }
        }

        public void GetEmployees()
        {
            try
            {
                if (employeeItem != null)
                {
                    employeeItem.Clear();

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
                            Background = Colors.LightSkyBlue,
                            Id = employee.Id
                        });

                        Debug.Write(employeeItem);
                    }


                    OnPropertyChanged(nameof(employeeItem));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "in viewModel generate files");
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
    }
}
