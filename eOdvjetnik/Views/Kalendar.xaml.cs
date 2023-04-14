using Syncfusion.Maui.Scheduler;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Xml;

namespace eOdvjetnik.Views;

public partial class Kalendar : ContentPage
{  
    
    public Kalendar()
    {
        
        InitializeComponent();
        Debug.WriteLine("inicijalizirano");
        _ = new SfScheduler();

        Scheduler.View = SchedulerView.Day;
        Scheduler.DaysView.TimeRegions = GetTimeRegion();
        //Scheduler.SelectableDayPredicate = (date) =>
        //{
        //    if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
        //    {
                
        //        return false;
        //    }
            
        //    return true;
        //};


    }

    private ObservableCollection<SchedulerTimeRegion> GetTimeRegion()
    {
        var startTime = DateTime.Today.Date.AddHours(19);
        var endTime = startTime.AddHours(10);

        var timeRegions = new ObservableCollection<SchedulerTimeRegion>();
        var timeRegion = new SchedulerTimeRegion()

        {

            StartTime = startTime,
            EndTime = endTime,
            //Text = "pauza",
            EnablePointerInteraction = true,
            RecurrenceRule = "FREQ=DAILY;INTERVAL=1",
            
         
        };
        timeRegions.Add(timeRegion);
        return timeRegions;
    }

    
    

    private void Scheduler_Tapped(object sender, SchedulerTappedEventArgs e)
    {
        if (e.Element == SchedulerElement.SchedulerCell || e.Element == SchedulerElement.Appointment)
        {
            if (e.Appointments != null)
            {
                Navigation.PushAsync(new AppointmentDialog((SchedulerAppointment)e.Appointments[0], (e.Appointments[0] as SchedulerAppointment).StartTime, this.Scheduler));
            }
            else
            {
                Navigation.PushAsync(new AppointmentDialog(null, (DateTime)e.Date, this.Scheduler));
            }
        }
    }

   

    //private void DoubleTapped(object sender, SchedulerDoubleTappedEventArgs e)
    //{
    //    if (e.Element == SchedulerElement.SchedulerCell || e.Element == SchedulerElement.Appointment)
    //    {
    //        if(e.Appointments != null)
    //        {
    //            popup.Show();
    //        }
    //        else
    //        {
    //            popup.Show();
    //        }
    //    }
    //}

   

   



}