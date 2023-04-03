using eOdvjetnik.Models;
using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace eOdvjetnik.Views;

public partial class AppointmentDialog : ContentPage
{
    public ObservableCollection<SchedulerAppointment> Appointments { get; set; } = new ObservableCollection<SchedulerAppointment>();
    public Meeting Meeting { get; set; } = new Meeting();
    public AppointmentDialog()
    {
        BindingContext = Meeting;
        InitializeComponent();
    }

    private void Save_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine($"Meeting EventName: {Meeting.EventName}");
        Debug.WriteLine($"Meeting StartTime: {Meeting.StartTime:f}");
        Debug.WriteLine($"Meeting EndTime: {Meeting.EndTime:f}");
        var appointment = new SchedulerAppointment()
        {
            Subject = Meeting.EventName,
            StartTime = Meeting.StartTime,
            EndTime = Meeting.EndTime
        };
        Appointments.Add(appointment);
        Navigation.PopAsync();

        Debug.WriteLine("svi eventi u oc u ");
        foreach (SchedulerAppointment app in Appointments)
        {
            Debug.WriteLine($"Subject: {app.Subject}, Start Time: {app.StartTime}, End Time: {app.EndTime}");
        }
    }
}
////No bugs found.