using Syncfusion.Maui.Scheduler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace sample
{
    public class ControlViewModel
    {
        public ObservableCollection<SchedulerAppointment> SchedulerEvents { get; set; }
        public  ControlViewModel() {
            SchedulerEvents = new ObservableCollection<SchedulerAppointment>
            {
                new SchedulerAppointment
                {
                    StartTime= new DateTime( 2023, 11, 18, 15, 0, 0),
                    EndTime= new DateTime( 2023, 11, 18, 17, 0, 0),
                    Subject= "Client Meeting",
                    Background = Colors.Black

                }
            };
        }
    }
}
