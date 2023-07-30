using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using eOdvjetnik.Models;
using System.Diagnostics;
using eOdvjetnik.Services;
using System.Windows.Input;

namespace eOdvjetnik.ViewModel
{
    public class KalendarViewModel : INotifyPropertyChanged
    {
        private Navigacija navigacija;
        private ObservableCollection<SchedulerAppointment> appointments;


        public KalendarViewModel()
        {
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
                    Debug.WriteLine("Usao usqlQuery *-*-**-*-*---------------DOHVAĆENI EVENTI iz MySQL----------------------------------**");

                    //Externa lista bez internih
                    List<int> ExtDifference = ExternalEventIDs.Except(InternalEventIDs).ToList();

                    foreach (Dictionary<string, string> appointmentRow in appointmentData)
                    {
                        // Add new appointment
                        Appointments.Add(new SchedulerAppointment()
                        {
                            Id = int.Parse(appointmentRow["internal_event_id"]),
                            StartTime = DateTime.Parse(appointmentRow["TimeFrom"]),
                            EndTime = DateTime.Parse(appointmentRow["TimeTo"]),
                            Subject = appointmentRow["EventName"],
                            IsAllDay = bool.Parse(appointmentRow["AllDay"]),
                            Notes = appointmentRow["DescriptionNotes"]
                        });
                    }
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message + "in kalendarViewModel init");
            }
        }
        public ICommand PocetnaClick => navigacija.PocetnaClick;
        public ICommand KalendarClick => navigacija.KalendarClick;
        public ICommand SpisiClick => navigacija.SpisiClick;
        public ICommand TarifaClick => navigacija.TarifaClick;
        public ICommand DokumentiClick => navigacija.DokumentiClick;
        public ICommand KontaktiClick => navigacija.KontaktiClick;
        public ICommand KorisnickaClick => navigacija.KorisnickaPodrskaClick;
        public ICommand PostavkeClick => navigacija.PostavkeClick;

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
