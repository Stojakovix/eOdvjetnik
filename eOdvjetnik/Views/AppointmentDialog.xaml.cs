namespace eOdvjetnik.Views;
using eOdvjetnik.Models;
using Syncfusion.Maui.Scheduler;

public partial class AppointmentDialog : ContentPage
{
    public SfScheduler Scheduler;
	public AppointmentDialog()
	{
		InitializeComponent();
        var saveButton = new Button { Text = "Save" };
        saveButton.Clicked += Add_Clicked;


	}

    private void Add_Clicked(object sender, EventArgs e)
    {
        
        // Create a new instance of our data model
        var newAppointment = new MyAppointment
        {
            StartTime = new DateTime(
                StartDatePicker.Date.Year,
                StartDatePicker.Date.Month,
                StartDatePicker.Date.Day,
                StartTimePicker.Time.Hours,
                StartTimePicker.Time.Minutes,
                0),
            EndTime = new DateTime(
                EndDatePicker.Date.Year,
                EndDatePicker.Date.Month,
                EndDatePicker.Date.Day,
                EndTimePicker.Time.Hours,
                EndTimePicker.Time.Minutes,
                0),
            Subject = SubjectEntry.Text
        };

        // Add the new appointment to the scheduler's AppointmentsSource
        Scheduler.AppointmentsSource = newAppointment;

        // Dismiss the dialog box
        Navigation.PopModalAsync();
    }




    private void Cancel_Clicked(object sender, EventArgs e)
    {
        // Dismiss the dialog box
        Navigation.PopModalAsync();
    }

}