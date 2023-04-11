
using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace eOdvjetnik.ViewModel
{
    public class KalendarViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<SchedulerAppointment> appointments;

        public KalendarViewModel()
        {
            //Appointments = new ObservableCollection<SchedulerAppointment>
            //{
            //    //Adding scheduler appointment in the scheduler appointment collection. 
            //    new SchedulerAppointment()
            //    {
            //        StartTime = DateTime.Today.AddHours(9),
            //        EndTime = DateTime.Today.AddHours(11),
            //        Subject = "Client Meeting",
            //        Location = "Hutchison road",
            //    }
            //};
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
