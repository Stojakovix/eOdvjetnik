using Syncfusion.Maui.Scheduler;
using System.Diagnostics;
using System.Collections.ObjectModel;
using eOdvjetnik.Services;
using eOdvjetnik.ViewModel;
using eOdvjetnik.Models;

namespace eOdvjetnik.Views;

public partial class Kalendar : ContentPage
{
    private void RočišnikMenuItem_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine("RočišnikMenuItem_Clicked");

    }
    public Kalendar()
    {
        
        InitializeComponent();
        Debug.WriteLine("inicijalizirano");
        _ = new SfScheduler();

        this.Scheduler.DragDropSettings.TimeIndicatorTextFormat = "HH:mm";
        Scheduler.DaysView.TimeRegions = GetTimeRegion();
        this.BindingContext = new KalendarViewModel();
        Scheduler.AppointmentDrop += OnSchedulerAppointmentDrop;


        //MySQL Query;
        var odvjetnik_nas = new ExternalSQLConnect();
        var hardware_id = Preferences.Get("key", "default_value");

        //Resources -
        var viewModel = new KalendarViewModel();
        var Resources = viewModel.Resources;

        // Adding the scheduler resource collection to the schedule resources of SfSchedule.
        this.Scheduler.ResourceView.Resources = Resources;


    }

    private void OnSchedulerAppointmentDrop(object? sender, AppointmentDropEventArgs eventArgs)
    {
        try
        {
            ExternalSQLConnect externalSQLConnect = new ExternalSQLConnect();
            var appointment = eventArgs.Appointment;
            var droptime = eventArgs.DropTime;
            var timeLength = appointment.EndTime - appointment.StartTime;
            
            if(eventArgs.Appointment != null)
            {
                appointment.StartTime = eventArgs.DropTime;
                appointment.EndTime = appointment.StartTime + timeLength;
            }
            
            Debug.WriteLine(appointment.StartTime + " " + appointment.EndTime);

            string query = $"UPDATE events SET TimeFrom = '{appointment.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}', TimeTo = '{appointment.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE internal_event_id = " + appointment.Id;
            externalSQLConnect.sqlQuery(query);

        }
        catch (Exception ex)
        {

            Debug.WriteLine(ex.Message);
        }
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

    private void Scheduler_DoubleTapped(object sender, SchedulerTappedEventArgs e)
    {

        try
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
        catch ( Exception ex)
        {

            Debug.WriteLine(ex.Message);
        }
    }


   

    

   

   



}