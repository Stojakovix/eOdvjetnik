using Syncfusion.Maui.Scheduler;
using System.Diagnostics;
using eOdvjetnik.Models;

namespace eOdvjetnik.Views;

public partial class Kalendar : ContentPage
{  
    
    public Kalendar()
    {
        
        InitializeComponent();
        Debug.WriteLine("inicijalizirano");
        _ = new SfScheduler();


        var mapping = new SchedulerAppointmentMapping()
        {
            StartTime = "StartTime",
            EndTime = "EndTime",
            Subject = "Subject"
        };

        // Set the mapping property of the sfScheduler control
        Scheduler.AppointmentsSource = mapping;
    }


    private async void AddAppointment_Clicked(object sender, SchedulerTappedEventArgs e)
    {
       if(e.Appointments == null)
        {
            var dialog = new AppointmentDialog();
            await Navigation.PushModalAsync(dialog);
            
            
        }
        
       
    }

}