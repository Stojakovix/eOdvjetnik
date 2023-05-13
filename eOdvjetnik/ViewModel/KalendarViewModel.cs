using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using eOdvjetnik.Models;
using eOdvjetnik.Data;
using System.Diagnostics;
using eOdvjetnik.Services;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Globalization;

namespace eOdvjetnik.ViewModel
{
    public class KalendarViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<SchedulerAppointment> appointments;

        public KalendarViewModel()
        {
            Appointments = new ObservableCollection<SchedulerAppointment>(); // Initialize the Appointments collection

            // Fetch appointments from the remote server
            FetchAppointmentFromRemoteServer();

            // Add appointments to the remote server
            var dataBaseAppointments = App.Database.GetSchedulerAppointment();
            if (dataBaseAppointments != null)
            {
                var schedulerAppointments = dataBaseAppointments.Select(appointment => new SchedulerAppointment()
                {
                    StartTime = appointment.From,
                    EndTime = appointment.To,
                    Subject = appointment.EventName,
                    IsAllDay = appointment.AllDay,
                    Id = appointment.ID
                });
                AddAppointmentToRemoteServer(schedulerAppointments);

                // Add appointments from the local database to the Appointments collection
                foreach (Appointment appointment in dataBaseAppointments)
                {
                    Appointments.Add(new SchedulerAppointment()
                    {
                        StartTime = appointment.From,
                        EndTime = appointment.To,
                        Subject = appointment.EventName,
                        IsAllDay = appointment.AllDay,
                        Id = appointment.ID
                    });

                    Debug.WriteLine($"{appointment.ID} {appointment.EventName} {appointment.AllDay} {appointment.From} {appointment.To}");
                }
            }
            else
            {
                // Handle the case when dataBaseAppointments is null
                Debug.WriteLine("dataBaseAppointments is null");
            }
        }

        private void FetchAppointmentFromRemoteServer()
        {
            try
            {
                ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
                string query = "SELECT * FROM Events";
                Dictionary<string, string>[] appointmentData = externalSQLConnect.sqlQuery(query);
                if (appointmentData != null)
                {
                    foreach (Dictionary<string, string> appointmentRow in appointmentData)
                    {

                        // Parsaj bool u čitljiv format
                        bool isAllDay;
                        bool.TryParse(appointmentRow["AllDay"], out isAllDay);

                        // parsaj date/time


                        Appointments.Add(new SchedulerAppointment()
                        {
                            StartTime = DateTime.Parse(appointmentRow["TimeFrom"]),
                            EndTime = DateTime.Parse(appointmentRow["TimeTo"]),
                            Subject = appointmentRow["EventName"],
                            IsAllDay = isAllDay,
                            Id = int.Parse(appointmentRow["ID"]),
                            Notes = appointmentRow["DescriptionNotes"]
                        });
                        Debug.WriteLine(Appointments.Count  + "IZVRŠIO QUERY DO KRAJA --------------");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "in kalendarViewModel addAppointmentToDb");

            }

        }

        private void AddAppointmentToRemoteServer(IEnumerable<SchedulerAppointment> appointments)
        {
            Debug.WriteLine("Ušao u add appointment");
            try
            {
                ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
                var hardware_id = Preferences.Get("key", "default_value");
                foreach (SchedulerAppointment appointment in appointments)
                {

                    string query = $"INSERT INTO Events (TimeFrom, TimeTo, EventName, AllDay, DescriptionNotes, internal_event_id, hardwareid) VALUES ('{appointment.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}', '{appointment.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}', '{appointment.Subject}', '{appointment.IsAllDay}', '{appointment.Notes}', '{appointment.Id}' , '{hardware_id}')";
                    externalSQLConnect.sqlQuery(query);

                    Debug.WriteLine("Appoinments added to remote server.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " in KalendarViewModel AddAppointmentToServer");
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
    }
}
