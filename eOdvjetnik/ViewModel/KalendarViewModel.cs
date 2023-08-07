﻿using Syncfusion.Maui.Scheduler;
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

namespace eOdvjetnik.ViewModel
{
    public class KalendarViewModel : INotifyPropertyChanged
    {
        private Navigacija navigacija;
        private ObservableCollection<SchedulerAppointment> appointments;

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

        public KalendarViewModel()
        {
            CategoryColor = new ObservableCollection<ColorItem>();

            GetColors();
            try
            {
                navigacija = new Navigacija();

                Appointments = new ObservableCollection<SchedulerAppointment>(); // Initialize the Appointments collection
                var hardware_id = Preferences.Get("key", "default_value");
                ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
                List<int> ExternalEventIDs = new List<int>();
                List<int> InternalEventIDs = new List<int>();
                string query = "Select * from events where hardwareid = '" + hardware_id + "' and TimeFrom > '2023-05-25 20:00:00'";
                Debug.WriteLine(query);

                //Rezultat query-ja u apointmentData
                Dictionary<string, string>[] appointmentData = externalSQLConnect.sqlQuery(query);
                if (appointmentData != null)
                {
                    Debug.WriteLine("Dohvatio evente ----------------------------------**");

                    //Externa lista bez internih
                    List<int> ExtDifference = ExternalEventIDs.Except(InternalEventIDs).ToList();

                    foreach (Dictionary<string, string> appointmentRow in appointmentData)
                    {
                        // Add new appointment

                        string colorName = appointmentRow["color"];
                        Color backgroundColor = (Color)TypeDescriptor.GetConverter(typeof(Color)).ConvertFromString(colorName);


                        Appointments.Add(new SchedulerAppointment()
                        {
                            Id = int.Parse(appointmentRow["internal_event_id"]),
                            StartTime = DateTime.Parse(appointmentRow["TimeFrom"]),
                            EndTime = DateTime.Parse(appointmentRow["TimeTo"]),
                            Subject = appointmentRow["EventName"],
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


        /// <summary>
        /// Property changed event handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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
