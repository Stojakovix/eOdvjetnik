using Syncfusion.Maui.Scheduler;
using System.Diagnostics;
using System.Collections.ObjectModel;
using eOdvjetnik.Data;
using eOdvjetnik.Models;
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
        

    }

    private ObservableCollection<SchedulerTimeRegion> GetTimeRegion()
    {
        var startTime = DateTime.UnixEpoch.AddHours(19);
        var endTime = startTime.AddHours(12);

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

        var wkStart = DateTime.UnixEpoch.AddHours(1);
        var wkEnd = DateTime.UnixEpoch.AddHours(23);
        timeRegion = new SchedulerTimeRegion()
        {
            StartTime = wkStart,
            EndTime = wkEnd,
            //Text = "pauza",
            EnablePointerInteraction = true,
            RecurrenceRule = "FREQ=WEEKLY;BYDAY=SA,SU;INTERVAL=1",
        };
        timeRegions.Add(timeRegion);
        return timeRegions;
    }
    private void CreateTabele()
    {
        var appointments = App.Database.GetSchedulerAppointment();
        if (appointments.Count == 0)
        {
            var todoItem = new Appointment() { From = DateTime.Today.AddHours(10), To = DateTime.Today.AddHours(12), AllDay = false, DescriptionNotes = "Meeting", EventName = "Annual", ID = 1 };
            App.Database.SaveSchedulerAppointmentAsync(todoItem);
        }
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