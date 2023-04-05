using eOdvjetnik.Models;
using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace eOdvjetnik.Views;

public partial class AppointmentDialog : ContentPage
{
    public ObservableCollection<SchedulerAppointment> Appointments { get; set; } = new();
    public Meeting Meeting { get; set; } = new Meeting();
    public AppointmentDialog()
    {
        InitializeComponent();
        BindingContext = Meeting;
    }

    private void Save_Clicked(object sender, EventArgs e)
    {
        //Debug.WriteLine($"Meeting EventName: {Meeting.EventName}");
        //Debug.WriteLine($"Meeting to: {Meeting.From:f}");
        //Debug.WriteLine($"Meeting EndTime: {Meeting.To:f}");
        

        var appointment = new SchedulerAppointment()
        {
            Subject = Meeting.EventName,
            StartTime = Meeting.From, 
            EndTime = Meeting.To,
            
            
        };
        

        Appointments.Add(appointment);
        Meeting = new();
        BindingContext = Meeting;


        Navigation.PopAsync();

        Debug.WriteLine("svi eventi u oc u ");
        foreach (SchedulerAppointment app in Appointments)
        {
            Debug.WriteLine($"Subject: {app.Subject}, Start Time: {app.StartTime}, End Time: {app.EndTime}");
        }
    }
}
////No bugs found.