using Syncfusion.Maui.Scheduler;
using System.Diagnostics;
using System.Windows.Input;

namespace eOdvjetnik.Views;

public partial class Kalendar : ContentPage
{  
    
    public Kalendar()
    {
        
        InitializeComponent();
        Debug.WriteLine("inicijalizirano");
        _ = new SfScheduler();
    }
    private void Scheduler_Tapped(object sender, SchedulerTappedEventArgs e)
    {
        if (e.Element == SchedulerElement.SchedulerCell || e.Element == SchedulerElement.Appointment)
        {
            if (e.Appointments != null)
            {
                this.Navigation.PushAsync(new AppointmentDialog((SchedulerAppointment)e.Appointments[0], (e.Appointments[0] as SchedulerAppointment).StartTime, this.Scheduler));
            }
            else
            {
                this.Navigation.PushAsync(new AppointmentDialog(null, (DateTime)e.Date, this.Scheduler));
            }
        }
    }

}