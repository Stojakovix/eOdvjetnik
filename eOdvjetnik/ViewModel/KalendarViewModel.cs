using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using eOdvjetnik.Models;
using eOdvjetnik.Data;
using System.Diagnostics;
using eOdvjetnik.Services;
using MySql.Data.MySqlClient;

namespace eOdvjetnik.ViewModel
{
    public class KalendarViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<SchedulerAppointment> appointments;

        public KalendarViewModel()
        {
            var dataBaseAppointments = App.Database.GetSchedulerAppointment();

            FetchAppointmentFromRemoteServer();

            if (Appointments != null)
            {

                Appointments = new ObservableCollection<SchedulerAppointment>();
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

        }

        private void FetchAppointmentFromRemoteServer()
        {
            try { 
            ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
            string query = "SELECT * FROM Events";
            Dictionary<string, string>[] appointmentData = externalSQLConnect.sqlQuery(query);
            if(appointmentData !=null )
            {
                foreach (Dictionary<string, string>appointmentRow in appointmentData) {

                    Appointments.Add(new SchedulerAppointment()
                    {
                        StartTime = DateTime.Parse(appointmentRow["TimeFrom"]),
                        EndTime = DateTime.Parse(appointmentRow["TimeTo"]),
                        Subject = appointmentRow["EventName"],
                        IsAllDay = bool.Parse(appointmentRow["AllDay"]),
                        Id = int.Parse(appointmentRow["ID"]),
                        Notes = appointmentRow["DescriptionNotes"]
                    });
                        Debug.WriteLine(Appointments.Count);
                    }
            }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
               
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
