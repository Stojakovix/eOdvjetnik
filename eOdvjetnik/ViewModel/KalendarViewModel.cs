using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using eOdvjetnik.Models;
using System.Diagnostics;
using eOdvjetnik.Services;


namespace eOdvjetnik.ViewModel
{
    public class KalendarViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<SchedulerAppointment> appointments;
        private object odvjetnik_nas;

        public KalendarViewModel()
        {
            try
            {
                Appointments = new ObservableCollection<SchedulerAppointment>(); // Initialize the Appointments collection
                var hardware_id = Preferences.Get("key", "default_value");
                ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
                List<int> ExternalEventIDs = new List<int>();
                List<int> InternalEventIDs = new List<int>();


                string query = "Select * from events where hardwareid = '" + hardware_id + "' and TimeFrom > '2023-05-25 20:00:00'";
                Debug.WriteLine(query);

                //Rezultat query-ja u apointmentData
                Dictionary<string, string>[] appointmentData = externalSQLConnect.sqlQuery(query);
                if (appointmentData != null){
                    Debug.WriteLine("Usao usqlQuery *-*-**-*-*---------------DOHVAĆENI EVENTI iz MySQL----------------------------------**");
                    //Za svaki apointmentData dodaj u listu
                    foreach (Dictionary<string, string> appointmentRow in appointmentData){
                        ExternalEventIDs.Add(int.Parse(appointmentRow["internal_event_id"]));
                    }
                    //Dohvaćanje Evenata sa uređaja
                    var dataBaseAppointments = App.Database.GetSchedulerAppointment();
                    if (dataBaseAppointments != null){
                        var schedulerAppointments = dataBaseAppointments.Select(appointment => new SchedulerAppointment()
                        {
                            StartTime = appointment.From,
                            EndTime = appointment.To,
                            Subject = appointment.EventName,
                            IsAllDay = appointment.AllDay,
                            Id = appointment.ID
                        });
                        foreach (Appointment appointment in dataBaseAppointments){
                            InternalEventIDs.Add(appointment.ID);
                            Debug.WriteLine("KalendarViewModel -> "+appointment.ID);
                        }
                    }else{
                        // Handle the case when dataBaseAppointments is null
                        Debug.WriteLine("dataBaseAppointments is null");
                    }

                    //Externa lista bez internih
                    List<int> ExtDifference = ExternalEventIDs.Except(InternalEventIDs).ToList();
                    foreach (int item in ExtDifference){
                        Debug.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++");
                        Debug.WriteLine("KalendarViewModel -> ExtDifference: " + item);
                        Debug.WriteLine("-----------------------------------------------");

                    }

                    //Interna lista bez externih
                    List<int> IntDifference = InternalEventIDs.Except(ExternalEventIDs).ToList();
                    foreach (int item in IntDifference){
                        Debug.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++");
                        Debug.WriteLine("KalendarViewModel -> IntDifference: " + item);
                        Debug.WriteLine("-----------------------------------------------");
                    }




                    
                    foreach (Dictionary<string, string> appointmentRow in appointmentData)
                    {
                        // Parse bool to a readable format
                        bool isAllDay;
                        bool.TryParse(appointmentRow["AllDay"], out isAllDay);

                        // Parse date/time
                        DateTime startTime = DateTime.Parse(appointmentRow["TimeFrom"]);
                        DateTime endTime = DateTime.Parse(appointmentRow["TimeTo"]);
                        string eventName = appointmentRow["EventName"];
                        string descriptionNotes = appointmentRow["DescriptionNotes"];
                        int appointmentId = int.Parse(appointmentRow["ID"]);
                        int internalEventId = int.Parse(appointmentRow["internal_event_id"]);
                        var existingAppointment = Appointments.FirstOrDefault(a => (int)a.Id == internalEventId);
                        if (existingAppointment != null)
                        {
                            // Update existing appointment
                            existingAppointment.StartTime = startTime;
                            existingAppointment.EndTime = endTime;
                            existingAppointment.Subject = eventName;
                            existingAppointment.IsAllDay = isAllDay;
                            existingAppointment.Notes = descriptionNotes;
                        }
                        else
                        {
                            // Add new appointment
                            Appointments.Add(new SchedulerAppointment()
                            {
                                StartTime = startTime,
                                EndTime = endTime,
                                Subject = eventName,
                                IsAllDay = isAllDay,
                                Id = appointmentId,
                                Notes = descriptionNotes
                            });
                        }
                        Debug.WriteLine(Appointments.Count + "IZVRŠIO QUERY DO KRAJA --------------");
                        Debug.WriteLine(Appointments.Count + "IZVRŠIO QUERY DO KRAJA --------------");
                        Debug.WriteLine(Appointments.Count + "IZVRŠIO QUERY DO KRAJA --------------");
                        Debug.WriteLine(Appointments.Count + "IZVRŠIO QUERY DO KRAJA --------------");
                    }
                }




















                // Fetch appointments from the remote server
                //FetchAppointmentFromRemoteServer();

                // Add appointments to the remote server

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message + "in kalendarViewModel init");
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


                    /*
                    foreach (Dictionary<string, string> appointmentRow in appointmentData)
                    {
                        // Parse bool to a readable format
                        bool isAllDay;
                        bool.TryParse(appointmentRow["AllDay"], out isAllDay);

                        // Parse date/time
                        DateTime startTime = DateTime.Parse(appointmentRow["TimeFrom"]);
                        DateTime endTime = DateTime.Parse(appointmentRow["TimeTo"]);
                        string eventName = appointmentRow["EventName"];
                        string descriptionNotes = appointmentRow["DescriptionNotes"];
                        int appointmentId = int.Parse(appointmentRow["ID"]);
                        int internalEventId = int.Parse(appointmentRow["internal_event_id"]);
                        var existingAppointment = Appointments.FirstOrDefault(a => (int)a.Id == internalEventId);
                        if (existingAppointment != null)
                        {
                            // Update existing appointment
                            existingAppointment.StartTime = startTime;
                            existingAppointment.EndTime = endTime;
                            existingAppointment.Subject = eventName;
                            existingAppointment.IsAllDay = isAllDay;
                            existingAppointment.Notes = descriptionNotes;
                        }
                        else
                        {
                            // Add new appointment
                            Appointments.Add(new SchedulerAppointment()
                            {
                                StartTime = startTime,
                                EndTime = endTime,
                                Subject = eventName,
                                IsAllDay = isAllDay,
                                Id = appointmentId,
                                Notes = descriptionNotes
                            });
                        }
                        Debug.WriteLine(Appointments.Count + "IZVRŠIO QUERY DO KRAJA --------------");
                        Debug.WriteLine(Appointments.Count + "IZVRŠIO QUERY DO KRAJA --------------");
                        Debug.WriteLine(Appointments.Count + "IZVRŠIO QUERY DO KRAJA --------------");
                        Debug.WriteLine(Appointments.Count + "IZVRŠIO QUERY DO KRAJA --------------");
                    }*/
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " in kalendarViewModel FetchAppointmentFromRemoteServer");
            }
        }


        private void AddAppointmentToRemoteServer(IEnumerable<SchedulerAppointment> appointments)
        {

            /*
            Debug.WriteLine("Ušao u add appointment");
            try
            {
                ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
                var hardware_id = Preferences.Get("key", "default_value");


                foreach (SchedulerAppointment appointment in appointments)
                {
                    string query = $"INSERT INTO events (TimeFrom, TimeTo, EventName, AllDay, DescriptionNotes, internal_event_id, hardwareid) VALUES ('{appointment.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}', '{appointment.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}', '{appointment.Subject}', '{appointment.IsAllDay}', '{appointment.Notes}', '{appointment.Id}' , '{hardware_id}')";
                    externalSQLConnect.sqlQuery(query);
                    Debug.WriteLine("Appoinments added to remote server.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " in KalendarViewModel AddAppointmentToServer");
            }

            */
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
