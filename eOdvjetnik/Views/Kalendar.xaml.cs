using Syncfusion.Maui.Scheduler;
using System.Diagnostics;
using System.Collections.ObjectModel;
using eOdvjetnik.Services;


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


        //MySQL Query;
        var odvjetnik_nas = new ExternalSQLConnect();

        try
        {
            //Call the function
            Dictionary<string, string>[] resultArray = odvjetnik_nas.sqlQuery("Select * from events");
            Debug.WriteLine("Usao usqlQuery");



            // Print the attribute names

            foreach (Dictionary<string, string> row in resultArray)
            {
                foreach (KeyValuePair<string, string> pair in row)
                {
                    Debug.WriteLine(pair.Key + ": " + pair.Value);
                }
                //Debug.WriteLine();
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message + "u Kalendar.xaml.csu");
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
    




    private void Scheduler_DoubleTapped(object sender, SchedulerDoubleTappedEventArgs e)
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


   

    

   

   



}